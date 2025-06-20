using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Models;
public class ReportPost : BaseModel
{
    public int PostId { get; set; }
    public int ReporterId { get; set; }
    //public string Reason { get; set; }
    public Post Post { get; set; } = default!;
    public User Reporter { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}