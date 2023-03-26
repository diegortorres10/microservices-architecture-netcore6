using N5Challenge.Api.Core.Interfaces;
using N5Challenge.Api.Core.Models;
using N5Challenge.Api.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Services.PermissionsTypeService
{
    public class PermissionsTypeService : IPermissionsTypeService
    {
        public IUnitOfWork _unitOfWork;

        public PermissionsTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreatePermissionType(PermissionTypesRequestDto permissionTypeRequest)
        {
            if (permissionTypeRequest != null)
            {
                var permissionType = new PermissionTypes
                {
                    Descripcion = permissionTypeRequest.Descripcion
                };

                await _unitOfWork.PermissionTypes.Add(permissionType);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public Task<bool> DeletePermissionType(int permissionTypeId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PermissionTypesRequestDto>> GetAllPermissionsType()
        {
            throw new NotImplementedException();
        }

        public Task<PermissionTypesRequestDto> GetPermissionTypeById(int permissionTypeId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePermissionType(PermissionTypesRequestDto permissionType)
        {
            throw new NotImplementedException();
        }
    }
}
