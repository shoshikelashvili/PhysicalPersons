using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.CustomValidations
{
    class ValidateDateOfBirth : ValidationAttribute
    {
        private readonly DateTime _maxValue = DateTime.UtcNow.AddYears(-18);
        private readonly DateTime _minValue = DateTime.MinValue;

        public override bool IsValid(object value)
        {
            DateTime val = (DateTime)value;
            return val >= _minValue && val <= _maxValue;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessage, _minValue, _maxValue);
        }
    }
}
