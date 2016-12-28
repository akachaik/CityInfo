using System;
using System.Linq;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        private readonly ILogger<PointOfInterestController> _logger;
        private readonly IMailService _mailService;

        public PointsOfInterestController(ILogger<PointOfInterestController> logger, IMailService mailService)
        {
            _logger = logger;
            _mailService = mailService;
            //var service = HttpContext.RequestServices.GetService(typeof(ILogger<PointOfInterestController>));
        }


        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                // throw new Exception("Exception sample");
                var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
                if (city == null)
                {
                    _logger.LogInformation($"city with id {cityId} was't found when accessing points of interest");
                    return NotFound();
                }

                return Ok(city.PointsOfInterest);

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}", ex);
                return StatusCode(500, "a problem happened while handling your request.");
            }
        }

        [HttpGet("{cityId}/pointsofinterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id) 
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pt = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            if (pt == null)
            {
                return NotFound();
            }

            return Ok(pt);

        }

        [HttpPost("{cityId}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(int cityId, 
            [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            //if (pointOfInterest.Name == pointOfInterest.Description)
            //{
            //    ModelState.AddModelError("Description", "The provided description shoud be different from the name");
            //}

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var max = CitiesDataStore.Current.Cities.SelectMany(
                c => c.PointsOfInterest).Max(p => p.Id);
            var pt = new PointOfInterestDto
            {
                Id = ++max,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            city.PointsOfInterest.Add(pt);

            return CreatedAtRoute("GetPointOfInterest", new {cityId = cityId, id = pt.Id}, pt);
        }

        [HttpPut("{cityId}/pointsofinterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id,
            [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Name == pointOfInterest.Description)
            {
                ModelState.AddModelError("Description", "The provided description shoud be different from the name");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var poi = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            if (poi == null)
            {
                return NotFound();
            }

            poi.Name = pointOfInterest.Name;
            poi.Description = pointOfInterest.Description;

            return NoContent();

        }

        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id
            , [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var poiFromStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == id);
            if (poiFromStore == null)
            {
                NotFound();
            }

            var poiToPatch = new PointOfInterestForUpdateDto()
            {
                Name = poiFromStore.Name,
                Description = poiFromStore.Description
            };

            patchDoc.ApplyTo(poiToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (poiToPatch.Name == poiToPatch.Description)
            {
                ModelState.AddModelError("Description", "The provided description shoud be different from the name");
            }

            TryValidateModel(poiToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            poiFromStore.Name = poiToPatch.Name;
            poiFromStore.Description = poiToPatch.Description;

            return NoContent();

        }

        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var poiFromStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == id);
            if (poiFromStore == null)
            {
                return NotFound();
            }

            city.PointsOfInterest.Remove(poiFromStore);

            _mailService.SendMail("Point of interest deleted", $"POI {poiFromStore.Name} with id {poiFromStore.Id} was deleted.");

            return NoContent();
        }
    }
}
