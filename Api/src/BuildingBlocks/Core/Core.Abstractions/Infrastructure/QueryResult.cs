using System.Collections.Generic;

namespace Core.Abstractions.Infrastructure
{
    public class QueryResult<T> where T : class
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public long Total { get; set; }
        public IEnumerable<T> Resultado { get; set; }
    }
}