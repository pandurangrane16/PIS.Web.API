using PIS.Framework.Entities;
using PIS.Framework.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Reflection;
using System.Dynamic;
using PIS.Framework.Models;
using PIS.Framework.Uow;

namespace PIS.Framework.Repositories
{
    public abstract class EntityRepositoryBase<TContext, TEntity> : RepositoryBase<TContext>, IRepository<TEntity> where TContext : DbContext where TEntity : EntityBase, new()
    {
        private readonly OrderBy<TEntity> DefaultOrderBy = new OrderBy<TEntity>(qry => qry.OrderBy(e => e.Id));
		protected EntityRepositoryBase(ILogger<Class1> logger, TContext context) : base(logger, context)
        {
			
		}

        public virtual IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = QueryDb(null, orderBy, includes);
            return result.ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = QueryDb(null, orderBy, includes);
            return await result.ToListAsync();
        }

		public virtual void Load(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = QueryDb(null, orderBy, includes);
            result.Load();
        }

        public virtual async Task LoadAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = QueryDb(null, orderBy, includes);
            await result.LoadAsync();
        }

        public virtual IEnumerable<TEntity> GetPage(int startRow, int pageLength, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            if (orderBy == null) orderBy = DefaultOrderBy.Expression;

            var result = QueryDb(null, orderBy, includes);
            return result.Skip(startRow).Take(pageLength).ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetPageAsync(int startRow, int pageLength, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            if (orderBy == null) orderBy = DefaultOrderBy.Expression;

            var result = QueryDb(null, orderBy, includes);
            return await result.Skip(startRow).Take(pageLength).ToListAsync();
        }

        public virtual TEntity Get(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (includes != null)
            {
                query = includes(query);
            }

            return query.SingleOrDefault(x => x.Id == id);
        }

        public virtual Task<TEntity> GetAsync(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (includes != null)
            {
                query = includes(query);
            }

            return query.SingleOrDefaultAsync(x => x.Id == id);
        }

        public virtual IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = QueryDb(filter, orderBy, includes);
            return result.ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = QueryDb(filter, orderBy, includes);
            return await result.ToListAsync();
        }

        public virtual void Load(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = QueryDb(filter, orderBy, includes);
            result.Load();
        }

        public virtual async Task LoadAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = QueryDb(filter, orderBy, includes);
            await result.LoadAsync();
        }

        public virtual IEnumerable<TEntity> QueryPage(int startRow, int pageLength, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            if (orderBy == null) orderBy = DefaultOrderBy.Expression;

            var result = QueryDb(filter, orderBy, includes);
            return result.Skip(startRow).Take(pageLength).ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> QueryPageAsync(int startRow, int pageLength, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            if (orderBy == null) orderBy = DefaultOrderBy.Expression;

            var result = QueryDb(filter, orderBy, includes);
            return await result.Skip(startRow).Take(pageLength).ToListAsync();
        }

        public virtual void Add(TEntity entity)
        {
            if (entity == null) throw new InvalidOperationException("Unable to add a null entity to the repository.");
            Context.Set<TEntity>().Add(entity);
        }

        public virtual TEntity Update(TEntity entity)
        {
            Context.Set<TEntity>().Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            return Context.Set<TEntity>().Update(entity).Entity;
        }

        public virtual void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Attach(entity);
            Context.Entry(entity).State = EntityState.Deleted;
            Context.Set<TEntity>().Remove(entity);
        }

        public virtual void Remove(int id)
        {
            var entity = new TEntity() { Id = id };
            this.Remove(entity);
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.Any();
        }

        public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.AnyAsync();
        }

        public virtual int Count(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.Count();
        }

        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.CountAsync();
        }

        public virtual Task<int> QueryCountAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (includes != null)
            {
                query = includes(query);
            }

            return query.CountAsync();
        }

        protected IQueryable<TEntity> QueryDb(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query;
        }

        public string GetPropertyValue(Type type, string propertyName, object instanceObject)
        {
            var t = type.GetProperty(propertyName).GetValue(instanceObject, null);
            if (t == null)
                return null;
            return t.ToString();
        }
        public void SetUnchanged(TEntity entity)
        {
            base.Context.Entry<TEntity>(entity).State = EntityState.Unchanged;
        }

        public async Task<IEnumerable<TEntity>> GetSearch(string searchTerm)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();
            var type = typeof(TEntity);
            var tEntity = new TEntity();
            var properties = Context.Entry<TEntity>(tEntity).Properties;
            var d = query.ToList();
            IQueryable<TEntity> returnQry;
        R:
            foreach (var item in d)
            {
                bool isVal = false;
                foreach (var property in properties)
                {
                    var name = property.Metadata.Name;
                    var val = GetPropertyValue(type, name, item);
                    if (val != null)
                    {
                        if (val.Contains(searchTerm))
                        {
                            isVal = true;
                            break;
                        }
                    }
                }
                if (isVal == false)
                {
                    d.Remove(item);
                    goto R;
                }
            }
            IEnumerable<TEntity> d1 = d;
            return d1;
        }

        public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> GetOrderBy(string orderColumn, string orderType)
        {
            Type typeQueryable = typeof(IQueryable<TEntity>);
            ParameterExpression argQueryable = Expression.Parameter(typeQueryable, "p");
            var outerExpression = Expression.Lambda(argQueryable, argQueryable);
            string[] props = orderColumn.Split('.');
            IQueryable<TEntity> query = new List<TEntity>().AsQueryable<TEntity>();
            Type type = typeof(TEntity);
            ParameterExpression arg = Expression.Parameter(type, "x");

            Expression expr = arg;
            foreach (string prop in props)
            {
                PropertyInfo pi = type.GetProperty(prop, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            LambdaExpression lambda = Expression.Lambda(expr, arg);
            string methodName = orderType == "asc" ? "OrderBy" : "OrderByDescending";

            MethodCallExpression resultExp =
                Expression.Call(typeof(Queryable), methodName, new Type[] { typeof(TEntity), type }, outerExpression.Body, Expression.Quote(lambda));
            var finalLambda = Expression.Lambda(resultExp, argQueryable);
            return (Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>)finalLambda.Compile();
        }

        public async Task<int> AddCacheMng(CacheManagement _mdl, IUowProvider _uow)
        {
            IEnumerable<CacheManagement> _cacheMng = null;
            using (var uow = _uow.CreateUnitOfWork())   
            {
                var repository = uow.GetRepository<CacheManagement>();
                var includes = new Includes<CacheManagement>(query =>
                {
                    return query.Where(x => x.TableName.ToLower() == _mdl.TableName.ToLower());
                });
                _cacheMng = await repository.GetAllAsync(null, includes.Expression);
                if (_cacheMng.Count() > 0)
                    repository.Update(_mdl);
                else
                    repository.Add(_mdl);
                return await uow.SaveChangesAsync();
            }
        }

        

    }
}