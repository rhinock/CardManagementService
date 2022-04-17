using Domain.Objects;

using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ILogger
    {
        LoggerOptions Options { get; }
        Task Info(string message);
        Task Error(string message);
    }
}
