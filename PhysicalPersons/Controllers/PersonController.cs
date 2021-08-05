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
using Microsoft.Extensions.Localization;

namespace PhysicalPersons.Controllers
{
    [Route("api/persons")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonsService _personsService;
        private readonly ILoggerManager _logger;
        private readonly IStringLocalizer<PersonController> _stringLocalizer;
        public PersonController(IPersonsService personsService, ILoggerManager loggerManager, IStringLocalizer<PersonController> stringLocalizer)
        {
            _personsService = personsService;
            _logger = loggerManager;
            _stringLocalizer = stringLocalizer;
        }

        [HttpGet("{id}")]
        public IActionResult GetPerson(int id)
        {
            var result = _personsService.GetPerson(id);

            if(!result.Success)
            {
                return NotFound(result.Message);
            }

            return Ok(result.PersonDto);
        }

        [HttpPost]
        public IActionResult CreatePerson([FromBody] PersonForCreationDto person)
        {
            var result = _personsService.CreatePerson(person);

            if(!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction("GetPerson", new { id = result.PersonDto.Id }, result.PersonDto);
        }

        [HttpDelete("{personId}")]
        public IActionResult DeletePerson(int personId)
        {
            var result = _personsService.DeletePerson(personId);

            if(!result.Success)
            {
                return BadRequest(result.Message);
            }

            return NoContent();
        }

        [HttpPut("{personId}")]
        public IActionResult UpdatePerson(int personId, [FromBody] PersonForUpdateDto person)
        {
            var result = _personsService.UpdatePerson(personId, person);

            if(!result.Success)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpPut("{personId}/image")]
        public IActionResult SetImage(int personId, [FromBody] ImageForUpdateDto image)
        {
            var result = _personsService.SetImage(personId,image);
            if(!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpPost("{personId}/related")]
        public IActionResult CreatePersonRelation(int personId,[FromBody] RelatedPersonForCreationDto relation)
        {
            var result = _personsService.CreateRelationship(personId, relation);

            if(!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpDelete("{personId}/related")]
        public IActionResult DeletePersonRelation(int personId, [FromBody] RelatedPersonForDeletionDto relation)
        {
            var result = _personsService.DeleteRelationship(personId, relation);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return NoContent();
        }

        [HttpGet("quicksearch/{term}")]
        public IActionResult QuickSearchPersons(string term)
        {
            var result = _personsService.QuickSearch(term);
            if(!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.PersonDtoCollection);
        }

        [HttpGet("search")]
        public IActionResult SearchPersons([FromQuery] PersonParameters personParameters)
        {
           
            var result = _personsService.Search(personParameters);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.PersonDtoCollection);
        }

        [HttpGet("{personId}/relationships")]
        public IActionResult GetPersonRelationships(int personId)
        {
            var result = _personsService.GetRelationshipStats(personId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.RelationShipStats);
        }
    }
}
