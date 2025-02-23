using Glow_Up.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Models
{
    public class Post : BaseModel
    {
        public string? Caption { get; set; }
        public DateTime CreatedAt { get; set; }
        public PostType PostType { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Media> MediaItems { get; set; } = new HashSet<Media>();
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public ICollection<Reaction> Reactions { get; set; } = new HashSet<Reaction>();

    }
}
