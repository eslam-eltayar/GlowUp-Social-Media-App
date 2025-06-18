using Glow_Up.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.DTOs.Profile
{
    public class UserProfileDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Bio { get; set; }
        public string Gender { get; set; }
        public string? ProfilePic { get; set; }
        public string? CoverPic { get; set; }
        public bool IsSad { get; set; }

        public int FollowersCount { get; set; } // Number of followers
        public int FollowingCount { get; set; } // Number of users being followed
    }
}
