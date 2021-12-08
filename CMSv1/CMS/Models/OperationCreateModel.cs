using CMS.Attributes;
using System;

namespace CMS.Models
{
    public class OperationCreateModel : Model
    {
        public Guid? CardId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        
        [IgnoreConvert, ValidateMember]
        public CardShortModel Card { get; set; }
    }
}
