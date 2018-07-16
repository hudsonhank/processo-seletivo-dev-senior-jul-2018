using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Abstractions.Infrastructure
{
    public interface ICacheRepository<T> where T : class
    {
        Task<T> GetAsync(string id, bool useIndexName = false);
        Task<T> GetAsync(long id);
        IEnumerable<string> GetIds();
        Task<bool> SetAsync(T entity);        
        Task<bool> DeleteAsync(string id);
        Task<QueryResult<T>> GetAllAsync(string texto = "", int pageSize = 10, int currentPage = 1);
    }
}
