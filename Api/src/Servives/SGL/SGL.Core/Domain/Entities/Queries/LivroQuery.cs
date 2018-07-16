using Newtonsoft.Json;
using System;

namespace SGL.Core.Domain.Entities.Queries
{
    public class LivroQuery //: BaseQuery<LivroQuery>
    {        
        public string Id { get; set; }
        public virtual string CodigoUnico { get; set; }
        public virtual string Titulo { get; set; }
        public virtual string Autor { get; set; }
        public virtual string Genero { get; set; }
        public virtual string Editora { get; set; }
        public virtual int PublicacaoAno { get; set; }
        public virtual DateTime DataPublicacao { get; set; }
        public virtual string TextoExtra { get; set; }

        public LivroQuery()
        {
        }

        public LivroQuery(string id, string codigoUnico, string titulo, string autor, string genero, string editora, DateTime dataPublicacao, string imagemNome)
        {
            Id = id;
            CodigoUnico = codigoUnico;
            Titulo = titulo;
            Autor = autor;
            Genero = genero;
            Editora = editora;
            PublicacaoAno = dataPublicacao.Year;
            DataPublicacao = dataPublicacao;
            TextoExtra = imagemNome;
        }

        //public override Expression<Func<LivroQuery, bool>> QueryExpression()
        //{
        //    Expression<Func<LivroQuery, bool>> expression = (c => (c.Ativo));
        //    Expression<Func<LivroQuery, bool>> TituloExpression = (c => (c.Titulo ?? string.Empty).ToUpper().Livroins(Titulo.ToUpper()));
        //    Expression<Func<LivroQuery, bool>> CodigoUnicoExpression = (c => (c.CodigoUnico ?? string.Empty).ToUpper().Livroins(CodigoUnico.ToUpper()));
        //    Expression<Func<LivroQuery, bool>> MarcaCodigoExpression = (c => (c.MarcaCodigo ?? string.Empty).ToUpper().Livroins(MarcaCodigo.ToUpper()));

        //    if (!string.IsNullOrEmpty(Titulo))
        //    {
        //        expression = expression == null ? TituloExpression : expression.CombineWithAndAlso(TituloExpression);
        //    }

        //    if (!string.IsNullOrEmpty(CodigoUnico))
        //    {
        //        expression = expression == null ? CodigoUnicoExpression : expression.CombineWithAndAlso(CodigoUnicoExpression);
        //    }

        //    if (!string.IsNullOrEmpty(MarcaCodigo))
        //    {
        //        expression = expression == null ? MarcaCodigoExpression : expression.CombineWithAndAlso(MarcaCodigoExpression);
        //    }

        //    return expression;
        //}
    }
}
