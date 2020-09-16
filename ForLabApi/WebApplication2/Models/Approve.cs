using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Models
{
    public class Approve
    {
        public int id { get; set; }
        public string mastertype { get; set; }
        public bool Isapprove { get; set; }
        public bool Isreject { get; set; }
    }
    public class PendingApprovelist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string mastertype { get; set; }
        public string userName { get; set; }
        public string country { get; set; }
    }
}
