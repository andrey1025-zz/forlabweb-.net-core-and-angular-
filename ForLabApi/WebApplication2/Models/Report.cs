using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Models
{
    public class Report
    {
    }
    public class columnname
    {
        public string data { get; set; }

    }
    public class Reportobject
    {
        public List<receivereportdata> receivereportdata { get; set; }
    }
    public class ReportedData
    {
        public string _categoryName { get; set; }
        public ForecastCategory _category { get; set; }
        public string _regionName { get; set; }
        public Region _region { get; set; }
        public string _siteName { get; set; }
        public Site _site { get; set; }
        public string _productName { get; set; }
        public MasterProduct _product { get; set; }
        public string _testname { get; set; }
        public Test _test { get; set; }
        public string _duration { get; set; }
        public decimal _amount { get; set; }
        public int _stockout { get; set; }
        public int _instrumentDownTime { get; set; }
        public int _rowno { get; set; }
        public bool _hasError { get; set; }
        public DateTime? _openingDate { get; set; }
        public DateTime? _closingDate { get; set; }
        public string errorDescription { get; set; }
        public ReportedData(int rowno, string rname, string sname, string pname, string duration, decimal amount, int stockout, int instrumentDownTime)
        {
            _rowno = rowno;
            _regionName = rname;
            _siteName = sname;
            _productName = pname;
            _duration = duration;
            _amount = amount;
            _stockout = stockout;
            _instrumentDownTime = instrumentDownTime;
            _hasError = false;
        }

        public ReportedData(int rowno, string catname, string pname, string duration, decimal amount, int stockout, int instrumentDownTime)
        {
            _categoryName = catname;
            _rowno = rowno;
            _productName = pname;
            _duration = duration;
            _amount = amount;
            _stockout = stockout;
            _instrumentDownTime = instrumentDownTime;
            _hasError = false;
        }
        public ReportedData(string catname, int rowno, string testname, string duration, decimal amount, int stockout, int instrumentDownTime)
        {
            _categoryName = catname;
            _rowno = rowno;
            _testname = testname;
            _duration = duration;
            _amount = amount;
            _stockout = stockout;
            _instrumentDownTime = instrumentDownTime;
            _hasError = false;
        }
        public ReportedData(string rname, int rowno, string sanme, string testname, string duration, decimal amount, int stockout, int instrumentDownTime)
        {
            _regionName = rname;
            _siteName = sanme;
            _rowno = rowno;
            _testname = testname;
            _duration = duration;
            _amount = amount;
            _stockout = stockout;
            _instrumentDownTime = instrumentDownTime;
            _hasError = false;
        }

    }
    public class Dynamicarray1
    {
        public Array data { get; set; }
        public IList<columnname> column { get; set; }
        public Array header { get; set; }

    }
    public class Dynamicarray
    {
        public DataTable data { get; set; }
        public IList<columnname> column { get; set; }
        public Array header { get; set; }

        public string title { get; set; }
        public string forecastperiod { get; set; }
        public string Finalcost { get; set; }

    }
    public class Productsummary
    {
        public string producttype { get; set; }
        public int producttypeid { get; set; }
        public string productname { get; set; }
        public int productid { get; set; }
        public string duration { get; set; }
        public decimal quantity { get; set; }

        public int? sitecategoryid { get; set; }
        public decimal cost { get; set; }
        public decimal packsize { get; set; }
        public int sno { get; set; }


        public bool isforcontrol { get; set; }

        public bool isforconumbles { get; set; }

    }


    public class receivereportdata
    {

        public int No { get; set; }
        public string CategoryName { get; set; }

        public string RegionName { get; set; }


        public string SiteName { get; set; }

        public string ProName { get; set; }

        public string testName { get; set; }
        public string Duration { get; set; }


        public decimal Amount { get; set; }
        public int StockOut { get; set; }
        public int InstrumentDownTime { get; set; }
        public string Description { get; set; }

        public int SiteID { get; set; }
        public int ProID { get; set; }
        public int testID { get; set; }
        public int CatID { get; set; }
        public string Duration1 { get; set; }
        public bool haserror { get; set; }
        public int forecastid { get; set; }

    }
    public class filterregionlist
    {
        public string ShortName { get; set; }
        public string RegionName { get; set; }
        public int NoofSites { get; set; }
        public int? countryid{ get; set; }
    public string countryname { get; set; }
    }
}
