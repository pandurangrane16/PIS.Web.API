using Microsoft.AspNetCore.Mvc;
using PIS.Framework.Controllers;
using PIS.User.Core.Domain;

namespace PIS.User.API.Controllers
{
    public class UserController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetClientDetails()
        {
            return Ok(1);
        }

    }
}
