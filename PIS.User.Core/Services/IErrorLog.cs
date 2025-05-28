using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIS.User.Core.Services
{
    public interface IErrorLog
    {
        void LogError(Exception ex);
    }
}
