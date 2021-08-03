using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IPersonRepository
    {
        Person GetPerson(int id, bool trackChanges);
        IEnumerable<PhoneNumber> GetPhoneNumbers(int personId, bool trackChanges);
        IEnumerable<Person> SearchPerson(string term, int page);
        int GetRelatedPersons(Person person, string relationType);
        void SetImage(string imageURL);
        void RemoveRelatedToPerson(Person person);
        void UpdatePerson(Person person);
        void CreatePerson(Person person);
        void DeletePerson(Person person);
    }
}
