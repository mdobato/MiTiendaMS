using MMLib.SwaggerForOcelot.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiTiendaMS.Api.Gateway.Repository
{
    public class DummySwaggerEndpointRepository : ISwaggerEndpointConfigurationRepository
    {
        private readonly Dictionary<string, ManageSwaggerEndpointData> _endpointDatas =
            new Dictionary<string, ManageSwaggerEndpointData>()
        {
            { "orders_v2", new ManageSwaggerEndpointData() { IsPublished = true } }
        };

        public ManageSwaggerEndpointData GetSwaggerEndpoint(SwaggerEndPointOptions endPoint, string version)
        {
            var lookupKey = $"{endPoint.Key}_{version}";
            var endpointData = new ManageSwaggerEndpointData();
            if (_endpointDatas.ContainsKey(lookupKey))
            {
                endpointData = _endpointDatas[lookupKey];
            }

            return endpointData;
        }
    }
}
