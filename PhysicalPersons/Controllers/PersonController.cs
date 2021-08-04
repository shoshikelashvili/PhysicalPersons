using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Entities;
using Entities.DTOs;

namespace PhysicalPersons.Controllers
{
    [Route("api/persons")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;

        public PersonController(IUnitOfWork unitOfWork, ILoggerManager logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public IActionResult GetPerson(int id)
        {
            try
            {
                var person = _unitOfWork.Person.GetPerson(id, false);
                var city = _unitOfWork.City.GetCityByPerson(person, false);
                var phoneNumbers = _unitOfWork.PhoneNumber.GetPhoneNumbersByPerson(person, false);

                var cityDto = new CityDto()
                {
                    Id = city.Id,
                    Name = city.Name
                };

                var phoneNumbersDtos = from p in phoneNumbers
                                       select new PhoneNumberDto()
                                       {
                                           Id = p.Id,
                                           Type = p.Type,
                                           Number = p.Number
                                       };

                //Self-referencing relation can also be described by MaxDepth property, but creating a relation DTO seemed more elegant, so we're going with it

                var personRelationsFromIds = _unitOfWork.PersonRelation.GetPersonRelationsFrom(person, false).Select(p=> p.RelatedToId).ToList();
                var relatedFromPersons = _unitOfWork.Person.GetPersonsByIds(personRelationsFromIds, false);

                var relatedFromDto = from p in relatedFromPersons
                                     select new RelatedPersonDto
                                     {
                                         Id = p.Id,
                                         Name = p.Name,
                                         LastName = p.LastName,
                                         RelationType = _unitOfWork.PersonRelation.GetRelationType(person.Id, p.Id, false).RelationType
                                     };

                var personRelationsToIds = _unitOfWork.PersonRelation.GetPersonRelationsTo(person, false).Select(p => p.RelatedFromId).ToList();
                var relatedToPersons = _unitOfWork.Person.GetPersonsByIds(personRelationsToIds, false);

                var relatedToDto = from p in relatedToPersons
                                   select new RelatedPersonDto
                                     {
                                         Id = p.Id,
                                         Name = p.Name,
                                         LastName = p.LastName,
                                         RelationType = _unitOfWork.PersonRelation.GetRelationType(p.Id, person.Id, false).RelationType
                                     };

                var personDto = new PersonDto()
                {
                    Id = person.Id,
                    Name = person.Name,
                    LastName = person.LastName,
                    Gender = person.Gender,
                    PersonalNumber = person.PersonalNumber,
                    Birthday = person.Birthday,
                    City = cityDto,
                    Image = person.Image,
                    PhoneNumbers = phoneNumbersDtos,
                    RelatedFrom = relatedFromDto,
                    RelatedTo = relatedToDto
                };
                return Ok(personDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured in the {nameof(GetPerson)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
