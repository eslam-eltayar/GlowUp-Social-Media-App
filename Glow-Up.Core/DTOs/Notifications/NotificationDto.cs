using Glow_Up.Core.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.DTOs.Notifications
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public int RecipientId { get; set; }
        public int? SenderId { get; set; }
        public SenderDto Sender { get; set; }
        public string Type { get; set; }
        public int? TargetId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
    }
}
