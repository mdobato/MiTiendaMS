using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiTiendaMS.Api.Autor.Application;
using MiTiendaMS.Api.Autor.Application.Dto;
using MiTiendaMS.Api.Autor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiTiendaMS.Api.Autor.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AutorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<AutorDto>>> GetAutores()
        {
            return await _mediator.Send(new AutorRDomain.AutoresRequest());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AutorDto>> GetAutor(string id)
        {
            return await _mediator.Send(new AutorRDomain.AutorRequest { Guid = id });
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(AutorWDomain.AutorRequest request)
        {
            return await _mediator.Send(request);
        }
    }
}
