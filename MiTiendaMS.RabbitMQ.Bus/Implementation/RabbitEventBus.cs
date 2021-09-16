using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MiTiendaMS.RabbitMQ.Bus.BusRabbit;
using MiTiendaMS.RabbitMQ.Bus.Commands;
using MiTiendaMS.RabbitMQ.Bus.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTiendaMS.RabbitMQ.Bus.Implementation
{
    public class RabbitEventBus : IRabbitEventBus
    {
        private readonly IMediator _mediator;
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly List<Type> _eventTypes;
        private readonly IServiceScopeFactory _srvScopeFactory;

        public RabbitEventBus(IMediator mediator, IServiceScopeFactory srvScopeFactory)
        {
            _mediator = mediator;
            _handlers = new Dictionary<string, List<Type>>();
            _eventTypes = new List<Type>();
            _srvScopeFactory = srvScopeFactory;
        }
        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

        public void Publish<T>(T evt) where T : Event
        {
            var factory = new ConnectionFactory() { HostName = "rabbit-web" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var evtName = evt.GetType().Name;
                    channel.QueueDeclare(evtName, false, false, false, null);
                    var msg = JsonConvert.SerializeObject(evt);
                    var body = Encoding.UTF8.GetBytes(msg);
                    channel.BasicPublish("", evtName, null, body);
                }
            }
        }

        public void Subscribe<T, S>()
            where T : Event
            where S : IEventHandler<T>
        {
            var evtName = typeof(T).Name;
            var handlerEvtType = typeof(S);

            if(!this._eventTypes.Contains(typeof(T)))
            {
                this._eventTypes.Add(typeof(T));
            }
            if(!this._handlers.ContainsKey(evtName))
            {
                this._handlers.Add(evtName, new List<Type>());
            }
            if(this._handlers[evtName].Any(x => x.GetType() == handlerEvtType))
            {
                throw new ArgumentException($"handlerEvtType {handlerEvtType.Name} ya había sido registrado con anterioridad por {evtName}");
            }
            _handlers[evtName].Add(handlerEvtType);

            var factory = new ConnectionFactory() { HostName = "rabbit-web", DispatchConsumersAsync = true };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(evtName, false, false, false, null);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.Received += Consumer_delegate;

            channel.BasicConsume(evtName, true, consumer);
        }

        private async Task Consumer_delegate(object sender, BasicDeliverEventArgs evt)
        {
            var evtName = evt.RoutingKey;
            var msg = Encoding.UTF8.GetString(evt.Body.ToArray());
            try
            {
                if(this._handlers.ContainsKey(evtName))
                {
                    using (var scope = _srvScopeFactory.CreateScope())
                    {
                        var subscriptions = this._handlers[evtName];
                        foreach (var sb in subscriptions)
                        {
                            var handler = scope.ServiceProvider.GetService(sb); //Activator.CreateInstance(sb);
                            if (handler == null) continue;

                            var evtType = this._eventTypes.SingleOrDefault(x => x.Name == evtName);
                            var evtDS = JsonConvert.DeserializeObject(msg, evtType);
                            var objEvtType = typeof(IEventHandler<>).MakeGenericType(evtType);
                            await (Task)objEvtType.GetMethod("Handle").Invoke(handler, new object[] { evtDS });
                        }

                    }
                }
            }
            catch
            {

            }
        }
    }
}
