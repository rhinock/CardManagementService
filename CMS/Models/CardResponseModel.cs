using System.Collections.Generic;

namespace CMS.Models
{
    public class CardResponseModel : ResponseModel
    {
        public IEnumerable<Card> Items { get; set; }
    }
}
