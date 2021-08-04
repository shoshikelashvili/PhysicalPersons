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

            var personToReturn = _personsService.CreatePerson(person);
            
            return CreatedAtAction("GetPerson", new { id = personToReturn.Id }, personToReturn);

        }
    }
}
