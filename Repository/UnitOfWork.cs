using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RepositoryContext _repositoryContext;
        private IPersonRepository _personRepository;
        private ICityRepository _cityRepository;
        private IPhoneNumberRepository _phoneNumberRepository;
        private IPersonRelationRepository _personRelationRepository;

        public UnitOfWork(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IPersonRepository Person
        {
            get
            {
                if(_personRepository == null)
                {
                    _personRepository = new PersonRepository(_repositoryContext);
                }
                return _personRepository;
            }
        }

        public IPhoneNumberRepository PhoneNumber
        {
            get
            {
                if (_phoneNumberRepository == null)
                {
                    _phoneNumberRepository = new PhoneNumberRepository(_repositoryContext);
                }
                return _phoneNumberRepository;
            }
        }

        public ICityRepository City
        {
            get
            {
                if (_cityRepository == null)
                {
                    _cityRepository = new CityRepository(_repositoryContext);
                }
                return _cityRepository;
            }
        }

        public IPersonRelationRepository PersonRelation
        {
            get
            {
                if (_personRelationRepository == null)
                {
                    _personRelationRepository = new PersonRelationRepository(_repositoryContext);
                }
                return _personRelationRepository;
            }
        }

        public void Save()
        {
            _repositoryContext.SaveChanges();
        }
    }
}
