using System.ComponentModel.DataAnnotations;

namespace CMS.Models
{
    public class CardEditModel
    {
        [Required]
        public string Name { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}";
        }
    }
}
