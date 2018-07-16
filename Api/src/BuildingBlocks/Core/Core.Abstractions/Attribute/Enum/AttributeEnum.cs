using System.ComponentModel;

namespace Core.Abstractions.Attribute.Enum
{
    public enum AttributeEnum
    {
        [Description("ObrigatorioAttribute")]
        ObrigatorioAttribute,
        [Description("MaximoAttribute")]
        MaximoAttribute,
        [Description("MinimoAttribute")]
        MinimoAttribute           
    }
}
