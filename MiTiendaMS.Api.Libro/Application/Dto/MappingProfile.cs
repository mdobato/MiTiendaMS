using AutoMapper;
using MiTiendaMS.Api.Common;
using MiTiendaMS.Api.Libro.Model;
using System.Linq;

namespace MiTiendaMS.Api.Libro.Application.Dto
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LibroModel, LibroDto>();
            CreateMap<PagedCollection<LibroModel>, PagedCollection<LibroDto>>().ConvertUsing<PagedCollectionTypeConverter>();
        }
    }

    public class PagedCollectionTypeConverter : ITypeConverter<PagedCollection<LibroModel>, PagedCollection<LibroDto>>
    {
        public PagedCollection<LibroDto> Convert(PagedCollection<LibroModel> source, PagedCollection<LibroDto> destination, ResolutionContext context)
        {
            var autoresPaged = source.Items.Select(m => context.Mapper.Map<LibroModel, LibroDto>(m)).ToList();
            return new PagedCollection<LibroDto> { Items = autoresPaged, Page = source.Page, Pages = source.Pages, Total = source.Total };
        }
    }
}
