using Microsoft.AspNetCore.SignalR;
using SeaBattle.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SeaBattle.Models
{
    public class Session
    {
        public IHubContext<GameHub> HubContext { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }

        public Session(IHubContext<GameHub> hc, Player player1, Player player2)
        {
            HubContext = hc;
            Player1 = player1;
            Player2 = player2;
            hc.Clients.Users(Player1.Name, Player2.Name).SendCoreAsync("SendField", new object[1]);
            //Timer timer = new Timer(callback, null, 0, 100);
        }
        public async Task Message(Player player, string data)
        {
            await HubContext.Clients.User(player.Name)
               .SendCoreAsync("OnMessage", new object[] { data });
        }
    }
}
