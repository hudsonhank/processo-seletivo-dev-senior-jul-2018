namespace Core.Abstractions.Domain.Mensagens
{
	public interface IMensagem
    {
        TipoMensagemEnum Tipo { get; set; }
        string TipoTexto { get; }
        string Texto { get; set; }

        
    }
}