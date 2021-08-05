using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.CustomValidations
{
    class ValidateDateOfBirth : ValidationAttribute
    {
        public ValidateDateOfBirth(): base(() => "Your age must be greater than 18") 
        {

        }

        private readonly DateTime _maxValue = DateTime.UtcNow.AddYears(-18);
        private readonly DateTime _minValue = DateTime.MinValue;

        public override bool IsValid(object value)
        {
            DateTime val = (DateTime)value;
            return val >= _minValue && val <= _maxValue;
        }
    }
}
