using Autofac;
using SGL.Application.Livro.Queries;
using SGL.Domain.Mensagens;
using SGL.Domain.Repository;
using SGL.Domain.Service.Livro;
using SGL.Domain.Validations;
using SGL.Infrastructure.Repository;

namespace SGL.API.Configuration
{
    /// <summary>
    /// Responsável por fazer o registro das interfaces do módulo da aplicação do projeto
    /// </summary>
    public class ApplicationModule : Module
    {
        /// <summary>
        /// Construtor base
        /// </summary>      
        public ApplicationModule()
        {
        }

        /// <summary>
        /// Método que executa o registro das interfaces do módulo da aplicação
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            //Livro
            builder.RegisterType<LivroRepository>().As<ILivroRepository>().InstancePerLifetimeScope();
            builder.RegisterType<LivroService>().As<ILivroService>().InstancePerLifetimeScope();
            builder.RegisterType<LivroValidacao>().As<ILivroValidacao>().InstancePerLifetimeScope();
            builder.RegisterType<LivroMensagem>().As<ILivroMensagem>().InstancePerLifetimeScope();
            //Query-Livro
            builder.RegisterType<LivroCacheRepository>().As<ILivroCacheRepository>().SingleInstance();
            builder.RegisterType<ImagemCacheRepository>().As<IImagemCacheRepository>().SingleInstance();
            builder.RegisterType<LivroQueries>().As<ILivroQueries>().SingleInstance();
        }
    }
}
