using Glow_Up.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Models.BlackHat
{
    public class BHPost : BaseModel
    {
        public string? Caption { get; set; }
        public DateTime CreatedAt { get; set; }

        public Category Category { get; set; }


        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<BHComment> Comments { get; set; } = new HashSet<BHComment>();
        public ICollection<BHMedia> Medias { get; set; } = new HashSet<BHMedia>();
        public ICollection<BHLike> Likes { get; set; } = new HashSet<BHLike>();



    }
}
