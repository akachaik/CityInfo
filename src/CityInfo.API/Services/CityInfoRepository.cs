using System;
using System.Collections.Generic;
using System.Linq;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
        {
            _context = context;
        }
        public IEnumerable<City> GetCities()
        {
            return _context.Cities.OrderBy(c => c.Name).ToList();
        }

        public City GetCity(int cityId, bool includePointsOfInterest)
        {
            return includePointsOfInterest ? 
                _context.Cities.Include(c => c.PointsOfInterest).FirstOrDefault(c => c.Id == cityId) : 
                _context.Cities.FirstOrDefault(c => c.Id == cityId);
        }

        public IEnumerable<PointOfInterest> GetPointsOfInterest(int cityId)
        {
            return _context.PointsOfInterest.Where(p => p.CityId == cityId).ToList();
        }

        public PointOfInterest GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            return
                _context.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId && p.CityId == cityId);
        }

        public bool CityExists(int cityId)
        {
            return _context.Cities.Any(c => c.Id == cityId);
        }

        public void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest)
        {
            var city = GetCity(cityId, false);
            city.PointsOfInterest.Add(pointOfInterest);
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}