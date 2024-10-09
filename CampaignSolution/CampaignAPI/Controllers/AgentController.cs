using CampaignAPI.DB.Interfaces;
using CampaignService.Constants;
using CampaignService.Enums;
using CampaignService.Interfaces;
using CampaignService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampaignAPI.Controllers
{
    public class AgentController : Controller
    {
        private readonly ISoapService _soapService;
        private readonly IEntityService<Agent> _entityService;
        public AgentController(ISoapService soapService, IEntityService<Agent> entityService)
        {
            _soapService = soapService;
            _entityService = entityService;
        }

        [HttpGet("get_all_agents", Name = "GetAllAgents")]
        [ProducesResponseType(typeof(List<Agent>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<Agent>>> GetAllAgents(bool fill_db = true)
        {
            try
            {
                List<Agent> agents = await _soapService.CreateAgentsFromEmployees();

                if (!agents.Any())
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "No agents found. Please try again later.");
                }

                if (fill_db)
                {
                    foreach (var item in agents)
                    {
                        await _entityService.AddAsync(item);
                    }
                }

                if (fill_db)
                {
                    var agentsDb = await _entityService.GetAllAsync();
                    agents = agentsDb.ToList();
                }

                return Ok(agents);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, Settings.MESSAGE_STATUS_INTERNAL_SERVER_ERROR);
            }
        }

        [HttpGet("get_agent/{id:int}", Name = "GetAgentById")]
        [ProducesResponseType(typeof(List<Agent>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = nameof(Roles.Agent))]
        public async Task<ActionResult<Agent>> GetAgentById(int id)
        {
            try
            {
                Agent agent = await _entityService.GetByIdAsync(id);

                return agent != null ? Ok(agent) : BadRequest($"The requested ID is out of the allowed range.Please enter an ID between {Settings.EMPLOYEE_ID_MIN} and {Settings.EMPLOYEE_ID_MAX}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, Settings.MESSAGE_STATUS_INTERNAL_SERVER_ERROR);
            }
        }

    }
}
