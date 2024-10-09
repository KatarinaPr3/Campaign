using CampaignAPI.DB.Interfaces;
using CampaignService.Constants;
using CampaignService.Enums;
using CampaignService.Helpers;
using CampaignService.Interfaces;
using CampaignService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text;

namespace CampaignAPI.Controllers
{
    public class CampaignController : Controller
    {
        private readonly IEntityService<Campaign> _entityService;
        private readonly IEntityService<Reward> _rewardService;

        private readonly ICampaignService _campaignService;
        private readonly ISoapService _soapService;

        public CampaignController(IEntityService<Campaign> entityService, ICampaignService campaignService, IEntityService<Reward> rewardService, ISoapService soapService)
        {
            _entityService = entityService;
            _campaignService = campaignService;
            _rewardService = rewardService;
            _soapService = soapService;
        }


        [HttpGet("get_all_campaigns", Name = "GetAllCampaigns")]
        [ProducesResponseType(typeof(IEnumerable<Campaign>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<ActionResult<Campaign>> GetCampaignById(int id)
        {
            if (id <= 0)
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

        [HttpGet("export/{campaignId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        [Authorize(Roles = nameof(Roles.Agent))]
        public async Task<IActionResult> ExportPurchasesToCsv(int campaignId)
        {
            try
            {
                Campaign c = await _entityService.GetByIdAsync(campaignId);

                if (c == null)
                {
                    return NotFound($"Campaign with ID {campaignId} not found.");
                }

                if (c.EndDate > DateTime.Now.Date)
                {
                    return BadRequest("The campaign has not finished yet.");
                }

                if (c.EndDate > DateTime.Now.Date.AddMonths(-1))
                {
                    return BadRequest("The campaign has already ended, and reports cannot be exported within one month after the campaign's end date.");
                }


                // Retrieve  rewards
                var allRewards = await _rewardService.GetAllAsync();
                var allRewardsByCampaignId = allRewards.Where(_ => _.CampaignId == campaignId).ToList();
                var allUsedRewardsByCampaignId = allRewards.Where(_ => _.CampaignId == campaignId && _.Used).ToList();

                List<Customer> customers = new();
                var rewards = allRewards.Where(_ => _.Used).ToList();

                foreach (var item in rewards)
                {
                    var customer = await _soapService.FindPersonById<Customer>(item.CustomerId);
                    if (customer != null)
                    {
                        customers.Add(customer);
                    }
                }

                // Generate CSV content
                StringBuilder csvData = CampaignHelper.GenerateCSVContent(c, allRewardsByCampaignId, allUsedRewardsByCampaignId, customers);

                return File(Encoding.UTF8.GetBytes(csvData.ToString()), "text/csv", $"campaign_id_{campaignId}_purchases.csv");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while exporting purchases to CSV. Please try again later.");
            }
        }


    }
}
