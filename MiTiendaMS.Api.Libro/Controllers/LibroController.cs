using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiTiendaMS.Api.Libro.Application;
using MiTiendaMS.Api.Libro.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiTiendaMS.Api.Libro.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class LibroController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LibroController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Devuelve todos los libros registrados
        /// </summary>
        /// <returns>Objetos libro</returns>

        [HttpGet]
        public async Task<ActionResult<List<LibroDto>>> GetLibros()
        {
            return await _mediator.Send(new LibroRDomain.LibrosIRequest());
        }

        /// <summary>
        /// Devuelve un libro por su ID
        /// </summary>
        /// <param name="id">Id del libro</param>
        /// <returns>Objeto libro</returns>

        [HttpGet("{id}")]
        public async Task<ActionResult<LibroDto>> GetLibro(string id)
        {
            return await _mediator.Send(new LibroRDomain.LibroIRequest { LibroGuid = id });
        }

        /// <summary>
        /// Crea un nuevo libro
        /// </summary>
        /// <param name="request">Datos del libro</param>
        /// <returns>Id del nuevo libro creado</returns>
        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(LibroWDomain.LibroRequest request)
        {
            return await _mediator.Send(request);
        }

    }
}
