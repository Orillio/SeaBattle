using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using SeaBattle.Hubs;
using SeaBattle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace SeaBattle.Services
{
    public class GameService
    {
        IHttpContextAccessor accessor;
        IHubContext<GameHub> hubContext;
        List<Session> Sessions { get; set; } = new List<Session>();
        List<Player> PendingPlayers { get; set; } = new List<Player>();
        public GameService(IHttpContextAccessor acc, IHubContext<GameHub> hc)
        {
            hubContext = hc;
            accessor = acc;
            // GameHub только для рассылки клиентам, серверу будут отправляться запросы по контроллеру, 
            //через контроллер информация будет отправляться сервису
            // в сессиях организовать игру между игроками
        }
        public void FindEnemy()
        {
            var name = accessor.HttpContext.User.Identity.Name;
            if (name == null) return;
            var user = PendingPlayers.FirstOrDefault(x => x.Name != name);
            if (user != null)
            {
                var session = new Session(hubContext, new Player() { Name = name }, user);
                Sessions.Add(session);
                PendingPlayers.Remove(user);
            }
            else if (!PendingPlayers.Any(x => x.Name == name) || !Sessions.Any(x => x.Player1.Name == name || x.Player2.Name == name))
            {
                PendingPlayers.Add(new Player() { Name = name });
            }
            else return;
        }
        public async Task SendField(string json)
        {
            var name = accessor.HttpContext.User.Identity.Name;
            var session = Sessions.FirstOrDefault(x => x.Player1.Name == name || x.Player2.Name == name);
            
            if (session != null)
            {
                var user = session.Player1.Name == name ? session.Player2.Name : session.Player1.Name;
                await hubContext.Clients.User(user)
                    .SendCoreAsync("ReceiveField", new object[] { json });
            }
            else
            {
                await hubContext.Clients.User(name)
                    .SendCoreAsync("Error", new object[] { @"Не получилось отправить данные противнику" });
            }

        }
    }
}
