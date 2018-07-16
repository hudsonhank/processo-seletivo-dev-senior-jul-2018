using Core.Abstractions.Application;
using MediatR;
using SGL.Domain.Mensagens;

namespace SGL.Application.Commands
{
    public class ImagemCommand : Command, IRequest<CommandResult>
    {
        public virtual string Nome { get; set; }
        public virtual string CodigoUnico { get; private set; }
        public virtual string ContentType { get; set; }
        public virtual byte[] Bytes { get; set; }

        public ImagemCommand(string nome,string contentType) : base(typeof(LivroMensagem))
        {
            Nome = nome;
            ContentType = contentType;
        }
    }
}
