using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;


namespace Repository
{
    class PhoneNumberRepository : RepositoryBase<PhoneNumber>, IPhoneNumberRepository
    {
        public PhoneNumberRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }
        public Person GetPerson(PhoneNumber phoneNumber)
        {
            throw new NotImplementedException();
        }

        public PhoneNumber GetPhoneNumber(int id, bool trackChanges)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PhoneNumber> GetPhoneNumbersByPerson(Person person, bool trackChanges) => FindByCondition(p => p.PersonId.Equals(person.Id), false);
    }
}
