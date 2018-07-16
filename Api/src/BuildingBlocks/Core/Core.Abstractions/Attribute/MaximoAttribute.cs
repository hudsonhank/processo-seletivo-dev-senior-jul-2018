using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Abstractions.Attribute
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class MaximoAttribute : ValidationAttribute
    {
        public int Quantidade { get; set; }
        public string Mensagem { get; private set; }

        public MaximoAttribute(int quantidade)
        {
            Quantidade = quantidade;
        }

        public MaximoAttribute(int quantidade, string mensagem)
        {
            Quantidade = quantidade;
            Mensagem = mensagem;
        }

        public override bool IsValid(object value)
        {

            //if(value != null)
            //{
            //    return value.ToString().Trim().Length<= Quantidade;
            //}
            //return true;

            return Validar(value as string);
        }

        bool Validar(string valor)
        {
            return ((valor ?? string.Empty).Trim().Length <= Quantidade);
        }

        public override string FormatErrorMessage(string name)
        {
            return Mensagem ?? String.Format("O campo {0} so pode conter {1} caracteres.", name, Quantidade);
        }
    }
}
