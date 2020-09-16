using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Models
{
    public class Dashboard
    {
        public string name { get; set; }
        public int[] data { get; set; }
    }
    public class Dashboardchartdata
    {
        public string name { get; set; }
        public int y { get; set; }
    }
    public class forecastsitedata
    {

        public int id { get; set; }
        public string name { get; set; }
    }
   
}
