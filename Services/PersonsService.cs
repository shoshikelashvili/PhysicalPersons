using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Contracts;
using Entities.DTOs;
using Entities.Models;
using Contracts;
using Entities.DTOs.UpdateDtos;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net;
using Entities.DTOs.CreationDtos;
using Entities.DTOs.DeletionDtos;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;
        private IHostingEnvironment _env;

        public PersonsService(IUnitOfWork unitOfWork, IMapper mapper, ILoggerManager loggerManager, IHostingEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggerManager = loggerManager;
            _env = env;
        }
        public PersonDto GetPerson(int id) 
        {
            var person = _unitOfWork.Person.GetPerson(id, false);
            if(person == null)
            {
                _loggerManager.LogError($"Person with id {id} does not exist");
                return null;
            }
            person.City = _unitOfWork.City.GetCityByPerson(person, false);
            person.PhoneNumbers = _unitOfWork.PhoneNumber.GetPhoneNumbersByPerson(person, false).ToList();

            var personRelationsFromIds = _unitOfWork.PersonRelation.GetPersonRelationsFrom(person, false).Select(p => p.RelatedToId).ToList();
            var relatedFromPersons = _unitOfWork.Person.GetPersonsByIds(personRelationsFromIds, false).ToList();
            var relatedFromDto = relatedFromPersons.AsEnumerable().Select(p => _mapper.Map<RelatedPersonDto>(p, opt =>
opt.AfterMap((src, dest) => dest.RelationType = _unitOfWork.PersonRelation.GetRelationship(person.Id, p.Id, false).RelationType)));

            var personRelationsToIds = _unitOfWork.PersonRelation.GetPersonRelationsTo(person, false).Select(p => p.RelatedFromId).ToList();
            var relatedToPersons = _unitOfWork.Person.GetPersonsByIds(personRelationsToIds, false);
            var relatedToDto = relatedToPersons.AsEnumerable().Select(p => _mapper.Map<RelatedPersonDto>(p, opt =>
opt.AfterMap((src, dest) => dest.RelationType = _unitOfWork.PersonRelation.GetRelationship(p.Id, person.Id, false).RelationType)));


            var personDto = _mapper.Map<PersonDto>(person, opt => opt.AfterMap((src, dest) =>
            {
                dest.RelatedFrom = relatedFromDto;
                dest.RelatedTo = relatedToDto;
            }));

            return personDto;
        }

        //TODO: Adjust the logic here so that cities can be assigned by ID instead of creating a new one
        public PersonDto CreatePerson(PersonForCreationDto person)
        {
            var personEntity = _mapper.Map<Person>(person);

            //Logic for many to many relationship is written later

            personEntity.RelatedFrom = null;
            personEntity.RelatedTo = null;

            _unitOfWork.Person.CreatePerson(personEntity);
            _unitOfWork.Save();

            List<PersonRelation> relatedFromList = new List<PersonRelation>();
            if(person.RelatedFrom != null)
            {
                foreach (var r in person.RelatedFrom)
                {
                    if (_unitOfWork.Person.GetPerson(r.RelatedToId, false) == null)
                    {
                        _loggerManager.LogError($"Person with id {r.RelatedToId} does not exist in the database.");
                        return null;
                    }

                    var personRelation = new PersonRelation()
                    {
                        RelatedFromId = personEntity.Id,
                        RelatedToId = r.RelatedToId,
                        RelationType = r.RelationType
                    };
                    relatedFromList.Add(personRelation);

                    _unitOfWork.PersonRelation.AddRelation(personRelation);
                    _unitOfWork.Save();
                }
            }

            var relatedFromDto = from p in relatedFromList
                                 select new RelatedPersonDto()
                                 {
                                     Id = p.RelatedToId,
                                     Name = _unitOfWork.Person.GetPerson(p.RelatedToId, false).Name,
                                     LastName = _unitOfWork.Person.GetPerson(p.RelatedToId, false).LastName,
                                     RelationType = p.RelationType
                                 };

            List<PersonRelation> relatedToList = new List<PersonRelation>();
            if (person.RelatedTo != null)
            {
                foreach (var r in person.RelatedTo)
                {
                    if(_unitOfWork.Person.GetPerson(r.RelatedFromId, false) == null)
                    {
                        _loggerManager.LogError($"Person with id {r.RelatedFromId} does not exist in the database.");
                        return null;
                    }

                    var personRelation = new PersonRelation()
                    {
                        RelatedFromId = r.RelatedFromId,
                        RelatedToId = personEntity.Id,
                        RelationType = r.RelationType
                    };
                    relatedToList.Add(personRelation);

                    _unitOfWork.PersonRelation.AddRelation(personRelation);
                    _unitOfWork.Save();
                }
            }

            var relatedToDto = from p in relatedToList
                                 select new RelatedPersonDto()
                                 {
                                     Id = p.RelatedFromId,
                                     Name = _unitOfWork.Person.GetPerson(p.RelatedFromId, false).Name,
                                     LastName = _unitOfWork.Person.GetPerson(p.RelatedFromId, false).LastName,
                                     RelationType = p.RelationType
                                 };

            personEntity.RelatedFrom = _unitOfWork.PersonRelation.GetPersonRelationsFrom(personEntity, false).ToList();
            personEntity.RelatedTo = _unitOfWork.PersonRelation.GetPersonRelationsTo(personEntity, false).ToList();

            return _mapper.Map<PersonDto>(personEntity, opt => opt.AfterMap((src, dest) =>
            {
                dest.RelatedFrom = relatedFromDto;
                dest.RelatedTo = relatedToDto;
            }));
        }

        public bool UpdatePerson(int personId, PersonForUpdateDto person)
        {
            var personEntity = _unitOfWork.Person.GetPerson(personId, true);
            if(personEntity == null)
            {
                _loggerManager.LogInfo($"Person with id: {personId} doesn't exist in the DB");
                return false;
            }

            //Code for updating phone numbers
            if (person.PhoneNumbers != null)
            {
                foreach (var p in person.PhoneNumbers)
                {
                    if(p.Id != 0)
                    {
                        var phoneNumber = _unitOfWork.PhoneNumber.GetPhoneNumber(p.Id, true);
                        if (phoneNumber == null)
                        {
                            _loggerManager.LogInfo($"Phone with id: {p.Id} doesn't exist in the DB");
                            return false;
                        }
                        if (phoneNumber.PersonId != personId)
                        {
                            _loggerManager.LogInfo($"Phone with id: {p.Id} doesn't belong to current user");
                            return false;
                        }

                        p.PersonId = personId;
                    }
                }
            }

            //Don't change the default value of City in case it's not passed
            if(person.CityId == 0)
            {
                person.CityId = (int)personEntity.CityId;
            }

            var mapped = _mapper.Map(person, personEntity);
            _unitOfWork.Save();

            return true;
        }

        public bool SetImage(int personId, ImageForUpdateDto image)
        {
            var webRoot = _env.WebRootPath;
            var PathWithFolderName = System.IO.Path.Combine(webRoot, "Persons");


            if (!Directory.Exists(PathWithFolderName))
            {
                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(PathWithFolderName);
            }

            var finalPath = PathWithFolderName + "\\" + personId.ToString() + ".jpg";
            //The image saving logic can be more sophisticated, but we're settling for minimalistic for now

            using (WebClient webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(image.ImageUrl);
                File.WriteAllBytes(finalPath, data);
            }

            var personEntity = _unitOfWork.Person.GetPerson(personId, true);
            personEntity.Image = Path.GetRelativePath(webRoot, finalPath);

            _unitOfWork.Save();

            return true;
        }

        public bool CreateRelationship(int personId, RelatedPersonForCreationDto relationship)
        {
            var personEntity = _unitOfWork.Person.GetPerson(personId,false);
            if(personEntity == null)
            {
                _loggerManager.LogError("Person does not exist for relationship creation");
                return false;
            }

            //Setting relationship starting from personEntity
            if(relationship.RelatedFromId == 0)
            {
                relationship.RelatedFromId = personEntity.Id;
                if(_unitOfWork.Person.GetPerson(relationship.RelatedToId,false) == null)
                {
                    _loggerManager.LogError("Person does not exist for relationship creation");
                    return false;
                }
            }
            else
            //Setting relationship ending from personEntity
            if (relationship.RelatedToId == 0)
            {
                relationship.RelatedToId = personEntity.Id;
                if (_unitOfWork.Person.GetPerson(relationship.RelatedFromId, false) == null)
                {
                    _loggerManager.LogError("Person does not exist for relationship creation");
                    return false;
                }

            }
            else
            {
                _loggerManager.LogError("Only One parameter should be passed to relationship creation");
                return false;
            }
            var relationEntity = _mapper.Map<PersonRelation>(relationship);

            _unitOfWork.PersonRelation.AddRelation(relationEntity);
            _unitOfWork.Save();

            return true;
        }

        public bool DeleteRelationship(int personId, RelatedPersonForDeletionDto relationship)
        {
            var personEntity = _unitOfWork.Person.GetPerson(personId, false);
            if (personEntity == null)
            {
                _loggerManager.LogError("Person does not exist for relationship creation");
                return false;
            }

            //Setting relationship starting from personEntity
            if (relationship.RelatedFromId == 0)
            {
                relationship.RelatedFromId = personEntity.Id;
                if (_unitOfWork.Person.GetPerson(relationship.RelatedToId, false) == null)
                {
                    _loggerManager.LogError("Person does not exist for relationship creation");
                    return false;
                }

                var relationshipEntity = _unitOfWork.PersonRelation.GetRelationship(relationship.RelatedFromId, relationship.RelatedToId, true);
                if(relationshipEntity == null)
                {
                    _loggerManager.LogError("Relationship already does not exist");
                    return false;
                }
                _unitOfWork.PersonRelation.DeleteRelationship(relationshipEntity);
                _unitOfWork.Save();
            }
            else
            //Setting relationship ending from personEntity
            if (relationship.RelatedToId == 0)
            {
                relationship.RelatedToId = personEntity.Id;
                if (_unitOfWork.Person.GetPerson(relationship.RelatedFromId, false) == null)
                {
                    _loggerManager.LogError("Person does not exist for relationship creation");
                    return false;
                }

                var relationshipEntity = _unitOfWork.PersonRelation.GetRelationship(relationship.RelatedToId, relationship.RelatedFromId, true);
                if (relationshipEntity == null)
                {
                    _loggerManager.LogError("Relationship already does not exist");
                    return false;
                }
                _unitOfWork.PersonRelation.DeleteRelationship(relationshipEntity);
                _unitOfWork.Save();

            }
            else
            {
                _loggerManager.LogError("Only One parameter should be passed to relationship deletion");
                return false;
            }

            return true;
        }

        //Person with relationships can't be deleted without deleting relationships first.
        //We can add more info about this later
        public bool DeletePerson(int id)
        {
            var person = _unitOfWork.Person.GetPerson(id, false);
            if(person == null)
            {
                _loggerManager.LogInfo($"Person with id: {id} doesn't exist in the database.");
                return false;
            }

            _unitOfWork.Person.DeletePerson(person);
            _unitOfWork.Save();
            return true;
        }

        public IEnumerable<PersonDto> QuickSearch(string term)
        {
            var persons = _unitOfWork.Person.QuickSearch(term,100).ToList();
            List<PersonDto> container = new List<PersonDto>();
            foreach(var p in persons)
            {
                container.Add(GetPerson(p.Id));
            }

            return container;
        }
    }
}
