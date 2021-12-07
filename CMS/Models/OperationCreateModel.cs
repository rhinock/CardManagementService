using System;

namespace CMS.Models
{
    public class OperationCreateModel
    {
        public Guid? CardId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public CardShortModel Card { get; set; }
    }
}
