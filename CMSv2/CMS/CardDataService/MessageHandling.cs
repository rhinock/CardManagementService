using CardDataService.Objects;
using Domain.Enums;
using Domain.Objects;
using Infrastructure;
using ObjectTools;
using System;
using System.Collections.Generic;

namespace CardDataService
{
    public class MessageHandling
    {
        private readonly ResourceConnection _connection;

        public MessageHandling(ResourceConnection connection)
        {
            _connection = connection;
        }

        public async void Run(Event @event)
        {
            if(@event.EventType == EventType.MessageAboutCreating)
            {
                await _connection.Repository().Create(new Card 
                {
                    Id = Guid.Parse(@event.Arg["Id"]?.ToString()),
                    Cvc = @event.Arg["Cvc"]?.ToString(),
                    Pan = @event.Arg["Pan"]?.ToString(),
                    ExpirationDate = @event.Arg["ExpirationDate"]?.ToString()
                });
            }
        }
    }
}
