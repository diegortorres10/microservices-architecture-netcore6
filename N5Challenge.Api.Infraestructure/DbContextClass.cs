using Microsoft.EntityFrameworkCore;
using N5Challenge.Api.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Infraestructure
{
    public class DbContextClass : DbContext
    {
        public DbContextClass(DbContextOptions<DbContextClass> contextOptions) : base(contextOptions)
        {
        }

        public DbSet<Permissions> Permissions { get; set; }
        public DbSet<PermissionTypes> PermissionTypes { get; set; }
    }
}
