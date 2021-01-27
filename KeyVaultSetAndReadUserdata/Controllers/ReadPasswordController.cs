using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeyVaultService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadPasswordController
    {
        private readonly IConfiguration _configuration;

        public ReadPasswordController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public string Get(string username)
        {
            var password = _configuration[username];
            return "Das verschlüsselte Passwort von " + username + " lautet: " + password;
        }
    }
}
