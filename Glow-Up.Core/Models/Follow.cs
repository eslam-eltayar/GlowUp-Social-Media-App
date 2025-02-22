using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Models
{
    public class Follow : BaseModel
    {
        public int FollowerId { get; set; } // The user who is following المستخدم الذي يتابع
        public int FolloweeId { get; set; } // The user being followed المستخدم الذي تتم متابعته

        // Navigation properties
        public User Follower { get; set; }
        public User Followee { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
