using Domain.Objects;
using Domain.Interfaces;

using Infrastructure;

using BalancerService.Objects;

namespace BalancerService
{
    public class InitialData
    {
        private readonly IRepository _repository;
        private readonly Route[] _routes;

        public InitialData(ResourceConnection resourceConnection, Route[] routes)
        {
            _repository = RepositoryManager.GetRepository(resourceConnection);
            _routes = routes;
        }

        public void Init()
        {
            foreach (var route in _routes)
            {
                _repository.Create(route);
            }
        }
    }
}
