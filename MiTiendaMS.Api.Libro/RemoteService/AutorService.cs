using Microsoft.Extensions.Logging;
using MiTiendaMS.Api.Libro.RemoteInterface;
using MiTiendaMS.Api.Libro.RemoteModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiTiendaMS.Api.Libro.RemoteService
{
    public class AutorService : IAutorService
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<AutorService> _logger;
        public AutorService(IHttpClientFactory httpClient, ILogger<AutorService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<(bool Result, AutorRemoteModel Autor, string ErrorMsg)> GetAutor(string id)
        {
            try
            {
                var client = _httpClient.CreateClient("Autores");
                var response = await client.GetAsync($"/Autor/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var contenido = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var autor = JsonSerializer.Deserialize<AutorRemoteModel>(contenido, options);
                    return (true, autor, null);
                }
                return (false, null, response.ReasonPhrase);
            }
            catch(Exception ex)
            {
                _logger?.LogError($"Se ha producido un error: {ex.Message}");
                return (false, null, ex.Message);
            }
        }
    }
}
