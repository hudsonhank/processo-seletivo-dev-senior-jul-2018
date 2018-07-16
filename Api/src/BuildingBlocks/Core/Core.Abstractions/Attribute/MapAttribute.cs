using System;

namespace Core.Abstractions.Attribute
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MapAttribute : System.Attribute
    {
        public string TipoMensagem { get; private set; }
        public MapAttribute(string nomePropriedadePai)
        {
            TipoMensagem = nomePropriedadePai;
        }

        public MapAttribute()
        {                        
        }
    }
}