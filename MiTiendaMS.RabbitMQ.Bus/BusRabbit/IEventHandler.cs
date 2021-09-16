using MiTiendaMS.RabbitMQ.Bus.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiTiendaMS.RabbitMQ.Bus.BusRabbit
{
    public interface IEventHandler<in TEvent> : IEventHandler where TEvent: Event
    {
        Task Handle(TEvent evt);

        
    }

    public interface IEventHandler
    {

    }
}
