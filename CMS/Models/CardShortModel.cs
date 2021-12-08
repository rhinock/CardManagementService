using CMS.Attributes;
using CMS.Types;
using System.ComponentModel.DataAnnotations;

namespace CMS.Models
{
    public class CardShortModel : Model
    {
        [StringLength(3)]
        public string Cvc { get; set; }

        [PanValidation(ErrorMessage = "Card Number is invalid")]
        [StringLength(16)]
        public string Pan { get; set; }

        [Required]
        [ExpireValidation(ErrorMessage = "Month or year is less than current month or year")]
        public Expire Expire { get; set; }

        // TODO
        // "CardHolder" :"Ivan Ivanov" - optional,
    }
}
