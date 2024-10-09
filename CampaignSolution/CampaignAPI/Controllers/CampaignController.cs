using CampaignAPI.DB.Interfaces;
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
        public async Task<ActionResult<IEnumerable<Campaign>>> GetAllAA()
        {
            var campaigns = await _entityService.GetAllAsync();
            return campaigns.Any() 
                ? StatusCode(StatusCodes.Status200OK, campaigns) 
                : StatusCode((int)StatusCodes.Status500InternalServerError, "We're experiencing technical difficulties. Please try again later or contact support if the issue persists.");
        }
        [HttpPost("add_campaign", Name = "CreateCampaign")]
        public async Task<ActionResult<Campaign>> CreateCampaign([FromBody] Campaign campaign)
        {
            campaign = await _entityService.AddAsync(campaign);
            return campaign != null
                ? Ok(campaign)
                : StatusCode(StatusCodes.Status500InternalServerError, "We're experiencing technical difficulties. Please try again later or contact support if the issue persists.");
        }
        [HttpGet("get_campaign_by_id/{id:int}", Name = "GetCampaignById")]
        public async Task<ActionResult<Campaign>> GetCampaignById(int id)
        {
            Campaign campaign = await _entityService.GetByIdAsync(id);
            return campaign != null
                           ? Ok(campaign)
                           : StatusCode(StatusCodes.Status500InternalServerError, "We're experiencing technical difficulties. Please try again later or contact support if the issue persists.");
        }
    }
}
