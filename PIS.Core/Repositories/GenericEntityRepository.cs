using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PIS.Framework.Entities;

namespace PIS.Framework.Repositories
{
    public class GenericEntityRepository<TEntity> : EntityRepositoryBase<DbContext, TEntity> where TEntity : EntityBase, new()
    {
		public GenericEntityRepository(ILogger<Class1> logger) : base(logger, null)
		{ }
	}
}