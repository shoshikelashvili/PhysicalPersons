using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }

        public void DeletePerson(Person person)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Person> DetailedSearch(string term, int page)
        {
            throw new NotImplementedException();
        }

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

        public IEnumerable<Person> QuickSearch(string term, int page)
        {
            throw new NotImplementedException();
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
