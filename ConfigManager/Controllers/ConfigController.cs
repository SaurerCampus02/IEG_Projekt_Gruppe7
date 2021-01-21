using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ConfigManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            string t = System.IO.File.ReadAllText(@"./configs.json");
            JsonApi json = JsonConvert.DeserializeObject<JsonApi>(t);
            return json.ConsulUrl;
        }

        public class JsonApi
        {
            public string ConsulUrl { get; set; }
     
        }
    }
}