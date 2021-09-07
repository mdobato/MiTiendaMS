using AutoMapper;
using MiTiendaMS.Api.Common;
using MiTiendaMS.Api.Autor.Model;
using System.Linq;

namespace MiTiendaMS.Api.Autor.Application.Dto
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AutorModel, AutorDto>();
            CreateMap<PagedCollection<AutorModel>, PagedCollection<AutorDto>>().ConvertUsing<PagedCollectionTypeConverter>();
        }
    }

    public class PagedCollectionTypeConverter : ITypeConverter<PagedCollection<AutorModel>, PagedCollection<AutorDto>>
    {
        public PagedCollection<AutorDto> Convert(PagedCollection<AutorModel> source, PagedCollection<AutorDto> destination, ResolutionContext context)
        {
            var autoresPaged = source.Items.Select(m => context.Mapper.Map<AutorModel, AutorDto>(m)).ToList();
            return new PagedCollection<AutorDto> { Items = autoresPaged, Page = source.Page, Pages = source.Pages, Total = source.Total };
        }
    }

}
