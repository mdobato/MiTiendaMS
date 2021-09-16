using MiTiendaMS.RabbitMQ.Bus.Commands;
using MiTiendaMS.RabbitMQ.Bus.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiTiendaMS.RabbitMQ.Bus.BusRabbit
{
    public interface IRabbitEventBus
    {
        Task SendCommand<T>(T command) where T : Command;

        void Publish<T>(T evt) where T : Event;

        void Subscribe<T, TH>() where T : Event
                                where TH : IEventHandler<T>;
    }
}
