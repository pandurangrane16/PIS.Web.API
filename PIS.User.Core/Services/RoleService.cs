using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIS.Framework;
using PIS.Framework.Models;
using PIS.Framework.Query;
using PIS.Framework.Repositories;
using PIS.User.Core.Domain;

namespace PIS.User.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUowProvider _uowProvider;
        public RoleService(IUowProvider uowProvider)
        {
            _uowProvider = uowProvider;
        }

        public async Task<ResponseVM> GetRoleMastersAsync(CommonInputVM _vm)
        {
            IEnumerable<RoleMaster> _roleData = null;
            ResponseVM responseVM = new ResponseVM();

            using (var uow = _uowProvider.CreateUnitOfWork())
            {
                var clientRepo = uow.GetRepository<RoleMaster>();
                var includes = new Includes<RoleMaster>(query =>
                {
                    if (_vm.PageSize != 0)
                    {
                        if (!string.IsNullOrEmpty(_vm.searchItem))
                        {
                            return query.Where(x => x.IsDeleted != true && (x.RoleName.ToLower().Contains(_vm.searchItem.ToLower()) || x.Description.ToLower().Contains(_vm.searchItem.ToLower())));
                        }
                        else
                            return query.Where(x => x.IsDeleted != true);
                    }
                    else
                        return query.Where(x => x.IsDeleted != true);

                });
                if (_vm.PageSize != 0)
                {
                    var _orderBy = clientRepo.GetOrderBy("id", "desc");
                    _roleData = await clientRepo.GetPageAsync(_vm.StartId, _vm.PageSize, _orderBy, includes.Expression);
                }
                else
                    _roleData = await clientRepo.GetAllAsync(null, includes.Expression);
            }
            responseVM.data = _roleData;
            responseVM.totalRecords = await GetRoleMasterFilterCount(_vm);
            responseVM.pageSize = _vm.PageSize;
            responseVM.currentPage = _vm.CurrentPage;
            return responseVM;
        }

        public async Task<int> GetRoleMasterFilterCount(CommonInputVM vm)
        {
            using (var uow = _uowProvider.CreateUnitOfWork())
            {
                IRepository<RoleMaster> repoPlReport = uow.GetRepository<RoleMaster>();
                var includes = new Includes<RoleMaster>(query =>
                {
                    if (!string.IsNullOrEmpty(vm.searchItem))
                    {
                        return query.Where(x => x.IsDeleted != true && (x.RoleName.ToLower().Contains(vm.searchItem)));
                    }
                    else
                        return query.Where(x => x.IsDeleted != true);

                });

                int count = await repoPlReport.QueryCountAsync(includes.Expression);
                return count;
            }
        }

        public async Task<int> PostRoleMasterAsync(RoleMaster _details)
        {
            try
            {
                using (var uow = _uowProvider.CreateUnitOfWork())
                {
                    _details.CreatedDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    var repository = uow.GetRepository<RoleMaster>();
                    repository.Add(_details);
                    return await uow.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}
