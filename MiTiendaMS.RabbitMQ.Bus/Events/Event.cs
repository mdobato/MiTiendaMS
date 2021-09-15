using System;
using System.Collections.Generic;
using System.Text;

namespace MiTiendaMS.RabbitMQ.Bus.Events
{
    public abstract class Event
    {
        public DateTime Timestamp { get; protected set; }

        public Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}
