using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.DTOs.Messages;
public class ChatDto
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string ProfilePic { get; set; }
    public MessageDto LastMessage { get; set; }
    public int UnreadCount { get; set; }
}