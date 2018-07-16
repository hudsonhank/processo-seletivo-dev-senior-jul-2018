using Core.Abstractions.Attribute;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Abstractions.Infrastructure.Data.EF
{
    public class Contexto : DbContext
    {
        private Assembly Entities;
        //private IMediator Mediator;

        public Contexto(DbContextOptions<Contexto> options, Assembly entities/*, IMediator mediator*/) : base(options)
        {
            Entities = entities ?? throw new ArgumentNullException(nameof(entities));
            //Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        //public DbSet<Threat> Threats { get; set; }        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityType = Entities.GetTypes().Where(t => t.GetCustomAttribute(typeof(MapAttribute), false) != null).FirstOrDefault();
            modelBuilder.UseEntityTypeConfiguration(Assembly.GetAssembly(entityType));

            base.OnModelCreating(modelBuilder);
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    //modelBuilder.HasDefaultSchema(schema: DBGlobals.SchemaName);

        //    //modelBuilder.Entity<Threat>().HasIndex(i => i.Referer).IsUnique();

        //    //modelBuilder.Entity<Threat>().Property(p => p.Identifier).HasComputedColumnSql("CONCAT('" + DBGlobals.IdentifierFormat + "',[Id])");

        //    //modelBuilder.Entity<ThreatType>();

        //    //modelBuilder.Entity<Status>();

        //    base.OnModelCreating(modelBuilder);
        //}


        public override int SaveChanges()
        {
            //Task.FromResult(Audit());
            return base.SaveChanges();
        }

        //private async Task Audit()
        //{
        //    var entries = ChangeTracker.Entries().Where(x => x.Entity is Entity && (x.State == EntityState.Added || x.State == EntityState.Modified));

        //    foreach (var entry in entries)
        //    {
        //        if (entry.State == EntityState.Added)
        //        {
        //            ((Entity)entry.Entity).DataCadastro = DateTime.UtcNow;
        //        }

        //        ((Entity)entry.Entity).DataAtualizacao = DateTime.UtcNow;
        //    }

        //    var executado = await Mediator.DispararEventoDominio(ChangeTracker.Entries<Entity>());
        //}

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            //await Audit();

            //var cont = this.ChangeTracker.Entries<Entity>();

            //await _mediator.DispararEventoDominio(cont);

            //await _mediator.DispararEventoDominio(this);

            return await base.SaveChangesAsync();
        }
    }
}
