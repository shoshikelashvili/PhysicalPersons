using Entities.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    public class Person
    {
        //Identity field will be automatically generated and unique
        //We could also add the check constraint here to limit it to only positive integers
        //But logically it shouldn't be needed, since it will never become negative by default
        [Column("PersonId")] 
        public int Id { get; set; }

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

        [Column(TypeName = "char")]
        [Required(ErrorMessage = "Personal Number is a required field.")]
        [StringLength(11)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Only numbers are allowed.")]
        public string PersonalNumber { get; set; }

        [DataType(DataType.Date)]
        [ValidateDateOfBirth]
        public DateTime Birthday { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }

        public ICollection<PhoneNumber> PhoneNumbers { get; set; }

        public string Image { get; set; }

        public ICollection<PersonRelation> RelatedTo { get; set; }
        public ICollection<PersonRelation> RelatedFrom { get; set; }
    }
}
