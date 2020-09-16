using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Models
{
    public class TestingProtocol
    {
        [Key]
        public int ID { get; set; }
        public int TestID { get; set; }
        public int PatientGroupID { get; set; }
        public int ForecastinfoID { get; set; }
        public decimal PercentagePanel { get; set; }
        public int Baseline { get; set; }

       

        public int TotalTestPerYear { get; set; }
        public int? UserId { get; set; }
    }
    public class PercentageVal
    {
        [Key]
        public int ID { get; set; }
        public int TestN { get; set; }
        public int PGrpID { get; set; }
        public decimal PerNew { get; set; }
        public decimal PerOld { get; set; }
        public decimal TotalTestPerYear { get; set; }
        public int? UserId { get; set; }
        public int? sitecategoryid { get; set; }
    }
    public class Temptbl1
    {
        [Key]
        public int ID { get; set; }
        public int Tst { get; set; }
        public int PGrp { get; set; }
        public int Num { get; set; }
        public int Valu { get; set; }
        public int? UserId { get; set; }
        public int? sitecategoryid { get; set; }
    }


    public class TestByMonth
    {
        [Key]
        public int ID { get; set; }
        public int ForeCastID { get; set; }

        public int TestID { get; set; }
        public string Month { get; set; }
        public decimal TstNo { get; set; }
        public int PGrp { get; set; }
        public int SNo { get; set; }
        public decimal NewPatient { get; set; }

        public int TotalTestPerYear { get; set; }

        public decimal ExistingPatient { get; set; }
        public decimal Duration { get; set; }
        public int? UserId { get; set; }
        public int? sitecategoryid { get; set; }

    }
}
