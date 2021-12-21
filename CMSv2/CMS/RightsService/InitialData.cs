using Domain.Interfaces;
using Domain.Objects;
using Infrastructure;
using RightsService.Objects;

namespace RightsService
{
    public class InitialData
    {
        private readonly IRepository _repository;
        private readonly User[] _users;

        public InitialData(ResourceConnection resourceConnection, User[] users)
        {
            _repository = RepositoryManager.GetRepository(resourceConnection);
            _users = users;
        }

        public void Init()
        {
            foreach (var user in _users)
                _repository.Create(user);
        }
    }
}
