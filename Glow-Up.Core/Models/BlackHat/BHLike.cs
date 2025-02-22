using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Models.BlackHat
{
    public class BHLike : BaseModel
    {
        public User User { get; set; }
        public int UserId { get; set; }

        public BHPost BHPost { get; set; }
        public int BHPostId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
