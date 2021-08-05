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
    [ApiExplorerSettings(GroupName = "v1")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonsService _personsService;
        public PersonController(IPersonsService personsService)
        {
            _personsService = personsService;
        }

        /// <summary>
        /// Get a physical person
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A single Physical Person</returns>
        /// <response code="404">If a person doesn't exist</response>
        /// <response code="200">Returns the person data</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetPerson(int id)
        {
            var result = _personsService.GetPerson(id);

            if(!result.Success)
            {
                return NotFound(result.Message);
            }

            return Ok(result.PersonDto);
        }

        /// <summary>
        /// Create a physical person
        /// </summary>
        /// <param name="person"></param>
        /// <returns>Created person data</returns>
        /// <response code="201">Person created successfully</response>
        /// <response code="400">Passed model is invalid</response>
        
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult CreatePerson([FromBody] PersonForCreationDto person)
        {
            var result = _personsService.CreatePerson(person);

            if(!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction("GetPerson", new { id = result.PersonDto.Id }, result.PersonDto);
        }

        /// <summary>
        /// Deletes a person
        /// </summary>
        /// <param name="personId"></param>
        /// <returns>No Content</returns>
        /// <response code="204">Person deleted successfully</response>
        /// <response code="400">Passed model is invalid</response>
        
        [HttpDelete("{personId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult DeletePerson(int personId)
        {
            var result = _personsService.DeletePerson(personId);

            if(!result.Success)
            {
                return BadRequest(result.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// Update a physical person
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="person"></param>
        /// <returns>Details about update</returns>
        /// <response code="200">Person updated successfully</response>
        /// <response code="404">Person not found</response>
        [HttpPut("{personId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePerson(int personId, [FromBody] PersonForUpdateDto person)
        {
            var result = _personsService.UpdatePerson(personId, person);

            if(!result.Success)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Message);
        }

        /// <summary>
        /// Sets or updates Person image
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="image"></param>
        /// <returns>Update status message</returns>
        /// <response code="200">Person image updated successfully</response>
        /// <response code="400">Passed Model is invalid</response>
        [HttpPut("{personId}/image")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult SetImage(int personId, [FromBody] ImageForUpdateDto image)
        {
            var result = _personsService.SetImage(personId,image);
            if(!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        /// <summary>
        /// Create a relationship for a person
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="relation"></param>
        /// <returns>Relationship creation status</returns>
        /// <response code="200">Person relation added successfully</response>
        /// <response code="400">Passed Model is invalid</response>
        [HttpPost("{personId}/related")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreatePersonRelation(int personId,[FromBody] RelatedPersonForCreationDto relation)
        {
            var result = _personsService.CreateRelationship(personId, relation);

            if(!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        /// <summary>
        /// Delete a relationship from a person
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="relation"></param>
        /// <returns>Relationship deletion status</returns>
        /// <response code="204">Person relation deleted successfully</response>
        /// <response code="400">Passed Model is invalid</response>
        [HttpDelete("{personId}/related")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult DeletePersonRelation(int personId, [FromBody] RelatedPersonForDeletionDto relation)
        {
            var result = _personsService.DeleteRelationship(personId, relation);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// Quick search persons
        /// </summary>
        /// <param name="term"></param>
        /// <returns>Searched Person Collection</returns>
        /// <response code="200">Search completed successfully</response>
        /// <response code="400">Passed Model is invalid</response>
        [HttpGet("quicksearch/{term}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult QuickSearchPersons(string term)
        {
            var result = _personsService.QuickSearch(term);
            if(!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.PersonDtoCollection);
        }

        /// <summary>
        /// Detailed search for persons
        /// </summary>
        /// <param name="personParameters"></param>
        /// <returns>Searched Person Collection with pagination possibility</returns>
        /// <response code="200">Search completed successfully</response>
        /// <response code="400">Passed Model is invalid</response>
        [HttpGet("search")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult SearchPersons([FromQuery] PersonParameters personParameters)
        {
           
            var result = _personsService.Search(personParameters);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.PersonDtoCollection);
        }

        /// <summary>
        /// Get Person relationship statistics
        /// </summary>
        /// <param name="personId"></param>
        /// <returns>Stats about relationship types and amounts</returns>
        /// <response code="200">Stats generated successfully</response>
        /// <response code="400">Passed Model is invalid</response>
        [HttpGet("{personId}/relationships")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
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
