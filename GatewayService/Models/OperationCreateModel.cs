using GatewayService.Attributes;
using ObjectTools.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace GatewayService.Models
{
    public class OperationCreateModel : Model
    {
        public Guid? CardId { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal Amount { get; set; }

        [IgnoreConvert, ValidateMember]
        public OperationCardModel Card { get; set; }

        public override string ToString()
        {
            return $"{{ CardId: {CardId}, Name: {Name}, Amount: {Amount}, Card: {Card?.ToString()} }}";
        }
    }
}
