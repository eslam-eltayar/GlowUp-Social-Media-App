using Glow_Up.Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.DTOs.Profile
{
    public class UpdateProfileDto
    {
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public string? address { get; set; }
        public string? bio { get; set; }
        //public string? gender { get; set; }
        public IFormFile? profilePic { get; set; }
        public IFormFile? coverPic { get; set; }
    }
}
