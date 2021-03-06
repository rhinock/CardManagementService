using System;

namespace GatewayService.Models
{
    public class OperationDefaultModel : Model
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }

        public override string ToString()
        {
            return $"{{ Id: {Id}, Name: {Name}, Amount: {Amount} }}";
        }
    }
}
