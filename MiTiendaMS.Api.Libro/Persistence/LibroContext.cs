using Microsoft.EntityFrameworkCore;
using MiTiendaMS.Api.Libro.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiTiendaMS.Api.Libro.Persistence
{
    public class LibroContext : DbContext
    {
        public LibroContext(DbContextOptions<LibroContext> options) : base(options) { }

        public DbSet<LibroModel> Libro { get; set; }
    }
}
