using MiTiendaMS.RabbitMQ.Bus.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiTiendaMS.RabbitMQ.Bus.Queue
{
    public class EmailQueueEvent : Event
    {
        public string Subject { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public EmailQueueEvent(string subject, string title, string body)
        {
            Subject = subject;
            Title = title;
            Body = body;
        }
    }
}
