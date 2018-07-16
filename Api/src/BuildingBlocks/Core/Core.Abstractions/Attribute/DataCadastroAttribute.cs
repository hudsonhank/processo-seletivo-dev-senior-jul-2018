using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Abstractions.Attribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class DataCadastroAttribute : ValidationAttribute
    {
        public DataCadastroAttribute()
        {                        
        }

        public override bool IsValid(object value)
        {
            return true;
        }
    }
}