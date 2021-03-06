using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MiTiendaMS.Api.Autor.Application;
using MiTiendaMS.Api.Autor.Persistence;
using MiTiendaMS.Api.Autor.Rabbit;
using MiTiendaMS.RabbitMQ.Bus.BusRabbit;
using MiTiendaMS.RabbitMQ.Bus.Implementation;
using MiTiendaMS.RabbitMQ.Bus.Queue;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

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
            services.AddSingleton<IRabbitEventBus, RabbitEventBus>(sp => {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitEventBus(sp.GetService<IMediator>(), scopeFactory);
            });

            services.AddTransient<EmailEventHandler>();

            services.AddTransient<IEventHandler<EmailQueueEvent>, EmailEventHandler>();
            services.AddDbContext<AutorContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("ConnectionDB"));
            });
            services.AddControllers();
            services.AddMediatR(typeof(AutorCreateDomain.AutorCreateCommandHandler).Assembly);
            services.AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<AutorCreateDomain>());
            services.AddAutoMapper(typeof(AutorRDomain));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Autor", Version = "v1" });
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
                            TokenUrl = new Uri(Configuration["IdentityServer:TokenUrl"]),
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Autor v1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var eventBus = app.ApplicationServices.GetRequiredService<IRabbitEventBus>();
            eventBus.Subscribe<EmailQueueEvent, EmailEventHandler>();
        }
    }
}
