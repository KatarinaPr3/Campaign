using CampaignService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
