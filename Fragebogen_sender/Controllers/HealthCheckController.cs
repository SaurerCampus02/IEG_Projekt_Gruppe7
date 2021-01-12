using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IEGEasyCreditCardService.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class HealthCheckController : ControllerBase
    {

        [HttpGet("")]
        [HttpHead("")]
        public IActionResult Ping()
        {
            return Ok();
        }

    }
}