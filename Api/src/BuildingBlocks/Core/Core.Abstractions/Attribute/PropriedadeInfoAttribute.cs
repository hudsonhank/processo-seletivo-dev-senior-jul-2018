using Core.Abstractions.Attribute.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Abstractions.Attribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class PropriedadeInfoAttribute : ValidationAttribute
    {
        public ComponenteTipoEnum Tipo { get; } = ComponenteTipoEnum.TextBox;

        public string Label { get; } = "";
        public string PlaceHolder { get; } = "";
        public string Valor { get; } = "";
        
        public PropriedadeInfoAttribute(ComponenteTipoEnum tipo, string label = "", string placeHolder = "", string valor = "")
        {
            Tipo = tipo;
            Label = label;
            PlaceHolder = placeHolder;
            Valor = valor;            
        }

        public override bool IsValid(object value)
        {
            return true;
        }
    }

}
