using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Core.Models
{
    public class PermissionsRequestDto
    {
        [Required]
        public string NombreEmpleado { get; set; }
        [Required]
        public string ApellidoEmpleado { get; set; }
        [Required]
        public string PermissionType { get; set; }

        public void Normalize()
        {
            this.NombreEmpleado = this.NombreEmpleado.ToUpper();
            this.ApellidoEmpleado = this.ApellidoEmpleado.ToUpper();
            this.PermissionType = this.PermissionType.ToUpper();
        }
    }
}
