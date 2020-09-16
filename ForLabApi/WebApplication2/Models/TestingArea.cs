using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Models
{
    public class TestingArea
    {

      
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int TestingAreaID { get; set; }
        public string AreaName { get; set; }
        public Boolean UseInDemography { get; set; }
        public string Category { get; set; }
        public int? UserId { get; set; }
        public bool Isapprove { get; set; }
        public bool Isreject { get; set; }
    }
  
}
