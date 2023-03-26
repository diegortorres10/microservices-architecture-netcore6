using N5Challenge.Api.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Infraestructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContextClass _dbContext;

        public IPermissionsRepository Permissions { get; }

        public IPermissionTypesRepository PermissionTypes { get; }

        public UnitOfWork(DbContextClass dbContext, IPermissionsRepository permissions, IPermissionTypesRepository permissionTypes)
        {
            _dbContext = dbContext;
            Permissions = permissions;
            PermissionTypes = permissionTypes;
        }

        public int Save()
        {
            return _dbContext.SaveChanges();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
    }
}
