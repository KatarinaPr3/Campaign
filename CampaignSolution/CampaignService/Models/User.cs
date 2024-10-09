using Microsoft.AspNetCore.Identity;

namespace CampaignService.Models
{
    public class User : IdentityUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
