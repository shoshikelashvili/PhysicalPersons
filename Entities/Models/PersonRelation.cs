using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class PersonRelation
    {
        public int RelatedFromId { get; set; }
        public int RelatedToId { get; set; }

        [MaxLength(8, ErrorMessage = "Maximum length for the number type is 8 characters.")]
        [RegularExpression(@"კოლეგა|ნაცნობი|ნათესავი|სხვა", ErrorMessage = "You can only choose from these values: \"მობილური\", \"ოფისის\", \"სახლის\"")]
        public string RelationType { get; set; }
        public Person PersonRelatedFrom { get; set; }
        public Person PersonRelatedTo { get; set; }
    }
}
