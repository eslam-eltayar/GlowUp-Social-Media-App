using Glow_Up.Core.DTOs.Account;
using Glow_Up.Core.DTOs.Profile;
using Glow_Up.Core.DTOs.Users;
using Glow_Up.Core.Enums;
using Glow_Up.Core.Models;
using Glow_Up.Core.Repositories;
using Glow_Up.Core.Services.Files;
using Glow_Up.Core.Services.Notifications;
using Glow_Up.Core.Services.Users;
using Glow_Up.Core.Specifications.User_Spec;
using Glow_Up.Services.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Services.Users
{
    public class UserService(
        IUnitOfWork unitOfWork,
        IFileUploadService fileUploadService,
        IWebHostEnvironment webHostEnvironment,
        INotificationService notificationService,
        UserManager<AppUser> userManager) : IUserService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFileUploadService _fileUploadService = fileUploadService;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
        private readonly INotificationService _notificationService = notificationService;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<UserToReturnDto> CreateUserAsync(RegisterDto dto)
        {
            if (dto == null) throw new ArgumentNullException("User Data cannot be null");

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,

            };

            if (Enum.TryParse<Gender>(dto.Gender, true, out var parsedStatus))
            {
                user.Gender = parsedStatus;
            }
            else
            {
                throw new ArgumentException($"Invalid Gender value: {dto.Gender}");
            }

            _unitOfWork.Repository<User>().Add(user);

            int result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
                throw new Exception("An Error While adding User");

            return new UserToReturnDto
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
        }

        public async Task<UserToReturnDto> GetUserByEmail(string Email)
        {
            var user = await _unitOfWork.Repository<User>().FirstOrDefaultAsync(u => u.Email == Email);

            if (user == null)
                throw new Exception("The User Not Founded in Database");

            return new UserToReturnDto
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task<UserProfileDto> GetUserProfileAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid UserId");

            var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId);

            if (user == null) throw new Exception("User Not Foundede");


            var followersCount = await _unitOfWork.Repository<Follow>()
                .CountAsync(f => f.FolloweeId == userId);

            var followingCount = await _unitOfWork.Repository<Follow>()
                .CountAsync(f => f.FollowerId == userId);

            return new UserProfileDto
            {
                UserId = user.Id,
                Address = user.Address,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Bio = user.Bio,
                CoverPic = user.CoverPic,
                Email = user.Email,
                Gender = user.Gender.ToString(),
                Phone = user.Phone,
                ProfilePic = user.ProfilePic,
                FollowersCount = followersCount,
                FollowingCount = followingCount,
                IsSad = user.IsSad,
            };
        }


        public async Task<bool> UpdateProfileAsync(int userId, UpdateProfileDto dto)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid UserId");

            if (dto == null)
                throw new Exception("The Input cannot be null. Enter valid data");

            var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId);

            if (user == null)
                throw new Exception("User Not Founded");

            user.FirstName = dto.firstName;
            user.LastName = dto.lastName;
            user.Phone = dto.phone;
            user.Bio = dto.bio;
            user.Address = dto.address;

            if (!string.IsNullOrWhiteSpace(dto.email) && dto.email != user.Email)
            {

                var userExist = await _unitOfWork.Repository<User>()
                                                 .FirstOrDefaultAsync(u => u.Email == dto.email && u.Id != user.Id);
                if (userExist != null)
                    throw new Exception("The Email is already in use!");

                var appUser = await _userManager.FindByEmailAsync(user.Email);

                if (appUser == null)
                    throw new Exception("User not found in Identity!");

                var emailToken = await _userManager.GenerateChangeEmailTokenAsync(appUser, dto.email);

                var result = await _userManager.ChangeEmailAsync(appUser, dto.email, emailToken);

                if (!result.Succeeded)
                    throw new Exception("Failed to update email in Identity!");

                appUser.NormalizedEmail = dto.email.ToUpperInvariant();

                appUser.UserName = dto.email.Split('@')[0];

                appUser.NormalizedUserName = appUser.UserName.ToUpperInvariant();

                user.Email = dto.email;

                var updateResult = await _userManager.UpdateAsync(appUser);

                if (!updateResult.Succeeded)
                    throw new Exception("Failed to update user in Identity!");
            }



            if (dto.profilePic != null && dto.profilePic.Length > 0)
            {
                if (!string.IsNullOrEmpty(user.ProfilePic))
                {

                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "profiles", user.ProfilePic);

                    imagePath = $"wwwroot{imagePath}";

                    if (File.Exists(imagePath))
                        File.Delete(imagePath);

                }

                var newProfilePic = await _fileUploadService.UploadFileAsync(dto.profilePic, "profiles");

                user.ProfilePic = newProfilePic;
            }

            if (dto.coverPic != null && dto.coverPic.Length > 0)
            {
                if (!string.IsNullOrEmpty(user.CoverPic))
                {

                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "covers", user.CoverPic);

                    imagePath = $"wwwroot{imagePath}";

                    if (File.Exists(imagePath))
                        File.Delete(imagePath);

                }

                var newCoverPic = await _fileUploadService.UploadFileAsync(dto.coverPic, "covers");

                user.CoverPic = newCoverPic;
            }


            _unitOfWork.Repository<User>().Update(user);

            int Saveresult = await _unitOfWork.CompleteAsync();

            if (Saveresult <= 0)
                throw new Exception("An Error While Updating profile!");


            return true;


        }


        public async Task<bool> FollowUserAsync(int followerId, int followeeId)
        {
            if (followerId == followeeId)
            {
                throw new Exception("A user cannot follow themselves.");
            }

            var existingFollow = await _unitOfWork.Repository<Follow>()
                .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FolloweeId == followeeId);

            if (existingFollow != null)
            {
                throw new Exception("You are already following this user.");
            }

            var follow = new Follow
            {
                FollowerId = followerId,
                FolloweeId = followeeId,
                CreatedAt = DateTime.UtcNow
            };

            _unitOfWork.Repository<Follow>().Add(follow);

            // Save changes to the database
            int result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
            {
                throw new Exception("An error occurred while following the user.");
            }

            await _notificationService.CreateFollowNotificationAsync(followerId, followeeId);

            return true;
        }

        public async Task<bool> UnfollowUserAsync(int followerId, int followeeId)
        {
            var follow = await _unitOfWork.Repository<Follow>()
                        .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FolloweeId == followeeId);

            if (follow == null)
            {
                throw new Exception("You are not following this user.");
            }

            _unitOfWork.Repository<Follow>().Delete(follow);

            int result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
            {
                throw new Exception("An error occurred while unfollowing the user.");
            }

            return true;
        }

        public async Task<bool> IsFollowingAsync(int followerId, int followeeId)
        {
            var follwed = await _unitOfWork.Repository<Follow>()
                                           .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FolloweeId == followeeId);
            return follwed != null;
        }

        public async Task<IReadOnlyList<FollowerDto>> GetFollowersAsync(int userId)
        {
            var spec = new FollowersSpecification(userId);

            var followers = await _unitOfWork.Repository<Follow>().GetAllWithSpecAsync(spec);

            if (followers == null || !followers.Any())
                throw new Exception("The Followers List is Empty!");

            return followers.Select(f => new FollowerDto
            {
                UserId = f.Follower.Id,
                ProfilePic = f.Follower.ProfilePic,
                UserName = $"{f.Follower.FirstName} {f.Follower.LastName}",
                DateOfFollowing = Helper.FormatDate(f.CreatedAt),

            }).ToList().AsReadOnly();
        }

        public async Task<IReadOnlyList<FollowerDto>> GetFolloweesAsync(int userId)
        {
            var spec = new FolloweesSpecification(userId);

            var followers = await _unitOfWork.Repository<Follow>().GetAllWithSpecAsync(spec);

            if (followers == null || !followers.Any())
                throw new Exception("The Followers List is Empty!");

            return followers.Select(f => new FollowerDto
            {
                UserId = f.Followee.Id,
                ProfilePic = f.Followee.ProfilePic,
                UserName = $"{f.Followee.FirstName} {f.Followee.LastName}",
                DateOfFollowing = Helper.FormatDate(f.CreatedAt),

            }).ToList().AsReadOnly();
        }

        public async Task<IReadOnlyList<UserReturnDto>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.Repository<User>().GetAllAsync();

            if (users == null || !users.Any())
                throw new Exception("No Users founded");

            return users.Select(user => new UserReturnDto
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePic = user.ProfilePic,

            }).ToList().AsReadOnly();

        }

        public async Task<IReadOnlyList<MutualFollowerDto>> GetMutualFollowersAsync(int userId, int otherUserId)
        {
            if (userId <= 0 || otherUserId <= 0)
                throw new ArgumentException("Invalid UserId");

            var userFollowersSpec = new FollowersSpecification(userId);

            var otherUserFollowersSpec = new FollowersSpecification(otherUserId);

            var userFollowers = await _unitOfWork.Repository<Follow>().GetAllWithSpecAsync(userFollowersSpec);
            var otherUserFollowers = await _unitOfWork.Repository<Follow>().GetAllWithSpecAsync(otherUserFollowersSpec);

            var mutualFollowers = userFollowers
                .Where(f => otherUserFollowers.Any(of => of.FollowerId == f.FollowerId))
                .Select(f => new MutualFollowerDto
                {
                    UserId = f.Follower.Id,
                    UserName = $"{f.Follower.FirstName} {f.Follower.LastName}",
                    ProfilePic = f.Follower.ProfilePic
                })
                .ToList()
                .AsReadOnly();

            return mutualFollowers;
        }

        public async Task<IReadOnlyList<UserReturnDto>> SearchUsersAsync(string searchTerm)
        {
            var spec = new UserSearchSpecification(searchTerm);
            var users = await _unitOfWork.Repository<User>().GetAllWithSpecAsync(spec);

            if (!users.Any())
                throw new Exception("No users found matching the search criteria.");

            return users.Select(user => new UserReturnDto
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePic = user.ProfilePic,
            }).ToList().AsReadOnly();

        }
    }
}