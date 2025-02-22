using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.DTOs.Post
{
    public class CreatePostDto
    {
        public string? Caption { get; set; } = string.Empty;
        public List<IFormFile> MediaFiles { get; set; } = new List<IFormFile>();

    }
}
