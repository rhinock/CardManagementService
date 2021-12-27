using System;

namespace OperationDataService.Models
{
    public class CardModel
    {
        public Guid Id { get; set; }
        public string Cvc { get; set; }

        public string Pan { get; set; }

        public string ExpirationDate { get; set; }
    }
}
