using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    public class PhoneNumber
    {
        [Column("PhoneNumberId")]
        public int Id { get; set; }

        [MaxLength(8, ErrorMessage = "Maximum length for the number type is 8 characters.")]
        [RegularExpression(@"მობილური|ოფისის|სახლის", ErrorMessage = "You can only choose from these values: \"მობილური\", \"ოფისის\", \"სახლის\"")]
        public string Type { get; set; }

        [Required]
        [MinLength(4, ErrorMessage = "Minimum length for the Name is 4 characters."), MaxLength(50, ErrorMessage = "Maximum length for the Name is 50 characters.")]
        public string Number { get; set; }

        [ForeignKey(nameof(Person))]
        public int PersonId { get; set; }
    }
}
