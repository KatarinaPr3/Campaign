using CampaignService.Constants;
using CampaignService.Interfaces;
using CampaignService.Models;
using CampaignService.Services;
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
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            if (id >= Settings.EMPLOYEE_ID_MIN && id <= Settings.EMPLOYEE_ID_MAX)
            {
                var employee = await _soapService.FindPersonById<Employee>(id);

                return employee != null ? StatusCode(StatusCodes.Status200OK, employee) : StatusCode(StatusCodes.Status502BadGateway, "We're experiencing technical difficulties. Please try again later or contact support if the issue persists.");
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, $"The requested ID is out of the allowed range.Please enter an ID between {Settings.EMPLOYEE_ID_MIN} and {Settings.EMPLOYEE_ID_MAX}.");
            }

            
        }
    }
}
