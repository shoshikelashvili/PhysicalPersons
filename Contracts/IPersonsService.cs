﻿using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IPersonsService
    {
        public PersonDto GetPerson(int id);
        public PersonDto CreatePerson(PersonForCreationDto person);
    }
}
