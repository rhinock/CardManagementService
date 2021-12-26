//using Domain.Interfaces;

//using Newtonsoft.Json;

//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace BalancerService.Objects
//{
//    public class Route : IDataObject
//    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int Number { get; set; }

//        public string ObjectName { get; set; }

//        public string ResourceConnection { get; set; }

//        [NotMapped]
//        [JsonIgnore]
//        public string IdentityName => nameof(Number);

//        [NotMapped]
//        [JsonIgnore]
//        public string SourceName => nameof(Route);
//    }
//}
