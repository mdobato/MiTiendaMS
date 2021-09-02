using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MiTiendaMS.Api.Libro.Model;
using MiTiendaMS.Api.Libro.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiTiendaMS.Api.Libro.Application
{
    public class LibroCreateWDomain
    {
        public class LibroCreateCommand : IRequest<string>
        {
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public string AutorGuid { get; set; }

        }

        public class LibroValidation : AbstractValidator<LibroCreateCommand>
        {
            public LibroValidation()
            {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.AutorGuid).NotEmpty();
            }
        }

        public class LibroCreateCommandHandler : IRequestHandler<LibroCreateCommand, string>
        {
            private readonly LibroContext _context;
            public LibroCreateCommandHandler(LibroContext context)
            {
                _context = context;
            }
            /// <summary>
            /// Datos venidos del controlador y que se pasaron por el usuario
            /// </summary>
            /// <param name="cmd"></param>
            /// <param name="cancellationToken"></param>
            /// <returns>Devuelve el id del nuevo libro</returns>
            public async Task<string> Handle(LibroCreateCommand cmd, CancellationToken cancellationToken)
            {
                var libro = new LibroModel
                {
                    Titulo = cmd.Titulo,
                    Descripcion = cmd.Descripcion,
                    LibroGuid = System.Convert.ToString(Guid.NewGuid()),
                    AutorGuid = cmd.AutorGuid
                };

                _context.Add(libro);

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return libro.LibroGuid;
                }

                throw new Exception("No se pudo insertar el Libro");

            }
        }
    }

    public class LibroUpdateDomain
    {
        public class LibroUpdateCommand : IRequest
        {
            public string Id { get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public string AutorGuid { get; set; }

        }

        public class LibroValidation : AbstractValidator<LibroUpdateCommand>
        {
            public LibroValidation()
            {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.AutorGuid).NotEmpty();
            }
        }

        public class LibroUpdateCommandHandler : IRequestHandler<LibroUpdateCommand>
        {
            private readonly LibroContext _context;
            public LibroUpdateCommandHandler(LibroContext context)
            {
                _context = context;
            }
            /// <summary>
            /// Datos venidos del controlador y que se pasaron por el usuario
            /// </summary>
            /// <param name="cmd"></param>
            /// <param name="cancellationToken"></param>
            /// <returns> OK / KO </returns>
            public async Task<Unit> Handle(LibroUpdateCommand cmd, CancellationToken cancellationToken)
            {
                var libro = await _context.Libro.Where(a => a.LibroGuid == cmd.Id).FirstOrDefaultAsync();

                if (libro == null) throw new Exception("No existe el libro");

                libro.Titulo = cmd.Titulo;
                libro.Descripcion = cmd.Descripcion;
                libro.AutorGuid = cmd.AutorGuid;

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo actualizar el libro");

            }
        }
    }

    public class LibroDeleteDomain
    {
        public class LibroDeleteCommand : IRequest
        {
            public string Guid { get; set; }

        }


        public class LibroDeleteCommandHandler : IRequestHandler<LibroDeleteCommand>
        {
            private readonly LibroContext _context;
            public LibroDeleteCommandHandler(LibroContext context)
            {
                _context = context;
            }
            /// <summary>
            /// Datos venidos del controlador y que se pasaron por el usuario
            /// </summary>
            /// <param name="cmd"></param>
            /// <param name="cancellationToken"></param>
            /// <returns> OK / KO </returns>
            public async Task<Unit> Handle(LibroDeleteCommand cmd, CancellationToken cancellationToken)
            {
                var libro = await _context.Libro.Where(a => a.LibroGuid == cmd.Guid).FirstOrDefaultAsync();

                if (libro == null) throw new Exception("No existe el libro");

                _context.Libro.Remove(libro);

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo eliminar el libro");

            }
        }
    }
}
