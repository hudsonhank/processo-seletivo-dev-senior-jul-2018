using Core.Abstractions.Domain.Validation;
using Core.Abstractions.Domain.Validation.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using SGL.Core.Domain.Entities;
using SGL.Core.Domain.Service;
using SGL.Domain.Mensagens;
using SGL.Domain.Repository;
using SGL.Infrastructure.Repository;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SGL.Domain.Validations
{
    public interface ILivroValidacao : IValidacao<LivroEntity>
    {
        Task<bool> ValidarUploadCapa(Imagem capa);
        Task<LivroEntity> ValidarDesativarLivro(long id);
        Task<string> ValidarEditarInformacoes(LivroEntity entity);
        Task ValidarCriarNovoLivro(LivroEntity entity);
    }

    public class LivroValidacao : BaseValidacao<LivroEntity>, ILivroValidacao
    {
        public readonly ILivroRepository Repositorio;
        public IImagemCacheRepository ImagemCache { get; }

        public LivroValidacao(ILivroRepository repositorio, ILivroMensagem mensagem, IImagemCacheRepository imagemCache) : base(mensagem)
        {
            Repositorio = repositorio;
            ImagemCache = imagemCache ?? throw new ArgumentException(nameof(imagemCache));
        }

        public async Task<bool> ValidarUploadCapa(Imagem capa)
        {
            if (!new String[] { "image/jpeg", "image/jpg", "image/bmp", "image/gif" }.Contains(capa.ContentType.ToLower()))
            {
                AddInvalido(new RegraNegocioValidacao<LivroEntity>(true, ((LivroMensagem)MensagemLivroiner).ImagemTipoCapaInvalido));
                EncerrarSeInvalido();
                return await Task.FromResult(false);
            }            
            return await Task.FromResult(true);
        }

        public async Task<LivroEntity> ValidarDesativarLivro(long id)
        {
            var transiente = await Repositorio.BuscarPorId(id);

            EncerrarSeNaoEncontrado(transiente);

            if (!transiente.Ativo)
            {
                AddInvalido(new RegraNegocioValidacao<LivroEntity>(true, ((LivroMensagem)MensagemLivroiner).TituloJaCadastrado));
                EncerrarSeInvalido();
            }

            return transiente;
        }

        private async Task ValidacoesEmComun(LivroEntity entity)
        {
            //Cria a consulta do Título do livro
            Expression<Func<LivroEntity, bool>> expressionTitulo = (c => (c.Ativo) && c.Id != entity.Id && c.Titulo.ToLower().Trim() == entity.Titulo.ToLower().Trim());
            Expression<Func<LivroEntity, bool>> expressionImagem = (c => (c.Ativo) && c.Id != entity.Id && c.CapaId == entity.CapaId);

            if (await Task.FromResult( Repositorio.Filtrar().Any(expressionTitulo)))
            {
                AddInvalido(new RegraNegocioValidacao<LivroEntity>(true, ((LivroMensagem)MensagemLivroiner).TituloJaCadastrado));
                EncerrarSeInvalido();
            }

            if (await Task.FromResult(Repositorio.Filtrar().Any(expressionImagem)))
            {
                AddInvalido(new RegraNegocioValidacao<LivroEntity>(true, ((LivroMensagem)MensagemLivroiner).ImagemCapaJaCadastrado));
                EncerrarSeInvalido();
            }

            var naoExisteCapa = !((ImagemCacheRepository)ImagemCache).ExistId(entity.CapaId.ToString());
            if (naoExisteCapa)
            {
                AddInvalido(new RegraNegocioValidacao<LivroEntity>(true, ((LivroMensagem)MensagemLivroiner).RequisicaoInvalida));
                EncerrarSeInvalido();
            }
        }

        public async Task ValidarCriarNovoLivro(LivroEntity entity)
        {
            base.ValidarCriar(entity);
            await ValidacoesEmComun(entity);            
            EncerrarSeInvalido();
        }

        public async Task<string> ValidarEditarInformacoes(LivroEntity entity)
        {
            var transiente = await Repositorio.BuscarPorId(entity.Id);
            if (transiente == null || (transiente!=null && !transiente.Ativo ))
            {
                AddInvalido(new RegraNegocioValidacao<LivroEntity>(true, ((LivroMensagem)MensagemLivroiner).LivroNaoExiste));
                EncerrarSeInvalido();
            }

            base.ValidarEditar(entity);

            await ValidacoesEmComun(entity);
            
            EncerrarSeInvalido();

            return transiente.CodigoUnico;
        }
    }
}