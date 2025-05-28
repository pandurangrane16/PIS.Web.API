using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIS.User.Core.VM
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
