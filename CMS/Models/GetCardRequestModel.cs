using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.Models
{
    public class GetCardRequestModel
    {
        public Guid? Id { get; set; }

        public Guid? UserId { get; set; }
    }
}
