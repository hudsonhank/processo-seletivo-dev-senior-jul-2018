using Core.Abstractions.Attribute;
using Core.Abstractions.Domain;
using Core.Abstractions.Extension;
using Newtonsoft.Json;
using SGL.Domain.Mensagens;
using System;

namespace SGL.Core.Domain.Entities
{
    public class LivroEntity : Entity, IEntityRoot
    {        
        [Obrigatorio(ErrorMessage = LivroMensagem.TITULOOBRIGATORIO)]
        [Minimo(10, ErrorMessage = LivroMensagem.TITULOMINIMO)]
        [Maximo(150, ErrorMessage = LivroMensagem.TITULOMAXIMO)]
        public virtual string Titulo { get; set; }

        [Obrigatorio(ErrorMessage = LivroMensagem.AUTOROBRIGATORIO)]
        [Minimo(10, ErrorMessage = LivroMensagem.AUTORMINIMO)]
        [Maximo(150, ErrorMessage = LivroMensagem.AUTORMAXIMO)]
        public virtual string Autor { get; set; }

        [Obrigatorio(ErrorMessage = LivroMensagem.GENEROOBRIGATORIO)]
        [Minimo(5, ErrorMessage = LivroMensagem.GENEROMINIMO)]
        [Maximo(150, ErrorMessage = LivroMensagem.GENEROMAXIMO)]
        public virtual string Genero { get; set; }
        
        [Obrigatorio(ErrorMessage = LivroMensagem.EDITORAOBRIGATORIO)]
        [Minimo(5, ErrorMessage = LivroMensagem.EDITORAMINIMO)]
        [Maximo(150, ErrorMessage = LivroMensagem.EDITORAMAXIMO)]
        public virtual string Editora { get; set; }

        [Obrigatorio(ErrorMessage = LivroMensagem.DESCRICAOOBRIGATORIO)]
        [Minimo(10, ErrorMessage = LivroMensagem.DESCRICAOMINIMO)]
        [Maximo(150, ErrorMessage = LivroMensagem.DESCRICAOMAXIMO)]
        public virtual string Descricao { get; set; }

        [Obrigatorio(ErrorMessage = LivroMensagem.SINOPSEOBRIGATORIO)]        
        [Minimo(10, ErrorMessage = LivroMensagem.SINOPSEMINIMO)]
        [Maximo(150, ErrorMessage = LivroMensagem.SINOPSEMAXIMO)]
        public virtual string Sinopse { get; set; }
        
        [Obrigatorio(ErrorMessage = LivroMensagem.PAGINASOBRIGATORIO, TipoValidacao = TipoValidacaoObrig.NumeroPositivo)]
        public virtual int Paginas { get; set; }

        [Obrigatorio(ErrorMessage = LivroMensagem.DATAPUBLICACAOOBRIGATORIO)]
        public virtual DateTime DataPublicacao { get; set; }

        public virtual string Link { get; set; }

        [Obrigatorio(ErrorMessage = LivroMensagem.CAPAIDOBRIGATORIO, TipoValidacao = TipoValidacaoObrig.NumeroPositivo)]
        public virtual long CapaId { get; set; }

        [EntityPai("LivroEntity")]
        [JsonIgnore]
        public virtual Imagem Capa { get; set; }

        string _codigoUnico;
        public virtual string CodigoUnico
        {
            get { return _codigoUnico; }
            private set { _codigoUnico = value; }
        }

        // Gerar um novo código
        public void GerarNovoLivro()
        {
            this.NewId();
            _codigoUnico = Guid.NewGuid().ToString("N").Substring(0, 10);
        }

        public void ManterDados(string CodigoUnico)
        {
            _codigoUnico = CodigoUnico;
        }

        public LivroEntity()
        {
        }

        public LivroEntity(long id, string codigoUnico,long capaId, string titulo, string genero, string autor, string editora, string descricao, string sinopse, int paginas, 
            DateTime dataPublicacao, string link, Imagem capa)
        {
            Id = id;
            _codigoUnico = codigoUnico;
            CapaId = capaId;
            Titulo = titulo;
            Genero = genero;
            Autor = autor;
            Capa = capa;
            Editora = editora;
            Descricao = descricao;
            Sinopse = sinopse;
            Paginas = paginas;
            DataPublicacao = dataPublicacao;
            Link = link;
            Ativo = true;
        }

    }
}
    
