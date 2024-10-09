using CampaignService.Constants;
using CampaignService.Interfaces;
using CampaignService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CampaignAPI.Controllers
{
    public class AgentController : Controller
    {
        private readonly ISoapService _soapService;
        public AgentController(ISoapService soapService)
        {
            _soapService = soapService;
        }

        [HttpGet("get_all_agents", Name = "GetAllAgents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<List<Agent>>> GetAllAgents(bool fill_db = true)
        {
            List<Agent> agents = await _soapService.CreateAgentsFromEmployees();
            if (agents.Any())
            {
                // Should feel db with agent data 
                //if (fill_db)
                //{
                //}
            }

            return agents.Any() ? StatusCode(StatusCodes.Status200OK, agents) : StatusCode((int)StatusCodes.Status500InternalServerError, "We're experiencing technical difficulties. Please try again later or contact support if the issue persists.");
        }
        [HttpGet("get_customer/{id:int}", Name = "GetCustomer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            if (id >= Settings.CUSTOMER_ID_MIN && id <= Settings.CUSTOMER_ID_MAX)
            {
                var customer = await _soapService.FindPersonById<Customer>(id);

                return customer != null ? StatusCode(StatusCodes.Status200OK, (customer)) : StatusCode((int)StatusCodes.Status502BadGateway, "We're experiencing technical difficulties. Please try again later or contact support if the issue persists.");
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, $"The requested ID is out of the allowed range.Please enter an ID between {Settings.CUSTOMER_ID_MIN} and {Settings.CUSTOMER_ID_MAX}.");
            }

        }
    }
}
