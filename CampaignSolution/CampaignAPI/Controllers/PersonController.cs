using CampaignService.Models;
using Microsoft.AspNetCore.Mvc;
using CampaignService.Interfaces;
using CampaignService.Constants;
using Microsoft.AspNetCore.Authorization;
using CampaignService.Enums;

namespace CampaignAPI.Controllers
{
    public class PersonController : Controller
    {
        private readonly ISoapService _soapService;
        public PersonController(ISoapService soapService)
        {
            _soapService = soapService;
        }

        [HttpGet("get_customer/{id:int}", Name = "GetCustomer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = nameof(Roles.Agent))]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            if (id < Settings.CUSTOMER_ID_MIN || id > Settings.CUSTOMER_ID_MAX)
            {
                return BadRequest($"The requested ID is out of the allowed range. Please enter an ID between {Settings.CUSTOMER_ID_MIN} and {Settings.CUSTOMER_ID_MAX}.");
            }

            var customer = await _soapService.FindPersonById<Customer>(id);

            if (customer == null)
            {
                return NotFound($"Customer with ID {id} not found.");
            }

            return Ok(customer);
        }


        [HttpGet("get_people", Name = "GetPeople")]
        [ProducesResponseType(typeof(List<Person>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(Roles.Agent))]
        public async Task<ActionResult<List<Person>>> GetPeople()
        {
            try
            {
                List<Person> people = await _soapService.GetPeople();

                if (people == null || !people.Any())
                {
                    return NotFound("There are no available data for people.");
                }

                return Ok(people);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

    }
}
