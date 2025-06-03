using PIS.Framework.Models;
using PIS.User.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIS.User.Core.Services
{
	public interface IUserLoginService
	{

		Task<ResponseVM> GetUserLogin(CommonInputVM _vm, int? id);
		Task<int> SaveUserLoginAsync(UserLogin userlogin);
		Task<int> PutUserLoginMasterAsync(UserLogin userlogin);

		
	}
}
