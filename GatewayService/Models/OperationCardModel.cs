using GatewayService.Types;
using GatewayService.Attributes;

using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Text;

namespace GatewayService.Models
{
    public class OperationCardModel : Model
    {
        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Cvc { get; set; }

        [Required]
        [PanValidation(ErrorMessage = "Card Number is invalid")]
        [StringLength(16, MinimumLength = 16)]
        public string Pan { get; set; }

        [Required]
        [ExpireValidation(ErrorMessage = "Month or year is less than current month or year")]
        public Expire Expire { get; set; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append($"Cvc: {Regex.Replace(Cvc ?? "", @"[\d]", "*")}, ");
            if (Pan != null)
            {
                stringBuilder.Append($"Pan: {Regex.Replace(Pan.Substring(0, Pan.Length - 4), @"\d", "*")}{Pan.Substring(Pan.Length - 4, 4)}, ");
            }
            stringBuilder.Append($"Expire: {Expire?.Month}/{Expire?.Year}, ");

            return $"{{ {stringBuilder} }}";
        }
    }
}
