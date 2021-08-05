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
using Entities.DTOs.CreationDtos;
using Entities.DTOs.DeletionDtos;
using Entities.Parameters;

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
            if(personDto == null)
            {
                return NotFound();
            }
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

            if (personToReturn == null)
            {
                return NotFound();
            }

            return CreatedAtAction("GetPerson", new { id = personToReturn.Id }, personToReturn);
        }

        [HttpDelete("{personId}")]
        public IActionResult DeletePerson(int personId)
        {
            _personsService.DeletePerson(personId);
            return NoContent();
        }

        [HttpPut("{personId}")]
        public IActionResult UpdatePerson(int personId, [FromBody] PersonForUpdateDto person)
        {
            if(person == null)
            {
                _logger.LogError("PersonForUpdateDto object sent from client is null.");
                return BadRequest("PersonForUpdateDto object is null");
            }

            var updatedPerson = _personsService.UpdatePerson(personId, person);

            if(updatedPerson == false)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("{personId}/image")]
        public IActionResult SetImage(int personId, [FromBody] ImageForUpdateDto image)
        {
            if(image == null)
            {
                _logger.LogError("image sent from client is null.");
                return BadRequest("image is null");
            }

            _personsService.SetImage(personId,image);

            return NoContent();
        }

        [HttpPost("{personId}/related")]
        public IActionResult CreatePersonRelation(int personId,[FromBody] RelatedPersonForCreationDto relation)
        {
            if (relation == null)
            {
                _logger.LogError("RelatedPersonForCreationDto object sent from client is null.");
                return BadRequest("RelatedPersonForCreationDto object is null");
            }

            var personToReturn = _personsService.CreateRelationship(personId, relation);

            if (personToReturn == false)
            {
                return NotFound();
            }

            return Ok("Relation created succesfully");
        }

        [HttpDelete("{personId}/related")]
        public IActionResult DeletePersonRelation(int personId, [FromBody] RelatedPersonForDeletionDto relation)
        {
            if (relation == null)
            {
                _logger.LogError("RelatedPersonForDeletionDto object sent from client is null.");
                return BadRequest("RelatedPersonForDeletionDto object is null");
            }

            var personToReturn = _personsService.DeleteRelationship(personId, relation);

            if (personToReturn == false)
            {
                return NotFound();
            }

            return Ok("Relation deleted succesfully");
        }

        [HttpGet("quicksearch/{term}")]
        public IActionResult QuickSearchPersons(string term)
        {
            var result = _personsService.QuickSearch(term);
            return Ok(result);
        }

        [HttpGet("search")]
        public IActionResult SearchPersons([FromQuery] PersonParameters personParameters)
        {
           
            var result = _personsService.Search(personParameters);
            return Ok(result);
        }

        [HttpGet("{personId}/relationships")]
        public IActionResult GetPersonRelationships(int personId)
        {
            var result = _personsService.GetRelationshipStats(personId);
            return Ok(result);
        }
    }
}
