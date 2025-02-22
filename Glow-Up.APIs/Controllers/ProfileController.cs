using Glow_Up.Core.DTOs.Account;
using Glow_Up.Core.DTOs.Profile;
using Glow_Up.Core.Services.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Glow_Up.APIs.Controllers
{
    public class ProfileController : ApiBaseController
    {
        private readonly IUserService _userService;

        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut("{userId:int}")]
        public async Task<IActionResult> UpdateProfile(int userId, [FromForm] UpdateProfileDto dto)
        {
            try
            {
                var result = await _userService.UpdateProfileAsync(userId, dto);

                return Ok(new { Message = $"Profile Updated Successfuly" });
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = $"{ex.Message}" });
            }
        }

        [HttpGet("UserProfile/{userId:int}")]
        public async Task<ActionResult<UserProfileDto>> GetUserProfile(int userId)
        {
            try
            {
                var result = await _userService.GetUserProfileAsync(userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = $"{ex.Message}" });
            }
        }
    }
}
