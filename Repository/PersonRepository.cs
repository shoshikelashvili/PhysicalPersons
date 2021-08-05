using Contracts;
using Entities;
using Entities.Models;
using Entities.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    class PersonRepository : RepositoryBase<Person>, IPersonRepository
    {
        public PersonRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }

        public void CreatePerson(Person person)
        {
            Create(person);
        }

        public void DeletePerson(Person person) => Delete(person);

        public IEnumerable<Person> Search(PersonParameters personParameters, int amount)
        {
            var result = RepositoryContext.Persons.AsQueryable();
            if(personParameters != null)
            {
                if (personParameters.Id.HasValue)
                    result = result.Where(x => x.Id == personParameters.Id);
                if (!string.IsNullOrEmpty(personParameters.Name))
                    result = result.Where(x => x.Name == personParameters.Name);
                if (!string.IsNullOrEmpty(personParameters.LastName))
                    result = result.Where(x => x.LastName == personParameters.LastName);
                if (!string.IsNullOrEmpty(personParameters.Gender))
                    result = result.Where(x => x.Gender == personParameters.Gender);
                if (!string.IsNullOrEmpty(personParameters.PersonalNumber))
                    result = result.Where(x => x.PersonalNumber == personParameters.PersonalNumber);
                if (personParameters.Birthday.HasValue)
                    result = result.Where(x => x.Birthday == personParameters.Birthday);
                if (personParameters.CityId.HasValue)
                    result = result.Where(x => x.CityId == personParameters.CityId);
            }

            return result.OrderBy(e => e.Id).Skip((personParameters.PageNumber - 1) * personParameters.PageSize).Take(personParameters.PageSize);
        }

        public IEnumerable<Person> GetPersonsByIds(IEnumerable<int> ids, bool trackChanges) => FindByCondition(p => ids.Contains(p.Id), trackChanges).ToList();

        public Person GetPerson(int id, bool trackChanges)
        {
            return RepositoryContext.Persons.Find(id);
        }

        public IEnumerable<PhoneNumber> GetPhoneNumbers(int personId, bool trackChanges)
        {
            throw new NotImplementedException();
        }

        public int GetRelatedPersons(Person person, string relationType)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Person> QuickSearch(string term, int amount)
        {
            var results = (from c in RepositoryContext.Persons
                          where c.Name.Contains(term) || c.LastName.Contains(term) || c.PersonalNumber.Contains(term)
                           orderby c.Id ascending
                           select c).Take(amount);

            return results;
        }

        public void RemoveRelatedToPerson(Person person)
        {
            throw new NotImplementedException();
        }

        public void SetImage(string imageURL)
        {
            throw new NotImplementedException();
        }

        public void UpdatePerson(Person person)
        {
            throw new NotImplementedException();
        }
    }
}
