using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public IActionResult GetPerson()
        {
            try
            {
                var persons = _unitOfWork.Person.GetPerson(1, false);

                return Ok(persons);
            }
            catch(Exception ex)
            {
                _logger.LogError($"An error occured in the {nameof(GetPerson)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
