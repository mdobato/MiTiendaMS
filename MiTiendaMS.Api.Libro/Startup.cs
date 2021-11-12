using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MiTiendaMS.Api.Libro.Application;
using MiTiendaMS.Api.Libro.Persistence;
using MiTiendaMS.Api.Libro.RemoteInterface;
using MiTiendaMS.Api.Libro.RemoteService;
using MiTiendaMS.RabbitMQ.Bus.BusRabbit;
using MiTiendaMS.RabbitMQ.Bus.Implementation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MiTiendaMS.Api.Libro
{
    // Método Startup
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
            //services.AddTransient<IRabbitEventBus, RabbitEventBus>();
            services.AddSingleton<IRabbitEventBus, RabbitEventBus>(sp => {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitEventBus(sp.GetService<IMediator>(), scopeFactory);
            });
            services.AddDbContext<LibroContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("ConnectionDB"));
            });
            services.AddControllers();
            services.AddMediatR(typeof(LibroCreateWDomain.LibroCreateCommandHandler).Assembly);
            services.AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<LibroCreateWDomain>());
            services.AddAutoMapper(typeof(LibroRDomain));
            services.AddHttpClient("Autores", cfg => {
                cfg.BaseAddress = new Uri(Configuration["Services:Autores"]);
            });
            services.AddScoped<IAutorService, AutorService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Libro", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        ClientCredentials = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri(Configuration["IdentityServer:Token"]),
                            Scopes = new Dictionary<string, string>
                            {
                                {Configuration["IdentityServer:Scope"], "Demo API - full access"}
                            }
                        }
                    }
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                        },
                        new[] { Configuration["IdentityServer:Scope"] }
                    }
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Libro v1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
