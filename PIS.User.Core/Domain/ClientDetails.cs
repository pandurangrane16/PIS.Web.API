using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PIS.Framework.Entities;

namespace PIS.User.Core.Domain
{
    public class ClientDetails : EntityBase
    {
        public string ClientCode { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public string Logo { get; set; }
        public string TagLine { get; set; }
        public ClientStatus Status { get; set; }
    }

    public enum ClientStatus
    {
        Initiate,
        Approved,
        Expired,
        Reject
    }

    public class ClientDetailsVM
    {
        public string ClientCode { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public IFormFile Logo { get; set; }
        public string TagLine { get; set; }
        public string CreatedBy { get; set; }
        public ClientStatus Status { get; set; }
    }
}
