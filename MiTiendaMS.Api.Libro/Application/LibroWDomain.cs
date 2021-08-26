using FluentValidation;
using MediatR;
using MiTiendaMS.Api.Libro.Model;
using MiTiendaMS.Api.Libro.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiTiendaMS.Api.Libro.Application
{
    public class LibroWDomain
    {
        public class LibroRequest : IRequest
        {
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public string AutorGuid { get; set; }

        }

        public class LibroValidation : AbstractValidator<LibroRequest>
        {
            public LibroValidation()
            {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.AutorGuid).NotEmpty();
            }
        }

        public class LibroRequestHandler : IRequestHandler<LibroRequest>
        {
            private readonly LibroContext _context;
            public LibroRequestHandler(LibroContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(LibroRequest request, CancellationToken cancellationToken)
            {
                var libro = new LibroModel
                {
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    LibroGuid = System.Convert.ToString(Guid.NewGuid()),
                    AutorGuid = request.AutorGuid
                };

                _context.Add(libro);

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo insertar el Libro");

            }
        }
    }
}
