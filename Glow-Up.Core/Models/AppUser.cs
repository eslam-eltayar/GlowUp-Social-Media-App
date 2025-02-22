using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Models
{
    public class AppUser : IdentityUser
    {
        public override string Email { get; set; } = string.Empty;
    }
}
