using Microsoft.AspNetCore.Identity;

namespace CampaignService.Models
{
    public class Role : IdentityRole
    {
        public string RoleName { get; set; }
    }
}
