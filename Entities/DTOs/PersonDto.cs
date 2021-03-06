using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs
{
    public class PersonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string PersonalNumber { get; set; }
        public DateTime Birthday { get; set; }

        public CityDto City { get; set; }
        public IEnumerable<PhoneNumberDto> PhoneNumbers { get; set; }
        public string Image { get; set; }
        public IEnumerable<RelatedPersonDto> RelatedTo { get; set; }
        public IEnumerable<RelatedPersonDto> RelatedFrom { get; set; }
    }
}
