using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PIS.Framework.Models;

namespace PIS.Framework.Repositories
{
    public interface ICacheProvider<TEntity> where TEntity : class
    {
        Task<int> SetCache(string cacheKey, TEntity _data);

        Task<IEnumerable<CacheManagement>> GetCacheManagement(string cacheKey);
        Task<int> RemoveCache(string cacheKey);
    }
}
