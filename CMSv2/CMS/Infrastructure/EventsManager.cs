using Messaging;

using Domain.Objects;
using Domain.Interfaces;

namespace Infrastructure
{
    public static class EventsManager
    {
        public static IEvents GetEvents(ResourceConnection connection)
        {
            return new Events(connection);
        }

        public static IEvents Events(this ResourceConnection connection)
        {
            return GetEvents(connection);
        }
    }
}
