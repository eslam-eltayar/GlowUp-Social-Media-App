using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.DTOs.BHComment
{
    public class BHCommentToReturnDto
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string Text { get; set; }

        public int VoteCount { get; set; }
    }
}
