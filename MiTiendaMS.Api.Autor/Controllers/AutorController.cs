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
        /// <param name="cmd">Datos del autor</param>
        /// <returns>Id del nuevo autor creado</returns>
        [HttpPost]
        public async Task<ActionResult<string>> Crear(AutorCreateDomain.AutorCreateCommand cmd)
        {
            return await _mediator.Send(cmd);
        }
        /// <summary>
        /// Actualiza los datos del autor
        /// </summary>
        /// <param name="id">Id del autor</param>
        /// <param name="cmd">Datos del autor</param>
        /// <returns> OK / KO </returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Actualiza(string id, AutorUpdateDomain.AutorUpdateCommand cmd)
        {
            if (cmd.Id != id)
            {
                return BadRequest();
            }
            return await _mediator.Send(cmd);
        }
        /// <summary>
        /// Elimina el autor
        /// </summary>
        /// <param name="id">Id del autor</param>
        /// <returns> OK / KO </returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Elimina(string id)
        {
            return await _mediator.Send(new AutorDeleteDomain.AutorDeleteCommand { Guid = id });
        }
    }
}
