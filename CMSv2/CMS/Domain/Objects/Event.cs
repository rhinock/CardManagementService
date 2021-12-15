using Domain.Enums;
using System.Collections.Generic;

namespace Domain.Objects
{
    public class Event
    {
        public Dictionary<string, object> Arg { get; set; }

        public EventType EventType { get; set; }
    }
}
