using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        [HttpGet]
        public IActionResult GetCities()
        {
            var cities = CitiesDataStore.Current.Cities;

            return Ok(cities);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return Ok(city);
        }
    }


    public class PointOfInterestController : Controller
    {
        
    }
}
