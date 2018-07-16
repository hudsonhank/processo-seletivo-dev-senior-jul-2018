using System;

namespace Core.Abstractions.Domain.Mensagens
{    
    [AttributeUsage(AttributeTargets.Class)]
    public class Mensagensattribute : System.Attribute
    {
        public Mensagensattribute()
        {     
        }
    }
}