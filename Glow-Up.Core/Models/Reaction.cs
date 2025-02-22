using Glow_Up.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Models
{
    public class Reaction : BaseModel
    {
        public ReactType Type { get; set; }

        public int UserId { get; set; } 
        public User User { get; set; } 

        public int? PostId { get; set; } 
        public Post? Post { get; set; } 

        public int? CommentId { get; set; } 
        public Comment? Comment { get; set; } 

    }
}
