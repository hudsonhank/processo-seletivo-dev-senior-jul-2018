using Core.Abstractions.Application;
using Core.Abstractions.Attribute;
using MediatR;
using SGL.Domain.Mensagens;

namespace SGL.Application.Livro.Commands
{
    public class DesativarLivroCommand : Command, IRequest<CommandResult>
    {
        public virtual long Id { get; set; }
      
        public DesativarLivroCommand(long id) : base(typeof(LivroMensagem))
        {
            Id = id;            
        }
    }
}