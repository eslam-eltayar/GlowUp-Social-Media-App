using Glow_Up.Core.Enums;
using Glow_Up.Core.Models.BlackHat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Glow_Up.Core.Models
{
    public class User : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Bio { get; set; }
        public Gender Gender { get; set; }
        public string? ProfilePic { get; set; }
        public string? CoverPic { get; set; }

        public bool IsSad { get; set; } = false;

        public ICollection<Post> Posts { get; set; } = new HashSet<Post>();
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public ICollection<Reaction> Reactions { get; set; } = new HashSet<Reaction>();
        public ICollection<Reply> Replies { get; set; } = new HashSet<Reply>();


        public ICollection<Follow> Followers { get; set; } = new List<Follow>();
        public ICollection<Follow> Following { get; set; } = new List<Follow>();


        public ICollection<BHPost> BHPosts { get; set; } = new HashSet<BHPost>();
        public ICollection<BHComment> BHComment { get; set; } = new HashSet<BHComment>();
        public ICollection<BHLike> BHLike { get; set; } = new HashSet<BHLike>();
        public ICollection<CommentVoteHistory> CommentVotes { get; set; } = new HashSet<CommentVoteHistory>();
    }

}

