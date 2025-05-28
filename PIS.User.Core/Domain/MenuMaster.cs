using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PIS.Framework.Entities;

namespace PIS.User.Core.Domain
{
    public class MenuMaster : EntityBase
    {
        public string MenuName { get; set; }
        public string MenuPath { get; set; }
        public int ParentId { get; set; }
        public int DisplayOrder { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class MenuMasterVM
    {
        public string MenuName { get; set; }
        public string MenuPath { get; set; }
        public int ParentId { get; set; }
        public int DisplayOrder { get; set; }
        public IFormFile Icon { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
    }
}
