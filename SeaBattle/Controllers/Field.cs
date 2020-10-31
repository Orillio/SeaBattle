using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SeaBattle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Field : ControllerBase
    {
        [Route("/api/sendField")]
        public void ReceiveField(string json)
        {

        }
    }
}
