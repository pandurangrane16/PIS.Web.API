using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PIS.Framework.Context;
using PIS.User.Core.Domain;

namespace PIS.User.Core
{
    public class UserContext : EntityContextBase<UserContext>
    {

        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }

        public DbSet<ClientDetails> clientdetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.LowerCaseTablesAndFields();
        }
    }
}
