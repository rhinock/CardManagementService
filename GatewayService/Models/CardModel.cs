using System;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

using GatewayService.Types;
using GatewayService.Attributes;

namespace GatewayService.Models
{
    public class CardModel : Model
    {
        public Guid Id { get; set; }

        [StringLength(3, MinimumLength = 3)]
        public string Cvc { get; set; }

        [Required]
        [PanValidation(ErrorMessage = "Card Number is invalid")]
        [StringLength(16, MinimumLength = 16)]
        public string Pan { get; set; }

        [Required]
        [ExpireValidation(ErrorMessage = "Month or year is less than current month or year")]
        public Expire Expire { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsDefault { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append($"Id: {Id}, ");
            stringBuilder.Append($"Cvc: {Regex.Replace(Cvc ?? "", @"[\d]", "*")}, ");
            if (Pan != null)
            {
                stringBuilder.Append($"Pan: {Regex.Replace(Pan.Substring(0, Pan.Length - 4), @"\d", "*")}{Pan.Substring(Pan.Length - 4, 4)}, ");
            }
            stringBuilder.Append($"Expire: {Expire?.Month}/{Expire?.Year}, ");
            stringBuilder.Append($"IsDefault: {IsDefault}, ");
            stringBuilder.Append($"UserId: {UserId}");

            return $"{{ {stringBuilder} }}";
        }
    }
}
