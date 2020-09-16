using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Models
{
    public class Conductforecast
    {
    }

    public class ConductforecastDasboard
    {
        public string name { get; set; }
        public decimal[] data { get; set; }
    }
    public class ConductDashboardchartdata
    {
        public string name { get; set; }
        public decimal y { get; set; }
    }
    public class Costclass
    {
        public string totalcost { get; set; }
        public string qccost { get; set; }
        public string cccost { get; set; }
    }
}
