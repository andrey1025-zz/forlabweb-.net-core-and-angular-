using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Models
{
    public class ConsumableUsage
    {
   
         [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            [Key]
            public int Id { get; set; }
            public int ConsumableId { get; set; }
            public bool PerTest { get; set; }
            public bool PerPeriod { get; set; }
            public bool PerInstrument { get; set; }
            public int NoOfTest { get; set; }
            public string Period { get; set; }
            public int ProductId { get; set; }
            public int?  InstrumentId { get; set; }
            public decimal UsageRate { get; set; }


        public int? UserId { get; set; }


        public bool Isapprove { get; set; }


    }
}
