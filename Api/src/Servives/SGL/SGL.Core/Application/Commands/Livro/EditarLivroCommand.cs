using Core.Abstractions.Application;
using Core.Abstractions.Attribute;
using Core.Abstractions.Attribute.Enum;
using MediatR;
using SGL.Application.Commands;
using SGL.Domain.Mensagens;

namespace SGL.Application.Livro.Commands
{
    public class EditarLivroCommand : LivroCommand, IRequest<CommandResult>
    {
        [Obrigatorio(ErrorMessage = LivroMensagem.IDOBRIGATORIO)]
        [PropriedadeInfo(ComponenteTipoEnum.Hidden, valor:"0")]
        public string Id { get; set; }
        [PropriedadeInfo(ComponenteTipoEnum.Hidden)]
        public virtual string TextoExtra { get; set; }

        public EditarLivroCommand() : base()
        {
            Id = "0";
        }

        public EditarLivroCommand(string id) : base()
        {
            Id = id;            
        }
    }
}
