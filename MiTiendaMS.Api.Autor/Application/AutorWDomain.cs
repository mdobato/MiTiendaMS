using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MiTiendaMS.Api.Autor.Model;
using MiTiendaMS.Api.Autor.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace MiTiendaMS.Api.Autor.Application
{

    public class AutorCreateDomain
    {
        public class AutorCreateCommand : IRequest<string>
        {
            public string Nombre { get; set; }
            public string Apellido { get; set; }
        }

        public class AutorValidation : AbstractValidator<AutorCreateCommand>
        {
            public AutorValidation()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellido).NotEmpty();
            }
        }

        public class AutorCreateCommandHandler : IRequestHandler<AutorCreateCommand, string>
        {
            private readonly AutorContext _context;

            public AutorCreateCommandHandler(AutorContext context)
            {
                _context = context;
            }
            /// <summary>
            /// Datos venidos del controlador y que se pasaron por el usuario
            /// </summary>
            /// <param name="cmd">Datos enviados por el usuario</param>
            /// <param name="cancellationToken"></param>
            /// <returns>Devuelve el id del nuevo autor</returns>
            public async Task<string> Handle(AutorCreateCommand cmd, CancellationToken cancellationToken)
            {
                AutorModel autor = new AutorModel
                {
                    Nombre = cmd.Nombre,
                    Apellido = cmd.Apellido,
                    AutorGuid = System.Convert.ToString(Guid.NewGuid())
                };
                _context.Autor.Add(autor);

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return autor.AutorGuid;
                }

                throw new Exception("No se pudo insertar el Autor del libro");
            }
        }
    }
    public class AutorUpdateDomain
    {
        public class AutorUpdateCommand : IRequest
        {
            public string Id { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
        }

        public class AutorValidation : AbstractValidator<AutorUpdateCommand>
        {
            public AutorValidation()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellido).NotEmpty();
            }
        }

        public class AutorUpdateCommandHandler : IRequestHandler<AutorUpdateCommand>
        {
            private readonly AutorContext _context;

            public AutorUpdateCommandHandler(AutorContext context)
            {
                _context = context;
            }
            /// <summary>
            /// Datos venidos del controlador y que se pasaron por el usuario
            /// </summary>
            /// <param name="cmd">Datos enviados por el usuario</param>
            /// <param name="cancellationToken"></param>
            /// <returns> Devuelve OK/KO </returns>
            public async Task<Unit> Handle(AutorUpdateCommand cmd, CancellationToken cancellationToken)
            {
                var autor = await _context.Autor.Where(a => a.AutorGuid == cmd.Id).FirstOrDefaultAsync();

                if (autor == null) throw new Exception("No existe el autor");

                autor.Nombre = cmd.Nombre;
                autor.Apellido = cmd.Apellido;

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo actualizar el Autor del libro");
            }
        }
    }
    public class AutorDeleteDomain
    {
        public class AutorDeleteCommand : IRequest
        {
            public string Guid { get; set; }

        }


        public class AutorDeleteCommandHandler : IRequestHandler<AutorDeleteCommand>
        {
            private readonly AutorContext _context;
            public AutorDeleteCommandHandler(AutorContext context)
            {
                _context = context;
            }
            /// <summary>
            /// Datos venidos del controlador y que se pasaron por el usuario
            /// </summary>
            /// <param name="cmd"></param>
            /// <param name="cancellationToken"></param>
            /// <returns> OK / KO </returns>
            public async Task<Unit> Handle(AutorDeleteCommand cmd, CancellationToken cancellationToken)
            {
                var autor = await _context.Autor.Where(a => a.AutorGuid == cmd.Guid).FirstOrDefaultAsync();

                if (autor == null) throw new Exception("No existe el Autor");

                _context.Autor.Remove(autor);

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo eliminar el Autor");

            }
        }
    }

}
