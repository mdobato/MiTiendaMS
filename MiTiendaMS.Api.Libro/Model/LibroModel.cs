using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiTiendaMS.Api.Libro.Model
{
    public class LibroModel
    {
        [Key]
        public int LibroId { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string LibroGuid { get; set; }

        public string AutorGuid { get; set; }
    }
}
