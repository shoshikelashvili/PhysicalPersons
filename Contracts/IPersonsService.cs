using Entities.DTOs;
using Entities.DTOs.UpdateDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IPersonsService
    {
        public PersonDto GetPerson(int id);
        public PersonDto CreatePerson(PersonForCreationDto person);
        public bool UpdatePerson(int personId, PersonForUpdateDto person);
        public bool SetImage(int personId, ImageForUpdateDto image);
    }
}
