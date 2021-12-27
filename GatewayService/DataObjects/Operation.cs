using System;
using Domain.Interfaces;

namespace GatewayService.DataObjects
{
    public class Operation : IDataObject
    {
        public Guid Id { get; set; }

        public Guid? CardId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }

        public Card Card { get; set; }
        
        public string IdentityName => nameof(Id);

        public string SourceName => nameof(Operation);
    }
}