using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Abstractions.Attribute
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class MinimoAttribute : ValidationAttribute
    {
        public int Quantidade { get; set; }

        public MinimoAttribute(int quantidade)
        {
            Quantidade = quantidade;
        }

        public override bool IsValid(object value)
        {

            //if (value != null)
            //{
            //    return value.ToString().Trim().Length <= Quantidade;
            //}
            //return true;

            var valor = value as string;

            return string.IsNullOrEmpty(valor) || Validar(valor);
        }

        bool Validar(string valor)
        {
            return (valor.Trim().Length >= Quantidade);
        }

        public override string FormatErrorMessage(string name)
        {
            
            return String.Format("O campo {0} deverá conter no mínimo {1} caracteres.", name, Quantidade);

            
        }

        
    }
}
