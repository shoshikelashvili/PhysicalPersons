using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DTOs.CreationDtos
{
    public class RelatedPersonForCreationDto
    {
        public int RelatedFromId { get; set; }
        public int RelatedToId { get; set; }

        [MaxLength(8, ErrorMessage = "Maximum length for the number type is 8 characters.")]
        [RegularExpression(@"კოლეგა|ნაცნობი|ნათესავი|სხვა", ErrorMessage = "You can only choose from these values: \"კოლეგა\", \"ნაცნობი\", \"ნათესავი\"\", \"სხვა\" ")]
        public string RelationType { get; set; }
    }
}
