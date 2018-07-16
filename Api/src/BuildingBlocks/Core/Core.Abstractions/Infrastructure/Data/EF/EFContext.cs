using Core.Abstractions.Attribute;
using Core.Abstractions.Domain;
using Core.Abstractions.Extension;
using Core.Abstractions.Infrastructure.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Abstractions.Infrastructure.Data.EF
{
    public class EFContext : IDatabaseContext
    {
        #region Propriedades
        public IDatabase Database { get; set; }
        public DbContext Context { get; private set; }
        private readonly Assembly Entities;
        private readonly IMediator Mediator;
        private string ConnectionString { get; set; }
        private string TId { get; set; }
        #endregion

        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
            }
        }

        public EFContext()
        {
        }

        public EFContext(DbContextOptionsBuilder<Contexto> builder, Assembly entities/*, IMediator mediator*/)
        {                        
            Entities = entities;
            //Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            CriarContext(builder);
        }

        public EFContext(IDatabase database, string connectionString, Assembly entities, IMediator mediator)
        {
            Database = database;
            ConnectionString = connectionString;
            Entities = entities;
            Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            CriarContext();
        }

        private void CriarContext(DbContextOptionsBuilder<Contexto> builder = null)
        {
            if(builder!=null && Context == null)
            {
                builder.EnableSensitiveDataLogging();
                Context = new Contexto(builder.Options, Entities);
                return;
            }
            
            if (Database == null)
            {
                throw new System.Exception("Database não pode ser nulo.");
            }

            if (ConnectionString == string.Empty)
            {
                throw new System.Exception("String de conexão não pode ser nulo.");
            }

            if (Context == null)
            {
                switch (Database.DatabaseName)
                {
                    #region Oracle
                    case DatabaseEnum.Oracle:
                        throw new System.Exception("Database ORACLE em construção.");
                    #endregion

                    #region SQLServer
                    case DatabaseEnum.SQLServer:
                        if (Context == null)
                        {
                            var SDKbuilder = new DbContextOptionsBuilder<Contexto>();                           
                            SDKbuilder.UseSqlServer(ConnectionString);
                            SDKbuilder.EnableSensitiveDataLogging();
                            Context = new Contexto(SDKbuilder.Options, Entities);
                        }
                        break;
                    #endregion

                    default:
                        throw new System.Exception("Database não pode ser nulo.");
                }
            }
        }

        #region Transaction

        public void BeginTransaction()
        {
            if (Context != null && Context.Database != null && Context.Database.CurrentTransaction == null)
            {
                Context.Database.BeginTransaction();
            }

            if (string.IsNullOrEmpty(TId))
            {
                TId = Guid.NewGuid().ToString("N");
            }
        }

        public async Task Commit(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                if (Context != null && Context.Database != null && Context.Database.CurrentTransaction != null)
                {
                    if(Mediator!=null)
                    {
                        var executado = await Mediator.DispararEventoDominio(Context.ChangeTracker.Entries<Entity>());
                        if (executado)
                        {
                            await Context.SaveChangesAsync(cancellationToken);
                            Context.Database.CommitTransaction();
                        }
                    }
                    else
                    {
                        await Context.SaveChangesAsync(cancellationToken);
                        Context.Database.CommitTransaction();
                    }

                    //await Context.SaveChangesAsync(cancellationToken);
                    //Context.Database.CommitTransaction();
                }
                //TId = string.Empty;
            }
            catch (Exception exc)
            {
                Rollback();
                throw exc;
            }
        }

        public void Rollback()
        {
            try
            {
                if (Context != null && Context.Database != null && Context.Database.CurrentTransaction != null)
                {
                    Context.Database.RollbackTransaction();
                }
                //TId = string.Empty;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        #endregion

        #region CRUD

        private async Task<T> ApplyValores<T>(T entity, bool atualizacao = true) where T : Entity
        {
            T transiente = null;
            if (atualizacao)
            {
                var id = Convert.ToInt64(entity.GetId());
                transiente = await Include<T>(id);
                if ((entity as Entity).DomainEvents != null)
                {
                    foreach (var item in (entity as Entity).DomainEvents)
                    {
                        (transiente as Entity).AddDomainEvent(item);
                    }
                }
                transiente.DataAtualizacao = DateTime.UtcNow;
                transiente.TId = Guid.NewGuid().ToString("N");
                //ApplyEntityState(transiente, entity);
                GerarUpateValores(transiente, entity);
            }
            else
            {
                entity.DataCadastro = DateTime.UtcNow;
                entity.TId = Guid.NewGuid().ToString("N");
            }
            return atualizacao ? transiente : entity;
        }

        public async Task<T> Criar<T>(T entity, CancellationToken cancellationToken = default(CancellationToken)) where T : Entity
        {
            var transiente = await ApplyValores(entity, false);
            //Context.Entry(domain).State = EntityState.Added;
            await Context.Set<T>().AddAsync(transiente, cancellationToken);

            return transiente;
        }

        public async Task<T> Editar<T>(T entity) where T : Entity
        {
            var transiente = await ApplyValores(entity);
            //Context.Entry(transiente).State = EntityState.Modified;
            Context.Set<T>().Update(transiente);

            return transiente;
        }

        public async Task Excluir<T>(long id) where T : Entity
        {
            var transiente = await Include<T>(id);            
            if(transiente!=null)
            {
                transiente.Ativo = false;
                Context.Entry(transiente).State = EntityState.Modified;
            }
        }

        public async Task Excluir<T>(Expression<Func<T, bool>> condition = null) where T : Entity
        {
            var transiente = await Include(condition);
            if (transiente != null)
            {
                transiente.Ativo = false;
                Context.Entry(transiente).State = EntityState.Modified;
            }
        }

        #endregion

        

        #region Query / SQL

        public IQueryable<T> Query<T>() where T : Entity
        {
            return Context.Set<T>().AsQueryable();
        }

        public async Task<T> BuscarPorId<T>(long id, bool includes = true) where T : Entity
        {
            if(includes)
            {
                return await Include<T>(id);
            }

            return Context.Find<T>(id);
        }

        public async Task<T> Buscar<T>(Expression<Func<T, bool>> filter = null, bool includes = true) where T : Entity
        {
            if (includes)
            {
                return await Include(filter);
            }

            return await Query<T>().FirstOrDefaultAsync(filter);
        }

        public IEnumerable<T> BuscarLista<T>(Expression<Func<T, bool>> filter = null, int pageSize = 10, int currentPage = 1, bool includes = true) where T : Entity
        {
            var query = Query<T>();

            var entitade = (T)Activator.CreateInstance(typeof(T));

            if (includes)
            {

                var lista = GetIncludePropiedades(entitade);
                if (lista != null && lista.Count > 0)
                {
                    foreach (var item in lista)
                    {
                        query = query.Include(item);
                    }
                }
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (pageSize <= 0 && currentPage <= 0)
            {
                query = query.Skip((currentPage - 1) * pageSize);
            }

            return query.AsEnumerable();
        }

        private async Task<T> Include<T>(long id) where T : Entity
        {
//            var idTransiente = Convert.ToInt64(id);
            Expression<Func<T, bool>> IdExpression = (c => (Convert.ToInt64(c.GetId("Id"))) == id);

            return await Include(IdExpression);
        }

        private async Task<T> Include<T>(Expression<Func<T, bool>> filter = null, bool includes = true) where T : Entity
        {
            var query = Query<T>();
            if (includes)
            {
                var propriedades = GetIncludePropiedades(default(T));
                if (propriedades != null && propriedades.Count > 0)
                {
                    foreach (var propriedade in propriedades)
                    {
                        query = query.Include(propriedade);
                    }
                }                
            }
            return await query.SingleOrDefaultAsync(filter);
        }

        #endregion

        #region Métodos Auxiliares

        public DateTime BuscarDatabaseDateTime()
        {
            switch (Database.DatabaseName)
            {
                case DatabaseEnum.Oracle:
                    //return new DateTime();
                    return DateTime.UtcNow;
                //Convert.ToDateTime(Context.CreateSQLQuery("select sysdate from dual").UniqueResult());

                case DatabaseEnum.SQLServer:
                    return DateTime.UtcNow;
                    //return Convert.ToDateTime(Session.CreateSQLQuery("SELECT SYSDATETIME()").UniqueResult());
            }

            throw new System.Exception("Não foi possível obter datahora do banco.");
        }

        private void ApplyEntityState<T>(T salvo, T entity)
        {
            var props = salvo.GetType().GetProperties().ToList().Where(x => x.GetCustomAttribute<EntityPaiAttribute>() != null).ToList();
            DeleteStatePai(salvo, props);
            object propertyValor;
            if (props != null)
            {
                foreach (var property in props)
                {
                    propertyValor = property.GetValue(entity);
                    if (propertyValor as IList != null && (propertyValor as IList).Count > 0)
                    {
                        foreach (var item in (propertyValor as IList))
                        {
                            ApplyReferenciaPai(item, entity);
                        }
                    }
                    else if (propertyValor != null)
                    {
                        ApplyReferenciaFilho(propertyValor, entity, property.Name);
                    }
                    property.SetValue(salvo, propertyValor);
                }
            }
        }

        private void GerarUpateValores(object salvo, object novo)
        {

            var propriedadesPermitidas = new List<string>() { "string", "int", "int32", "uint", "uint64", "uint32", "ulong", "bool", "boolean", "decimal", "datetime" };

            var props = salvo.GetType().GetProperties();
            var propsNovo = novo.GetType().GetProperties();
            if (props != null)
            {
                foreach (var prop in props.Where(x => x.SetMethod !=null && x.Name != "TId" && propriedadesPermitidas.Contains(x.PropertyType.Name.ToLower())))
                {
                    //prop.SetValue(domain, prop.GetValue(novo));

                    if (propsNovo.Any(x => x.PropertyType.Name.ToLower() == prop.PropertyType.Name.ToLower()))
                    {
                        switch (prop.PropertyType.Name.ToLower())
                        {
                            case "string":
                                prop.SetValue(salvo, prop.GetValue(novo));
                                break;
                            case "int":
                            case "int32":
                                prop.SetValue(salvo, Convert.ToInt32(prop.GetValue(novo)));
                                break;
                            case "uint":
                            case "uint64":
                            case "uint32":
                            case "ulong":
                            case "decimal":
                                prop.SetValue(salvo, Convert.ToDecimal(prop.GetValue(novo)));
                                break;
                            case "bool":
                            case "boolean":
                                prop.SetValue(salvo, Convert.ToBoolean(prop.GetValue(novo)));
                                break;
                            case "datetime":
                                prop.SetValue(salvo, Convert.ToDateTime(prop.GetValue(novo)));
                                break;
                        }
                    }
                }
            }
        }

        private void ApplyReferenciaFilho<T>(object entityFilho, T entityPai, string propriedade)
        {
            var propertyItem = entityPai.GetType().GetProperty(propriedade);
            if (propertyItem != null)
            {
                propertyItem.SetValue(entityPai, entityFilho);
            }
        }

        private bool ApplyReferenciaFilhoFK(object entityFilho, object entityPaiFK)
        {
            var props = entityFilho.GetType().GetProperties();

            foreach (PropertyInfo property in props)
            {
                if (System.Attribute.GetCustomAttributes(property).Any(a => a.GetType() == typeof(System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute)))
                {
                    property.SetValue(entityFilho, (int)entityPaiFK.GetId());
                    return true;
                }
            }
            return false;
        }

        private void ApplyReferenciaPai(object entity, object pai)
        {
            var props = entity.GetType().GetProperties().ToList();
            var idPai = Convert.ToInt64( pai.GetId());
            var propsFilhos = props.Where(x => x.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute>() != null).ToList();
            if (propsFilhos != null)
            {
                foreach (var prop in propsFilhos)
                {
                    prop.SetValue(entity, idPai);
                }
            }

            object propertyValor;
            foreach (var property in props.Where(x => x.GetCustomAttribute<EntityPaiAttribute>() != null))
            {
                var attributes = System.Attribute.GetCustomAttributes(property).Where(a => a.GetType() == typeof(EntityPaiAttribute)).ToList();
                foreach (EntityPaiAttribute atributo in attributes)
                {
                    propertyValor = property.GetValue(entity);

                    if (propertyValor as IList != null && (propertyValor as IList).Count > 0)
                    {
                        foreach (var item in (propertyValor as IList))
                        {
                            ApplyReferenciaPai(item, entity);
                        }

                    }
                    else if (propertyValor != null)
                    {
                        ApplyReferenciaFilhoFK(entity, pai);
                    }
                }
            }
        }

        private void DeleteStatePai(object entity, List<PropertyInfo> proprieades)
        {
            object propertyValor;
            List<PropertyInfo> props = null;

            if (proprieades != null && proprieades.Count > 0)
            {
                foreach (var property in proprieades)
                {
                    var attributes = System.Attribute.GetCustomAttributes(property).Where(a => a.GetType() == typeof(EntityPaiAttribute)).ToList();
                    foreach (EntityPaiAttribute atributo in attributes)
                    {
                        propertyValor = property.GetValue(entity);

                        if (propertyValor as IList != null && (propertyValor as IList).Count > 0)
                        {
                            foreach (var item in (propertyValor as IList))
                            {
                                Context.Entry(item).State = EntityState.Deleted;
                                props = item.GetType().GetProperties().ToList().Where(x => x.GetCustomAttribute<EntityPaiAttribute>() != null).ToList();
                                DeleteStatePai(item, props);
                            }
                        }
                        else if (propertyValor != null)
                        {
                            Context.Entry(propertyValor).State = EntityState.Deleted;
                        }
                    }
                }
            }
        }

        private string GetIncludePropiedadesFilhos(object entity)
        {
            var listaIncludes = new List<string>();
            var include = string.Empty;
            var props = entity.GetType().GetProperties().ToList().Where(x => x.GetCustomAttribute<EntityPaiAttribute>() != null).ToList();

            foreach (var property in props)
            {
                if (property.PropertyType.Name == "IList`1")
                {
                    var tipoFilho = property.PropertyType.GetGenericArguments()[0];
                    var entitade = Activator.CreateInstance(tipoFilho);
                    var includeFilho = GetIncludePropiedadesFilhos(entitade);
                    include = property.Name + (string.IsNullOrEmpty(includeFilho) ? "" : "." + includeFilho);
                }
                else
                {
                    include = property.Name;

                }
            }
            return include;
        }

        private List<string> GetIncludePropiedades(object entity)
        {
            var listaIncludes = new List<string>();
            var include = string.Empty;
            //var props = entity.GetType().GetProperties().ToList().Where(x => x.GetCustomAttribute<EntityPaiAttribute>() != null).ToList();

            var props = entity.GetPropertiesByCustomAttributes<object, EntityPaiAttribute>();


            if (props !=null)
            {
                foreach (var property in props)
                {
                    if (property.PropertyType.Name == "IList`1")
                    {
                        var tipoFilho = property.PropertyType.GetGenericArguments()[0];
                        var entitade = Activator.CreateInstance(tipoFilho);
                        var includeFilho = GetIncludePropiedadesFilhos(entitade);
                        include = property.Name + (string.IsNullOrEmpty(includeFilho) ? "" : "." + includeFilho);
                        listaIncludes.Add(include);
                    }
                    else
                    {
                        listaIncludes.Add(property.Name);
                    }
                }
            }


            return listaIncludes;
        }
        
        #endregion

        

       
    }
}