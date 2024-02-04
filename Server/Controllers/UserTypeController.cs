using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using fooddelivary.Server.Models;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace fooddelivary.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]

    public class UserTypeController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserTypeController> _logger;

        public UserTypeController(UserManager<ApplicationUser> userManager, ILogger<UserTypeController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<string>> GetUserType()
        {
            var user = await _userManager.GetUserAsync(User);


            // If the user is not found, log a warning and return an error
            if (user == null)
            {
                _logger.LogWarning("User not found in UserManager");
                return NotFound("User not found");
            }

            // Return the UserType property of the user
            return user.UserType;
        }
    }
}