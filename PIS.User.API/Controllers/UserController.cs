using Microsoft.AspNetCore.Mvc;
using PIS.Framework.Controllers;
using PIS.User.Core.Domain;
using PIS.User.Core.VM;

namespace PIS.User.API.Controllers
{
    public class UserController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> LoggedIn(LoginVM _vm)
        {

            return Ok(1);
        }

    }
}
