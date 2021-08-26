using AutoMapper;
using MiTiendaMS.Api.Autor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiTiendaMS.Api.Autor.Application.Dto
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AutorModel, AutorDto>();
        }
    }
}
