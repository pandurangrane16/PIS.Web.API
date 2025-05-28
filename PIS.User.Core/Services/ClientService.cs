using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIS.Framework;
using PIS.Framework.Query;
using PIS.User.Core.Domain;

namespace PIS.User.Core.Services
{
    public class ClientService : IClientService
    {
        private readonly IUowProvider _uowProvider;
        private readonly IErrorLog _errorLog;
        public ClientService(IUowProvider uowProvider, IErrorLog errorLog)
        {
            _uowProvider = uowProvider;
            _errorLog = errorLog;
        }
        public async Task<IEnumerable<ClientDetails>> GetClientDetails()
        {
            try
            {
                IEnumerable<ClientDetails> clientDetails = null;

                using (var uow = _uowProvider.CreateUnitOfWork())
                {
                    var clientRepo = uow.GetRepository<ClientDetails>();
                    var includes = new Includes<ClientDetails>(query =>
                    {
                        return query;
                    });
                    clientDetails = await clientRepo.GetAllAsync(null, includes.Expression);
                   // int i = Convert.ToInt32(clientDetails);
                }

                return clientDetails;
            }
            catch(Exception ex)
            {
                _errorLog.LogError(ex);
                return null;
            }
            
        }

        public async Task<int> AddClientDetails(ClientDetails _details)
        {
            try
            {
                using (var uow = _uowProvider.CreateUnitOfWork())
                {
                    _details.CreatedDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    var repository = uow.GetRepository<ClientDetails>();
                    repository.Add(_details);
                    return await uow.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                _errorLog.LogError(ex);
                return -1;
            }
        }
    }
}
