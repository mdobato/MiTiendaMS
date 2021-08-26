using FluentValidation;
using MediatR;
using MiTiendaMS.Api.Autor.Model;
using MiTiendaMS.Api.Autor.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace MiTiendaMS.Api.Autor.Application
{
    /// <summary>
    /// Operaciones de escritura sobre el contexto DB
    /// </summary>
    public class AutorWDomain
    {
        public class AutorRequest : IRequest
        {
            public string Nombre { get; set; }
            public string Apellido { get; set; }
        }

        public class AutorValidation : AbstractValidator<AutorRequest>
        {
            public AutorValidation()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellido).NotEmpty();
            }
        }

        public class AutorRequestHandler : IRequestHandler<AutorRequest>
        {
            private readonly AutorContext _context;

            public AutorRequestHandler(AutorContext context)
            {
                _context = context;
            }
            /// <summary>
            /// Datos venidos del controlador y que se pasaron por el usuario
            /// </summary>
            /// <param name="request">Datos enviados por el usuario</param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<Unit> Handle(AutorRequest request, CancellationToken cancellationToken)
            {
                AutorModel autor = new AutorModel
                {
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    AutorGuid = System.Convert.ToString(Guid.NewGuid())
                };
                _context.Autor.Add(autor);

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo insertar el Autor del libro");
            }
        }
    }


}
