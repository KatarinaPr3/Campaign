using CampaignAPI.DB.Interfaces;
using CampaignService.Models;
using Microsoft.EntityFrameworkCore;

namespace CampaignAPI.DB.Service
{
    public class Campaign_Service : ICampaignService
    {
        public double GetCampaignDiscount(Campaign campaign)
        {
            return campaign.Discount;
        }

        public Campaign GetFirstActiveCampaignAsync(IEnumerable<Campaign> campaigns)
        {
            var activeCampaign = campaigns.FirstOrDefault(c => c.EndDate > DateTime.Now);

            return activeCampaign == null ? null : activeCampaign;
        }
    }
}
