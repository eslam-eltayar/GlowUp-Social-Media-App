using Glow_Up.Core.DTOs.Users;
using Glow_Up.Core.Services.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Glow_Up.APIs.Controllers
{

    public class UsersController : ApiBaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("FollowUser/{followerId:int}/{followeeId:int}")]
        public async Task<IActionResult> FollowUser(int followerId, int followeeId)
        {
            try
            {
                var result = await _userService.FollowUserAsync(followerId, followeeId);

                if (result)
                    return Ok(new { Message = "Followed Successfully" });
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"{ex.Message}" });
            }
        }

        [HttpPost("UnFollowUser/{followerId:int}/{followeeId:int}")]
        public async Task<IActionResult> UnFollowUser(int followerId, int followeeId)
        {
            try
            {
                var result = await _userService.UnfollowUserAsync(followerId, followeeId);

                if (result)
                    return Ok(new { Message = "UnFollowed Successfully" });
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"{ex.Message}" });
            }
        }

        [HttpGet("FollowersList/{userId:int}")]
        public async Task<ActionResult<IReadOnlyList<FollowerDto>>> GetFollowers(int userId)
        {
            try
            {
                var result = await _userService.GetFollowersAsync(userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = $"{ex.Message}" });
            }
        }

        [HttpGet("FolloweesList/{userId:int}")]
        public async Task<ActionResult<IReadOnlyList<FollowerDto>>> GetFollowees(int userId)
        {
            try
            {
                var result = await _userService.GetFolloweesAsync(userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = $"{ex.Message}" });
            }
        }


        [HttpGet("IsFollowing/{followerId:int}/{followeeId:int}")]
        public async Task<ActionResult<bool>> IsFollowing(int followerId, int followeeId)
        {
            try
            {
                var result = await _userService.IsFollowingAsync(followerId, followeeId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"{ex.Message}" });
            }
        }

        [HttpGet("AllUsers")]
        public async Task<ActionResult<IReadOnlyList<UserReturnDto>>> GetAllUsers()
        {
            try
            {
                var result = await _userService.GetAllUsersAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = $"{ex.Message}" });
            }
        }

        [HttpGet("MutualFollowers/{userId}/{otherUserId}")]
        public async Task<ActionResult<IReadOnlyList<MutualFollowerDto>>> GetMutualFollowers(int userId, int otherUserId)
        {
            try
            {
                var mutualFollowers = await _userService.GetMutualFollowersAsync(userId, otherUserId);
                return Ok(mutualFollowers);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("Search")]
        public async Task<ActionResult<IReadOnlyList<UserReturnDto>>> SearchUsers([FromQuery] string searchTerm)
        {
            try
            {
                var users = await _userService.SearchUsersAsync(searchTerm);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}
