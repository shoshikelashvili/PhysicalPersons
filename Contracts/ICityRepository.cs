using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface ICityRepository
    {
        City GetCity(int id, bool trackChanges);
        IEnumerable<Person> GetPersons(City city);
        City GetCityByPerson(Person person, bool trackChanges);
        void CreateCity(Person person);
    }
}
