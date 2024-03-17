using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stossion.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stossion.DbManagement.StossionDbManagement
{
    public class StossionDbContext : IdentityDbContext<StossionUser>
    {
        public  StossionDbContext(DbContextOptions<StossionDbContext> options) : base(options)
        {

        }

        public DbSet<Country> Country { get; set; }
        public DbSet<Gender> Gender {  get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Templates> Templates { get; set; }

    }

}
