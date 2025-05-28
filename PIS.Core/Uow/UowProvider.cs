using System;
using Microsoft.Extensions.Logging;
using PIS.Framework.Context;
using Microsoft.EntityFrameworkCore;

namespace PIS.Framework.Uow
{
    public class UowProvider : IUowProvider, IDisposable
    {
        public UowProvider()
        { }

        public UowProvider(ILogger<Class1> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        
        public IUnitOfWork CreateUnitOfWork(bool trackChanges = true, bool enableLogging = false)
        {
            var _context = (DbContext)_serviceProvider.GetService(typeof(IEntityContext));

            if ( !trackChanges )
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            var uow = new UnitOfWork(_context, _serviceProvider);
            return uow;
        }

        public IUnitOfWork CreateUnitOfWork<TEntityContext>(bool trackChanges = true, bool enableLogging = false) where TEntityContext : DbContext
        {
            var _context = (TEntityContext)_serviceProvider.GetService(typeof(IEntityContext));

            if (!trackChanges)
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            var uow = new UnitOfWork(_context, _serviceProvider);
            return uow;
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
