using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/dummy")]
    public class DummyController : Controller
    {
        private readonly CityInfoContext _ctx;

        public DummyController(CityInfoContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok();
        }
    }
}