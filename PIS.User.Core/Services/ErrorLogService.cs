using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PIS.Framework;
using PIS.Framework.Models;
using PIS.User.Core.Domain;

namespace PIS.User.Core.Services
{
    public class ErrorLogService : IErrorLog
    {
        private readonly IUowProvider _uowProvider;
        public ErrorLogService(IUowProvider uowProvider)
        {
            _uowProvider = uowProvider;
        }
        public async void LogError(Exception ex)
        {
            DateTime dt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            ErrorLog _details = new ErrorLog();
            _details.StackTrace = ex.StackTrace == null ? "" : ex.StackTrace;
            _details.Message = ex.Message;
            _details.CreatedDate = dt;
            _details.LoggedAt = dt;
            using (var uow = _uowProvider.CreateUnitOfWork())
            {
                var repository = uow.GetRepository<ErrorLog>();
                repository.Add(_details);
                //await uow.SaveChangesAsync();
            }
        }
    }
}
