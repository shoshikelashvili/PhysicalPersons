using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
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

        public IEnumerable<Person> GetPersons(City city)
        {
            throw new NotImplementedException();
        }
    }
}
