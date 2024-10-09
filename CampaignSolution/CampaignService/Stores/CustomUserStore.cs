using CampaignService.Enums;
using CampaignService.Helpers;
using CampaignService.Interfaces;
using CampaignService.Models;
using CampaignService.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampaignService.Stores
{

    public class CustomUserStore : IUserStore<User>
    {
        private readonly ISoapService _soapService;
        public CustomUserStore(ISoapService soapService)
        {
            _soapService = soapService;
        }
        public async Task<Tuple<Roles, int>> ValidateUserAsync(string username, string password)
        {

            List<Agent> agents = await _soapService.CreateAgentsFromEmployees();
            Agent agent = agents.SingleOrDefault(_ => _.Username == username && _.Password == password);

            if (agent != null)
            {
                return Tuple.Create(Roles.Agent, agent.ID); // Return Agent role and ID
            }

            List<Person> customers = await _soapService.GetAllCustomers();
            var user = customers.SingleOrDefault(u => UsernameHelper.GenerateUsernamePassword(u) == username);

            if (user != null && password == Constants.Settings.CUSTOMER_PASSWORD)
            {
                return Tuple.Create(Roles.User, user.ID); // Return User role and ID
            }

            return Tuple.Create(Roles.NoRole, 0); // Default for no role
        }


        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User cannot be null." });
            }

            return IdentityResult.Success;

        }

        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            return null;
        }

        public void Dispose()
        {
        }

        public Task<User?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return null;
        }

        public Task<User?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return null;
        }

        public Task<string?> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return null;
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return null;
        }

        public Task<string?> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return null;
        }

        public Task SetNormalizedUserNameAsync(User user, string? normalizedName, CancellationToken cancellationToken)
        {
            return null;
        }

        public Task SetUserNameAsync(User user, string? userName, CancellationToken cancellationToken)
        {
            return null;
        }

        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            return null;
        }
    }


}
