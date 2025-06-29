using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PIS.Framework.Context;
using PIS.Framework.Models;
using PIS.User.Core.Domain;

namespace PIS.User.Core
{
    public class UserContext : EntityContextBase<UserContext>
    {

        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }
        public DbSet<ErrorLog> errorLogs { get; set; }
        public DbSet<ClientDetails> clientdetails { get; set; }
        public DbSet<UserLogin> userlogin { get; set; }
        public DbSet<MenuMaster> menuMasters { get; set; }
        public DbSet<RegistrationDetails> registrationDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.LowerCaseTablesAndFields();
        }
    }
}
