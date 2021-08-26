using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MiTiendaMS.Api.Autor.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MiTiendaMS.Api.Autor.Application;
using FluentValidation.AspNetCore;

namespace MiTiendaMS.Api.Autor
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
            services.AddDbContext<AutorContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("ConnectionDB"));
            });
            services.AddControllers();
            services.AddMediatR(typeof(AutorWDomain.AutorRequestHandler).Assembly);
            services.AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<AutorWDomain>());
            services.AddAutoMapper(typeof(AutorRDomain));
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
