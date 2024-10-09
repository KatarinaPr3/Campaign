using CampaignAPI.DB.Service;
using CampaignService.Enums;
using CampaignService.Models;
using CampaignService.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CampaignAPI.Controllers
{
    public class Login : Controller
    {
        private readonly CustomUserStore _customUserStore;
        private readonly CustomRoleStore _customRoleStore;
        private readonly AuthorizationService _authService;

        public Login(CustomUserStore customUserStore, CustomRoleStore customRoleStore, AuthorizationService authService)
        {
            _customRoleStore = customRoleStore;
            _customUserStore = customUserStore;
            _authService = authService;
        }

        [Authorize]
        [HttpGet("protected")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult ProtectedEndpoint()
        {
            var username = User.Identity.Name;
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new { message = $"Hello {username}, your role is {roleClaim}." });
        }


        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LoginUser(string username, string password)
        {
            Tuple<Roles, int> result = await _customUserStore.ValidateUserAsync(username, password);
            if (result.Item1 == Roles.NoRole)
            {
                return Unauthorized("Invalid username or password.");
            }
            User u = new()
            {
                Username = username,
                Password = password,
                Id = result.Item2
            };

            var token = _authService.GenerateToken(u, result.Item1);

            var claims = new[]
            {
                new Claim(ClaimTypes.Role, result.Item1.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, "login");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            return Ok( new { message = $"User with role {result.Item1.ToString()} logged in.", token });
        }
    }
}
