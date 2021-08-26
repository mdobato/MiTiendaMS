using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MiTiendaMS.Api.Autor.Application.Dto;
using MiTiendaMS.Api.Autor.Model;
using MiTiendaMS.Api.Autor.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiTiendaMS.Api.Autor.Application
{
    public class AutorRDomain
    {
        public class AutoresRequest : IRequest<List<AutorDto>>
        {

        }

        public class AutoresRequestHandler : IRequestHandler<AutoresRequest, List<AutorDto>>
        {
            private readonly AutorContext _context;
            private readonly IMapper _mapper;

            public AutoresRequestHandler(AutorContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<AutorDto>> Handle(AutoresRequest request, CancellationToken cancellationToken)
            {
                var autores = await _context.Autor.ToListAsync();
                var autoresDto = _mapper.Map<List<AutorModel>, List<AutorDto>>(autores);
                return autoresDto;
            }
        }

        public class AutorRequest : IRequest<AutorDto>
        {
            public string Guid { get; set; }
        }

        public class AutorRequestHandler : IRequestHandler<AutorRequest, AutorDto>
        {
            private readonly AutorContext _context;
            private readonly IMapper _mapper;

            public AutorRequestHandler(AutorContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<AutorDto> Handle(AutorRequest request, CancellationToken cancellationToken)
            {
                var autor = await _context.Autor.Where(x => x.AutorGuid == request.Guid).FirstOrDefaultAsync();

                if (autor == null) throw new Exception("No existe el autor");

                var autorDto = _mapper.Map<AutorModel, AutorDto>(autor);

                return autorDto;
            }
        }
    }
}
