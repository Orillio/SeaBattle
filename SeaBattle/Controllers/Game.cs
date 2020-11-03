using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SeaBattle.Hubs;
using SeaBattle.Services;

namespace SeaBattle.Controllers
{
    public class Game : Controller
    {
        GameService service;
        IHubContext<GameHub> hubContext;
        public Game(IHubContext<GameHub> hub, GameService s)
        {
            hubContext = hub;
            service = s;
        }
        [Route("/api/findEnemy")]
        public IActionResult FindEnemy()
        {
            service.FindEnemy();
            return Ok();
        }
        [HttpPost]
        [Route("/api/sendField")]
        public async Task<IActionResult> SendField(string json)
        {
            await service.SendField(json.ToString());
            return Ok();
        }
    }
}
