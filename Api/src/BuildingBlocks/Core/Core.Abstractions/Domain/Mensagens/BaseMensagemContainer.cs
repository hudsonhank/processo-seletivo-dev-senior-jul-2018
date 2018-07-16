
using System.Collections.Generic;
using System.Linq;

namespace Core.Abstractions.Domain.Mensagens
{
    public abstract class BaseMensagemLivroiner<T> : IMensagemLivroiner<BaseMensagem> where T : BaseMensagem
    {
        public IList<BaseMensagem> Mensagens { get; private set; }

        public IList<BaseMensagem> MensagensAlerta
        {
            get { return Mensagens.Where(c => c.Tipo == TipoMensagemEnum.Alerta).ToList(); }
        }

        public IList<BaseMensagem> MensagensErro
        {
            get { return Mensagens.Where(c => c.Tipo == TipoMensagemEnum.Erro).ToList(); }
        }

        public IList<BaseMensagem> MensagensInformacao
        {
            get { return Mensagens.Where(c => c.Tipo == TipoMensagemEnum.Informacao).ToList(); }
        }

        public IList<BaseMensagem> MensagensSucesso
        {
            get { return Mensagens.Where(c => c.Tipo == TipoMensagemEnum.Sucesso).ToList(); }
        }

        public BaseMensagemLivroiner()
        {
            Mensagens = new List<BaseMensagem>();
        }
        
        public void Add(BaseMensagem mensagem)
        {
            Mensagens.Add(mensagem);
        }

        public void AddAlerta(string chave, string texto)
        {
            Add(new BaseMensagem (chave, texto));
        }

        public void AddErro(string chave, string texto)
        {
            Add(new BaseMensagem(chave, texto, TipoMensagemEnum.Erro));            
        }

        public void AddInformacao(string chave, string texto)
        {
            Add(new BaseMensagem(chave, texto, TipoMensagemEnum.Informacao));            
        }

        public void AddSucesso(string chave, string texto)
        {
            Add(new BaseMensagem(chave, texto, TipoMensagemEnum.Sucesso));
        }
    }
}