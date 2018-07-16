using System;
using System.Linq.Expressions;

namespace Core.Abstractions.Application
{
    public interface IBaseQuery<TEntity>
    {
        Expression<Func<TEntity, bool>> QueryExpression();
    }

    public class BaseQuery<TEntity> : IBaseQuery<TEntity>
    {        
        public virtual Expression<Func<TEntity, bool>> QueryExpression()
        {
            Expression<Func<TEntity, bool>> expression = null;
            return expression;
        }
    }
}
