using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    interface IPersonRelationRepository
    {
        void AddRelation(Person relatedFrom, Person relatedTo);
    }
}
