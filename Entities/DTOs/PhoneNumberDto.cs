using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs
{
    public class PhoneNumberDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Number { get; set; }
    }
}
