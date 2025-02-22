using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.DTOs.Comment
{
    public class CommentToReturnDto
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public string? MediaUrl { get; set; }
        public int PostId { get; set; }
        public string CreatedAt { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string? UserImage { get; set; }

        public int? ParentCommentId { get; set; } 
        public List<CommentToReturnDto> Replies { get; set; } = new(); 
    }
}
