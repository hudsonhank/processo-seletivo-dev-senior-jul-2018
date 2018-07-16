using System;
using System.ComponentModel;
using System.Reflection;

namespace Core.Abstractions.Extension
{
    public static class EnumExtension
    {
        public static string GetDescription(this Enum valor)
        {
            FieldInfo field = valor.GetType().GetField(valor.ToString());

            var retorno = string.Empty;
            
            if(field!=null)
            {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0)
                {
                    retorno = attributes[0].Description;
                }                
            }
            else
            {
                retorno =  valor.ToString();
            }

            return retorno;

        }
    }
}