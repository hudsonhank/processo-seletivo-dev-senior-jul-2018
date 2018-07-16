using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.Abstractions.Infrastructure.Data.EF
{
    public interface IEntityTypeMapping<TEntity> where TEntity : class
    {
        //void Map(ModelBuilder modelBuilder);
        void Map(EntityTypeBuilder<TEntity> entityTypeBuilder);
    }

    public static class EntityTypeConfigurationExtensions
    {
        private static readonly MethodInfo EntityMethod = typeof(ModelBuilder).GetTypeInfo().GetMethods().Single(x => x.Name == "Entity" 
        && x.IsGenericMethod && x.GetParameters().Length == 0);
        private static readonly IDictionary<Assembly, IEnumerable<Type>> TypesPerAssembly = new Dictionary<Assembly, IEnumerable<Type>>();

        private static Type GetEntityType(Type type)
        {
            Type interfaceType = type.GetInterfaces().First(x => x.GetTypeInfo().IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntityTypeMapping<>));
            return interfaceType.GetGenericArguments().First();
        }

        private static ModelBuilder ApplyConfiguration<T>(this ModelBuilder modelBuilder, IEntityTypeMapping<T> configuration) where T : class
        {
            Type entityType = GetEntityType(configuration.GetType());

            dynamic entityTypeBuilder = EntityMethod
                .MakeGenericMethod(entityType)
                .Invoke(modelBuilder, new object[0]);

            configuration.Map(entityTypeBuilder);

            return modelBuilder;
        }
        
        public static ModelBuilder UseEntityTypeConfiguration(this ModelBuilder modelBuilder, Assembly CurrentAssembly)
        {

            if (!TypesPerAssembly.TryGetValue(CurrentAssembly, out IEnumerable<Type> configurationTypes))
            {
                TypesPerAssembly[CurrentAssembly] = configurationTypes = CurrentAssembly
                    .GetExportedTypes()
                    .Where(x => x.GetTypeInfo().IsClass && !x.GetTypeInfo().IsAbstract && x.GetInterfaces().Any(y => y.GetTypeInfo().IsGenericType 
                    && y.GetGenericTypeDefinition() == typeof(IEntityTypeMapping<>)));
            }

            IEnumerable<dynamic> configurations = configurationTypes.Select(Activator.CreateInstance);

            foreach (dynamic configuration in configurations)
            {
                ApplyConfiguration(modelBuilder, configuration);
            }

            return modelBuilder;
        }
    }


}
