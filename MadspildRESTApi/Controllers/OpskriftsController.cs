using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MadspildRESTApi.Managers;
using ModelLib;
using System.Web;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MadspildRESTApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpskriftsController : ControllerBase
    {
        private OpskriftManager manager = new OpskriftManager();

        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<Opskrift> Get()
        {
            return manager.GetAll();
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public IActionResult GetOne(int id)
        {

            Opskrift result = manager.GetOne(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        // GET api/<ValuesController>/q/flødekartofler
        [HttpGet("/q/{navn}")]
        public IActionResult GetOne(string navn)
        {

            Opskrift result = manager.GetOne(navn);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
