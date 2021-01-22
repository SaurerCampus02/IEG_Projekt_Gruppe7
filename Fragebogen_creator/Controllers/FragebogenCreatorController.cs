using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Fragebogen_creator.Models;
using Fragebogen_creator.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Fragebogen_creator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FragebogenCreatorController : ControllerBase
    {
        private String webHookUrl = "http://localhost:1234";

        FragebogenRepository _fragebogenRepository = new FragebogenRepository();

        [HttpGet]
        public IEnumerable<Fragebogen> Get()
        {
            return _fragebogenRepository.Get();
        }

        [HttpGet("{id}")]
        public Fragebogen Get(int id)
        {
            return _fragebogenRepository.Get(id);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Fragebogen fragebogen)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            else
            {
                _fragebogenRepository.Add(fragebogen);
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(webHookUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                HttpResponseMessage response =
                    client.PostAsync(webHookUrl + "/api/WebHook/SendMail", new StringContent("")).Result;
                response.EnsureSuccessStatusCode();

                return CreatedAtAction("Get", new {id = fragebogen.FragebogenId});
            }
        }

        [HttpDelete]
        public void Delete(int id)
        {
            _fragebogenRepository.Remove(id);
        }
    }
}