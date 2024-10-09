using CampaignAPI.DB.Interfaces;
using CampaignAPI.DB.Service;
using CampaignService.Constants;
using CampaignService.Enums;
using CampaignService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CampaignAPI.Controllers
{
    public class RewardController : Controller
    {
        private readonly IEntityService<Reward> _rewardService;
        private readonly IEntityService<Campaign> _campaignService;
        private readonly ICampaignService _customCampaignService;

        public RewardController(IEntityService<Reward> rewardService, IEntityService<Campaign> campaignService, ICampaignService customCampaignService)
        {
            _rewardService = rewardService;
            _campaignService = campaignService;
            _customCampaignService = customCampaignService;
        }
        [HttpGet("get_all_rewards", Name = "GetAllRewards")]
        [Authorize(Roles = nameof(Roles.Agent))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Reward>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

        public async Task<ActionResult<List<Reward>>> GetAllRewards()
        {
            IEnumerable<Reward> rewards = await _rewardService.GetAllAsync();
            return rewards.ToList().Any() ? Ok(rewards) : BadRequest();
        }

        [HttpGet("get_all_rewards_by_agent_id/{id:int}", Name = "GetAllRewardsByAgent")]
        [Authorize(Roles = nameof(Roles.Agent))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Reward>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

        public async Task<ActionResult<List<Reward>>> GetAllRewardsByAgent(int id)
        {
            IEnumerable<Reward> rewards = await _rewardService.GetAllAsync();

            var filteredRewards = rewards.Where(reward => reward.AgentId == id).ToList();

            if (!filteredRewards.Any())
            {
                return NotFound($"No rewards found for Agent ID {id}.");
            }

            return Ok(filteredRewards);
        }


        [HttpGet("get_all_rewards_by_customer_id/{id:int}", Name = "GetAllRewardsByCustomer")]
        [Authorize(Roles = nameof(Roles.Agent))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Reward>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<List<Reward>>> GetAllRewardsByCustomer(int id)
        {
            try
            {
                IEnumerable<Reward> rewards = await _rewardService.GetAllAsync();

                var filteredRewards = rewards.Where(reward => reward.CustomerId == id).ToList();

                if (!filteredRewards.Any())
                {
                    return NotFound($"No rewards found for Customer ID {id}.");
                }

                return Ok(filteredRewards);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving rewards.");
            }
        }


        [HttpGet("get_all_unused_rewards", Name = "GetAllUnusedRewards")]
        [Authorize(Roles = nameof(Roles.Agent))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Reward>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<List<Reward>>> GetAllUnusedRewards()
        {
            try
            {
                IEnumerable<Reward> rewards = await _rewardService.GetAllAsync();

                var unusedRewards = rewards.Where(reward => !reward.Used).ToList();

                if (!unusedRewards.Any())
                {
                    return NoContent();
                }

                return Ok(unusedRewards);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving unused rewards.");
            }
        }



        [HttpPost("add_reward", Name = "CreateReward")]
        [Authorize(Roles = nameof(Roles.Agent))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Reward))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Reward>> CreateReward([FromBody] Reward reward)
        {
            int userId = 0;

            var nameIdentifierClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

            if (nameIdentifierClaim != null && int.TryParse(nameIdentifierClaim.Value, out userId))
            {
                // Validate that the logged-in user's ID matches the reward's AgentId
                
                if (reward is null || reward.AgentId != userId)
                {
                    return BadRequest($"You are attempting to add a reward with Agent ID {reward.AgentId} while logged in as Agent ID {userId}.");
                }
                
                if (reward.CustomerId < Settings.CUSTOMER_ID_MIN || reward.CustomerId > Settings.CUSTOMER_ID_MAX)
                {
                    return BadRequest($"Customer id must be between {Settings.CUSTOMER_ID_MIN} - {Settings.CUSTOMER_ID_MIN}.");
                }
                // Check if the issued date of the reward is within the allowed range
                if (reward.DateIssued >= Settings.GIVING_REWARDS_START_DATE &&
                    reward.DateIssued <= Settings.GIVING_REWARDS_START_DATE.AddDays(7).Date.AddHours(23).AddMinutes(59).AddSeconds(59))
                {
                    return BadRequest($"Reward allocation is possible from {Settings.GIVING_REWARDS_START_DATE} to {Settings.GIVING_REWARDS_START_DATE.AddDays(7).Date.AddHours(23).AddMinutes(59).AddSeconds(59)}.");
                }

                if (reward.DateIssued.Date < DateTime.Now.Date)
                {
                    return BadRequest("The reward date cannot be earlier than today.");
                }

                if (reward.DateIssued.Date > DateTime.Now.Date)
                {
                    return BadRequest("The reward date cannot be later than today.");
                }

                var allRewards = await _rewardService.GetAllAsync();
                List<Reward> rewards = allRewards.Where(_ => _.AgentId == reward.AgentId && _.DateIssued.Date == DateTime.Now.Date).ToList();

                if (rewards.Count > Settings.AGENT_CAMPAIGN_LIMIT_MAX)
                {
                    return BadRequest($"Agent with ID {reward.AgentId} has reached the reward allocation limit, which is {Settings.AGENT_CAMPAIGN_LIMIT_MAX}.");
                }

                await _rewardService.AddAsync(reward);
                return Ok(reward);
            }

            return BadRequest("Failed to retrieve user ID.");
        }


        [HttpGet("get_reward_by_id/{id:int}", Name = "GetRewardById")]
        [Authorize(Roles = nameof(Roles.Agent))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Reward))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Reward>> GetRewardById(int id)
        {
            Reward r = await _rewardService.GetByIdAsync(id);

            if (r == null)
            {
                return NotFound($"Reward with ID {id} not found.");
            }

            return Ok(r);
        }

        [HttpGet("get_reward_by_agent_id_and_date/{id:int}/{date:datetime}", Name = "GetRewardByAgentIdAndDate")]
        [Authorize(Roles = nameof(Roles.Agent))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Reward>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<Reward>> GetRewardByAgentIdAndDate(int id, DateTime date)
        {
            IEnumerable<Reward> rewards = await _rewardService.GetAllAsync();
            var filteredRewards = rewards.Where(_ => _.AgentId == id && _.DateIssued == date).ToList();

            if (!filteredRewards.Any())
            {
                return NotFound($"No rewards found for Agent ID {id} on {date.ToShortDateString()}.");
            }

            return Ok(filteredRewards);
        }
    }
}
