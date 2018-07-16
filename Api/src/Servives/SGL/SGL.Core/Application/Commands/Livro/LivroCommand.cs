using Core.Abstractions.Application;
using Core.Abstractions.Attribute;
using Core.Abstractions.Attribute.Enum;
using SGL.Domain.Mensagens;
using System;
using System.ComponentModel.DataAnnotations;

namespace SGL.Application.Commands
{
    public abstract class LivroCommand : Command
    {
        [Obrigatorio(ErrorMessage = LivroMensagem.TITULOOBRIGATORIO)]
        [Minimo(10, ErrorMessage = LivroMensagem.TITULOMINIMO)]
        [Maximo(150, ErrorMessage = LivroMensagem.TITULOMAXIMO)]
        [PropriedadeInfo(ComponenteTipoEnum.TextBox, "Titulo*", "Título do livro", "Título AAA111")]
        [Options("Principal", css: "col-md-4")]
        public virtual string Titulo { get; set; }

        [Obrigatorio(ErrorMessage = LivroMensagem.AUTOROBRIGATORIO)]
        [Minimo(10, ErrorMessage = LivroMensagem.AUTORMINIMO)]
        [Maximo(150, ErrorMessage = LivroMensagem.AUTORMAXIMO)]
        [PropriedadeInfo(ComponenteTipoEnum.TextBox, "Autor*", "Autor do livro", "Autor AAA111")]
        [Options("Principal", css: "col-md-4")]
        public virtual string Autor { get; set; }

        [Obrigatorio(ErrorMessage = LivroMensagem.GENEROOBRIGATORIO)]
        [Minimo(5, ErrorMessage = LivroMensagem.GENEROMINIMO)]
        [Maximo(150, ErrorMessage = LivroMensagem.GENEROMAXIMO)]

        //      [PropriedadeInfo(ComponenteTipoEnum.DropDown, "Gênero*", "Selecione uma opção", "Sim")]
        //        [Options("Principal", css: "col-md-4", listaValores: "Romance|Cientifico|Fisica")]
        [PropriedadeInfo(ComponenteTipoEnum.TextBox, "Gênero*", "Gênero do livro", "Gênero AAA111")]
        [Options("Principal", css: "col-md-4")]
        public virtual string Genero { get; set; }

        [Obrigatorio(ErrorMessage = LivroMensagem.EDITORAOBRIGATORIO)]
        [Minimo(5, ErrorMessage = LivroMensagem.EDITORAMINIMO)]
        [Maximo(150, ErrorMessage = LivroMensagem.EDITORAMAXIMO)]
        [PropriedadeInfo(ComponenteTipoEnum.TextBox, "Editora*", "Editora do livro", "Editora AAA111")]
        [Options("Principal", css: "col-md-4")]
        public virtual string Editora { get; set; }

        [Obrigatorio(ErrorMessage = LivroMensagem.DATAPUBLICACAOOBRIGATORIO)]
        [PropriedadeInfo(ComponenteTipoEnum.TextBox, "Data de Publicação*", "Data de Publicação", "03/02/2001")]
        [Options("Principal", css: "col-md-4", pattern: "date", typeData: nameof(DataType.Date))]
        public virtual DateTimeOffset DataPublicacao { get; set; }

        [Obrigatorio(ErrorMessage = LivroMensagem.DESCRICAOOBRIGATORIO)]
        [Minimo(10, ErrorMessage = LivroMensagem.DESCRICAOMINIMO)]
        [Maximo(500, ErrorMessage = LivroMensagem.DESCRICAOMAXIMO)]
        [PropriedadeInfo(ComponenteTipoEnum.TextBox, "Descrição*", "Descrição do livro", "Descrição AAA111")]
        [Options("Principal", css: "col-md-4", listagem: false)]
        public virtual string Descricao { get; set; }

        [Obrigatorio(ErrorMessage = LivroMensagem.SINOPSEOBRIGATORIO)]
        [Minimo(10, ErrorMessage = LivroMensagem.SINOPSEMINIMO)]
        [Maximo(500, ErrorMessage = LivroMensagem.SINOPSEMAXIMO)]
        [PropriedadeInfo(ComponenteTipoEnum.TextBox, "Sinopse*", "Sinopse do livro", "Sinopse AAA111")]
        [Options("Principal", css: "col-md-4", listagem: false)]
        public virtual string Sinopse { get; set; }

        [Obrigatorio(ErrorMessage = LivroMensagem.PAGINASOBRIGATORIO, TipoValidacao = TipoValidacaoObrig.NumeroPositivo)]
        [Maximo(5, ErrorMessage = LivroMensagem.SINOPSEMAXIMO)]
        [PropriedadeInfo(ComponenteTipoEnum.TextBox, "Nr de páginas*", "Paginas", "50")]
        [Options("Principal", css: "col-md-4", listagem: false, pattern: "number", TypeData = "number")]
        public virtual int Paginas { get; set; }

        [PropriedadeInfo(ComponenteTipoEnum.TextBox, "Link para venda*", "Link para venda", "https://ofertas.mercadolivre.com.br/livros")]
        [Options("Principal", css: "col-md-4", listagem: false)]
        public virtual string Link { get; set; }

        [Obrigatorio(ErrorMessage = LivroMensagem.CAPAIDOBRIGATORIO, TipoValidacao = TipoValidacaoObrig.NumeroPositivo)]
        [PropriedadeInfo(ComponenteTipoEnum.Hidden, valor: "0")]
        public virtual long CapaId { get; set; }


        protected LivroCommand() : base(typeof(LivroMensagem))
        {
        }
    }
}
