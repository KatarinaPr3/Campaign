using CampaignService.Models;
using System.Text.RegularExpressions;

namespace CampaignService.Helpers
{
    public class UsernameHelper
    {
        public static string GenerateUsernamePassword(Person p)
        {
            string[] parts = Regex.Split(p.Name.ToLower().Trim(), @"[,\s]+");
            string result = string.Join("_", parts);
            string info = $"{result}{p.ID}";
            return $"{result}{p.ID}";

        }
        public static Agent GenerateAgentWithUsernamePassword(Employee employee)
        {
            string[] parts = Regex.Split(employee.Name.ToLower().Trim(), @"[,\s]+");
            string result = string.Join("_", parts);
            return new Agent
            {
                ID = employee.ID,
                Username = $"{result}_{employee.ID}",
                Password = Constants.Settings.AGENT_PASSWORD
            };
        }
    }
}
