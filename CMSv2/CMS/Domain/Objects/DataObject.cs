using Domain.Interfaces;

namespace Domain.Objects
{
    public abstract class DataObject : IDataObject
    {
        public abstract string SourceName { get; }
        public abstract string IdentityName { get; }
    }
}