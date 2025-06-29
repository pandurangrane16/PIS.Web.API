using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PIS.Framework.Controllers;
using PIS.User.Core.Domain;
using PIS.User.Core.VM;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;
using PIS.Framework.Controllers;
using PIS.Framework.Repositories;
using PIS.User.Core.Services;
using System.Data;
using PIS.Framework;

namespace PIS.User.API.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserLoginService _userlogin;
        private readonly IConfiguration _config;
        private readonly IUowProvider _uowProvider;


        public UserController(IUowProvider uowProvider, IUserLoginService userlogin, IConfiguration configuration)
        {

            _userlogin = userlogin;
            _config = configuration;
            _uowProvider = uowProvider;


        }


        [HttpPost("login")]
        public async Task<IActionResult> LoggedIn(LoginVM _vm)
        {
            try
            {
                using (var uow = _uowProvider.CreateUnitOfWork())
                {
                    var repository = uow.GetRepository<UserLogin>();
                    if (string.IsNullOrEmpty(_vm.UserName) || string.IsNullOrEmpty(_vm.Password))
                        return BadRequest("Username or password cannot be empty.");
                    string padded = EncDecService.CleanBase64String(_vm.Password);
                    string decryptedPassword = EncDecService.Decrypt(padded);

                    //string query = "SELECT passwordhash FROM public.userlogin WHERE username = (" + _vm.UserName + ") AND isactive = true";
                    string query = "SELECT passwordhash FROM public.userlogin WHERE username = '" + _vm.UserName + "' AND isactive = true";

                    string _connection = _config.GetValue<string>("ConnectionPG");
                    DataTable _dt = await uow.returnDataTable(query, _connection);

                    if (_dt != null && _dt.Rows.Count > 0)

                    {
                        string storedHash = _dt.Rows[0]["passwordhash"].ToString();

                        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(decryptedPassword, storedHash);

                        if (!isPasswordValid)
                            return Unauthorized("Invalid password.");

                        return Ok(new { message = "Login successful" });
                    }
                    else
                    {
                        return Ok(new { message = "User does not exist." });
                    }


                }

            }
            catch (CryptographicException ex)
            {
                return BadRequest($"Decryption error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetUserLogin(Framework.Models.CommonInputVM _vm, int? id)
        {

            var userlogin = await _userlogin.GetUserLogin(_vm, id);
            return Ok(userlogin);
        }

        [HttpPost]
        public async Task<IActionResult> SaveUserLoginAsync(UserLogin userlogin)
        {

            var _d = await _userlogin.SaveUserLoginAsync(userlogin);
            return Ok(_d);
        }

        [HttpPut]
        public async Task<IActionResult> PutUserLoginMasterAsync(UserLogin userlogin)
        {
            var p = await _userlogin.PutUserLoginMasterAsync(userlogin);
            return Ok(p);
        }


        [HttpPost]
        public async Task<IActionResult> UserRegistration(RegistrationDetails _register)
        {
            var _d = await _userlogin.UserRegistration(_register);
            return Ok(_d);
        }




    }
}
