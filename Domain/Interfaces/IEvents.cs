using Domain.Objects;
using System;

namespace Domain.Interfaces
{
    public interface IEvents
    {
        void Add(Event item);

        void Handle(Action<Event> handling);
    }
}
