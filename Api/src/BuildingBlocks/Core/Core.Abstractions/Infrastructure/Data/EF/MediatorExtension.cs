using Core.Abstractions.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Abstractions.Infrastructure.Data.EF
{
    static class MediatorExtension
    {
        public static async Task<bool> DispararEventoDominio(this IMediator mediator, IEnumerable<EntityEntry<Entity>> entities)
        {

            //try
            //{
            //var domainEntities = ctx.ChangeTracker
            //    .Entries<Entity>()
            //    .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEntities = entities.ToList().Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) => {
                    await mediator.Publish(domainEvent);
                });

            await Task.WhenAll(tasks);

            return true;
            //}
            //catch (System.Exception exc)
            //{
            //    throw exc;
            //}

        }
    }
}
