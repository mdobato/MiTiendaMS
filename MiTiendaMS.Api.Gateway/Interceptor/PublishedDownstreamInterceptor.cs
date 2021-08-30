using Microsoft.AspNetCore.Http;
using MiTiendaMS.Api.Gateway.Repository;
using MMLib.SwaggerForOcelot.Configuration;
using MMLib.SwaggerForOcelot.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiTiendaMS.Api.Gateway.Interceptor
{
    public class PublishedDownstreamInterceptor : ISwaggerDownstreamInterceptor
    {
        private readonly ISwaggerEndpointConfigurationRepository _endpointConfigurationRepository;

        public PublishedDownstreamInterceptor(ISwaggerEndpointConfigurationRepository endpointConfigurationRepository)
        {
            _endpointConfigurationRepository = endpointConfigurationRepository;
        }


        public bool DoDownstreamSwaggerEndpoint(HttpContext httpContext, string version, SwaggerEndPointOptions endPoint)
        {
            //httpContext.Request.Headers.Add("Bearer", "eyJhbGciOiJSUzI1NiIsImtpZCI6IjQzNkI5QTE0NUYzMEIyOEMxMDNCQUY4MjYzNkMyN0FBIiwidHlwIjoiYXQrand0In0.eyJuYmYiOjE2MzAzNDU4NzEsImV4cCI6MTYzMDM0OTQ3MSwiaXNzIjoiaHR0cHM6Ly9kZW1vLmlkZW50aXR5c2VydmVyLmlvIiwiYXVkIjoiYXBpIiwiY2xpZW50X2lkIjoiaW50ZXJhY3RpdmUuY29uZmlkZW50aWFsIiwic3ViIjoiMSIsImF1dGhfdGltZSI6MTYzMDM0NTg3MCwiaWRwIjoibG9jYWwiLCJqdGkiOiI0RDFDM0ZEQzYwOUI1NzdFNzU5QzI4QjlDRTlCMUE4QSIsInNpZCI6IkI4RDY0N0I5RUZCOTRBN0RENjg2RDM2N0M4Qzg0RUZGIiwiaWF0IjoxNjMwMzQ1ODcxLCJzY29wZSI6WyJvcGVuaWQiLCJwcm9maWxlIiwiYXBpIiwiZW1haWwiLCJvZmZsaW5lX2FjY2VzcyJdLCJhbXIiOlsicHdkIl19.Igyc1RELJ4UQOnwySGta2EcUgRBiRBp1TnvxAR2ESda6ihYVaFSCAXzzE2jpevnmepJ7I_ksspg_DOQcn7YdwguwQSTA0XY1q1IJOMo8bTTDJgUuM0_S6XxmujRwE-mZa7o_SeISkh_kwR2d2QeNp2dSwP9fcc8aeeEOraFHCYK0q8LMjF5t_yD2cbS8He88pCkIVzqqgpfqPuKdDD0a3AcPN7lKCGk1LyuHH9ZJtZHmkSO0ww6vPAeJeryXVIy-XVMUbQ2dlfsH2OhyR-gRGScDTwmEcI9_9MHf90ByzPGotYEywONyl60P5Q_WkZKYMDT01zt85SqonS5imXtxWg");

            //var myEndpointConfiguration = _endpointConfigurationRepository.GetSwaggerEndpoint(endPoint, version);

            //if (!myEndpointConfiguration.IsPublished)
            //{
            //    httpContext.Response.StatusCode = 404;
            //    httpContext.Response.WriteAsync("This enpoint is under development, please come back later.");
            //}

            return true;//myEndpointConfiguration.IsPublished;
        }
    }
}
