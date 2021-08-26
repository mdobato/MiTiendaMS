using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiTiendaMS.Api.Autor.Model;

namespace MiTiendaMS.Api.Autor.Persistence
{
    public class AutorContext : DbContext
    {
        public AutorContext(DbContextOptions<AutorContext> options) : base(options) { }

        public DbSet<AutorModel> Autor { get; set; }
    }
}
