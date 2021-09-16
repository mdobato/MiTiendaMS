using MiTiendaMS.RabbitMQ.Bus.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiTiendaMS.RabbitMQ.Bus.Commands
{
    public abstract class Command : Message
    {
        public DateTime Timestamp { get; protected set; }

        protected Command()
        {
            Timestamp = DateTime.Now;
        }
    }
}
