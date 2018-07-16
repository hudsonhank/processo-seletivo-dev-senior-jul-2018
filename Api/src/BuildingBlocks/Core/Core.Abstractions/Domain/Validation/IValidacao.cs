using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Abstractions.Domain.Validation.Interfaces
{

    public interface IValidacao<T> 
        where T : class
        
    {        
        IList<Validacao> Invalidos { get; }
        IList<Validacao> Validacoes { get; }
        IList<Validacao> ValidacoesCriar { get; }
        IList<Validacao> ValidacoesEditar { get; }
        IList<Validacao> ValidacoesDesabilitar { get; }
        IList<Validacao> ValidacoesListar { get; }
        IList<Validacao> ValidacoesAtributos(T item);

        bool ValidarAtributos(T item);
        void ValidarCriar(T item, IList<Validacao> validacoes = null);
        void ValidarListar(Expression<Func<T, bool>> filter = null);
        void ValidarEditar(T item, IList<Validacao> validacoes = null);
        void ValidarDesabilitar(T item, IList<Validacao> validacoes = null);

        
        void EncerrarSeInvalido();
        void EncerrarSeNaoEncontrado(object item);
        void Add(Validacao validacao);        
        void Add(object valor, string mensagem);        
        void AddInvalido(IList<Validacao> validacoes);


        //Crud
        void AddCriar(Validacao validacao);
        void AddEditar(Validacao validacao);
        void AddDesabilitar(Validacao validacao);
        void AddListar(Validacao validacao);

        //void CriarValidacoesRegrasNegocio(T item);
        //void CriarValidacoesComuns(T item);
    }
}