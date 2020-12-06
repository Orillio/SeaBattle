using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using SeaBattle.Hubs;
using SeaBattle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SeaBattle.Services
{
    public class GameService
    {
        IHttpContextAccessor accessor; // Акссессор к HTTP контексту
        IHubContext<GameHub> hubContext; // SignalR Хаб
        public List<Session> Sessions { get; set; } = new List<Session>(); // Список сессий
        public List<Player> PendingPlayers { get; set; } = new List<Player>(); // Игроки, ожидающие игры

        public string ContextPlayerName =>
            accessor.HttpContext.User.Identity.Name; // Имя игрока, который делает запрос

        public Session ContextSession =>
            Sessions.FirstOrDefault(x => x.Player1.Name == ContextPlayerName || x.Player2.Name == ContextPlayerName); // Сессия, из которой приходит запрос от игрока внутри нее

        public Player ContextSender
        {
            get
            {
                if (ContextSession != null)
                {
                    return ContextSession.Player1.Name == ContextPlayerName ? ContextSession.Player1 : ContextSession.Player2;
                }
                return null;
            }
        } // Игрок, отправляющий запрос в пределах одной сессии

        public Player ContextReceiver
        {
            get
            {
                if (ContextSession != null)
                {
                    return ContextSession.Player1.Name == ContextPlayerName ? ContextSession.Player2 : ContextSession.Player1;
                }
                return null;
            }
        } // Игрок, получающий информацию от другого игрока, который сделал запрос

        
        public GameService(IHttpContextAccessor acc, IHubContext<GameHub> hc)
        {
            hubContext = hc;
            accessor = acc;
            // GameHub только для рассылки клиентам, серверу будут отправляться запросы по контроллеру, 
            //через контроллер информация будет отправляться сервису
            // в сессиях организовать игру между игроками
        }

        public async Task OnEnd() // Вызывается, когда игрок сдается
        {
            await hubContext.Clients.Users(ContextReceiver.Name, ContextSender.Name)
                .SendCoreAsync("OnEnd", new object[1]); // вызывается клиентский метод для завершения игры

            await ContextSession.Message(ContextSender, "Вы проиграли! Вы можете начать новую игру.",
                Session.MessageType.Red);

            await ContextSession.Message(ContextReceiver, "Вы победили! Вы можете начать новую игру.",
                Session.MessageType.Blue);

            Sessions.Remove(ContextSession);
        }

        public void EnterQueue() // Добавляет игрока в очередь
        {
            var name = ContextPlayerName;
            if (name == null) return;
            var user = PendingPlayers.FirstOrDefault(x => x.Name != name);
            if (user != null)
            {
                var session = new Session(hubContext, new Player() { Name = name }, user);
                Sessions.Add(session);
                PendingPlayers.Remove(user);
            }
            else if (!PendingPlayers.Any(x => x.Name == name) && !Sessions.Any(x => x.Player1.Name == name || x.Player2.Name == name))
            {
                PendingPlayers.Add(new Player() { Name = name });
            }
            else return;
        }

        public void EscapeQueue()
        {
            var name = ContextPlayerName;
            var escPlayer = PendingPlayers.FirstOrDefault(x => x.Name == name);
            if(escPlayer != null) PendingPlayers.Remove(escPlayer);
        }
        public async Task HitEnemy(int x, int y, int shipIndex)
        {
            if (ContextSession == null) return;
            if (shipIndex == -1)
            {
                await ContextSession.Message(ContextSender,
                    $"Вы промахнулись! Ход игрока - {ContextReceiver.Name}", Session.MessageType.Red);

                await ContextSession.Message(ContextReceiver,
                    $"Оппонент промахнулся! Ход игрока - {ContextReceiver.Name}", Session.MessageType.Blue);
                ContextSession.ChangeTurn();
            }
            else
            {
                await ContextSession.Message(ContextSender,
                    $"Вы попали! Ход игрока - {ContextSender.Name}", Session.MessageType.Blue);
                await ContextSession.Message(ContextReceiver,
                    $"Оппонент попал! Ход игрока - {ContextSender.Name}", Session.MessageType.Red);
            }

            ContextReceiver.Field.Hit(x, y, shipIndex);
            await hubContext.Clients.User(ContextReceiver.Name)
                .SendCoreAsync("ReceiveOwnField", new object[] { ContextReceiver?.Field.SerializeJson() });

            if (ContextReceiver.Field.IsDefeated) await OnEnd();
        }

        public async Task<bool> ReturnGameFieldIfGameStarted()
        {
            if(ContextSession != null)
            {
                await hubContext.Clients.User(ContextSender.Name)
                    .SendCoreAsync("ReceiveOwnField", new object[] { ContextSender?.Field.SerializeJson() });

                await hubContext.Clients.User(ContextSender.Name)
                    .SendCoreAsync("ReceiveEnemyField", new object[] { ContextReceiver?.Field.SerializeJson() });

                await ContextSession.Message(ContextSender,
                    $"Ход игрока - {(ContextSender.MyTurn ? ContextSender.Name : ContextReceiver.Name)}", Session.MessageType.Blue);

                return true;
            }
            return false;
        }
        public async Task SendField(string json)
        {

            ContextSender.Field = Field.DeserializeJson(json);

            if(ContextReceiver?.Name != null)
            {
                await hubContext.Clients.User(ContextReceiver.Name)
                    .SendCoreAsync("ReceiveEnemyField", new object[] { json });
            }

        }
    }
}
