using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using PIS.Framework.Models;
using PIS.Framework.Query;
using PIS.Framework.Uow;

namespace PIS.Framework.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class CacheManagement
    {
        public IConfiguration configuration;
        private readonly IUowProvider _uowProvider;
        public CacheManagement(IUowProvider uowProvider)
        {
            _uowProvider = uowProvider;
        }
        [HttpPost]
        public async Task<int> AddCacheManagement(PIS.Framework.Models.CacheManagement _cache)
        {
            try
            {
                using (var uow = _uowProvider.CreateUnitOfWork())
                {
                    var repository = uow.GetRepository<PIS.Framework.Models.CacheManagement>();
                    repository.Add(_cache);
                    return await uow.SaveChangesAsync();
                }
            }
            catch
            {
                return 0;
            }
        }

        [HttpGet]
        public async Task<IEnumerable<PIS.Framework.Models.CacheManagement>> GetCacheData(string moduleName, string tablename)
        {
            try
            {
                using (var uow = _uowProvider.CreateUnitOfWork())
                {
                    var repository = uow.GetRepository<PIS.Framework.Models.CacheManagement>();
                    var includes = new Includes<PIS.Framework.Models.CacheManagement>(query =>
                    {
                        if (!String.IsNullOrEmpty(moduleName) && !String.IsNullOrEmpty(tablename))
                            return query.Where(x => x.ModuleName.ToLower() == moduleName.ToLower() && x.TableName.ToLower() == tablename.ToLower());
                        else if (!String.IsNullOrEmpty(moduleName) && String.IsNullOrEmpty(tablename))
                            return query.Where(x => x.ModuleName.ToLower() == moduleName.ToLower());
                        else if (!String.IsNullOrEmpty(tablename) && String.IsNullOrEmpty(moduleName))
                            return query.Where(x => x.TableName.ToLower() == tablename.ToLower());
                        else return query;
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
    }
}
