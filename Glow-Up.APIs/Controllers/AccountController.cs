using Glow_Up.Core.DTOs.Account;
using Glow_Up.Core.Models;
using Glow_Up.Core.Services.Account;
using Glow_Up.Core.Services.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Glow_Up.APIs.Controllers
{
    public class AccountController : ApiBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserService _userService;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager, IUserService userService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto model)
        {

            var existingUser = await _userManager.Users.AnyAsync(u => u.Email == model.Email);

            if (existingUser)
            {
                return BadRequest("Email is already Exist.");
            }


            var user = new AppUser
            {
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new { Message = "User registration failed." });
            }

            var AddUserResult = await _userService.CreateUserAsync(model);

            if (AddUserResult == null)
                return BadRequest(new { Message = "User Not added to Database!" });

            var returnedUser = new UserDto()
            {
                UserId = AddUserResult.UserId,
                UserName = $"{AddUserResult.FirstName} {AddUserResult.LastName}",
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            };

            return Ok(returnedUser);

        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null) return Unauthorized(new { Message = "Invalid Login" });

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded) return Unauthorized(new { Message = "Invalid Login" });

            var userInDb = await _userService.GetUserByEmail(user.Email);

            return Ok(new UserDto()
            {
                UserId = userInDb.UserId,
                UserName = $"{userInDb.FirstName} {userInDb.LastName}", 
                Email = user?.Email ?? string.Empty,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
        }
    }
}
