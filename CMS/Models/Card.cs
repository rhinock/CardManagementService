using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.Models
{
    public class Card
    {
        public string Cvc { get; set; }

        [Key]
        public string Pan { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/YY")]
        public DateTime Expire { get; set; }

        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public Guid UserId { get; set; }
    }
}
