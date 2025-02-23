using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.DTOs.Post
{
    public class FeedDto
    {
        public int PostId { get; set; }
        public string? Caption { get; set; }

        public List<string> FilesUrls { get; set; } = new List<string>();

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string? UserImage { get; set; }

        public int ReactionsCount { get; set; }
        public int CommentsCount { get; set; }

        public int SharesCount { get; set; }

        public bool IsShared { get; set; }

        public string Date { get; set; }

        public string PostType { get; set; }
    }
}
