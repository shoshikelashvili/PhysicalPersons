using Entities.DTOs.CreationDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs
{
    public class PersonForCreationDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string PersonalNumber { get; set; }
        public DateTime Birthday { get; set; }
        public CityForCreationDto City { get; set; }
        public string Image { get; set; }
        public IEnumerable<PhoneNumberForCreationDto> PhoneNumbers { get; set; }
        public IEnumerable<RelatedPersonForCreationDto> RelatedFrom { get; set; }
        public IEnumerable<RelatedPersonForCreationDto> RelatedTo { get; set; }
    }
}
