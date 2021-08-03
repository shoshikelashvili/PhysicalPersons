using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
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
    }
}
