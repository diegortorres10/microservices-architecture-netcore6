using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Core.Persistence
{
    public class PermissionTypes
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Descripcion { get; set; }
    }
}
