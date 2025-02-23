using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Enums
{
    public enum PostType
    {
        [EnumMember(Value ="Clip")]
        Clip,

        [EnumMember(Value ="Video")]
        Video,

        [EnumMember(Value ="Post")]
        Post

    }
}
