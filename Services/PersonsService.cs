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
using Entities.Parameters;
using Entities.Responses;
using Microsoft.Extensions.Localization;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;
        private IHostingEnvironment _env;
        private readonly IStringLocalizer<PersonsService> _stringLocalizer;

        public PersonsService(IUnitOfWork unitOfWork, IMapper mapper, ILoggerManager loggerManager, IHostingEnvironment env, IStringLocalizer<PersonsService> stringLocalizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggerManager = loggerManager;
            _env = env;
            _stringLocalizer = stringLocalizer;
        }
        public PersonResponse GetPerson(int id) 
        {
            var person = _unitOfWork.Person.GetPerson(id, false);
            if(person == null)
            {
                _loggerManager.LogError($"Person with id {id} does not exist");
                return new PersonResponse(_stringLocalizer["Person with the sent ID does not exist"].Value);
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


            var personDto = new PersonResponse(_mapper.Map<PersonDto>(person, opt => opt.AfterMap((src, dest) =>
            {
                dest.RelatedFrom = relatedFromDto;
                dest.RelatedTo = relatedToDto;
            })));

            return personDto;
        }

        //TODO: Adjust the logic here so that cities can be assigned by ID instead of creating a new one
        public PersonResponse CreatePerson(PersonForCreationDto person)
        {
            if(person == null)
            {
                _loggerManager.LogError("PersonForCreationDto object sent from client is null.");
                return new PersonResponse(_stringLocalizer["PersonForCreationDto object sent from client is null."].Value);
            }

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
                        return new PersonResponse(_stringLocalizer["One or more persons from the relationship array do not exist"].Value);
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
                        return new PersonResponse(_stringLocalizer["One or more persons from the relationship array do not exist"].Value);
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

            
            return new PersonResponse(_mapper.Map<PersonDto>(personEntity, opt => opt.AfterMap((src, dest) =>
            {
                dest.RelatedFrom = relatedFromDto;
                dest.RelatedTo = relatedToDto;
            })));
        }

        public BaseResponse UpdatePerson(int personId, PersonForUpdateDto person)
        {
            var personEntity = _unitOfWork.Person.GetPerson(personId, true);
            if(personEntity == null)
            {
                _loggerManager.LogInfo($"Person with id: {personId} doesn't exist in the DB");
                return new BaseResponse(false, _stringLocalizer["Person with the sent ID does not exist"].Value);
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
                            return new BaseResponse(false, _stringLocalizer["One of the phonenumber entities does not exist in the database"].Value);
                        }
                        if (phoneNumber.PersonId != personId)
                        {
                            _loggerManager.LogInfo($"Phone with id: {p.Id} doesn't belong to current user");
                            return new BaseResponse(false, _stringLocalizer["One of the phonenumber entities does not exist to the given user"].Value);
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

            return new BaseResponse(true, _stringLocalizer["User Updated Succesfully"].Value); ;
        }

        public BaseResponse SetImage(int personId, ImageForUpdateDto image)
        {
            if(image.ImageUrl == null)
            {
                return new BaseResponse(false, _stringLocalizer["Url sent from the client is null"].Value);
            }

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

            return new BaseResponse(true, _stringLocalizer["Image Updated Succesfully"].Value);
        }

        public BaseResponse CreateRelationship(int personId, RelatedPersonForCreationDto relationship)
        {
            if (relationship == null)
            {
                _loggerManager.LogError("RelatedPersonForCreationDto object sent from client is null.");
                return new BaseResponse(false, _stringLocalizer["RelatedPersonForCreationDto object sent from client is null"].Value);
            }

            var personEntity = _unitOfWork.Person.GetPerson(personId,false);
            if(personEntity == null)
            {
                _loggerManager.LogError("Person does not exist for relationship creation");
                return new BaseResponse(false, _stringLocalizer["Person does not exist for relationship creation"].Value);
            }

            //Setting relationship starting from personEntity
            if(relationship.RelatedFromId == 0)
            {
                relationship.RelatedFromId = personEntity.Id;
                if(_unitOfWork.Person.GetPerson(relationship.RelatedToId,false) == null)
                {
                    _loggerManager.LogError("Related Person does not exist for relationship creation");
                    return new BaseResponse(false, _stringLocalizer["Person does not exist for relationship creation"].Value);
                }
            }
            else
            //Setting relationship ending from personEntity
            if (relationship.RelatedToId == 0)
            {
                relationship.RelatedToId = personEntity.Id;
                if (_unitOfWork.Person.GetPerson(relationship.RelatedFromId, false) == null)
                {
                    _loggerManager.LogError("Related Person does not exist for relationship creation");
                    return new BaseResponse(false, _stringLocalizer["Person does not exist for relationship creation"].Value);
                }

            }
            else
            {
                _loggerManager.LogError("Only One parameter should be passed to relationship creation");
                return new BaseResponse(false, _stringLocalizer["Only One parameter should be passed to relationship creation"].Value);
            }
            var relationEntity = _mapper.Map<PersonRelation>(relationship);

            _unitOfWork.PersonRelation.AddRelation(relationEntity);
            _unitOfWork.Save();

            return new BaseResponse(true, _stringLocalizer["Relationship Added Succesfully"].Value);
        }

        public BaseResponse DeleteRelationship(int personId, RelatedPersonForDeletionDto relationship)
        {
            if (relationship == null)
            {
                _loggerManager.LogError("RelatedPersonForCreationDto object sent from client is null.");
                return new BaseResponse(false, _stringLocalizer["RelatedPersonForCreationDto object sent from client is null"].Value);
            }

            var personEntity = _unitOfWork.Person.GetPerson(personId, false);
            if (personEntity == null)
            {
                _loggerManager.LogError("Person does not exist for relationship creation");
                return new BaseResponse(false, _stringLocalizer["Person does not exist for relationship creation"].Value);
            }

            //Setting relationship starting from personEntity
            if (relationship.RelatedFromId == 0)
            {
                relationship.RelatedFromId = personEntity.Id;
                if (_unitOfWork.Person.GetPerson(relationship.RelatedToId, false) == null)
                {
                    _loggerManager.LogError("Person does not exist for relationship creation");
                    return new BaseResponse(false, _stringLocalizer["Related person does not exist for relationship creation"].Value);
                }

                var relationshipEntity = _unitOfWork.PersonRelation.GetRelationship(relationship.RelatedFromId, relationship.RelatedToId, true);
                if(relationshipEntity == null)
                {
                    _loggerManager.LogError("Relationship already does not exist");
                    return new BaseResponse(false, _stringLocalizer["Relationship already does not exist"].Value);
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
                    return new BaseResponse(false, _stringLocalizer["Related Person does not exist for relationship creation"].Value);
                }

                var relationshipEntity = _unitOfWork.PersonRelation.GetRelationship(relationship.RelatedToId, relationship.RelatedFromId, true);
                if (relationshipEntity == null)
                {
                    _loggerManager.LogError("Relationship already does not exist");
                    return new BaseResponse(false, _stringLocalizer["Relationship already does not exist"].Value);
                }
                _unitOfWork.PersonRelation.DeleteRelationship(relationshipEntity);
                _unitOfWork.Save();

            }
            else
            {
                _loggerManager.LogError("Only One parameter should be passed to relationship deletion");
                return new BaseResponse(false, _stringLocalizer["Only One parameter should be passed to relationship deletion"].Value);
            }

            return new BaseResponse(true, _stringLocalizer["Relationship Deleted Succesfully"].Value);
        }

        //Person with relationships can't be deleted without deleting relationships first.
        //We can add more info about this later
        public BaseResponse DeletePerson(int id)
        {
            var person = _unitOfWork.Person.GetPerson(id, false);
            if(person == null)
            {
                _loggerManager.LogInfo($"Person with id: {id} doesn't exist in the database.");
                return new BaseResponse(false, _stringLocalizer["Person with the sent ID does not exist"].Value);
            }

            _unitOfWork.Person.DeletePerson(person);
            _unitOfWork.Save();
            return new BaseResponse(true, _stringLocalizer["Person deleted succesfully"].Value);
        }

        public PersonCollectionResponse QuickSearch(string term)
        {
            var persons = _unitOfWork.Person.QuickSearch(term,100).ToList();
            List<PersonDto> container = new List<PersonDto>();
            foreach(var p in persons)
            {
                container.Add(GetPerson(p.Id).PersonDto);
            }

            return new PersonCollectionResponse(container);
        }

        public PersonCollectionResponse Search(PersonParameters personParameters)
        {
            var persons = _unitOfWork.Person.Search(personParameters, 10).ToList();
            List<PersonDto> container = new List<PersonDto>();
            foreach (var p in persons)
            {
                container.Add(GetPerson(p.Id).PersonDto);
            }

            return new PersonCollectionResponse(container);
        }

        public RelationshipStatsResponse GetRelationshipStats(int personId)
        {
            var person = _unitOfWork.Person.GetPerson(personId, false);
            if(person == null)
            {
                _loggerManager.LogInfo($"Person with id: {personId} doesn't exist in the database.");
                return new RelationshipStatsResponse(_stringLocalizer["Person with the sent ID does not exist"].Value);
            }

            Dictionary<string, int> result = new Dictionary<string, int>();

            var relationShipsFrom = _unitOfWork.PersonRelation.GetPersonRelationsFrom(person, false);
            var relationShipsTo = _unitOfWork.PersonRelation.GetPersonRelationsTo(person, false);
            //კოლეგა|ნაცნობი|ნათესავი|სხვა
            result.Add("კოლეგა", relationShipsFrom.Where(x => x.RelationType.Equals("კოლეგა")).Count() + relationShipsTo.Where(x => x.RelationType.Equals("კოლეგა")).Count());
            result.Add("ნაცნობი", relationShipsFrom.Where(x => x.RelationType.Equals("ნაცნობი")).Count() + relationShipsTo.Where(x => x.RelationType.Equals("ნაცნობი")).Count());
            result.Add("ნათესავი", relationShipsFrom.Where(x => x.RelationType.Equals("ნათესავი")).Count() + relationShipsTo.Where(x => x.RelationType.Equals("ნათესავი")).Count());
            result.Add("სხვა", relationShipsFrom.Where(x => x.RelationType.Equals("სხვა")).Count() + relationShipsTo.Where(x => x.RelationType.Equals("სხვა")).Count());

            return new RelationshipStatsResponse(result);
        }
    }
}
