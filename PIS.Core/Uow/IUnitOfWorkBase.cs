using System;
using System.Threading;
using System.Threading.Tasks;
using PIS.Framework.Entities;
using PIS.Framework.Repositories;

namespace PIS.Framework.Uow
{
    public interface IUnitOfWorkBase : IDisposable
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        IRepository<TEntity> GetRepository<TEntity>();
        TRepository GetCustomRepository<TRepository>();
        IRepository<TEntity> GetNotValidatedRepository<TEntity>();

    }
}