using Microsoft.EntityFrameworkCore;
using PIS.Framework.Models;

namespace PIS.Framework.Context
{
    public class EntityContextBase<TContext> : DbContext, IEntityContext where TContext : DbContext
    {
        public EntityContextBase(DbContextOptions<TContext> options) : base(options)
        {
        }
        public DbSet<ErrorLog> errorLogs { get; set; }
        public DbSet<CacheManagement> cacheManagements { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			
			base.OnModelCreating(modelBuilder);
            modelBuilder.LowerCaseTablesAndFields();
			
		}
    }
}
