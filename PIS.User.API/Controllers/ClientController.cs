using System.IO;
using Microsoft.AspNetCore.Mvc;
using PIS.Framework.Controllers;
using PIS.User.Core.Domain;
using PIS.User.Core.Services;

namespace PIS.User.API.Controllers
{
    public class ClientController : BaseController
    {
        public readonly IClientService _clientService;
        public readonly IConfiguration _config;
        public ClientController(IClientService clientService, IConfiguration config)
        {
            _clientService = clientService;
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> GetClientDetails()
        {
            var _data = await _clientService.GetClientDetails();
            return Ok(_data);
        }

        [HttpPost]
        public async Task<IActionResult> AddClientDetails([FromForm] ClientDetailsVM _detailsVM)
        {
            ClientDetails _details = new ClientDetails();
            string path = _config.GetValue<string>("FilePath");
            if (_detailsVM.Logo != null)
            {
                MemoryStream ms = new MemoryStream();
                _detailsVM.Logo.CopyTo(ms);
                byte[] _arr = ms.ToArray();
                ms.Dispose();
                DirectoryInfo _dirInfo = new DirectoryInfo(path + "Logo//");
                if (!_dirInfo.Exists)
                {
                    _dirInfo.Create();
                }
                string fname = _detailsVM.Logo.FileName.Substring(0, _detailsVM.Logo.FileName.LastIndexOf('.'));
                _details.Status = 0;
                _details.Email = _detailsVM.Email;
                _details.Address = _detailsVM.Address;
                _details.CreatedDate = DateTime.Now;
                _details.CreatedBy = _detailsVM.CreatedBy;
                _details.ContactNo = _detailsVM.ContactNo;
                _details.ClientCode = _detailsVM.ClientCode;
                _details.TagLine = _detailsVM.TagLine;
                _details.Name = _detailsVM.Name;
                _details.Logo = fname + "_" + _detailsVM.ClientCode + ".png";
                System.IO.File.Create(path + "Logo\\" + _details.Logo + ".png").Close();
                System.IO.File.WriteAllBytes(path + "Logo\\" + _details.Logo + ".png", _arr);
            }
            var _d = await _clientService.AddClientDetails(_details);

            return Ok(_d);
        }
    }
}
