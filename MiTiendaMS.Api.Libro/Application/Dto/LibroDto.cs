using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiTiendaMS.Api.Libro.Application.Dto
{
    public class LibroDto
    {
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string LibroGuid { get; set; }

        public string AutorGuid { get; set; }

        public string NombreAutor { get; set; }
        public string ApellidoAutor { get; set; }
    }
}
