using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Domain.Interfaces;

namespace OperationDataService.Objects
{
    public class Operation : IDataObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid CardId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }

        [NotMapped]
        public string IdentityName => nameof(Id);
    }
}
