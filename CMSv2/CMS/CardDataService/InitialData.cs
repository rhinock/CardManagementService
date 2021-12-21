using CardDataService.Objects;
using Domain.Interfaces;
using Domain.Objects;
using Infrastructure;

namespace CardDataService
{
    public class InitialData
    {
        private readonly IRepository _repository;
        private readonly Card[] _cards;

        public InitialData(ResourceConnection resourceConnection, Card[] cards)
        {
            _repository = RepositoryManager.GetRepository(resourceConnection);
            _cards = cards;
        }

        public void Init()
        {
            foreach (var card in _cards)
                _repository.Create(card);
        }
    }
}