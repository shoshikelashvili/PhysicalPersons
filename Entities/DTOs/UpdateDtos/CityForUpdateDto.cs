using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DTOs.UpdateDtos
{
    public class CityForUpdateDto
    {
        [MaxLength(60, ErrorMessage = "Maximum length for the city name is 60 characters.")]
        public string Name { get; set; }
    }
}
