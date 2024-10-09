using CampaignAPI.DB.Interfaces;
using CampaignService.Constants;
using CampaignService.Enums;
using CampaignService.Models;
using CampaignService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;

namespace CampaignAPI.Controllers
{
    public class CampaignController : Controller
    {
        private readonly IEntityService<Campaign> _entityService;
        private readonly ICampaignService _campaignService;

        public CampaignController(IEntityService<Campaign> entityService, ICampaignService campaignService)
        {
            _entityService = entityService;
            _campaignService = campaignService;
        }


        [HttpGet("get_all_campaigns", Name = "GetAllCampaigns")]
        [ProducesResponseType(typeof(IEnumerable<Campaign>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Campaign>>> GetAllCampaigns()
        {
            var campaigns = await _entityService.GetAllAsync();

            if (campaigns == null || !campaigns.Any())
            {
                return NotFound("No campaigns found.");
            }

            return Ok(campaigns);
        }

        [HttpPost("add_campaign", Name = "CreateCampaign")]
        [ProducesResponseType(typeof(Campaign), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles =nameof(Roles.Director))]
        public async Task<ActionResult<Campaign>> CreateCampaign([FromBody] Campaign campaign)
        {
            if (campaign == null)
            {
                return BadRequest("Campaign data is required.");
            }

            var createdCampaign = await _entityService.AddAsync(campaign);

            if (createdCampaign == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Settings.MESSAGE_STATUS_INTERNAL_SERVER_ERROR);
            }

            return Ok(createdCampaign);
        }

        [HttpGet("get_campaign_by_id/{id:int}", Name = "GetCampaignById")]
        [ProducesResponseType(typeof(Campaign), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<ActionResult<Campaign>> GetCampaignById(int id)
        {
            if (id <= 0) // Proverava da li je ID validan
            {
                return BadRequest("Invalid ID.");
            }

            Campaign campaign = await _entityService.GetByIdAsync(id);

            if (campaign == null)
            {
                return NotFound($"Campaign with ID {id} not found.");
            }

            return Ok(campaign);
        }

    }
}
