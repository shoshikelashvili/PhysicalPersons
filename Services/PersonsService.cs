using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Contracts;
using Entities.DTOs;
using Entities.Models;
using Contracts;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;
        public PersonsService(IUnitOfWork unitOfWork, IMapper mapper, ILoggerManager loggerManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggerManager = loggerManager;
        }
        public PersonDto GetPerson(int id) 
        {
            var person = _unitOfWork.Person.GetPerson(id, false);
            person.City = _unitOfWork.City.GetCityByPerson(person, false);
            person.PhoneNumbers = _unitOfWork.PhoneNumber.GetPhoneNumbersByPerson(person, false).ToList();

            var personRelationsFromIds = _unitOfWork.PersonRelation.GetPersonRelationsFrom(person, false).Select(p => p.RelatedToId).ToList();
            var relatedFromPersons = _unitOfWork.Person.GetPersonsByIds(personRelationsFromIds, false).ToList();
            var relatedFromDto = relatedFromPersons.AsEnumerable().Select(p => _mapper.Map<RelatedPersonDto>(p, opt =>
opt.AfterMap((src, dest) => dest.RelationType = _unitOfWork.PersonRelation.GetRelationType(person.Id, p.Id, false).RelationType)));

            var personRelationsToIds = _unitOfWork.PersonRelation.GetPersonRelationsTo(person, false).Select(p => p.RelatedFromId).ToList();
            var relatedToPersons = _unitOfWork.Person.GetPersonsByIds(personRelationsToIds, false);
            var relatedToDto = relatedToPersons.AsEnumerable().Select(p => _mapper.Map<RelatedPersonDto>(p, opt =>
opt.AfterMap((src, dest) => dest.RelationType = _unitOfWork.PersonRelation.GetRelationType(p.Id, person.Id, false).RelationType)));


            var personDto = _mapper.Map<PersonDto>(person, opt => opt.AfterMap((src, dest) =>
            {
                dest.RelatedFrom = relatedFromDto;
                dest.RelatedTo = relatedToDto;
            }));

            return personDto;
        }

        public PersonDtoAfterCreation CreatePerson(PersonForCreationDto person)
        {
            var personEntity = _mapper.Map<Person>(person);

            //Logic for many to many relationship is written later

            personEntity.RelatedFrom = null;
            
            _unitOfWork.Person.CreatePerson(personEntity);
            _unitOfWork.Save();

            foreach (var r in person.RelatedFrom)
            {
                var personRelation = new PersonRelation()
                {
                    RelatedFromId = personEntity.Id,
                    RelatedToId = r.RelatedToId,
                    RelationType = r.RelationType
                };

                _unitOfWork.PersonRelation.AddRelation(personRelation);
                _unitOfWork.Save();
            }

            personEntity.RelatedFrom = _unitOfWork.PersonRelation.GetPersonRelationsFrom(personEntity, false).ToList();
            return _mapper.Map<PersonDtoAfterCreation>(personEntity);
        }
    }
}
