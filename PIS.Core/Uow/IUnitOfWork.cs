using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using PIS.Framework.Entities;
using PIS.Framework.Repositories;

namespace PIS.Framework
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> ExecutableQuery(string query, string _connectionString);
        Task<DataTable> returnDataTable(string query, string _connectionString);
        IRepository<TEntity> GetRepository<TEntity>();
        TRepository GetCustomRepository<TRepository>();
        IRepository<TEntity> GetNotValidatedRepository<TEntity>();
    }
}
