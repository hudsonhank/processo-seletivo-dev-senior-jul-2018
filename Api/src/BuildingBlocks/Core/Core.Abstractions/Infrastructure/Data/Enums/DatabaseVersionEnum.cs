using System.ComponentModel;

namespace Core.Abstractions.Infrastructure.Data.Enums
{
    public enum DatabaseVersionEnum
    {
        [Description("9")]
        v9,
        [Description("10")]
        v10,
        [Description("2012")]
        v2012,
        [Description("2016")]
        v2016
    }
}
