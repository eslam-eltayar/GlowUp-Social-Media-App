using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.DTOs.Post
{
    public record AddReactionDto(
        int UserId, 
        string ReactType
        );

}
