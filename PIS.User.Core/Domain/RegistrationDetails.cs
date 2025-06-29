using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIS.Framework.Entities;

namespace PIS.User.Core.Domain
{
    public class RegistrationDetails : EntityBase
    {
        public string Username { get; set; }
        public string CompanyName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string State { get; set; }
        public string UserType { get; set; }
        public string MobileNo { get; set; }
    }
}
