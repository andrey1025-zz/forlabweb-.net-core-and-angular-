using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Models
{
    public class Instrument
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int InstrumentID { get; set; }
        public string InstrumentName { get; set; }
        public int MaxThroughPut { get; set; }
        public int MonthMaxTPut { get; set; }
        public TestingArea testingArea { get; set; }
        public int DailyCtrlTest { get; set; }
        public int MaxTestBeforeCtrlTest { get; set; }
        public int WeeklyCtrlTest { get; set; }
        public int MonthlyCtrlTest { get; set; }
        public int QuarterlyCtrlTest { get; set; }


        public string CtrlTestDuration { get; set; }
        public int CtrlTestNoOfRun { get; set; }

        public int? UserId { get; set; }
        public bool Isapprove { get; set; }
        public bool Isreject { get; set; }

    }
    public class InstrumentList
    {
        public int InstrumentID { get; set; }
        public string InstrumentName { get; set; }
        public int MaxThroughPut { get; set; }
        public int MonthMaxTPut { get; set; }
        public string testingArea { get; set; }
        public int testingAreaid { get; set; }
        public int DailyCtrlTest { get; set; }
        public int MaxTestBeforeCtrlTest { get; set; }
        public int WeeklyCtrlTest { get; set; }
        public int MonthlyCtrlTest { get; set; }
        public int QuarterlyCtrlTest { get; set; }


        public string CtrlTestDuration { get; set; }
        public int CtrlTestNoOfRun { get; set; }
        public int? UserId { get; set; }

    }

    public class forecastinslist
    {
        public int forecastID { get; set; }
        public int InsID { get; set; }
        public decimal Quantity { get; set; }
        public decimal TestRunPercentage { get; set; }
        public string AreaName { get; set; }
        public string InstrumentName { get; set; }
        public int TestingAreaID { get; set; }
    }
    public class getinstrument
    {
        public string Frequency { get; set; }
        public int InstrumentID { get; set; }
        public string InstrumentName { get; set; }
        public int MaxThroughPut { get; set; }
        public int TestingArea { get; set; }
        public int noofrun { get; set; }
        public int? UserId { get; set; }

    }



    public class ForecastIns {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]

        public int ID { get; set; }

        public int InsID { get; set; }

        public int forecastID { get; set; }
        public int UserId { get; set; }
        public int Quantity { get; set; }
        public decimal TestRunPercentage { get; set; }
    }
    public class ForecastInsmodel
    {
        public List<ForecastIns> ForecastIns { get; set; }

    }
        public class ForecastInstrumentlist
    {

        public int testareaid { get; set; }
        public string testareaname { get; set; }

        public Array instruments { get; set; }
    }
    public class InstrumentList1
    {

        public int InstrumentID { get; set; }
        public string InstrumentName { get; set; }
        public int TestingAreaID { get; set; }
        public string type { get; set; }
    }

}
