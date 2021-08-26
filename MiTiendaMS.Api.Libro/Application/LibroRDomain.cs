using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MiTiendaMS.Api.Libro.Application.Dto;
using MiTiendaMS.Api.Libro.Model;
using MiTiendaMS.Api.Libro.Persistence;
using MiTiendaMS.Api.Libro.RemoteInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiTiendaMS.Api.Libro.Application
{
    public class LibroRDomain
    {
        public class LibrosIRequest : IRequest<List<LibroDto>>
        {

        }

        public class LibrosIRequestHandler : IRequestHandler<LibrosIRequest, List<LibroDto>>
        {
            private readonly LibroContext _context;
            private readonly IMapper _mapper;
            private readonly IAutorService _autorSrv;

            public LibrosIRequestHandler(LibroContext context, IMapper mapper, IAutorService autorSrv)
            {
                _context = context;
                _mapper = mapper;
                _autorSrv = autorSrv;
            }

            public async Task<List<LibroDto>> Handle(LibrosIRequest request, CancellationToken cancellationToken)
            {
                var libros = await _context.Libro.ToListAsync();
                var librosDto = _mapper.Map<List<LibroModel>, List<LibroDto>>(libros);

                foreach (var libro in librosDto)
                {
                    var response = await _autorSrv.GetAutor(libro.AutorGuid);
                    if (response.Result)
                    {
                        var autor = response.Autor;
                        libro.NombreAutor = autor.Nombre;
                        libro.ApellidoAutor = autor.Apellido;
                    }
                }

                return librosDto;

            }
        }

        public class LibroIRequest : IRequest<LibroDto>
        {
            public string LibroGuid { get; set; }
        }

        public class LibroIRequestHandler : IRequestHandler<LibroIRequest, LibroDto>
        {
            private readonly LibroContext _context;
            private readonly IMapper _mapper;
            private readonly IAutorService _autorSrv;

            public LibroIRequestHandler(LibroContext context, IMapper mapper, IAutorService autorSrv)
            {
                _context = context;
                _mapper = mapper;
                _autorSrv = autorSrv;
            }

            public async Task<LibroDto> Handle(LibroIRequest request, CancellationToken cancellationToken)
            {
                var libro = await _context.Libro.Where(x => x.LibroGuid == request.LibroGuid).FirstOrDefaultAsync();

                if (libro == null) throw new Exception("No existe el libro");

                var libroDto = _mapper.Map<LibroModel, LibroDto>(libro);

                var response = await _autorSrv.GetAutor(libroDto.AutorGuid);
                if (response.Result)
                {
                    var autor = response.Autor;
                    libroDto.NombreAutor = autor.Nombre;
                    libroDto.ApellidoAutor = autor.Apellido;
                }

                return libroDto;

            }
        }
    }
}
