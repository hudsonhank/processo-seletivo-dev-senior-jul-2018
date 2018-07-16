using Core.Abstractions.Domain;
using Core.Abstractions.Infrastructure.Data;
using Microsoft.Extensions.Caching.Memory;
using SGL.Core.Domain.Entities;
using SGL.Core.Domain.Entities.Queries;
using SGL.Core.Domain.Service;
using SGL.Domain.Events;
using SGL.Domain.Repository;
using SGL.Domain.Validations;
using SGL.Infrastructure.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SGL.Domain.Service.Livro
{
    public interface ILivroService : IBaseService<LivroEntity>
    {
        Task<string> UploadCapa(Imagem capa, string comandName, CancellationToken cancellationToken = default(CancellationToken));
        Task DesativarLivro(long id, string comandName, CancellationToken cancellationToken = default(CancellationToken));
        Task<object> CriarNovoLivro(LivroEntity entity, string comandName, CancellationToken cancellationToken = default(CancellationToken));
        Task<object> EditarLivro(LivroEntity entity, string comandName, CancellationToken cancellationToken = default(CancellationToken));
    }

    public class LivroService : BaseService<LivroEntity>, ILivroService
    {
        public IImagemCacheRepository ImagemCache { get; }
        
        public LivroService(IUnitOfWork unitOfWork, ILivroRepository repository, ILivroValidacao validacao, IImagemCacheRepository imagemCache, bool inMemory = false) : base(repository, validacao, unitOfWork, inMemory)
        {
            ImagemCache = imagemCache ?? throw new ArgumentException(nameof(imagemCache));         
        }

        private void CriarIndexPequisaLivroEvent(LivroEntity entity)
        {
            var bus = new LivroQuery(entity.Id.ToString(), entity.CodigoUnico, entity.Titulo, entity.Autor, entity.Genero, entity.Editora, entity.DataPublicacao,(entity.Capa!=null)? entity.Capa.Nome:"");
            entity.AddDomainEvent(new AtualizarIndexPequisaLivroEvent(bus));
        }

        public async Task<string> UploadCapa(Imagem capa, string comandName, CancellationToken cancellationToken = default(CancellationToken))
        {
            var valido  = await ((LivroValidacao)Validacao).ValidarUploadCapa(capa);

            var existeCapa = ((ImagemCacheRepository)ImagemCache).ExistId(capa.Id.ToString());
            if (!existeCapa)
            {
                await ImagemCache.SetAsync(capa);
            }

            return capa.Id.ToString();
        }

        public async Task DesativarLivro(long id, string comandName, CancellationToken cancellationToken = default(CancellationToken))
        {
            var entity = await ((LivroValidacao)Validacao).ValidarDesativarLivro(id);            
            entity.Ativo = false;
            entity.DataAtualizacao = DateTime.UtcNow;
            await Repository.Excluir(entity.Id);                            
        }

        public async Task<object> CriarNovoLivro(LivroEntity entity, string comandName, CancellationToken cancellationToken = default(CancellationToken))
        {




            return await ExecuteAsync(async () =>
            {
                await ((LivroValidacao)Validacao).ValidarCriarNovoLivro(entity);

                var capa = await ImagemCache.GetAsync(entity.CapaId.ToString(), true);
                if (capa!=null)
                {
                    entity.Capa = capa;
                }

                entity.GerarNovoLivro();
                CriarIndexPequisaLivroEvent(entity);
                await Repository.Criar(entity, cancellationToken);
                capa.Ativo = entity.Ativo;
                capa.DataCadastro = entity.DataCadastro;
                capa.TId = entity.TId;
                await ((ImagemCacheRepository)ImagemCache).DeleteAsync(entity.CapaId.ToString());

                return entity.Id;
            });
        }

        public async Task<object> EditarLivro(LivroEntity entity, string comandName, CancellationToken cancellationToken = default(CancellationToken))
        {

            return await ExecuteAsync(async () =>
            {
                var codigoOriginal = await ((LivroValidacao)Validacao).ValidarEditarInformacoes(entity);

                entity.ManterDados(codigoOriginal);

                CriarIndexPequisaLivroEvent(entity);
                await Repository.Editar(entity);
                return entity.CodigoUnico;
            });

            
        }
    }
}

