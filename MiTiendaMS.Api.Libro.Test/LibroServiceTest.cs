using AutoMapper;
using MiTiendaMS.Api.Libro.Application;
using MiTiendaMS.Api.Libro.Application.Dto;
using MiTiendaMS.Api.Libro.Persistence;
using MiTiendaMS.Api.Libro.RemoteInterface;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using GenFu;
using MiTiendaMS.Api.Libro.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MiTiendaMS.Api.Test.Common.Entities;

namespace MiTiendaMS.Api.Libro.Test
{
    public class LibroServiceTest
    {
        private IEnumerable<LibroModel> GetTestData()
        {
            A.Configure<LibroModel>()
                .Fill(p => p.Titulo).AsArticleTitle()
                .Fill(p => p.LibroGuid, () => { return Guid.NewGuid().ToString(); })
                .Fill(p => p.Descripcion).AsArticleTitle();

            var libros = A.ListOf<LibroModel>(50);
            libros[0].LibroGuid = Guid.Empty.ToString();
            return libros;
        }

        private Mock<LibroContext> createContext()
        {
            var data = GetTestData().AsQueryable();
            var mc = new MockContext<LibroContext, LibroModel>(data);
            return mc.GetDataContext(x => x.Libro);
        }

        [Fact]
        public async void GetLibroById()
        {
            var autorSrv = new Mock<IAutorService>();

            var mockContext = createContext();
            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = mapConfig.CreateMapper();

            var request = new LibroRDomain.LibroIRequest();
            request.LibroGuid = Guid.Empty.ToString();

            var handler = new LibroRDomain.LibroIRequestHandler(mockContext.Object, mapper, autorSrv.Object);
            var libro = await handler.Handle(request, new System.Threading.CancellationToken());

            Assert.NotNull(libro);
            Assert.True(libro.LibroGuid == Guid.Empty.ToString());
        }

        [Fact]
        public async void GetLibros()
        {
            int page = 2;
            int take = 2;

            // 1. instanciar el contexto EF (emulado) -> usar objectos de tipo moc
            var libroContext = createContext();

            // 2. Objeto Mapper
            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = mapConfig.CreateMapper();

            // 3. Emular objeto AutorRemote
            var autorSrv = new Mock<IAutorService>();

            // 4. Instanciamos a la clase manejador, pasando los mocks que hemos creado
            LibroRDomain.LibrosIRequestHandler handler = new LibroRDomain.LibrosIRequestHandler(libroContext.Object, mapper, autorSrv.Object);

            // 5. Crear la consulta
            var request = new LibroRDomain.LibrosIRequest() { 
                Page = page, Take = take
            };

            var libros = await handler.Handle(request, new System.Threading.CancellationToken());

            Assert.True(libros.Items.Any());
            Assert.True(libros.Page == page);
            Assert.True(libros.Items.Count() == take);

        }
    }
}
