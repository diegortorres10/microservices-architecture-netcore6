using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N5Challenge.Api.Core.Models;
using N5Challenge.Api.Core.Persistence;

namespace N5Challenge.Api.Services.PermissionsService
{
    public interface IPermissionService
    {
        Task<bool> CreatePermission(PermissionsRequestDto permission);
        Task<IEnumerable<Permissions>> GetAllPermissions();
        Task<Permissions> GetPermissionById(int permissionId);
        Task<bool> UpdatePermission(int permissionId, PermissionsRequestDto permissionModifyRequest);
        Task<bool> DeletePermission(int permissionId);
    }
}
