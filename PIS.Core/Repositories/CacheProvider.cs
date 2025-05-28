using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIS.Framework.Models;
using PIS.Framework.Query;

namespace PIS.Framework.Repositories
{
    public class CacheProvider<TEntity> : ICacheProvider<TEntity> where TEntity : class
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IUowProvider _uow;
        public CacheProvider(IMemoryCache memoryCache, IUowProvider uow)
        {
            _memoryCache = memoryCache;
            _uow = uow;
        }

        public async Task<IEnumerable<CacheManagement>> GetCacheManagement(string cacheKey)
        {
            try
            {
                using (var uow = _uow.CreateUnitOfWork())
                {
                    var repository = uow.GetRepository<PIS.Framework.Models.CacheManagement>();
                    var includes = new Includes<PIS.Framework.Models.CacheManagement>(query =>
                    {
                        return query.Where(x => x.TableName.ToLower() == cacheKey.ToLower());
                    });

                    var _cacheData = await repository.GetAllAsync(null, includes.Expression);
                    return _cacheData;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> SetCache(string cacheKey, TEntity _data)
        {
            var _cacheData = await GetCacheManagement(cacheKey);
            if (_cacheData == null || _cacheData.Count() == 0)
            {
                _memoryCache.Set(cacheKey, _data,
                    new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromDays(30)));
            }
            else
            {
                bool _val = _cacheData.FirstOrDefault().IsChange;
                if (_val)
                {
                    await RemoveCache(cacheKey);
                    _memoryCache.Set(cacheKey, _data,
                    new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromDays(30)));
                }
            }
            return 1;
        }
        public async Task<int> RemoveCache(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);
            return 1;
        }

    }
}
