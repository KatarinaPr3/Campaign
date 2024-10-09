using CampaignService.Models;
using Microsoft.EntityFrameworkCore;

namespace CampaignAPI.DB.Interfaces
{
    public interface ICampaignService
    {
        Task<Campaign> GetFirstActiveCampaignAsync();
        Task<double> GetCampaignDiscount(int id);
    }
}
