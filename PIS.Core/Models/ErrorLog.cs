using System;
using System.Collections.Generic;
using System.Text;
using PIS.Framework.Entities;

namespace PIS.Framework.Models
{
    public class ErrorLog : EntityBase
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DateTime LoggedAt { get; set; } = DateTime.Now;
    }
}
