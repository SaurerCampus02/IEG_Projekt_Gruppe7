using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HelloKeyVault.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadKeyController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ReadKeyController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public string Get()
        {
            var value = _configuration["Benutzername"];
            return "Hallo: " + value;
        }
    }
}
