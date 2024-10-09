namespace CampaignService.Models
{
    public class Reward
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public int AgentId { get; set; }
        public int CustomerId { get; set; }
        public DateTime DateIssued { get; set; }
        public bool Used { get; set; } = false;
    }
}
