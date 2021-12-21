using Domain.Interfaces;
using Domain.Objects;
using Infrastructure;
using OperationDataService.Objects;

namespace OperationDataService
{
    public class InitialData
    {
        private readonly IRepository _repository;
        private readonly Operation[] _operations;

        public InitialData(ResourceConnection resourceConnection, Operation[] operations)
        {
            _repository = RepositoryManager.GetRepository(resourceConnection);
            _operations = operations;
        }

        public void Init()
        {
            foreach (var operation in _operations)
                _repository.Create(operation);
        }
    }
}
