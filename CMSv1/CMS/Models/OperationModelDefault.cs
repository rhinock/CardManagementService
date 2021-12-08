using System;

namespace CMS.Models
{
    public class OperationModelDefault : Model
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}; Name: {Name}; Amount: {Amount};";
        }
    }
}
