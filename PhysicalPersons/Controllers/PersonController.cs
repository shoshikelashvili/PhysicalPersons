using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Entities;
using Entities.DTOs;
using AutoMapper;

namespace PhysicalPersons.Controllers
{
    [Route("api/persons")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public PersonController(IUnitOfWork unitOfWork, ILoggerManager logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public IActionResult GetPerson(int id)
        {
            try
            {
                var person = _unitOfWork.Person.GetPerson(id, false);
                person.City = _unitOfWork.City.GetCityByPerson(person, false);
                person.PhoneNumbers = _unitOfWork.PhoneNumber.GetPhoneNumbersByPerson(person, false).ToList();
                
                var personRelationsFromIds = _unitOfWork.PersonRelation.GetPersonRelationsFrom(person, false).Select(p=> p.RelatedToId).ToList();
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
