using CMS.Repositories;
using CMS.Types;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Entities
{
    public class Card : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(3)]
        public string Cvc { get; set; }

        /// <summary>
        /// Card Number
        /// </summary>
        [StringLength(16)]
        public string Pan { get; set; }

        /// <summary>
        /// Month and Year
        /// </summary>
        [NotMapped]
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
    }
}
