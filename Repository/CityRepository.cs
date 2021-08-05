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

        public City GetCityByPerson(Person person, bool trackChanges) => FindByCondition(c => c.Id.Equals(person.CityId), trackChanges).SingleOrDefault();
    }
}
