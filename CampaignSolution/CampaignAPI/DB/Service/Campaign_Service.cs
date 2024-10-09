using CampaignAPI.DB.Interfaces;
using CampaignService.Models;
using Microsoft.EntityFrameworkCore;

namespace CampaignAPI.DB.Service
{
    public class Campaign_Service : ICampaignService
    {
        private readonly EntityService<Campaign> _entityService;

        public Campaign_Service(EntityService<Campaign> entityService)
        {
            _entityService = entityService;
        }

        public async Task<double> GetCampaignDiscount(int id)
        {
            Campaign campaign = await _entityService.GetByIdAsync(id);
            return campaign == null ? 0 : campaign.Discount;
        }

        public async Task<Campaign> GetFirstActiveCampaignAsync()
        {
            var allCampaigns = await _entityService.GetAllAsync();
            var activeCampaign = allCampaigns.FirstOrDefault(c => c.EndDate > DateTime.Now);

            return activeCampaign == null ? null : activeCampaign;
        }

    }
}
