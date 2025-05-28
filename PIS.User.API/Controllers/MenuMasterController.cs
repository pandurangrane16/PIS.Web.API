using Microsoft.AspNetCore.Mvc;
using PIS.Framework.Models;
using PIS.User.Core.Domain;
using PIS.User.Core.Services;

namespace PIS.User.API.Controllers
{
    public class MenuMasterController : Controller
    {
        public readonly IMenuMaster _clientService;
        public readonly IConfiguration _config;
        public MenuMasterController(IMenuMaster clientService, IConfiguration config)
        {
            _clientService = clientService;
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> AddMenuDetails([FromForm] MenuMasterVM _detailsVM)
        {
            MenuMaster _details = new MenuMaster();
            string path = _config.GetValue<string>("FilePath");
            if (_detailsVM.Icon != null)
            {
                MemoryStream ms = new MemoryStream();
                _detailsVM.Icon.CopyTo(ms);
                byte[] _arr = ms.ToArray();
                ms.Dispose();
                DirectoryInfo _dirInfo = new DirectoryInfo(path + "Icon//");
                if (!_dirInfo.Exists)
                {
                    _dirInfo.Create();
                }
                string fname = _detailsVM.Icon.FileName.Substring(0, _detailsVM.Icon.FileName.LastIndexOf('.'));
                _details.Icon = fname + ".png";
                _details.CreatedDate = DateTime.Now;
                _details.CreatedBy = _detailsVM.CreatedBy;
                _details.DisplayOrder = _detailsVM.DisplayOrder;
                _details.MenuName = _detailsVM.MenuName;
                _details.MenuPath = _detailsVM.MenuPath;
                _details.ParentId = _detailsVM.ParentId;
                System.IO.File.Create(path + "Logo\\" + _details.Icon + ".png").Close();
                System.IO.File.WriteAllBytes(path + "Logo\\" + _details.Icon + ".png", _arr);
            }
            var _d = await _clientService.PostMenuMasterAsync(_details);

            return Ok(_d);
        }

        [HttpPost]
        public async Task<IActionResult> GetMenuMasterAsync(CommonInputVM input)
        {
            var _data = await _clientService.GetMenuMastersAsync(input);
            return Ok(_data);
        }
    }
}
