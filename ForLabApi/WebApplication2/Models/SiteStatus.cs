using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Models
{
    public class SiteStatus
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public DateTime? OpenedFrom { get; set; }
        public DateTime? ClosedOn { get; set; }
        public int SiteID { get; set; }
        public int? UserId { get; set; }
        public bool Isapprove { get; set; }
    }
}
