using Glow_Up.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Models;
public class ActivityLog : BaseModel
{
    public int UserId { get; set; }
    public User User { get; set; }
    public ActivityType Type { get; set; }
    public int TargetId { get; set; }  // Post/Comment ID
    public DateTime CreatedAt { get; set; }
    public string? AdditionalInfo { get; set; }  // For storing any additional context
}
