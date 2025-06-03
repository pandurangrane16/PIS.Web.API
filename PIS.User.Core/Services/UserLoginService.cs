using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PIS.Framework;
using PIS.Framework.Models;
using PIS.Framework.Query;
using PIS.Framework.Repositories;
using PIS.User.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PIS.User.Core.Services
{

	public class UserLoginService : IUserLoginService
	{
		private readonly IUowProvider _uowProvider;
		private readonly IConfiguration _configuration;

		public UserLoginService(IUowProvider uowProvider, IConfiguration configuration)
		{
			_uowProvider = uowProvider;
			_configuration = configuration;
		}

	
		public async Task<ResponseVM> GetUserLogin(CommonInputVM _vm, int? id)
		{
			try
			{
				IEnumerable<UserLogin> User = null;
				ResponseVM responseVM = new ResponseVM();
				List<UserLogin> FilteredList = new List<UserLogin>();
				using (var uow = _uowProvider.CreateUnitOfWork())
				{
					var repository = uow.GetRepository<UserLogin>();
					var includes = new Includes<Domain.UserLogin>(query =>
					{
						if (_vm.PageSize == 0)
						{
							if (id != null)
							{
								return query.Where(x => x.Id == id);
							}
							
							else
								return query.OrderByDescending(x => x.Id);
						}
						else
						{
							
								return (query.OrderByDescending(x => x.Id));
						}
					});

					User = await repository.GetAllAsync(null, includes.Expression);
					
					
					if (_vm.PageSize != 0)
					{
						var _orderBy = repository.GetOrderBy("id", "desc");
						User = User.Skip(_vm.StartId).Take(_vm.PageSize);

					}
					responseVM.data = User;
					responseVM.totalRecords = await GetUsersFilterCount(_vm);
					responseVM.pageSize = _vm.PageSize;
					responseVM.currentPage = _vm.CurrentPage;
					
				}
				return responseVM;
			}
			catch (Exception ex)
			{
				//Log("API_Error", "Error : " + ex.Message);
				throw ex;
			}


		}
		public async Task<int> PutUserLoginMasterAsync(UserLogin userlogin)
		{
			using (var uow = _uowProvider.CreateUnitOfWork())
			{
				var repository = uow.GetRepository<UserLogin>();
				repository.Update(userlogin);
				return await uow.SaveChangesAsync();
			}
		}
		public async Task<int> SaveUserLoginAsync(UserLogin userlogin)
		{
			


			try
			{
				int res = 0;
				using (var uow = _uowProvider.CreateUnitOfWork())
				{
					userlogin.CreatedDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
					userlogin.ModifiedDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
					var repository = uow.GetRepository<UserLogin>();
					repository.Add(userlogin);
					res = await uow.SaveChangesAsync();
				}
				return res;

			}
			catch (Exception ex)
			{

				return (0);
				
			}
		}


		public async Task<int> GetUsersFilterCount(CommonInputVM _request)
		{
			try
			{
				using (var uow = _uowProvider.CreateUnitOfWork())
				{
					IRepository<UserLogin> repoPlReport = uow.GetRepository<UserLogin>();
					var includes = new Includes<UserLogin>(query =>
					{
						return query;
					});

					int count = await repoPlReport.QueryCountAsync(includes.Expression);
					return count;
				}
			}
			catch (Exception ex)
			{
				//Log("API_Error", "Error Filter Count : " + ex.Message);
				//Log("GetAiredReportFilterCount", "Error : " + ex.Message);
				return 0;
			}
		}

		//public void Log(string filename, string error)
		//{
		//	try
		//	{
		//		DateTime now = DateTime.Now;
		//		if (!Directory.Exists("C:\\VMS_Service\\API_Logs\\"))
		//		{
		//			Directory.CreateDirectory("C:\\VMS_Service\\API_Logs\\");
		//		}
		//		using (StreamWriter SW = new StreamWriter("C:\\VMS_Service\\API_Logs\\" + filename + "-LOG-" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + "", true))
		//		{
		//			SW.WriteLine(DateTime.Now.ToString() + " | " + error);
		//			//SW.WriteLine(error);
		//			SW.Close();
		//		}
		//	}
		//	catch (Exception ex)
		//	{

		//	}
		//}
	}
}
