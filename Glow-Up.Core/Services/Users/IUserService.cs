using Glow_Up.Core.DTOs.Account;
using Glow_Up.Core.DTOs.Profile;
using Glow_Up.Core.DTOs.Users;
using Glow_Up.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Services.Users
{
    public interface IUserService
    {
        Task<UserToReturnDto> CreateUserAsync(RegisterDto dto);
        Task<UserToReturnDto> GetUserByEmail(string Email);

        Task<bool> UpdateProfileAsync(int userId , UpdateProfileDto dto);
        Task<UserProfileDto> GetUserProfileAsync(int userId);

        Task<bool> FollowUserAsync(int followerId, int followeeId);
        Task<bool> UnfollowUserAsync(int followerId, int followeeId);

        Task<bool> IsFollowingAsync(int followerId, int followeeId);

        Task<IReadOnlyList<FollowerDto>> GetFollowersAsync(int userId);

        Task<IReadOnlyList<FollowerDto>> GetFolloweesAsync(int userId);


        Task<IReadOnlyList<UserReturnDto>> GetAllUsersAsync();

        Task<IReadOnlyList<MutualFollowerDto>> GetMutualFollowersAsync(int userId, int otherUserId);

        Task<IReadOnlyList<UserReturnDto>> SearchUsersAsync(string searchTerm);
    }
}
