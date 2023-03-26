using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Core.Persistence
{
    public class Permissions
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string NombreEmpleado { get; set; }
        [Required]
        public string ApellidoEmpleado { get; set; }
        [Required]
        public PermissionTypes TipoPermiso { get; set; }
        [Required]
        public DateTime FechaPermiso { get; set; }
    }
}
