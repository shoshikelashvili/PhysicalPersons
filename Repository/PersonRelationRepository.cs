using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Repository
{
    class PersonRelationRepository : RepositoryBase<PersonRelation>, IPersonRelationRepository
    {
        public PersonRelationRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }

        public void AddRelation(Person relatedFrom, Person relatedTo)
        {
            throw new NotImplementedException();
        }

        public void AddRelation(PersonRelation relation)
        {
             Create(relation);
        }

        public void DeleteRelationship(PersonRelation relation) => Delete(relation);

        public IEnumerable<PersonRelation> GetPersonRelationsFrom(Person person, bool trackChanges) => FindByCondition(p => p.RelatedFromId.Equals(person.Id), false);

        public IEnumerable<PersonRelation> GetPersonRelationsTo(Person person, bool trackChanges) => FindByCondition(p => p.RelatedToId.Equals(person.Id), false);

        public PersonRelation GetRelationship(int relatedFrom, int relatedTo, bool trackChanges) => FindByCondition(p => p.RelatedFromId.Equals(relatedFrom) && p.RelatedToId.Equals(relatedTo), false).SingleOrDefault();
    }
}
