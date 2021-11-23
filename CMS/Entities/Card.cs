﻿using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.Entities
{
    public class Card
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(3)]
        public string Cvc { get; set; }

        [Required]
        [RegularExpression(@"^\d{4}\s*\d{4}\s*\d{4}\s*\d{4}$", ErrorMessage = "Card Number is invalid")]
        public string Pan { get; set; }

        [Required]
        public Expire Expire { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsDefault { get; set; }

        [Required]
        public Guid UserId { get; set; }
    }
}
