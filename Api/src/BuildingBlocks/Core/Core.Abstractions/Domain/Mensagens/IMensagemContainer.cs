using System.Collections.Generic;

namespace Core.Abstractions.Domain.Mensagens
{
    public interface IMensagemLivroiner<T> where T : class
    {
        
        void AddAlerta(string chave, string texto);
        void AddErro(string chave, string texto);
        void AddInformacao(string chave, string texto);
        void AddSucesso(string chave, string texto);
    }
}
