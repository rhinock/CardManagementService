using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.Entities
{
    public class Card
    {
        [Required]
        [StringLength(3)]
        public string Cvc { get; set; }

        [Required]
        [Key]
        [RegularExpression(@"^\d{4}\s*\d{4}\s*\d{4}\s*\d{4}$", ErrorMessage = "Card Number is invalid")]
        public string Pan { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/YY")]
        public DateTime Expire { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsDefault { get; set; }

        [Required]
        public Guid UserId { get; set; }
    }
}
