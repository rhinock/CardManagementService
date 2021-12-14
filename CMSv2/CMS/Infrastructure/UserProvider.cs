using DataServices;
using Domain.Interfaces;
using Domain.Objects;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class UserProvider
    {
        public static async Task<IUser> GetUser(ResourceConnection connection, string accessKey)
        {
            IUser user = new UserCredential(connection);
            await user.Load(accessKey);

            return user;
        }

        public static async Task<IUser> User(this ResourceConnection connection, string accessKey)
        {
            return await GetUser(connection, accessKey);
        }
    }
}
