using CMS.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Entities
{
    public class Operation : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid CardId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        
        [ForeignKey("CardId")]
        public Card Card { get; set; }
    }
}
