using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.DTOs.Comment
{
    public class CreateReplyDto
    {
        public string? Text { get; set; } 
        public IFormFile? Media { get; set; } 
        public int ParentCommentId { get; set; } 
    }
}
