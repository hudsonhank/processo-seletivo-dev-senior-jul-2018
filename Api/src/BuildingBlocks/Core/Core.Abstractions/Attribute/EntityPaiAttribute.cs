using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Abstractions.Attribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class EntityPaiAttribute : ValidationAttribute
    {
        public string TituloPropriedadePai { get; set; }
        //public string Tipo { get; set; }
        public EntityPaiAttribute(string nomePropriedadePai)
        {            
            TituloPropriedadePai = nomePropriedadePai;            
        }

        public override bool IsValid(object value)
        {
            return true;
        }
    }
}