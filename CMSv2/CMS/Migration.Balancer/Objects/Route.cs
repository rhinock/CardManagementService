
using Newtonsoft.Json;
using Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Balancer.Objects
{
    public class Route : IDataObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Number { get; set; }

        public string ObjectName { get; set; }

        public string ResourceConnection { get; set; }

        public string Test { get; set; }

        [NotMapped]
        [JsonIgnore]
        public string IdentityName => nameof(Number);

        [NotMapped]
        [JsonIgnore]
        public string SourceName => nameof(Route);
    }
}