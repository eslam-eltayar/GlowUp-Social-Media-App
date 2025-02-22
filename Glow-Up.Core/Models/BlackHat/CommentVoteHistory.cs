using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Models.BlackHat
{
    public class CommentVoteHistory : BaseModel
    {
        public int BHCommentId { get; set; }
        public BHComment BHComment { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

    }
}
