using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SeaBattle.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SeaBattle.Hubs
{
    [Authorize]
    public class GameHub : Hub
    {
        GameService service;
        public GameHub(GameService s)
        {
            service = s;
        }
        Random r = new Random();
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var player = service.PendingPlayers.FirstOrDefault(x => x.Name == service.ContextPlayerName);

            if (player != null) service.PendingPlayers.Remove(player);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
