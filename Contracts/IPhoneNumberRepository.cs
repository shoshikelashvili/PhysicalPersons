using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IPhoneNumberRepository
    {
        PhoneNumber GetPhoneNumber(int id, bool trackChanges);

        IEnumerable<PhoneNumber> GetPhoneNumbersByPerson(Person person, bool trackChanges);
    }
}
