using Microsoft.EntityFrameworkCore;
using N5Challenge.Api.Core.Interfaces;
using N5Challenge.Api.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Infraestructure.Repositories
{
    public class PermissionsRepository : GenericRepository<Permissions>, IPermissionsRepository
    {
        private readonly DbContextClass _dbContext;

        public PermissionsRepository(DbContextClass context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<Permissions>> GetAllPermissions()
        {
            return _dbContext.Set<Permissions>().Include(x => x.TipoPermiso);
        }
    }
}
