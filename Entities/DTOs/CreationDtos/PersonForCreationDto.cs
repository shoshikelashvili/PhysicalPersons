using Entities.CustomValidations;
using Entities.DTOs.CreationDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DTOs
{
    public class PersonForCreationDto
    {
        [Required(ErrorMessage = "Person name is a required field.")]
        [MinLength(2, ErrorMessage = "Minimum length for the Name is 2 characters."), MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters.")]
        [RegularExpression(@"^[a-zA-Z]*$|^[ა-ჰ]*$", ErrorMessage = "Only English or Georgian letters are allowed.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Person last name is a required field.")]
        [MinLength(2, ErrorMessage = "Minimum length for the last name is 2 characters."), MaxLength(60, ErrorMessage = "Maximum length for the last name is 60 characters.")]
        [RegularExpression(@"^[a-zA-Z]*$|^[ა-ჰ]*$", ErrorMessage = "Only English or Georgian letters are allowed.")]
        public string LastName { get; set; }

        [StringLength(4)]
        [RegularExpression(@"ქალი|კაცი", ErrorMessage = "You can only choose one from these values: \"ქალი\", \"კაცი\".")]
        public string Gender { get; set; }


        [Required(ErrorMessage = "Personal Number is a required field.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Personal Number can only be 11 characters long")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Only numbers are allowed.")]
        public string PersonalNumber { get; set; }

        [DataType(DataType.Date)]
        [ValidateDateOfBirth]
        public DateTime Birthday { get; set; }
        public CityForCreationDto City { get; set; }
        public string Image { get; set; }
        public IEnumerable<PhoneNumberForCreationDto> PhoneNumbers { get; set; }
        public IEnumerable<RelatedPersonForCreationDto> RelatedFrom { get; set; }
        public IEnumerable<RelatedPersonForCreationDto> RelatedTo { get; set; }
    }
}
