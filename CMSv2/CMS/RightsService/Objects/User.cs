using Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RightsService.Objects
{
    public class User : IDataObject
    {
        [Key]
        public string Name { get; set; }

        public string Password { get; set; }

        public string AccessToken { get; set; }

        public string IdentityName => nameof(Name);
        public string SourceName => nameof(User);
    }
}
