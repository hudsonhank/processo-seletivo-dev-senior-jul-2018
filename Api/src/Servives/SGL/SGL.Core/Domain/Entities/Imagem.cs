using Core.Abstractions.Attribute;
using Core.Abstractions.Domain;
using Core.Abstractions.Extension;
using SGL.Domain.Mensagens;
using System;

namespace SGL.Core.Domain.Entities
{
    public class Imagem : Entity, IEntityRoot
    {
        
        [Obrigatorio(ErrorMessage = LivroMensagem.TITULOOBRIGATORIO)]
        public virtual string Nome { get; set; }
        [Obrigatorio(ErrorMessage = LivroMensagem.TITULOOBRIGATORIO)]
        public virtual string ContentType { get; set; }
        [Obrigatorio(ErrorMessage = LivroMensagem.TITULOOBRIGATORIO)]
        public virtual byte[] Bytes { get;  set; }
        
        public Imagem(long id, string nome, string contentType, byte[] bytes):this(nome, contentType, bytes)
        {
            Id = id;       
        }

        public Imagem()
        {            
            Ativo = false;
        }

        public Imagem(string nome, string contentType, byte[] bytes)
        {
            this.NewId();
            Bytes = bytes;
            Ativo = false;
            Nome = nome;
            ContentType = contentType;
        }
    }
}
