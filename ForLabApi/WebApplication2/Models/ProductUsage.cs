using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Models
{
    public class ProductUsage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public decimal Rate { get; set; }
        public int ProductId { get; set; }
        public int TestId { get; set; }
        public int InstrumentId { get; set; }
        public string ProductUsedIn { get; set; }
        public bool IsForControl { get; set; }
        public int? UserId { get; set; }
        public bool Isapprove { get; set; }
        public bool Isreject { get; set; }
    }
    public class MasterConsumable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int MasterCID { get; set; }
        public int TestingAreaId { get; set; }
        public int TestId { get; set; }
        public int? UserId { get; set; }
        public bool Isapprove { get; set; }
        public bool Isreject { get; set; }
        //  public IList<ConsumableUsage> _consumablesUsages { get; set; }


    }
    public class Masterconsumablelist
    {

        public int MasterCID { get; set; }
        public int TestingAreaId { get; set; }
        public int TestId { get; set; }
        public IList<ConsumableUsage> _consumablesUsages { get; set; }
        public int? UserId { get; set; }


    }
}
