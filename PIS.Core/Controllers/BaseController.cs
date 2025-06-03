using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using System.Text;
using PIS.Framework.Models;
using System.Security.Cryptography;

namespace PIS.Framework.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class BaseController : ControllerBase
    {
        public IConfiguration configuration;
        [HttpPost]
        public IActionResult ValidateToken(AuthToken _auth)
        {
            try
            {
                if (_auth.TokenData != string.Empty)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var validationParameters = GetValidationParameters();

                    SecurityToken validatedToken;
                    IPrincipal principal = tokenHandler.ValidateToken(_auth.TokenData, validationParameters, out validatedToken);
                    return Ok(true);
                }
                else
                {
                    return BadRequest(false);
                }
            }
            catch
            {
                return BadRequest(false);
            }
        }
        private TokenValidationParameters GetValidationParameters()
        {
            string key = "VMS_8CFB2EC534E14D56";
            string issuer = "VMS";
            string audience = "http://localhost:4200/";
            return new TokenValidationParameters()
            {
                ValidateLifetime = false, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token
                ValidateIssuer = false,   // Because there is no issuer in the generated token
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)) // The same key as the one that generate the token
            };
        }



		
	}
}
