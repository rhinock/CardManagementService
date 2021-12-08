using CMS.Attributes;
using CMS.Types;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace CMS.Models
{
    public class CardModel : Model
    {
        public Guid Id { get; set; }

        [StringLength(3)]
        public string Cvc { get; set; }

        [PanValidation(ErrorMessage = "Card Number is invalid")]
        [StringLength(16)]
        public string Pan { get; set; }

        [Required]
        [ExpireValidation(ErrorMessage = "Month or year is less than current month or year")]
        public Expire Expire { get; set; }

        public string Name { get; set; }

        public bool IsDefault { get; set; }

        public Guid UserId { get; set; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"Id: {Id}");
            stringBuilder.AppendLine($"Cvc: {Regex.Replace(Cvc, @"[\d]", "*")}");

            stringBuilder.AppendLine($"Pan: " +
                $"{Regex.Replace(Pan.Substring(0, Pan.Length - 4), @"\d", "*")}" +
                $"{Pan.Substring(Pan.Length - 4, 4)}");

            stringBuilder.AppendLine($"Expire: " +
                $"Month {Expire.Month}, " +
                $"Year {Expire.Year}");

            stringBuilder.AppendLine($"IsDefault: {IsDefault}");
            stringBuilder.AppendLine($"UserId: {UserId}");

            return stringBuilder.ToString();
        }
    }
}
