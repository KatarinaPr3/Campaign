using CampaignService.Models;
using System.Text.RegularExpressions;

namespace CampaignService.Interfaces
{
    public interface ISoapService
    {
        Task<List<Person>> GetPeople();
        Task<T> FindPersonById<T>(int id) where T : Person, new();

        Task<List<Employee>> GetAllEmployeesRoleAgent();

        Task<List<Agent>> CreateAgentsFromEmployees();
        Task<List<Person>> GetAllCustomers();
    }
}
