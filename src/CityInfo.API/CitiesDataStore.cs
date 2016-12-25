using System.Collections.Generic;
using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public  static  CitiesDataStore Current { get; } = new CitiesDataStore();

        public List<CityDto> Cities { get; set; }

        public CitiesDataStore()
        {
            Cities = new List<CityDto>
            {
                new CityDto
                {
                    Id = 1,
                    Name = "Bangkok",
                    Description = "The city of smiles",
                    PointsOfInterest = new List<PointOfInterestDto>
                    {
                        new PointOfInterestDto
                        {
                            Id = 1,
                            Name = "Central Park",
                            Description = "Desc 1"
                        },
                        new PointOfInterestDto
                        {
                            Id = 2,
                            Name = "Central Park 2",
                            Description = "Desc 2"
                        }
                    }
                },
                new CityDto
                {
                    Id = 2,
                    Name = "New York City",
                    Description = "The one with that big park",
                    PointsOfInterest = new List<PointOfInterestDto>
                    {
                        new PointOfInterestDto
                        {
                            Id = 3,
                            Name = "Central Park 3",
                            Description = "Desc 3"
                        },
                        new PointOfInterestDto
                        {
                            Id = 4,
                            Name = "Central Park 4",
                            Description = "Desc 4"
                        }
                    }
                },
                new CityDto
                {
                    Id = 3,
                    Name = "Antwerp",
                    Description = "The one with cathedral that was never really finished",
                    PointsOfInterest = new List<PointOfInterestDto>
                    {
                        new PointOfInterestDto
                        {
                            Id = 5,
                            Name = "Central Park 5",
                            Description = "Desc 5"
                        },
                        new PointOfInterestDto
                        {
                            Id = 6,
                            Name = "Central Park 6",
                            Description = "Desc 6"
                        }
                    }
                }
            };
        }
    }
}
