using CampaignAPI.DB.Interfaces;
using CampaignAPI.DB.Service;
using CampaignService.Constants;
using CampaignService.Interfaces;
using CampaignService.Models;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<List<Agent>>> GetAllAgents(bool fill_db = true)
        {
            try
            {
                List<Agent> agents = await _soapService.CreateAgentsFromEmployees();
                if (agents.Any())
                {
                    // Should feel db with agent data 
                    if (fill_db)
                    {
                        foreach (var item in agents)
                        {
                            await _entityService.AddAsync(item);
                        }
                    }
                }
                if (fill_db)
                {
                    var agentsDb = await _entityService.GetAllAsync();
                    agents = agents.ToList();
                }
                return agents.Any() ? StatusCode(StatusCodes.Status200OK, agents) : StatusCode((int)StatusCodes.Status500InternalServerError, "We're experiencing technical difficulties. Please try again later or contact support if the issue persists.");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error {ex}");
            }
            return NoContent();

            
        }

    }
}
