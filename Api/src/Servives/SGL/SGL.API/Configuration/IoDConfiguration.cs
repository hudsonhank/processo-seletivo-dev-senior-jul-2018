using Audit.Elasticsearch.Providers;
using AutoMapper;
using Core.Abstractions.Application;
using Core.Abstractions.Infrastructure.Data;
using MediatR;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.ServiceFabric;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SGL.API.Configuration;
using SGL.Application.Commands;
using SGL.Core.Domain.Service;
using SGL.Infrastructure;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Reflection;

namespace SGL.APIConfiguration
{
    /// <summary>
    /// Classe que gerencia a injeção das interfaces do projeto
    /// </summary>
    public static class IoDConfiguration
    {
        /// <summary>
        /// Aplica a injeção das classes base da aplicação
        /// </summary>
        /// <param name="services"></param>             
        /// <param name="configuration"></param>   
        public static IServiceCollection AddApplicationDependencias(this IServiceCollection services, IConfiguration configuration)
        {
            //Http
            services.AddSingleton(configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Memory Cache
            services.AddScoped<IMemoryCache, MemoryCacheService>();
            services.AddMemoryCache();

            // Context                                    
            services.AddScoped<IUnitOfWork, BaseUnitOfWork>();
            services.AddScoped<IDatabaseContext, ProjetoContext>();

            //configure autofac
            services.AddTransient<CommandHandler, CommandHandler>();

            //IMPORTANTE Buscar todos os Namespace Mediatr
            services.AddMediatR(typeof(LivroCommand).GetTypeInfo().Assembly);
          
            //AddAutoMapper
            services.AddAutoMapper();
            services.AddSingleton(Mapper.Configuration);
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));

            return services;
        }

        /// <summary>
        /// Customiza a injeção do pipeline do MVC
        /// </summary>
        /// <param name="services"></param>                     
        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {                
            }).AddJsonOptions(options =>
            {
                options.SerializerSettings.Formatting = Formatting.Indented;             
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            services.AddCors(options =>
            {
                options.AddPolicy("corsGlobalPolicy",                        
                        option => option.WithOrigins("http://localhost").AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            });
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("corsGlobalPolicy"));
            });

            return services;
        }

        /// <summary>
        /// Customiza a injeção do pipeline do Application Insights
        /// </summary>
        /// <param name="services"></param>             
        /// <param name="configuration"></param>             
        public static IServiceCollection AddApplicationInsights(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationInsightsTelemetry(configuration);
            var orchestratorType = configuration.GetValue<string>("OrchestratorType");

            if (orchestratorType?.ToUpper() == "K8S")
            {
                // Enable K8s telemetry initializer
                services.EnableKubernetes();
            }
            if (orchestratorType?.ToUpper() == "SF")
            {
                // Enable SF telemetry initializer
                services.AddSingleton<ITelemetryInitializer>((serviceProvider) =>
                    new FabricTelemetryInitializer());
            }

            return services;
        }

        /// <summary>
        /// Customiza a injeção do pipeline do Swagger
        /// </summary>
        /// <param name="services"></param>             
        /// <param name="configuration"></param>             
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.OperationFilter<FileUploadOperation>(); //Register File Upload Operation Filter
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Projeto SGL - HTTP API",
                    Version = "v1",
                    Description = "SGL - Métodos para manipular as informações da livraria",
                    TermsOfService = "Termos de aceite do uso da API do SGL",
                    Contact = new Contact() { Name = "SGL", Email = "hudsonhank@gmail.com", Url = "https://www.linkedin.com/in/hudson-ricardo-oliveira-a2a57027" }
                });
                
            });

            return services;
        }

        /// <summary>
        /// Customiza a injeção do pipeline do Cors da aplicação
        /// </summary>
        /// <param name="services"></param>                     
        public static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            var policy = new Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy();
            //policy.SupportsCredentials = true;
            services.AddCors(o => o.AddPolicy("corsGlobalPolicy", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));
            return services;
        }

        /// <summary>
        /// Customiza a injeção do pipeline do Cache Redis da aplicação
        /// </summary>
        /// <param name="services"></param>             
        /// <param name="configuration"></param>   
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            //DB-Cache-Redis
            services.AddSingleton(sp =>
            {
                var conf = ConfigurationOptions.Parse(configuration["Connection:Cache"], true);
                conf.AbortOnConnectFail = false;
                conf.ResolveDns = true;
                return ConnectionMultiplexer.Connect(conf);
            });
            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>             
        /// <param name="configuration"></param>   
        public static IServiceCollection AddRegisterEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            var subscriptionClientName = configuration["SubscriptionClientName"];

            //services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            //{
            //    var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
            //    var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
            //    var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
            //    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
            //    var retryCount = 5;
            //    if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
            //    {
            //        retryCount = int.Parse(configuration["EventBusRetryCount"]);
            //    }
            //    return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
            //});

            //services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            return services;
        }
    }
}
