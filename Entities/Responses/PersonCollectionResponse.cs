using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Responses
{
    public class PersonCollectionResponse : BaseResponse
    {
        public IEnumerable<PersonDto> PersonDtoCollection { get; set; }

        private PersonCollectionResponse(bool success, string message, IEnumerable<PersonDto> personDtoCollection) : base(success, message)
        {
            PersonDtoCollection = personDtoCollection;
        }

        public PersonCollectionResponse(IEnumerable<PersonDto> personDtoCollection) : this(true, string.Empty, personDtoCollection)
        {

        }

        public PersonCollectionResponse(string message) : this(false, message, null)
        {

        }
    }
}
