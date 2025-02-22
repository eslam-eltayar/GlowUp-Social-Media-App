using System.Runtime.Serialization;

namespace Glow_Up.Core.Enums
{
    public enum Category
    {
        [EnumMember(Value = "Sports")]
        Sports,

        [EnumMember(Value = "Sciences")]
        Sciences,

        [EnumMember(Value = "Politics")]
        Politics,

        [EnumMember(Value = "Businesses")]
        Businesses,

        [EnumMember(Value = "Religions")]
        Religions
    }
}
