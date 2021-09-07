using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MiTiendaMS.Api.Autor.Application.Dto;
using MiTiendaMS.Api.Common;
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
        public class AutoresRequest : IRequest<PagedCollection<AutorDto>>
        {
            public int Page { get; set; }
            public int Take { get; set; }
        }

        public class AutoresRequestHandler : IRequestHandler<AutoresRequest, PagedCollection<AutorDto>>
        {
            private readonly AutorContext _context;
            private readonly IMapper _mapper;

            public AutoresRequestHandler(AutorContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<PagedCollection<AutorDto>> Handle(AutoresRequest request, CancellationToken cancellationToken)
            {
                var autoresPaged = await _context.Autor.GetPagedAsync(request.Page, request.Take);
                var autoresDtoPaged = _mapper.Map<PagedCollection<AutorModel>, PagedCollection<AutorDto>>(autoresPaged);
                return autoresDtoPaged;
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
