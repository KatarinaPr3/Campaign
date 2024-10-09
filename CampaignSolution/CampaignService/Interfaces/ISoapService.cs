using CampaignService.Models;

namespace CampaignService.Interfaces
{
    public interface ISoapService
    {
        Task<List<Person>> GetAllCustomers();
        Task<T> FindPersonById<T>(int id) where T : Person, new();
    }
}
