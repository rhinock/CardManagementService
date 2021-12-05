using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.RequestModels
{
    public class EditCardRequestModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
