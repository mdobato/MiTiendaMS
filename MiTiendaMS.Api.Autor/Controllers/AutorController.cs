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

        /// <summary>
        /// Devuelve todos los autores registrados
        /// </summary>
        /// <returns>Objetos autor</returns>
        [HttpGet]
        public async Task<ActionResult<List<AutorDto>>> GetAutores()
        {
            return await _mediator.Send(new AutorRDomain.AutoresRequest());
        }
        /// <summary>
        /// Devuelve un autor por su ID
        /// </summary>
        /// <param name="id">Id del autor</param>
        /// <returns>Objeto autor</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<AutorDto>> GetAutor(string id)
        {
            return await _mediator.Send(new AutorRDomain.AutorRequest { Guid = id });
        }

        /// <summary>
        /// Crea un nuevo autor
        /// </summary>
        /// <param name="request">Datos del autor</param>
        /// <returns>Id del nuevo autor creado</returns>
        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(AutorWDomain.AutorRequest request)
        {
            return await _mediator.Send(request);
        }
    }
}
