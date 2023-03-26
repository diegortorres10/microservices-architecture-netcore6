using N5Challenge.Api.Core.Interfaces;
using N5Challenge.Api.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Infraestructure.Repositories
{
    public class PermissionTypesRepository : GenericRepository<PermissionTypes>, IPermissionTypesRepository
    {
        private readonly DbContextClass _dbContext;

        public PermissionTypesRepository(DbContextClass context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<PermissionTypes> GetPermissionTypeByDescription(string description)
        {
            return _dbContext.Set<PermissionTypes>().FirstOrDefault(p => p.Descripcion == description);
        }
    }
}
