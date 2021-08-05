using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IPersonRelationRepository
    {
        void AddRelation(Person relatedFrom, Person relatedTo);
        void AddRelation(PersonRelation relation);
        IEnumerable<PersonRelation> GetPersonRelationsFrom(Person person, bool trackChanges);
        IEnumerable<PersonRelation> GetPersonRelationsTo(Person person, bool trackChanges);
        PersonRelation GetRelationship(int relatedFrom, int relatedTo, bool trackChanges);

        void DeleteRelationship(PersonRelation relation);
    }
}
