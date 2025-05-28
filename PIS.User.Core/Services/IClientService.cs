using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIS.User.Core.Domain;

namespace PIS.User.Core.Services
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDetails>> GetClientDetails();
        Task<int> AddClientDetails(ClientDetails _details);
    }
}
