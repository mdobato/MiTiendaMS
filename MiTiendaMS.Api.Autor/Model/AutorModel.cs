using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiTiendaMS.Api.Autor.Model
{
    public class AutorModel
    {
        [Key]
        public int AutorId { get; set; } 
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string AutorGuid { get; set; }
    }
}
