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
using Entities.DTOs.UpdateDtos;

namespace PhysicalPersons.Controllers
{
    [Route("api/persons")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonsService _personsService;
        private readonly ILoggerManager _logger;

        public PersonController(IPersonsService personsService, ILoggerManager loggerManager)
        {
            _personsService = personsService;
            _logger = loggerManager;
        }

        [HttpGet("{id}")]
        public IActionResult GetPerson(int id)
        {
            var personDto = _personsService.GetPerson(id);
            return Ok(personDto);
        }

        [HttpPost]
        public IActionResult CreatePerson([FromBody] PersonForCreationDto person)
        {
            if (person == null)
            {
                _logger.LogError("PersonForCreationDto object sent from client is null.");
                return BadRequest("PersonForCreationDto object is null");
            }

            if(!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for PersonForCreationDto object");
                return UnprocessableEntity(ModelState);
            }

            var personToReturn = _personsService.CreatePerson(person);

            if(personToReturn == null)
            {
                return NotFound();
            }
            
            return CreatedAtAction("GetPerson", new { id = personToReturn.Id }, personToReturn);
        }

        [HttpPut("{personId}")]
        public IActionResult UpdatePerson(int personId, [FromBody] PersonForUpdateDto person)
        {
            if(person == null)
            {
                _logger.LogError("PersonForUpdateDto object sent from client is null.");
                return BadRequest("PersonForUpdateDto object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for PersonForUpdateDto object");
                return UnprocessableEntity(ModelState);
            }

            var updatedPerson = _personsService.UpdatePerson(personId, person);

            if(updatedPerson == false)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
