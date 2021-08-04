using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Repository
{
    class CityRepository : RepositoryBase<City>, ICityRepository
    {
        public CityRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }
        public void CreateCity(Person person)
        {
            throw new NotImplementedException();
        }

        public City GetCity(int id, bool trackChanges)
        {
            throw new NotImplementedException();
        }

        public City GetCityByPerson(Person person, bool trackChanges) => FindByCondition(c => c.Id.Equals(person.CityId), trackChanges).SingleOrDefault();

        public IEnumerable<Person> GetPersons(City city)
        {
            throw new NotImplementedException();
        }
    }
}
