using Core.Abstractions.Infrastructure.Data;
using Core.Abstractions.Infrastructure.Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using SGL.Core.Domain.Entities;
using SGL.Core.Domain.Service;
using SGL.Domain.Mensagens;
using SGL.Domain.Service.Livro;
using SGL.Domain.Validations;
using SGL.Infrastructure;
using SGL.Infrastructure.Repository;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace SGL.UnitTest.Setup
{
    public static class SetupTest
    {
        public static ProjetoContext ProjetoContext { get; private set; }
        
        public static LivroRepository LivroRepository { get; private set; }
        public static LivroValidacao LivroValidacao { get; private set; }
        public static LivroService LivroService { get; private set; }
        public static ImagemCacheRepository MemoryCacheService { get; private set; }
        public static BaseUnitOfWork UnitOfWork { get; private set; }
        public static LivroMensagem Mensagem { get; } = new LivroMensagem();

        public static void Setup()
        {
            var conf = ConfigurationOptions.Parse("localhost:32779", true);
            conf.AbortOnConnectFail = false;
            conf.ResolveDns = true;
            var redis = ConnectionMultiplexer.Connect(conf);

            var options = new DbContextOptionsBuilder<Contexto>().UseInMemoryDatabase(Guid.NewGuid().ToString());

            ProjetoContext = new ProjetoContext(options);
            LivroRepository = new LivroRepository(ProjetoContext);
            MemoryCacheService = new ImagemCacheRepository(redis);
            UnitOfWork = new BaseUnitOfWork(ProjetoContext);

            LivroValidacao = new LivroValidacao(LivroRepository, new LivroMensagem(), MemoryCacheService);
            LivroService = new LivroService(UnitOfWork, LivroRepository, LivroValidacao, MemoryCacheService, true);

            var listaImagems = new List<Imagem>
            {
                new Imagem(111,"A111AAA.jpg","image/jpeg",new byte[]{ 0,12,3} ),
                new Imagem(222,"A222AAA.jpg","image/jpeg",new byte[]{ 0,12,3} ),
                new Imagem(333,"A333AAA.jpg","image/jpeg",new byte[]{ 0,12,3} ),
                new Imagem(444,"A444AAA.jpg","image/jpeg",new byte[]{ 0,12,3} ),
                new Imagem(555,"A555AAA.jpg","image/jpeg",new byte[]{ 0,12,3} ),
                new Imagem(999,"A555AAA.jpg","image/jpeg",new byte[]{ 0,12,3} )
            };


            ProjetoContext.Context.AddRange(listaImagems);

            ProjetoContext.Context.AddRange(new List<LivroEntity>
            {
                new LivroEntity(111,"111AAA", 0, "Nome do LivroEntity - AAAAA","", "AAAAAAA-1","AAAAAAA-2","AAAAAAA-3","AAAAAAA-4",444, Convert.ToDateTime("10/10/2001"), null,listaImagems[0]),
                new LivroEntity(222,"222BBB", 0, "Nome do LivroEntity - BBBBB","", "BBBBBBB-1","BBBBBBB-2","BBBBBBB-3","BBBBBBB-4",333, Convert.ToDateTime("20/10/2000"), null,listaImagems[1]),
                new LivroEntity(333,"333CCC", 0, "Nome do LivroEntity - CCCCC","", "CCCCCCC-1","CCCCCCC-2","CCCCCCC-3","CCCCCCC-4",222, Convert.ToDateTime("30/10/2000"), null,listaImagems[2]),
                new LivroEntity(444,"444DDD", 0, "Nome do LivroEntity - DDDDD", "","DDDDDDD-1","DDDDDDD-2","DDDDDDD-3","DDDDDDD-4",111, Convert.ToDateTime("10/09/2001"), null,listaImagems[3]),
                new LivroEntity(555,"555EEE", 0, "Nome do LivroEntity - EEEEE","", "EEEEEEE-1","EEEEEEE-2","EEEEEEE-3","EEEEEEE-4",222, Convert.ToDateTime("20/09/2000"), null,listaImagems[4])
                
            });

            MemoryCacheService.SetAsync(listaImagems[5]);

            ProjetoContext.Context.SaveChanges();
        }


        public static LivroEntity CriarLivro()
        {


            var imagem = new Imagem(999, "A555AAA.jpg", "image/jpeg", new byte[] { 0, 12, 3 });

            var entity = new LivroEntity(999, "999AAA9", 999, "Nome do Livro Entity - AAAAA9", "Gênero do Livro AAA9", "Autor do Livro AAA9", "Editora do livro AAA9", "Descricao do livro AAA9", "Sinopse do livro AAA9", 150, Convert.ToDateTime("10/10/2000"), "https://ofertas.mercadolivre.com.br/livros", imagem);
            return entity;

        }


    }
}
