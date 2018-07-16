using AutoMapper;
using Core.Abstractions.Application;
using Core.Abstractions.Infrastructure.Data;
using MediatR;
using SGL.Application.Livro.Commands;
using SGL.Core.Domain.Entities;
using SGL.Domain.Mensagens;
using SGL.Domain.Service.Livro;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SGL.Application.Commands.Handlers
{
    public class LivroCommandHandler : CommandHandler,
                    IRequestHandler<ImagemCommand, CommandResult>,
                    IRequestHandler<CriarLivroCommand, CommandResult>,
                    IRequestHandler<EditarLivroCommand, CommandResult>,        
                    IRequestHandler<DesativarLivroCommand, CommandResult>
    {
        private readonly LivroService Service;
        private readonly LivroMensagem Alertas;

        public LivroCommandHandler(ILivroMensagem mensagems, ILivroService service, IUnitOfWork unitOfWork, IMediator mediator) : base(unitOfWork)
        {
            Service = (LivroService)service ?? throw new ArgumentNullException(nameof(service));
            Alertas = (LivroMensagem)mensagems ?? throw new ArgumentNullException(nameof(mensagems));
        }

        public async Task<CommandResult> Handle(CriarLivroCommand command, CancellationToken cancellationToken)
        {

            var Id = await Service.CriarNovoLivro(Mapper.Map<LivroEntity>(command), nameof(CriarLivroCommand), cancellationToken);
            return new CommandResult(true, Alertas.SucessoCriar.Texto, new { Id, command.Titulo });
        }

        public async Task<CommandResult> Handle(EditarLivroCommand command, CancellationToken cancellationToken)
        {
            var Id = await Service.EditarLivro(Mapper.Map<LivroEntity>(command), nameof(EditarLivroCommand), cancellationToken);
            return new CommandResult(true, Alertas.SucessoEditar.Texto, new { Id, command.Titulo });
        }

        public async Task<CommandResult> Handle(DesativarLivroCommand command, CancellationToken cancellationToken)
        {
            await Service.DesativarLivro(command.Id, nameof(DesativarLivroCommand), cancellationToken);
            return new CommandResult(true, Alertas.SucessoEditar.Texto, new { command.Id });
        }

        public async Task<CommandResult> Handle(ImagemCommand command, CancellationToken cancellationToken)
        {
            var entity = new Imagem(command.Nome, command.ContentType, command.Bytes);

            var capaId = await Service.UploadCapa(entity, nameof(ImagemCommand), cancellationToken);
            return new CommandResult(true, Alertas.SucessoUploadCapa.Texto + entity.Id, new { capaId, command.Nome });
        }
    }
}
