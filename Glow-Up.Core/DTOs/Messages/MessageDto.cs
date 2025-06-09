using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.DTOs.Messages;
public class MessageDto
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public string SenderName { get; set; }
    public string SenderProfilePic { get; set; }
    public int RecipientId { get; set; }
    public string RecipientName { get; set; }
    public string RecipientProfilePic { get; set; }
    public string Content { get; set; }
    public string SentAt { get; set; }
    public bool IsRead { get; set; }
}