using Microsoft.Extensions.Logging;
using MiTiendaMS.RabbitMQ.Bus.BusRabbit;
using MiTiendaMS.RabbitMQ.Bus.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiTiendaMS.Api.Autor.Rabbit
{
    public class EmailEventHandler : IEventHandler<EmailQueueEvent>
    {
        private readonly ILogger<EmailEventHandler> _logger;

        public EmailEventHandler() {}

        public EmailEventHandler(ILogger<EmailEventHandler> logger)
        {
            _logger = logger;

        }

        public Task Handle(EmailQueueEvent evt)
        {
            _logger.LogInformation($"Mensaje de la subscripción al bus: {evt.Subject} {evt.Title}");
            return Task.CompletedTask;
        }
    }
}
