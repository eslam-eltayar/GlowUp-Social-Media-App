using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Enums
{
    public enum ReactType
    {
        [EnumMember(Value = "Touched")]
        Touched,
        [EnumMember(Value = "Funny")]
        Funny,
        [EnumMember(Value = "Chill")]
        Chill,
        [EnumMember(Value = "Awesome")]
        Awesome
    }
}
