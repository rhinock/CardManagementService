using System;

namespace GatewayService.Models
{
    public class OperationModel : OperationModelDefault
    {
        public Guid CardId { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()} CardId: {CardId}";
        }
    }
}
