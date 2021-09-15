using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiTiendaMS.Api.Common;
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
        public async Task<ActionResult<PagedCollection<LibroDto>>> GetLibros(int pageParam = 1, int takeParam = 10)
        {
            return await _mediator.Send(new LibroRDomain.LibrosIRequest { Page = pageParam, Take = takeParam });
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
        /// <param name="cmd">Datos del libro</param>
        /// <returns>Id del nuevo libro creado</returns>
        [HttpPost]
        public async Task<ActionResult<string>> Crear(LibroCreateWDomain.LibroCreateCommand cmd)
        {
            return await _mediator.Send(cmd);
        }

        /// <summary>
        /// Actualiza los datos del libro por su Id
        /// </summary>
        /// <param name="id">Id del libro</param>
        /// <param name="cmd">Datos del libro</param>
        /// <returns> OK / KO </returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Actualiza(string id, LibroUpdateDomain.LibroUpdateCommand cmd)
        {
            if(cmd.Id != id)
            {
                return BadRequest();
            }
            return await _mediator.Send(cmd);
        }
        /// <summary>
        /// Elimina el libro
        /// </summary>
        /// <param name="id">Id del autor</param>
        /// <returns> OK / KO </returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Elimina(string id)
        {
            return await _mediator.Send(new LibroDeleteDomain.LibroDeleteCommand { Guid = id });
        }


    }
}
