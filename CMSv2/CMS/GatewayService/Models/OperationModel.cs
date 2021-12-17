using System;

namespace GatewayService.Models
{
    public class OperationModel : OperationDefaultModel
    {
        public Guid CardId { get; set; }

        public override string ToString()
        {
            return $"{base.ToString().TrimEnd('}').TrimEnd(' ')}, CardId: {CardId} }}";
        }
    }
}
