using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Models
{
    public class SiteInstrument
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }
        public int InstrumentID { get; set; }
        public int SiteID { get; set; }
        public int Quantity { get; set; }
        public decimal TestRunPercentage { get; set; }
        public int? UserId { get; set; }
        public bool Isapprove { get; set; }
        public bool Isreject { get; set; }
    }

    public class SiteInstrumentList
  {
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    [Key]
        public int ID { get; set; }
        public int InstrumentID { get; set; }

        public string InstrumentName { get; set; }
        public string Region { get; set; }
        public string Site { get; set; }
        public int testingareaId { get; set; }
        public string testingareaName { get; set; }

        public int SiteID { get; set; }
        public int Quantity { get; set; }
        public decimal TestRunPercentage { get; set; }
    }
}
