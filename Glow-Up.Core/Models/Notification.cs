using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Models
{
    public class Notification : BaseModel
    {
        // Foreign Key
        public int RecipientId { get; set; }
        public User? Recipient { get; set; }

        public int? SenderId { get; set; } // Can be null for system notifications
        public User? Sender { get; set; }

        public string Type { get; set; } // e.g., "like", "comment", "follow", "message"
        public int? TargetId { get; set; } // PostId, CommentId, etc. (depends on Type)

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
    }
}
