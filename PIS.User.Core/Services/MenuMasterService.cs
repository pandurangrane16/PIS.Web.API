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
    public class MenuMasterService : IMenuMaster
    {
        private readonly IUowProvider _uowProvider;
        private readonly IErrorLog _errorLog;
        public MenuMasterService(IUowProvider uowProvider, IErrorLog errorLog)
        {
            _uowProvider = uowProvider;
            _errorLog = errorLog;
        }

        public async Task<ResponseVM> GetMenuMastersAsync(CommonInputVM _vm)
        {
            try
            {
                IEnumerable<MenuMaster> _menuData = null;
                ResponseVM responseVM = new ResponseVM();

                using (var uow = _uowProvider.CreateUnitOfWork())
                {
                    var clientRepo = uow.GetRepository<MenuMaster>();
                    var includes = new Includes<MenuMaster>(query =>
                    {
                        if (_vm.PageSize != 0)
                        {
                            if (!string.IsNullOrEmpty(_vm.searchItem))
                            {
                                return query.Where(x => x.IsDeleted != true && (x.MenuName.ToLower().Contains(_vm.searchItem.ToLower()) || x.MenuPath.ToLower().Contains(_vm.searchItem.ToLower())));
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
                        _menuData = await clientRepo.GetPageAsync(_vm.StartId, _vm.PageSize, _orderBy, includes.Expression);
                    }
                    else
                        _menuData = await clientRepo.GetAllAsync(null, includes.Expression);
                }
                responseVM.data = _menuData;
                responseVM.totalRecords = await GetMenuMasterFilterCount(_vm);
                responseVM.pageSize = _vm.PageSize;
                responseVM.currentPage = _vm.CurrentPage;
                return responseVM;

            }
            catch (Exception ex)
            {
                _errorLog.LogError(ex);
                return null;
            }
        }

        public async Task<int> GetMenuMasterFilterCount(CommonInputVM vm)
        {
            try
            {
                using (var uow = _uowProvider.CreateUnitOfWork())
                {
                    IRepository<MenuMaster> repoPlReport = uow.GetRepository<MenuMaster>();
                    var includes = new Includes<MenuMaster>(query =>
                    {
                        if (!string.IsNullOrEmpty(vm.searchItem))
                        {
                            return query.Where(x => x.IsDeleted != true && (x.MenuName.ToLower().Contains(vm.searchItem) || x.MenuPath.ToLower().Contains(vm.searchItem)));
                        }
                        else
                            return query.Where(x => x.IsDeleted != true);

                    });

                    int count = await repoPlReport.QueryCountAsync(includes.Expression);
                    return count;
                }
            }
            catch (Exception ex)
            {
                _errorLog.LogError(ex);
                return -1;
            }

        }

        public async Task<int> PostMenuMasterAsync(MenuMaster _details)
        {
            try
            {
                using (var uow = _uowProvider.CreateUnitOfWork())
                {
                    _details.CreatedDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    var repository = uow.GetRepository<MenuMaster>();
                    repository.Add(_details);
                    return await uow.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _errorLog.LogError(ex);
                return -1;
            }
        }

        public async Task<int> PutMenuMasterAsync(MenuMaster _details)
        {
            try
            {
                using (var uow = _uowProvider.CreateUnitOfWork())
                {
                    _details.ModifiedDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    var repository = uow.GetRepository<MenuMaster>();
                    repository.Update(_details);
                    return await uow.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _errorLog.LogError(ex);
                return -1;
            }
        }


    }
}
