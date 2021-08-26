using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MiTiendaMS.Api.Libro.Application;
using MiTiendaMS.Api.Libro.Persistence;
using MiTiendaMS.Api.Libro.RemoteInterface;
using MiTiendaMS.Api.Libro.RemoteService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiTiendaMS.Api.Libro
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<LibroContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("ConnectionDB"));
            });
            services.AddControllers();
            services.AddMediatR(typeof(LibroWDomain.LibroRequestHandler).Assembly);
            services.AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<LibroWDomain>());
            services.AddAutoMapper(typeof(LibroRDomain));
            services.AddHttpClient("Autores", cfg => {
                cfg.BaseAddress = new Uri(Configuration["Services:Autores"]);
            });
            services.AddScoped<IAutorService, AutorService>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
