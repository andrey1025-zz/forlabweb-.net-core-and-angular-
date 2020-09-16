using ForLabApi.DataInterface;
using ForLabApi.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Models
{
    public class ForecastInfo
    {
        [Key]
        public int ForecastID { get; set; }
        public string ForecastNo { get; set; }
        public string Methodology { get; set; }
        public string DataUsage { get; set; }
        public string Period { get; set; }
        public int MonthInPeriod { get; set; }
        public string Status { get; set; }
        public int Extension { get; set; }
        public DateTime? ForecastDate { get; set; }
        public string ScopeOfTheForecast { get; set; }
        public DateTime StartDate { get; set; }
        public string Note { get; set; }
        public int ActualCount { get; set; }
        public int ROrder { get; set; }
        public string Method { get; set; }
        public string ForecastType { get; set; }
        public int ProgramId { get; set; }
        public decimal Westage { get; set; }
        public decimal Scaleup { get; set; }
        public DateTime LastUpdated { get; set; }
        public int? UserId { get; set; }
        public int? Countryid { get; set; }

        public int? months { get; set; }
        public bool? Forecastlock { get; set; }
        public IList<ForecastSite> _forecastSites { get; set; }
        public virtual IList<ForecastSite> GetNoneReportedForecastSite(int repsiteid)
        {
            IList<ForecastSite> result = new List<ForecastSite>();
            foreach (ForecastSite s in _forecastSites)
            {
                if (s.ReportedSiteId == repsiteid)
                    result.Add(s);
            }

            return result;
        }

      

      
    }

    public class ForecastInfoList
    {
        [Key]
        public int ForecastID { get; set; }
        public string ForecastNo { get; set; }
        public string Methodology { get; set; }
        public string DataUsage { get; set; }
        public string Period { get; set; }
        public int MonthInPeriod { get; set; }
        public string Status { get; set; }
        public int Extension { get; set; }
        public string ForecastDate { get; set; }
        public string ScopeOfTheForecast { get; set; }
        public string StartDate { get; set; }
        public string Note { get; set; }

        public int ActualCount { get; set; }
        public int ROrder { get; set; }
        public string Method { get; set; }
        public string ForecastType { get; set; }
        public int ProgramId { get; set; }
        public decimal Westage { get; set; }
        public decimal Scaleup { get; set; }
        public string LastUpdated { get; set; }
        public int? Countryid { get; set; }
        public int? userid { get; set; }
        public bool? Forecastlock { get; set; }
    }


    public class ForecastSiteInfo
    {
        [Key]
        public int ID { get; set; }
        public int SiteID { get; set; }
        public int ForecastinfoID { get; set; }
        public long CurrentPatient { get; set; }
        public long TargetPatient { get; set; }

        public long PopulationNumber { get; set; }



        public long PrevalenceRate { get; set; }
        public int? UserId { get; set; }
    }
    public class ForecastSiteInfonew
    {
        [Key]
        public int ID { get; set; }
        public int SiteID { get; set; }
        public string SiteName { get; set; }
        public int ForecastinfoID { get; set; }
        public long CurrentPatient { get; set; }
        public long TargetPatient { get; set; }
        public long PopulationNumber { get; set; }
        public long PrevalenceRate { get; set; }
    }
    public class ForecastSiteInfoList
    {
        public IList<ForecastSiteInfo> patientnumberusage { get; set; }
    }
    public class ForecastCategoryInfo
    {
        [Key]
        public int ID { get; set; }
        public int SiteCategoryId { get; set; }

        public string SiteCategoryName { get; set; }
        public int ForecastinfoID { get; set; }
        public long CurrentPatient { get; set; }
        public long TargetPatient { get; set; }
        public int? UserId { get; set; }
        public long PopulationNumber { get; set; }
        public long PrevalenceRate { get; set; }
      public string methodtype { get; set; }
    }

    public class ForecastCategorySiteInfo
    {
        [Key]
        public int ID { get; set; }
        public long ForecastInfoID { get; set; }
        public int CategoryID { get; set; }
        public int SiteID { get; set; }
        public int? UserId { get; set; }

    }
    public class ForecastCategoryInfoList
    {
        public IList<ForecastCategoryInfo> patientnumberusage { get; set; }
        public IList<ForecastCategorySiteInfo> categorysite { get; set; }
    }


    public class PatientGroup
    {
        [Key]
        public int ID { get; set; }
        public int ForecastinfoID { get; set; }
        public string PatientGroupName { get; set; }
        public decimal PatientPercentage { get; set; }
        public decimal PatientRatio { get; set; }
        public int GroupID { get; set; }
        public int? UserId { get; set; }

    }


    public class ForecastCategorySite
    {
        [Key]
        public int ID { get; set; }
        public int CategoryID { get; set; }
        public int SiteID { get; set; }
        public int? UserId { get; set; }
    }
    public class ForecastedTestByTest
    {
        [Key]
        public int ID { get; set; }
        public int ForeCastID { get; set; }
        public int Tst { get; set; }
        public int PGrp { get; set; }
        public decimal TotalTst { get; set; }
        public int? UserId { get; set; }
        public int? sitecategoryid { get; set; }
    }

    public class ForecastCategory
    {
    
        public int ForecastId { get; set; }
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? UserId { get; set; }

    }
    public class adjustedvolume
    {
        public int Id { get; set; }

        public int protestid { get; set; }
        public string type { get; set; }
        public int value { get; set; }

    }

    public class ForecastSite
    {
        [Key]
        public int Id { get; set; }
        public int ForecastInfoId { get; set; }
        public int SiteId { get; set; }
        public int ReportedSiteId { get; set; }
        public int? UserId { get; set; }
        //public virtual IList<int> GetUniqFSProduct()
        //{
        //    IList<int> temp = new List<int>();

        //    foreach (ForecastSiteProduct p in SiteProducts)
        //    {
        //        bool pexist = false;
        //        for (int i = 0; i < temp.Count; i++)
        //        {
        //            if (p.ProductID == temp[i])
        //            {
        //                pexist = true;
        //                break;
        //            }
        //        }

        //        if (!pexist)
        //        {
        //            temp.Add(p.ProductID);
        //        }
        //    }

        //    return temp;
        //}
        //public virtual IList<ForecastSiteProduct> SiteProducts
        //{
        //    get
        //    {
        //        if (_siteProducts == null)
        //        {
        //            _siteProducts = new List<ForecastSiteProduct>();
        //        }
        //        return _siteProducts;
        //    }
        //    set { _siteProducts = value; }
        //}


    }

    public class ForecastNRSite
    {
        [Key]
        public int Id { get; set; }
        public int ForecastSiteId { get; set; }
        public int NReportedSiteId { get; set; }
        public int UserId { get; set; }
    }
    public class BaseDataUsage : IBaseDataUsage
    {
        public int Id { get; set; }
    //    public int ForecastSiteID { get; set; }
      


    
        public string CDuration { get; set; }
        public decimal AmountUsed { get; set; }
        public int StockOut { get; set; }
        public decimal Adjusted { get; set; }
        public DateTime? DurationDateTime { get; set; }
        public int InstrumentDowntime { get; set; }
        public int? UserId { get; set; }
    }
    public class ForecastSiteProduct: BaseDataUsage,IBaseDataUsage
    {
       
     
        public int ForecastSiteID { get; set; }

        public int ProductID { get; set; }

    }

    public class ForecastCategoryProduct : BaseDataUsage,IBaseDataUsage
    {
     
        public int CategoryID { get; set; }
        public int ProductID { get; set; }

    }


    public class ForecastSiteTest: BaseDataUsage, IBaseDataUsage
    {
        public int ForecastSiteID { get; set; }
        public int TestID { get; set; }
    }

    public class ForecastCategoryTest : BaseDataUsage, IBaseDataUsage
    {
        public int CategoryID { get; set; }
        public int TestID { get; set; }
    }
    public class Datausage
    {
        public int productid { get; set; }
        public string productname { get; set; }
        public DataTable value { get; set; }

        
    }

    public class Datausagewithcontrol
    {
        public List<Durationwithids> controls { get; set; }
        public List<Datausage> Datausage { get; set; }


}

    public class Durationwithids
    {
        public string Duration { get; set; }

        public int  id { get; set; }
    }
    public class siteproductno
    {
        public int totalsiteno { get; set; }
        public int totalproductno { get; set; }
    }



    public class ForecastedResult
    {
        public int Id { get; set; }
        public int ForecastId { get; set; }
        public int ProductId { get; set; }
        public int TestId { get; set; }
        public decimal ForecastValue { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime DurationDateTime { get; set; }
        public int? SiteId { get; set; }
        public int CategoryId { get; set; }

        public string Duration { get; set; }

        public bool IsHistory { get; set; }

        public decimal HistoricalValue { get; set; }


        public int PackQty { get; set; }

        public decimal PackPrice { get; set; }

        public int ProductTypeId { get; set; }

        public string ProductType { get; set; }


        public bool ServiceConverted { get; set; }


        public string TestingArea { get; set; }


        public bool IsForControl { get; set; }



        public decimal ControlTest { get; set; }
        public bool IsForGeneralConsumable { get; set; }
        public int? UserId { get; set; }

        public int? Sno { get; set; }
    }
    public class Siteinsvalidation
    {
        public string Testname { get; set; }
        public string Sitename { get; set; }
    }


    public class ForecastCategorySiteInfolist
    {
        public int CategoryID { get; set; }
        public long ForecastInfoID { get; set; }
        public int ID { get; set; }
        public int SiteID { get; set; }
        public string SiteName { get; set; }
}
    public class ForecastTest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]

        public int ID { get; set; }

        public int TestID { get; set; }

        public int forecastID { get; set; }
        public int UserId { get; set; }
       
    }
    public class ForecastProductUsage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }       
        public decimal Rate { get; set; }
        public int ProductId { get; set; }
        public int TestId { get; set; }
        public int InstrumentId { get; set; }
        public string ProductUsedIn { get; set; }
        public bool IsForControl { get; set; }
        public int? UserId { get; set; }
        public int Forecastid { get; set; }
    }

    public class ForecastProductUsagelist
    {
        [Key]
        public int Id { get; set; }
        public decimal Rate { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int TestId { get; set; }
        public int InstrumentId { get; set; }
        public string InstrumentName { get; set; }
        public string ProductUsedIn { get; set; }
        public bool IsForControl { get; set; }
        public int? UserId { get; set; }
        public int Forecastid { get; set; }
    }
    public class ForecastProductUsageDetail
    {
        public int ID { get; set; }
        public string name { get; set; }
        public List<ForecastProductUsagelist> value { get; set; }
    }

    public class ForecastConsumableUsage
    {

       
        [Key]
        public int Id { get; set; }
        public int TestId { get; set; }
        public int Forecastid { get; set; }
        public bool PerTest { get; set; }
        public bool PerPeriod { get; set; }
        public bool PerInstrument { get; set; }
        public int NoOfTest { get; set; }
        public string Period { get; set; }
        public int ProductId { get; set; }
        public int? InstrumentId { get; set; }
        public decimal UsageRate { get; set; }


        public int? UserId { get; set; }





    }

    public class ForecastConsumableUsagelist
    {
        public int Id { get; set; }
        public int TestId { get; set; }
       

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int ProductTypeId { get; set; }

        public string ProductTypeName { get; set; }
        public int InstrumentId { get; set; }
        public string InstrumentName { get; set; }

        public decimal UsageRate { get; set; }
        public bool PerTest { get; set; }
        public bool PerPeriod { get; set; }
        public bool PerInstrument { get; set; }

        public string test { get; set; }
        public int NoOfTest { get; set; }
        public string Period { get; set; }
    }
    public class ForecastConsumableUsageDetail
    {
        public int ID { get; set; }
        public string name { get; set; }

        public List<ForecastConsumableUsagelist> value { get; set; }
    }


    public class forecastusagesmodel
    {
        public List<ForecastProductUsage> ForecastProductUsage { get; set; }

        public List<ForecastConsumableUsage> ForecastConsumableUsage { get; set; }
    }
}
