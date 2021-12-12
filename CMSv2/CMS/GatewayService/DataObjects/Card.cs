using GatewayService.Types;
using Domain.Interfaces;
using System;

namespace GatewayService.DataObjects
{
    public class Card : IDataObject
    {
        public Guid Id { get; set; }

        public string Cvc { get; set; }

        /// <summary>
        /// Card Number
        /// </summary>
        public string Pan { get; set; }

        /// <summary>
        /// Month and Year
        /// </summary>
        public Expire Expire { get; set; }

        public string Name { get; set; }

        public bool IsDefault { get; set; }

        public Guid UserId { get; set; }

        public string ExpirationDate
        {
            get
            {
                return Expire.ToString();
            }
            set
            {
                Expire = value;
            }
        }

        public string IdentityName => nameof(Id);

        public string SourceName => nameof(Card);
    }
}
