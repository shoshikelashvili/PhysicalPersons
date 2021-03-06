using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface ICityRepository
    {
        City GetCityByPerson(Person person, bool trackChanges);
    }
}
