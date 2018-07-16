using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Abstractions.Attribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class DataAtualizacaoAttribute : ValidationAttribute
    {        
        public DataAtualizacaoAttribute()
        {                        
        }

        public override bool IsValid(object value)
        {
            return true;
        }
    }
}