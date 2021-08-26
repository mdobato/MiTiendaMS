using AutoMapper;
using MiTiendaMS.Api.Libro.Application.Dto;
using MiTiendaMS.Api.Libro.Model;

namespace MiTiendaMS.Api.Autor.Application.Dto
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LibroModel, LibroDto>();
        }
    }
}
