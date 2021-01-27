using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using KeyVaultService.Models;
using Azure.Security.KeyVault.Secrets;

namespace KeyVaultService.Controllers
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
            var value = _configuration["Nachricht"];
            return "Value for Secret [Nachricht] is : " + value;
        }

    }
}
