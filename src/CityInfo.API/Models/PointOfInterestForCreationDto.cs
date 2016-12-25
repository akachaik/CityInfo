using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models
{
    public class PointOfInterestForCreationDto : IValidatableObject
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Name == Description)
            {
                yield return
                    new ValidationResult("Name and descrition should be different", new[] {"Name", "Description"});
            }
        }
    }
}