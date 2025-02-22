using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.DTOs.Users
{
    public class FollowerDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string DateOfFollowing { get; set; }

        public string? ProfilePic { get; set; }

    }
}
