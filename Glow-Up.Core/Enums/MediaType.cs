using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Enums
{
    public enum MediaType
    {
        [EnumMember(Value = "Video")]
        Video,
        [EnumMember(Value = "Image")]
        Image,
        [EnumMember(Value = "Document")]
        Document,
        [EnumMember(Value = "Other")]
        Other
    }
}
