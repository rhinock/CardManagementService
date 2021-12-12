using GatewayService.Attributes;
using ObjectTools.Attributes;
using System;

namespace GatewayService.Models
{
    public class OperationCreateModel : Model
    {
        public Guid? CardId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        
        [IgnoreConvert, ValidateMember]
        public OperationCardModel Card { get; set; }
    }
}
