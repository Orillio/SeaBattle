using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using SeaBattle.Hubs;
using SeaBattle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var user = PendingPlayers.FirstOrDefault(x => x.Name != name);
            if (user != null)
            {
                Sessions.Add(new Session(hubContext)
                {
                    Player1 = new Player() { Name = name },
                    Player2 = user
                });
                PendingPlayers.Remove(user);
            }
            else if (!PendingPlayers.Any(x => x.Name == name))
            {
                PendingPlayers.Add(new Player() { Name = name });
            }
            else return;
        }
    }
}
