using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeaBattle.Hubs
{
    public class GameHub : Hub
    {
        public async Task Send(string message)
        {
            await Clients.All.SendCoreAsync("Send", new object[] { "Hello"});
        }
    }
}
