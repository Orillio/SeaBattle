using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
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
        [Route("/api/enterQueue")]
        public IActionResult EnterQueue()
        {
            service.EnterQueue();
            return Ok();
        }
        [Route("/api/escapeQueue")]
        public IActionResult EscapeQueue()
        {
            service.EscapeQueue();
            return Ok();
        }
        [HttpPost]
        [Route("/api/sendField")]
        public async Task<IActionResult> SendField(string json)
        {
            await service.SendField(json.ToString());
            return Ok();
        }
        [Route("/api/myGameBegan")]
        public async Task<bool> ReturnGameFieldIfGameStarted()
        {
            return await service.ReturnGameFieldIfGameStarted();
        }
        
        [Route("/api/onEnd")]
        public async Task OnEnd()
        {
            await service.OnEnd();
        }

        [Route("/api/isMyTurn")]
        public bool IsMyTurn() => service.ContextSender.MyTurn;

        [HttpPost]
        [Route("/api/hitEnemy")]
        public async Task HitEnemy(string json)
        {
            var obj = JObject.Parse(json);
            var x = obj["x"].Value<int>();
            var y = obj["y"].Value<int>();
            var i = obj["shipIndex"].Value<int>();
            await service.HitEnemy(x, y, i);
        }
        
    }
}
