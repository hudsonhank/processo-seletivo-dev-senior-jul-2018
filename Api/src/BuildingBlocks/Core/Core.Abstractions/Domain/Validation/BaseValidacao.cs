using Core.Abstractions.Domain.Enums;
using Core.Abstractions.Domain.Mensagens;
using Core.Abstractions.Domain.Validation.Extensions;
using Core.Abstractions.Domain.Validation.Interfaces;
using Core.Abstractions.Types.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Abstractions.Domain.Validation
{
    public class BaseValidacao<T> : IValidacao<T> where T : class        
    {
        #region Propriedades

        public IMensagemLivroiner<IMensagem> MensagemLivroiner { get; set; }
        
        public IList<Validacao> Invalidos { get; set; }

        public IList<Validacao> Validacoes { get; set; }
        
        public IList<Validacao> ValidacoesCriar
        {
            get
            {
                return Validacoes.Where(c => c.Acao == Acao.Criar).ToList() ?? new List<Validacao>();
            }
        }

        public IList<Validacao> ValidacoesListar
        {
            get
            {
                return Validacoes.Where(c => c.Acao == Acao.Listar).ToList() ?? new List<Validacao>();
            }
        }

        public IList<Validacao> ValidacoesEditar
        {
            get
            {
                return Validacoes.Where(c => c.Acao == Acao.Editar).ToList() ?? new List<Validacao>();
            }
        }

        public IList<Validacao> ValidacoesDesabilitar
        {
            get
            {
                return Validacoes.Where(c => c.Acao == Acao.Desabilitar).ToList() ?? new List<Validacao>();
            }
        }

        #endregion

        public BaseValidacao()
        {
            Invalidos = new List<Validacao>();
            Validacoes = new List<Validacao>();            
        }

        public BaseValidacao(IMensagemLivroiner<IMensagem> mensagemLivroiner)
        {
            Invalidos = new List<Validacao>();
            Validacoes = new List<Validacao>();
            MensagemLivroiner = mensagemLivroiner;            
        }

        #region Métodos

        public IList<Validacao> ValidacoesAtributos(T item)
        {
            if (item != null)
            {
                return item.GetValidacoes();
            }
            return null;
        }

        public virtual void Add(Validacao validacao)
        {
            Validacoes.Add(validacao);
        }
                
        public virtual void AddCriar(Validacao validacao)
        {
            validacao.Acao = Acao.Criar;
            Add(validacao);
        }
               
        public virtual void AddEditar(Validacao validacao)
        {
            validacao.Acao = Acao.Editar;
            Add(validacao);
        }
               
        public virtual void AddDesabilitar(Validacao validacao)
        {
            validacao.Acao = Acao.Desabilitar;
            Add(validacao);
        }
               
        public virtual void AddListar(Validacao validacao)
        {
            validacao.Acao = Acao.Listar;
            Add(validacao);
        }
               
        public virtual void Add(object valor, string mensagem)
        {
            Validacoes.Add(new Validacao { Valor = valor, Mensagem = new BaseMensagem(mensagem) });
        }
               
        public virtual void AddInvalido(IList<Validacao> validacoes)
        {
            if(validacoes==null)
            {
                validacoes = new List<Validacao>();
            }
            
            var invalidos = validacoes.Where(x => !x.IsValido()).ToList();

            invalidos.ForEach(c =>
            {
                Invalidos.Add(c);
            });
        }

        public virtual void AddInvalido(Validacao validacao)
        {
            if(!validacao.IsValido())
            {
                if (Invalidos == null)
                {
                    Invalidos = new List<Validacao>();
                }
                Invalidos.Add(validacao);
            }            
        }

        public virtual bool ValidarAtributos(T item)
        {
            EncerrarSeNaoEncontrado(item);

            AddInvalido(ValidacoesAtributos(item));

            return Invalidos.Any();
        }

        public virtual void ValidarCriar(T item, IList<Validacao> validacoes = null)
        {

            EncerrarSeNaoEncontrado(item);

            if (validacoes == null)
            {
                validacoes = ValidacoesCriar;
            }

            ValidarAtributos(item);

            AddInvalido(validacoes);
        }

        public virtual void ValidarEditar(T item, IList<Validacao> validacoes = null)
        {
            EncerrarSeNaoEncontrado(item);

            if (validacoes == null)
            {
                validacoes = ValidacoesEditar;
            }
            ValidarAtributos(item);
            AddInvalido(validacoes);
        }
                
        public virtual void ValidarDesabilitar(T item, IList<Validacao> validacoes = null)
        {
            EncerrarSeNaoEncontrado(item);

            if (validacoes == null)
            {
                validacoes = ValidacoesDesabilitar;
            }
            
            AddInvalido(validacoes);
        }

        public virtual void ValidarListar(Expression<Func<T, bool>> filter = null)
        {
            EncerrarSeNaoEncontrado(filter);
            
            if(ValidacoesListar !=null)
            {
                AddInvalido(ValidacoesListar);
            }
        }

        public virtual void EncerrarSeInvalido()
        {
            if (Invalidos.Any())
            {
                throw new RequisicaoInvalidaException(Invalidos);
            }
        }
               
        public virtual void EncerrarSeNaoEncontrado(object item)
        {
            if (item == null)
            {
                throw new NaoEncontradoException();
            }
        }

        public virtual void CriarValidacoesComuns(T item)
        {
            throw new NotImplementedException();
        }

        public void CriarValidacoesRegrasNegocio(T item)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}