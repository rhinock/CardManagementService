using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Newtonsoft.Json;

using Domain.Interfaces;

namespace Data.CardDataService.Objects
{
    public class Card : IDataObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(3)]
        public string Cvc { get; set; }

        /// <summary>
        /// Card Number
        /// </summary>
        [StringLength(16)]
        public string Pan { get; set; }

        public string Name { get; set; }

        public bool IsDefault { get; set; }

        public Guid UserId { get; set; }

        public string ExpirationDate { get; set; }

        [NotMapped]
        [JsonIgnore]
        public string IdentityName => nameof(Id);

        [NotMapped]
        [JsonIgnore]
        public string SourceName => nameof(Card);
    }
}
