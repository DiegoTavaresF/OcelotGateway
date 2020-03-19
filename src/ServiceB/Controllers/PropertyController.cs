using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ServiceB.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Property A", "Property B" };
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return $"Property with id {id}";
        }
    }
}