using Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BalancerService.Objects
{
    public class Route : IDataObject
    {
        [Key]
        public int Number { get; set; }

        public string ObjectName { get; set; }

        public string ResourceConnection { get; set; }

        public string IdentityName => nameof(Number);
    }
}
