using Entities.Models;
using Entities.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IPersonRepository
    {
        Person GetPerson(int id, bool trackChanges);

        IEnumerable<Person> GetPersonsByIds(IEnumerable<int> ids, bool trackChanges);
        IEnumerable<Person> QuickSearch(string term, int page);
        IEnumerable<Person> Search(PersonParameters personParameters, int page);
        void CreatePerson(Person person);
        void DeletePerson(Person person);
    }
}
