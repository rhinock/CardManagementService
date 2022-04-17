using System.ComponentModel.DataAnnotations;

namespace GatewayService.Models
{
    public class CardEditModel : Model
    {
        [Required]
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{{Name: {Name}}}";
        }
    }
}
