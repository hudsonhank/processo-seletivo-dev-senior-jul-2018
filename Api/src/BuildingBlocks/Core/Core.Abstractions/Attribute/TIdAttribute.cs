using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Abstractions.Attribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class TIdAttribute : ValidationAttribute
    {        
        public TIdAttribute()
        {                        
        }

        public override bool IsValid(object value)
        {
            return true;
        }
    }
}