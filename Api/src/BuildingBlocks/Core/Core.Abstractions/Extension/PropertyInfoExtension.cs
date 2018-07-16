using System.Reflection;
using System.Linq;

namespace Core.Abstractions.Extension
{
    public static class PropertyInfoExtension
    {
        public static TCustomAttribute GetCustomAttribute<TCustomAttribute>(this PropertyInfo valor)
        {
            return valor.GetCustomAttributes(typeof(TCustomAttribute)).Cast<TCustomAttribute>().FirstOrDefault();
        }

        public static TCustomAttribute BuscarAtributoCustomizado<TCustomAttribute>(this PropertyInfo valor)
        {
            return valor.GetCustomAttributes(typeof(TCustomAttribute)).Cast<TCustomAttribute>().FirstOrDefault();
        }
    }
}