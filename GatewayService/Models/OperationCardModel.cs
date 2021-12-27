using GatewayService.Types;
using GatewayService.Attributes;

using System.ComponentModel.DataAnnotations;

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

        // TODO
        // "CardHolder" :"Ivan Ivanov" - optional,
    }
}
