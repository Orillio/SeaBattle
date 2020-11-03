using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SeaBattle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeaBattle.Hubs
{
    [Authorize]
    public class GameHub : Hub
    {
        Random r = new Random();
        public async Task Send(string message)
        {
            await Clients.All.SendCoreAsync("Send", new object[] { Context.UserIdentifier });
        }
    }
}
