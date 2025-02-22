using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.DTOs.BHPost
{
    public class CreateBHPostDto
    {
        public string? Caption { get; set; } = string.Empty;

        public List<IFormFile> MediaFiles { get; set; } = new List<IFormFile>();
        public string Category { get; set; } = string.Empty;
    }

}
