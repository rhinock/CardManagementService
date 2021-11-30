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
        [RegularExpression(@"^\d{4}\s*\d{4}\s*\d{4}\s*\d{4}$", ErrorMessage = "Card Number is invalid")]
        public string Pan { get; set; }

        /// <summary>
        /// Month and Year
        /// </summary>
        [Required]
        public Expire Expire { get; set; }

        public string Name { get; set; }

        public bool IsDefault { get; set; }

        public Guid UserId { get; set; }
    }
}
