using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Models
{
    public class Region
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int RegionID { get; set; }
        public string RegionName { get; set; }
        public string ShortName { get; set; }
        public int? UserId { get; set; }

        public int? CountryId { get; set; }
        public bool Isapprove { get; set; }
        public bool Isreject { get; set; }
    }
 
}
