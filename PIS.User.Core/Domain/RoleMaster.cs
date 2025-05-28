using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIS.Framework.Entities;

namespace PIS.User.Core.Domain
{
    public class RoleMaster : EntityBase
    {
        public string RoleName { get; set; }
        public string Description { get; set; }
        public bool  IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
