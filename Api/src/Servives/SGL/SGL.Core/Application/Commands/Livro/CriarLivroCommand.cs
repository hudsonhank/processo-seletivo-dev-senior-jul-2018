using Core.Abstractions.Application;
using Core.Abstractions.Attribute;
using Core.Abstractions.Attribute.Enum;
using MediatR;
using SGL.Application.Commands;
using SGL.Domain.Mensagens;

namespace SGL.Application.Livro.Commands
{
    //[Map(nameof(LivroMensagem))]
    public class CriarLivroCommand : LivroCommand, IRequest<CommandResult>

    {
        //[PropriedadeInfo(ComponenteTipoEnum.Hidden, valor: "0")]
        //public string Id { get; set; }


        public CriarLivroCommand() : base()
        {
        }
    }
}
