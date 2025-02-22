using Glow_Up.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Models.BlackHat
{
    public class BHMedia : BaseModel
    {
        public int BHPostId { get; set; } // FK
        public BHPost Post { get; set; }

        public string Url { get; set; }
        public MediaType Type { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
