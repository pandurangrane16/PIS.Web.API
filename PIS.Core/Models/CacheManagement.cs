using System;
using System.Collections.Generic;
using System.Text;
using PIS.Framework.Entities;

namespace PIS.Framework.Models
{
    public class CacheManagement : EntityBase
    {
        public string ModuleName { get; set; }
        public string TableName { get; set; }
        public bool IsChange { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
