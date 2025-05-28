using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIS.Framework.Models;
using PIS.User.Core.Domain;

namespace PIS.User.Core.Services
{
    public interface IMenuMaster
    {
        Task<ResponseVM> GetMenuMastersAsync(CommonInputVM _vm);
        Task<int> PostMenuMasterAsync(MenuMaster _details);
    }
}
