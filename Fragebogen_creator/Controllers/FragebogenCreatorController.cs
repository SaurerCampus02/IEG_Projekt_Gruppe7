using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fragebogen_creator.Models;
using Fragebogen_creator.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fragebogen_creator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FragebogenCreatorController : ControllerBase
    {
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
        public IActionResult Post([FromBody]Fragebogen fragebogen)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            else
            {
                _fragebogenRepository.Add(fragebogen);
                return CreatedAtAction("Get", new { id = fragebogen.FragebogenId });
            }
        }

        [HttpDelete]
        public void Delete(int id)
        {
            _fragebogenRepository.Remove(id);
        }
    }
}