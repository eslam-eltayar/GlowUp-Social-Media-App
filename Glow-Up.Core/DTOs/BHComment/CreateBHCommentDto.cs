using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.DTOs.BHComment
{
    public class CreateBHCommentDto
    {
        public int UserId { get; set; }
        public string Text { get; set; }
    }
}
