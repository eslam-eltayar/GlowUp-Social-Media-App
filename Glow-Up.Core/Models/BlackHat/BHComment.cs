using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Models.BlackHat
{
    public class BHComment : BaseModel
    {
        public string? Text { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; }
        public int UserId { get; set; }

        public BHPost BHPost { get; set; }
        public int BHPostId { get; set; }

        public int VoteCount { get; set; } = 0;

        public ICollection<CommentVoteHistory> VoteHistory { get; set; } = new HashSet<CommentVoteHistory>();
    }
}
