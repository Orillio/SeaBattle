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
    public class Main : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
