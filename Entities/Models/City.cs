using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    class City
    {
        [Column("CityId")]
        public int Id { get; set; }

        [MaxLength(60, ErrorMessage = "Maximum length for the city name is 60 characters.")]
        public string Name { get; set; }

        public ICollection<Person> Persons { get; set; }
    }
}
