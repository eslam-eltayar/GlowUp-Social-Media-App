using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Models
{
    public class Comment : BaseModel
    {
        public string? Text { get; set; }
        public string? MediaUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public int PostId { get; set; }
        public int UserId { get; set; }

        public Post Post { get; set; }
        public User User { get; set; }

        // Replies
        public int? ParentCommentId { get; set; } 
        public Comment? ParentComment { get; set; } 
        public ICollection<Comment> Replies { get; set; } = new List<Comment>(); 

        public ICollection<Reaction> Reactions { get; set; } = new HashSet<Reaction>();
        ///public ICollection<Reply> Replies { get; set; } = new HashSet<Reply>();
    }
}
