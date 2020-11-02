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
        public Session(IHubContext<GameHub> hc)
        {
            HubContext = hc;
            Timer timer = new Timer(callback, null, 0, 100);
        }

        private void callback(object state)
        {
            HubContext.Clients.All.SendCoreAsync("FindEnemy", new object[] { "String" });
        }

        public IHubContext<GameHub> HubContext { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }

    }
}
