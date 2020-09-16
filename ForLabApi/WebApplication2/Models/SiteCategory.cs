using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ForLabApi.Models
{
    public class SiteCategory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int? UserId { get; set; }
        public bool Isapprove { get; set; }
        public bool Isreject { get; set; }
    }


    public class Group
    {
        public Group( string name, string parent)
        {
           
            Name = name;
            Parent = parent;
        }
        public string Name { get; set; }

        public string Parent { get; set; }
    }
}
