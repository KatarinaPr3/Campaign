using CampaignService.Models;

namespace CampaignAPI.DB.Interfaces
{
    public interface ICampaignService
    {
        Campaign GetFirstActiveCampaignAsync(IEnumerable<Campaign> campaigns);
        double GetCampaignDiscount(Campaign campaign);
    }
}
