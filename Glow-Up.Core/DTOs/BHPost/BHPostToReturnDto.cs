using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.DTOs.BHPost
{
    public class BHPostToReturnDto
    {
        public int PostId { get; set; }
        public string? Caption { get; set; } = string.Empty;
        public string Categoty { get; set; }


        public List<string> FilesUrls { get; set; } = new List<string>();
        public int UserId { get; set; }

        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public string Date { get; set; }
    }
}
