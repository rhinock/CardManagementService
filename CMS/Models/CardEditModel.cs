using CMS.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace CMS.Models
{
    public class CardEditModel : ILoggable
    {
        [Required]
        public string Name { get; set; }

        public string GetData()
        {
            return $"Name: {Name}";
        }
    }
}
