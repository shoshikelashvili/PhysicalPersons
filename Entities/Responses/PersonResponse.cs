using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Responses
{
    public class PersonResponse : BaseResponse
    {
        public PersonDto PersonDto{ get; set; }

        private PersonResponse(bool success, string message, PersonDto personDto) : base(success, message)
        {
            PersonDto = personDto;
        }

        public PersonResponse(PersonDto personDto) : this(true, string.Empty, personDto)
        { 

        }

        public PersonResponse(string message) : this(false, message, null)
        {
            
        }
    }
}
