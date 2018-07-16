using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SGL.API.Configuration;
using SGL.APIConfiguration;
using SGL.Application;
using System;

namespace SGL.WebAPI
{
    /// <summary>
    /// Classe que faz o gerenciamento do pipeline da Aplicação
    /// </summary>
    public class Startup
    {
        private IHostingEnvironment Environment { get; }

        private IConfiguration Configuration { get; }

        /// <summary>
        /// Construtor da classe de gerenciamento
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Environment = environment;
            Configuration = configuration;
        }
        
        /// <summary>
        /// Método que realiza o registros das classes envolvidas no pipeline
        /// </summary>
        /// <param name="services"></param>     
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationDependencias(Configuration)
                    .AddApplicationInsights(Configuration)                    
                    .AddCustomSwagger(Configuration)
                    .AddRedis(Configuration)
                    .AddCustomCors()
                    .AddCustomMvc()
                    .AddOptions();

            

            //AutoFac
            var livroiner = new ContainerBuilder();
            livroiner.Populate(services);
            livroiner.RegisterModule(new ApplicationModule());
            return new AutofacServiceProvider(livroiner.Build());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>             
        /// <param name="loggerFactory"></param>     
        public void Configure(IApplicationBuilder application, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddApplicationInsights(application.ApplicationServices, LogLevel.Trace);
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            
            if (Environment.IsDevelopment())
            {
                application.UseDeveloperExceptionPage();
            }
            else
            {
                application.UseHsts();
            }

             application.UseCors(builder => builder.WithOrigins("*")            
            .AllowAnyHeader()
            .AllowAnyMethod())
            .UseMvc()
            .UseStaticFiles()
            .UseSwagger().UseSwaggerUI(s =>
            {                
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "SGL - API v1.0");
            });            
        }
    }
}