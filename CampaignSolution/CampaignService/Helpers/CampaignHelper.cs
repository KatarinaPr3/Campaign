using CampaignService.Models;
using System.Text;

namespace CampaignService.Helpers
{
    public class CampaignHelper
    {
        public static StringBuilder GenerateCSVContent(Campaign c, List<Reward> allRewards, List<Reward> allUsedRewardsByCampaignId, List<Customer> customers)
        {
            StringBuilder csvData = new StringBuilder();
            csvData.AppendLine($"Campaign: {c.CampaignName}");
            csvData.AppendLine($"Campaign Type: {c.CampaignType.ToString()}");
            csvData.AppendLine($"Campaign Discount: {c.Discount}");
            csvData.AppendLine($"Campaign Start Date: {c.StartDate}");
            csvData.AppendLine($"Campaign End Date: {c.EndDate}");

            csvData.AppendLine($"Given Rewards: {allRewards.Count()}");
            csvData.AppendLine($"Used Rewards: {allUsedRewardsByCampaignId.Count()}");

            csvData.AppendLine("Customers who made purchase with given discount");

            csvData.AppendLine("Id,Name,SSN");

            foreach (var customer in customers)
            {
                csvData.AppendLine($"{customer.ID},{customer.Name},{customer.SSN}");
            }
            return csvData;
        }
    }
}
