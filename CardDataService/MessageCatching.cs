using System;
using Domain.Enums;
using Domain.Objects;

using Infrastructure;

using Data.CardDataService.Objects;

namespace CardDataService
{
    public class MessageCatching
    {
        private readonly ResourceConnection _connection;

        public MessageCatching(ResourceConnection connection)
        {
            _connection = connection;
        }

        public async void Run(Event @event)
        {
            if (@event.EventType == EventType.MessageAboutCreating 
                && @event.SourceName.ToLower() == "card")
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
