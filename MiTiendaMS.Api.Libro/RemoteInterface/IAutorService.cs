using MiTiendaMS.Api.Libro.RemoteModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiTiendaMS.Api.Libro.RemoteInterface
{
    public interface IAutorService
    {
        Task<(bool Result, AutorRemoteModel Autor, string ErrorMsg)> GetAutor(string id);
    }
}
