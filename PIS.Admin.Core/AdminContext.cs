using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PIS.Framework.Context;

namespace PIS.Admin.Core
{
    public class AdminContext : EntityContextBase<AdminContext>
    {

        public AdminContext(DbContextOptions<AdminContext> options) : base(options)
        {

        }

        public DbSet<ClientDetails> clientdetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.LowerCaseTablesAndFields();
        }
    }
}
