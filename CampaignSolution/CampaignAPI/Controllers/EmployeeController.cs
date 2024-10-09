using CampaignService.Constants;
using CampaignService.Enums;
using CampaignService.Interfaces;
using CampaignService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampaignAPI.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ISoapService _soapService;

        public EmployeeController(ISoapService soapService)
        {
            _soapService = soapService;
        }

        [HttpGet("get_employee/{id:int}", Name = "GetEmployeeById")]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = nameof(Roles.Agent))]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            if (id < Settings.EMPLOYEE_ID_MIN || id > Settings.EMPLOYEE_ID_MAX)
            {
                return BadRequest($"The requested ID is out of the allowed range. Please enter an ID between {Settings.EMPLOYEE_ID_MIN} and {Settings.EMPLOYEE_ID_MAX}.");
            }

            var employee = await _soapService.FindPersonById<Employee>(id);

            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }

            return Ok(employee);
        }

    }
}
