using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CampaignService.Enums;

namespace CampaignService.Models
{
    public class Campaign
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? CampaignName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public double Discount { get; set; }
        public CampaignType CampaignType { get; set; }
    }
}
