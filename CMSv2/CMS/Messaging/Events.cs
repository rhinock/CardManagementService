using System;
using EasyNetQ;
using Domain.Objects;
using Domain.Interfaces;

namespace Messaging
{
    public class Events : IEvents
    {
        private readonly ResourceConnection _connection;
        private readonly string _id;

        public Events(ResourceConnection connection)
        {
            _connection = connection;
            _id = Guid.NewGuid().ToString();
        }

        public void Add(Event item)
        {
            using (IBus bus = RabbitHutch.CreateBus(_connection.Value))
            {
                bus.PubSub.Publish(item);
            }
        }

        public void Handle(Action<Event> handling)
        {
            IBus bus = RabbitHutch.CreateBus(_connection.Value);
            bus.PubSub.Subscribe(_id, handling);
        }
    }
}