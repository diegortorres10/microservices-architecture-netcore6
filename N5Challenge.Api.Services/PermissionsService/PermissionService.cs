using N5Challenge.Api.Core.Interfaces;
using N5Challenge.Api.Core.Models;
using N5Challenge.Api.Core.Persistence;
using N5Challenge.Api.Services.PermissionsTypeService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Services.PermissionsService
{
    public class PermissionService : IPermissionService
    {
        public IUnitOfWork _unitOfWork;

        public PermissionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreatePermission(PermissionsRequestDto permission)
        {
            if (permission != null)
            {
                permission.Normalize();
                permission.PermissionType.Normalize();

                // Validate permission type exists
                var permissionType = await _unitOfWork.PermissionTypes.GetPermissionTypeByDescription(permission.PermissionType);

                if (permissionType != null)
                {
                    // Create permission with type already created
                    var permissionCreate = new Permissions
                    {
                        NombreEmpleado = permission.NombreEmpleado,
                        ApellidoEmpleado = permission.ApellidoEmpleado,
                        TipoPermiso = permissionType,
                        FechaPermiso = DateTime.UtcNow
                    };

                    await _unitOfWork.Permissions.Add(permissionCreate);

                    var result = _unitOfWork.Save();

                    // Message to kafka
                    var message = new OperationMessage
                    {
                        Id = Guid.NewGuid(),
                        OperationName = "request" // Or "modify" or "get"
                    };

                    if (result > 0)
                        return true;
                    else
                        throw new Exception("No se pudo crear el permiso");
                } else
                {
                    // 1. Create permission type
                    var permissionTypeRequest = new PermissionTypes
                    {
                        Descripcion = permission.PermissionType
                    };

                    await _unitOfWork.PermissionTypes.Add(permissionTypeRequest);

                    var resultType = _unitOfWork.Save();

                    if (resultType > 0)
                    {
                        // 2. Create permission
                        var permissionTypeExists = await _unitOfWork.PermissionTypes.GetById(resultType);
                        var permissionCreate = new Permissions
                        {
                            NombreEmpleado = permission.NombreEmpleado,
                            ApellidoEmpleado = permission.ApellidoEmpleado,
                            TipoPermiso = permissionTypeExists,
                            FechaPermiso = DateTime.UtcNow
                        };

                        await _unitOfWork.Permissions.Add(permissionCreate);

                        var result = _unitOfWork.Save();

                        if (result > 0)
                            return true;
                        else
                            throw new Exception("No se pudo crear el permiso");
                    } else
                    {
                        throw new Exception("No se pudo crear el tipo de permiso");
                    }
                }
            }
            return false;
        }

        public Task<bool> DeletePermission(int permissionId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Permissions>> GetAllPermissions()
        {
            var permissionList = await _unitOfWork.Permissions.GetAllPermissions();
            return permissionList;
        }

        public async Task<Permissions> GetPermissionById(int permissionId)
        {
            if (permissionId > 0)
            {
                var productDetails = await _unitOfWork.Permissions.GetById(permissionId);
                if (productDetails != null)
                {
                    return productDetails;
                }
            }
            return null;
        }

        public async Task<bool> UpdatePermission(int permissionId, PermissionsRequestDto permissionModifyRequest)
        {
            if (permissionModifyRequest != null)
            {
                permissionModifyRequest.Normalize();

                // Validate permission type exists
                var permissionType = await _unitOfWork.PermissionTypes.GetPermissionTypeByDescription(permissionModifyRequest.PermissionType);

                if (permissionType != null)
                {
                    // Update permission with new type
                    var permission = await _unitOfWork.Permissions.GetById(permissionId);

                    if (permission != null)
                    {
                        permission.NombreEmpleado = permissionModifyRequest.NombreEmpleado;
                        permission.ApellidoEmpleado = permissionModifyRequest.ApellidoEmpleado;
                        permission.TipoPermiso = permissionType;
                        permission.FechaPermiso = DateTime.UtcNow;

                        _unitOfWork.Permissions.Update(permission);

                        var result = _unitOfWork.Save();

                        if (result > 0)
                            return true;
                        else
                            return false;
                    } else
                    {
                        throw new Exception("No se pudo encontrar el permiso a modificar");
                    }
                }
                else
                {
                    // 1. Create permission type
                    var permissionTypeRequest = new PermissionTypes
                    {
                        Descripcion = permissionModifyRequest.PermissionType
                    };

                    await _unitOfWork.PermissionTypes.Add(permissionTypeRequest);

                    var resultType = _unitOfWork.Save();

                    if (resultType > 0)
                    {
                        // 2. Update permission
                        var permissionTypeExists = await _unitOfWork.PermissionTypes.GetById(resultType);

                        var permission = await _unitOfWork.Permissions.GetById(permissionId);

                        if (permission != null)
                        {
                            permission.NombreEmpleado = permissionModifyRequest.NombreEmpleado;
                            permission.ApellidoEmpleado = permissionModifyRequest.ApellidoEmpleado;
                            permission.TipoPermiso = permissionTypeExists;
                            permission.FechaPermiso = DateTime.UtcNow;

                            _unitOfWork.Permissions.Update(permission);

                            var result = _unitOfWork.Save();

                            if (result > 0)
                                return true;
                            else
                                return false;
                        }
                        else
                        {
                            throw new Exception("No se pudo encontrar el permiso a modificar");
                        }
                    }
                    else
                    {
                        throw new Exception("No se pudo crear el tipo de permiso");
                    }
                }
            }
            return false;
        }
    }
}
