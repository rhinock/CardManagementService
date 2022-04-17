using System;

namespace Domain.Interfaces
{
    public interface IDataSchema : IDisposable
    {
        void Actualize(object preData);
    }
}
