using Microsoft.AspNetCore.Mvc;
using PIS.Framework.Controllers;
using PIS.Framework.Models;
using PIS.User.Core.Domain;
using PIS.User.Core.Services;

namespace PIS.User.API.Controllers
{
    public class RoleMasterController : BaseController
    {
        public readonly IRoleService _roleService;
        public readonly IConfiguration _config;
        public RoleMasterController(IRoleService roleService, IConfiguration config)
        {
            _roleService = roleService;
            _config = config;
        }
        
        [HttpPost]
        public async Task<IActionResult> GetRoleMasterAsync(CommonInputVM _vm)
        {
            var _data = await _roleService.GetRoleMastersAsync(_vm);
            return Ok(_data);
        }

        [HttpPost]
        public async Task<IActionResult> PostRoleMasterAsync(RoleMaster _role)
        {
            var _data = await _roleService.PostRoleMasterAsync(_role);
            return Ok(_data);
        }
    }
}
