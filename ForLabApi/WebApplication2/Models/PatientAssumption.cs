using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Models
{
    public class PatientAssumption
    {
        [Key]
        public int ID { get; set; }
        public int ForecastinfoID { get; set; }
        public int? CategoryID { get; set; }
        public int? SiteID { get; set; }

        public int? Userid { get; set; }
    }
    public class MMGeneralAssumptionValue
    {
        [Key]
        public int ID { get; set; }
        public int Parameterid { get; set; }
        public decimal Parametervalue { get; set; }
        public int Forecastid { get; set; }
        public int? CategoryID { get; set; }
        public int? SiteID { get; set; }

        public int? ProductTypeID { get; set; }
        public int? TestID { get; set; }
        public int? PatientGroupID { get; set; }
        public int? Userid { get; set; }
    }
    public class Dynamiccontrol
    {
        public string name { get; set; }
        public int ID { get; set; }
        public string type { get; set; }


        public int datatype { get; set; }
    }
    public class TestingAssumption            ///use for product assumption
    {
        [Key]
        public int ID { get; set; }
        public int ForecastinfoID { get; set; }
        public int ProductTypeID { get; set; }
        public int? UserId { get; set; }
    }
    public class PatientNumberDetail
    {
        [Key]
        public int ID { get; set; }
        public int HeaderID { get; set; }
        public decimal Serial { get; set; }
        public string Columnname { get; set; }
        public long ForeCastId { get; set; }
        public int? SiteCategoryId { get; set; }
        public int? UserId { get; set; }
    }
    public class PatientNumberHeader
    {
        [Key]
        public int ID { get; set; }
        public int ForecastinfoID { get; set; }

        public int SiteID { get; set; }
        public int CategoryID { get; set; }

        public long CurrentPatient { get; set; }
        public long TargetPatient { get; set; }
        public int? UserId { get; set; }
    }
    public class patientnumberlist
        {
        public int ID { get; set; }
        public int ForecastinfoID { get; set; }    

        public long CurrentPatient { get; set; }
        public long TargetPatient { get; set; }
        public IList<PatientNumberDetail> patientdetaillist { get; set; }
        public int? UserId { get; set; }
    }
}
