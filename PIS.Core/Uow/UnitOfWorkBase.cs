using System;
using System.Threading;
using System.Threading.Tasks;
using PIS.Framework.Exceptions;
using PIS.Framework.Repositories;
using PIS.Framework.Entities;
using Microsoft.EntityFrameworkCore;
using UOW.Framework.Exceptions;
using System.Collections.Generic;
using Npgsql;
using static PIS.Framework.Options.Defaults;
using System.IO;
using System.Data;

namespace PIS.Framework.Uow
{
    public abstract class UnitOfWorkBase<TContext> : IUnitOfWorkBase where TContext : DbContext
    {
        protected internal UnitOfWorkBase(TContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        protected TContext _context;
        protected readonly IServiceProvider _serviceProvider;

        public int SaveChanges()
        {
            CheckDisposed();
            return _context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            CheckDisposed();
            return _context.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            CheckDisposed();
            return _context.SaveChangesAsync(cancellationToken);
        }

        public IRepository<TEntity> GetRepository<TEntity>()
        {
            CheckDisposed();
            var repositoryType = typeof(IRepository<TEntity>);
            var repository = (IRepository<TEntity>)_serviceProvider.GetService(repositoryType);
            if (repository == null)
            {
                throw new RepositoryNotFoundException(repositoryType.Name, String.Format("Repository {0} not found in the IOC container. Check if it is registered during startup.", repositoryType.Name));
            }

            ((IRepositoryInjection)repository).SetContext(_context);
            return repository;
        }

        public TRepository GetCustomRepository<TRepository>()
        {
            CheckDisposed();
            var repositoryType = typeof(TRepository);
            var repository = (TRepository)_serviceProvider.GetService(repositoryType);
            if (repository == null)
            {
                throw new RepositoryNotFoundException(repositoryType.Name, String.Format("Repository {0} not found in the IOC container. Check if it is registered during startup.", repositoryType.Name));
            }

            ((IRepositoryInjection)repository).SetContext(_context);
            return repository;
        }

        public IRepository<TEntity> GetNotValidatedRepository<TEntity>()
        {
            CheckDisposed();
            var repositoryType = typeof(IRepository<TEntity>);
            var repository = (IRepository<TEntity>)_serviceProvider.GetService(repositoryType);
            if (repository == null)
            {
                throw new RepositoryNotFoundException(repositoryType.Name, String.Format("Repository {0} not found in the IOC container. Check if it is registered during startup.", repositoryType.Name));
            }

            //((IRepositoryInjection)repository).SetContext(_context);
            return repository;
        }

        public async Task<int> ExecutableQuery(string query, string _connectionString)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string Query = query;

                    using (var command = new NpgsqlCommand(Query, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public async Task<DataTable> returnDataTable(string query, string _connectionString)
        {
            try
            {
                DataTable dt = new DataTable();
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string Query = query;

                    using (NpgsqlCommand cmd = new NpgsqlCommand(Query, connection))
                    {
                        cmd.CommandTimeout = 300;
                        NpgsqlDataAdapter _dtap = new NpgsqlDataAdapter(cmd);
                        _dtap.Fill(dt);
                        cmd.Dispose();
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #region IDisposable Implementation

        protected bool _isDisposed;

        protected void CheckDisposed()
        {
            if (_isDisposed) throw new ObjectDisposedException("The UnitOfWork is already disposed and cannot be used anymore.");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    if (_context != null)
                    {
                        _context.Dispose();
                        _context = null;
                    }
                }
            }
            _isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWorkBase()
        {
            Dispose(false);
        }
        public void Log(string filename, string error)
        {
            try
            {
                DateTime now = DateTime.Now;
                string _LogPath = "C:\\CVMS_QUERY_LOG\\" + DateTime.Now.Month + "_" + DateTime.Now.Year + "\\";
                if (!Directory.Exists(_LogPath))
                {
                    Directory.CreateDirectory(_LogPath);
                }
                using (StreamWriter SW = new StreamWriter(_LogPath + filename + "-LOG-" + DateTime.Now.Day.ToString() + "", true))
                {
                    SW.WriteLine(DateTime.Now.ToString() + " | " + error);
                    //SW.WriteLine(error);
                    SW.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}