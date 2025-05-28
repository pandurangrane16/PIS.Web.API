using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIS.Framework.Models;
using PIS.User.Core.Domain;

namespace PIS.User.Core.Services
{
    public interface IRoleService
    {
        Task<ResponseVM> GetRoleMastersAsync(CommonInputVM _vm);
        Task<int> PostRoleMasterAsync(RoleMaster _details);
    }
}
