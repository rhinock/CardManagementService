using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUser
    {
        public string Identity { get; }

        public bool IsAuth();

        public Task Load(string accessKey);
    }
}
