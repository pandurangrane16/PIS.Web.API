using System;
using System.Collections.Generic;
using System.Text;

namespace PIS.Framework.Models
{
    public class CommonInputVM
    {
        public string searchItem { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int StartId { get; set; }
        public string Cachekey { get; set; }
    }
}
