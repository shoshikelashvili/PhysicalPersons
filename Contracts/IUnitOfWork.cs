using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IUnitOfWork
    {
        IPersonRepository Person { get; }
        IPhoneNumberRepository PhoneNumber { get; }
        ICityRepository City { get; }
        IPersonRelationRepository PersonRelation {get; }

        void Save();
    }
}
