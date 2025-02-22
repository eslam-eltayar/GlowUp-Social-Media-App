using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Models
{
    public class Reply : BaseModel
    {
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        //public int CommentId { get; set; }
        //public Comment Comment { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
