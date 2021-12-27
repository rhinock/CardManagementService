using System;

namespace OperationDataService.Models
{
    public class OperationModel
    {
        public Guid Id { get; set; }

        public Guid? CardId { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public CardModel Card { get; set; }
    }
}
