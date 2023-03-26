using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N5Challenge.Api.Core.Models;

namespace N5Challenge.Api.Services.PermissionsTypeService
{
    public interface IPermissionsTypeService
    {
        Task<bool> CreatePermissionType(PermissionTypesRequestDto permissionType);
        Task<IEnumerable<PermissionTypesRequestDto>> GetAllPermissionsType();
        Task<PermissionTypesRequestDto> GetPermissionTypeById(int permissionTypeId);
        Task<bool> UpdatePermissionType(PermissionTypesRequestDto permissionType);
        Task<bool> DeletePermissionType(int permissionTypeId);
    }
}
