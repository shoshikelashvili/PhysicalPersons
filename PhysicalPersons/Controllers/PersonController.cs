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
using Entities.Models;

namespace PhysicalPersons.Controllers
{
    [Route("api/persons")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonsService _personsService;

        public PersonController(IPersonsService personsService)
        {
            _personsService = personsService;
        }

        [HttpGet("{id}")]
        public IActionResult GetPerson(int id)
        {
            var personDto = _personsService.GetPerson(id);
            return Ok(personDto);
        }

        //[HttpPost]
        //public IActionResult CreatePerson([FromBody] PersonForCreationDto person)
        //{
        //    if (person == null)
        //    {
        //        _logger.LogError("PersonForCreationDto object sent from client is null.");
        //        return BadRequest("PersonForCreationDto object is null");
        //    }

        //    var personEntity = _mapper.Map<Person>(person);
        //    _unitOfWork.Person.CreatePerson(personEntity);
        //    _unitOfWork.Save();
        //    var personToReturn = _mapper.Map<PersonDto>(personEntity);
        //    return CreatedAtAction("GetPerson", new { id = personToReturn.Id }, personToReturn);

        //}
    }
}
