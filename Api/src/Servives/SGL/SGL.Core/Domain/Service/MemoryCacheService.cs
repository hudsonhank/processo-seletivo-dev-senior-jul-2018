using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace SGL.Core.Domain.Service
{
    public class MemoryCacheService : IMemoryCache
    {
        public MemoryCacheService()
        {
        }

        public ICacheEntry CreateEntry(object key)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose();
        }

        public void Remove(object key)
        {
            var valor = this.Get(key);
            if (valor != null)
                Remove(key);
        }

        public bool Set(string key, object value)
        {
            return TryGetValue(key, out value);
        }

        public bool TryGetValue(object key, out object value)
        {
            if (!this.TryGetValue<object>(key, out value))
            {
                this.Set(key, value, DateTime.Now.AddMinutes(30));
            }
            return value != null;

        }
    }
}
