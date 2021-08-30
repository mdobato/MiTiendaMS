using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiTiendaMS.Api.Gateway.Common
{
    public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions options)
        {
            //options.OperationFilter<AuthorizeOperationFilter>();
            
            options.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
              
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri("https://demo.identityserver.io/connect/authorize"),
                        TokenUrl = new Uri("https://demo.identityserver.io/connect/token"),
                        Scopes = new Dictionary<string, string>
                                {
                                    {"api", "Demo API - full access"}
                                }
                    }
                },
                Description = "Balea Server OpenId Security Scheme"
            });
        }    
    }
}
