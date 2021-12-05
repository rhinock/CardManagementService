using CMS.Attributes;
using CMS.Types;
using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.Models
{
    public class Card
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(3)]
        public string Cvc { get; set; }

        /// <summary>
        /// Card Number
        /// </summary>
        [PanValidation(ErrorMessage = "Card Number is invalid")]
        [StringLength(16)]
        public string Pan { get; set; }

        /// <summary>
        /// Month and Year
        /// </summary>
        [Required]
        [ExpireValidation(ErrorMessage = "Month or year is less than current month or year")]
        public Expire Expire { get; set; }

        public string Name { get; set; }

        public bool IsDefault { get; set; }

        public Guid UserId { get; set; }
    }
}
