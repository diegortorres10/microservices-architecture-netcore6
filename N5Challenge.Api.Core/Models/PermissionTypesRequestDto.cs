using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Core.Models
{
    public class PermissionTypesRequestDto
    {
        [Required]
        public string Descripcion { get; set; }

        public void Normalize()
        {
            this.Descripcion = this.Descripcion.ToUpper();
        }
    }
}
