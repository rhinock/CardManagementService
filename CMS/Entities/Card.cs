using CMS.Types;
using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.Entities
{
    public class Card
    {
        [Key]
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
    }
}
