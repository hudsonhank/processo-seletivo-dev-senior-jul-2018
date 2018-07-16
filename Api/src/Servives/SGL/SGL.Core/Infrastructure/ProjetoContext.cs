using Core.Abstractions.Infrastructure.Data;
using Core.Abstractions.Infrastructure.Data.EF;
using Core.Abstractions.Infrastructure.Data.Enums;
using Core.Abstractions.Infrastructure.Databases;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SGL.Infrastructure.EntityTypeMapping;
using System.Reflection;

namespace SGL.Infrastructure
{
    public class ProjetoContext : EFContext, IDatabaseContext
    {
        public ProjetoContext(DbContextOptionsBuilder<Contexto> builder) : base(builder, Assembly.GetAssembly(typeof(LivroEntityTypeMapping)))
        {
        }

        public ProjetoContext(IConfiguration Configuration, IMediator mediator) : base(new SQLServer(DatabaseVersionEnum.v2016),
           Configuration["Connection:Relacional"], Assembly.GetAssembly(typeof(LivroEntityTypeMapping)), mediator)
        {         
        }
    }
}
