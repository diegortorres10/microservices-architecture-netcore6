using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Core.Models
{
    public class PermissionModifyRequest
    {
        [Required]
        public int PermisoId { get; set; }
        [Required]
        public string PermisoTypeDescription { get; set; }

        public void Normalize()
        {
            this.PermisoTypeDescription = this.PermisoTypeDescription.ToUpper();
        }
    }
}
