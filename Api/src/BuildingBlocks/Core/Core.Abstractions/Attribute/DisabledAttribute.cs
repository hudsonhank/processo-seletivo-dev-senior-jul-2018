using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Abstractions.Attribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class DisabledAttribute : ValidationAttribute
    {        
        public DisabledAttribute()
        {                        
        }

        public override bool IsValid(object value)
        {
            return true;
        }
    }
}