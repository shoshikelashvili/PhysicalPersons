using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Responses
{
    public class SavePersonResponse : BaseResponse
    {
        public PersonDto PersonDto{ get; set; }

        private SavePersonResponse(bool success, string message, PersonDto personDto) : base(success, message)
        {
            PersonDto = personDto;
        }

        public SavePersonResponse(PersonDto personDto) : this(true, string.Empty, personDto)
        { 

        }

        public SavePersonResponse(string message) : this(false, message, null)
        {
            
        }
    }
}
