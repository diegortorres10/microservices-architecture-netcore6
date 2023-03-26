using N5Challenge.Api.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Core.Interfaces
{
    public interface IPermissionsRepository : IGenericRepository<Permissions>
    {
        Task<IEnumerable<Permissions>> GetAllPermissions();
    }
}
