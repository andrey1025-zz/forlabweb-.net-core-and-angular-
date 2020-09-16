using ForLabApi.DataInterface;
using ForLabApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Repositories
{
    public class ReportAccessRepositories : IReport<columnname, Dynamicarray>
    {
        ForLabContext ctx;
        List<string> Lstmonthname = new List<string>();
        string varids = "";
        DataTable dtGetValue = new DataTable();
        DataTable dtlineargrowth = new DataTable();
        DataTable dtValue = new DataTable();
        public ReportAccessRepositories(ForLabContext c)
        {
            ctx = c;
            //return ctx;
        }
        public Array GetProductUsagelist(string param)
        {
            string[] p1;
            p1 = param.Split(",");

            Array A;

           var res = ctx.ProductUsage.Join(ctx.Test, b => b.TestId, c => c.TestID, (b, c) => new { b, c })
                .Join(ctx.Instrument, e => e.b.InstrumentId, f => f.InstrumentID, (e, f) => new { e, f })
                .Join(ctx.MasterProduct, g => g.e.b.ProductId, h => h.ProductID, (g, h) => new { g, h })
                .Join(ctx.TestingArea, j => j.g.e.c.TestingAreaID, k => k.TestingAreaID, (j, k) => new { j, k })
                           .Select(x => new
                {
                    AreaName = x.k.AreaName,
                    TestName = x.j.g.e.c.TestName,
                    InstrumentName = x.j.g.f.InstrumentName,
                    ProductName = x.j.h.ProductName,
                    Usagerate = x.j.g.e.b.Rate,
                    areaid=x.k.TestingAreaID,
                    typeid=x.j.h.ProductTypeId

                }).ToList();

            if(p1[0]=="0" && p1[1]=="0" )
            {
                A = res.ToArray();
            }
            else
            {
                A = res.Where(i => i.areaid == Convert.ToInt32(p1[0]) && i.typeid == Convert.ToInt32(p1[1])).ToArray();

            }


            return A;
        }

        public Array Getproductpricelist(int typeid)
        {
            MasterProduct p = new MasterProduct();
            var products = (from prd in ctx.MasterProduct                                                    
                            select new MasterProduct
                            {
                                ProductID = prd.ProductID,
                                ProductName = prd.ProductName,
                                ProductTypeId = prd.ProductTypeId,
                                SerialNo = prd.SerialNo,
                                BasicUnit = prd.BasicUnit,
                                MinimumPackPerSite = prd.MinimumPackPerSite,
                                _productPrices = ctx.ProductPrice.Where(b => b.ProductId == prd.ProductID).ToList()
                            }).ToList();

            if (typeid==0)
            {

            }
            else
            {
                products = products.Where(prd => prd.ProductTypeId == typeid).ToList();
            }

            var results = (from prd in products
                               //join pp in ctx.ProductPrice on prd.ProductID equals pp.Product.ProductID                       
                           select new
                           {
                               ProductID = prd.ProductID,
                               Product = prd.ProductName,
                               ProductType = ctx.ProductType.Where(b => b.TypeID == prd.ProductTypeId).Select(x => x.TypeName).FirstOrDefault(),
                               catalog = prd.SerialNo,
                               BasicUnit = prd.BasicUnit,
                               minpacksize = prd.MinimumPackPerSite,
                               packcost = prd.GetActiveProductPrice(DateTime.Now).packcost,
                               packsize = prd.GetActiveProductPrice(DateTime.Now).packsize,
                               fromdate = prd.GetActiveProductPrice(DateTime.Now).FromDate.Value.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture)
                           }).ToArray();




            return results;
        }
        public Array Getinstrumentlist(int araeid)
        {

            Array A;
            var res = ctx.Instrument.Join(ctx.TestingArea, e => e.testingArea.TestingAreaID, f => f.TestingAreaID, (e, f) => new { e, f })
                .Select(x => new
                {
                    TestingArea = x.f.AreaName,
                    Instrument = x.e.InstrumentName,
                    maxthroughput = x.e.MaxThroughPut,
                    monthlythroughput = x.e.MonthMaxTPut,
                    aftertestno = x.e.MaxTestBeforeCtrlTest,
                    daily = x.e.DailyCtrlTest,
                    weekly = x.e.WeeklyCtrlTest,
                    monthly = x.e.MonthlyCtrlTest,
                    Quarterly = x.e.QuarterlyCtrlTest,
                    TestingAreaId=x.f.TestingAreaID
                }).ToList();
            if (araeid==0)
            {
                A = res.ToArray();
            }
            else
            {
                A = res.Where(b => b.TestingAreaId == araeid).ToArray();
            }
            return A;
        }
        public Array Getproductlist(int proid)
        {
            Array A;
          var  res = ctx.MasterProduct.Join(ctx.ProductType, e => e.ProductTypeId, f => f.TypeID, (e, f) => new { e, f })
              .Select(x => new
                {
                    ProductType = x.f.TypeName,
                    Typeid=x.f.TypeID,
                    Product = x.e.ProductName,
                    Catalog = x.e.SerialNo,
                    BasicUnit = x.e.BasicUnit,
                    minimumpacksize = x.e.MinimumPackPerSite

                }).ToList();


            if (proid==0)
            {
                A = res.ToArray();
            }
            else
            {
                A = res.Where(b => b.Typeid == proid).ToArray();
            }
            return A;
        }
        public IList<columnname> getcolumnname()
        {
            List<columnname> Db = new List<columnname>();

            DataTable DT = new DataTable();
            int index;
            DT.Columns.Add("region");
            DT.Columns.Add("category");
            DT.Columns.Add("site");
            DT.Columns.Add("generalWorking Days");
            var getworkingdays = ctx.SiteTestingdays.Join(ctx.TestingArea, b => b.testingareaid, c => c.TestingAreaID, (b, c) => new { b, c })
             .GroupBy(g => g.b.testingareaid)
              .Select(x => new
              {

                  TestingArea = x.Max(s => s.c.AreaName),

              }).ToList();
            for (int j = 0; j < getworkingdays.Count; j++)
            {

                DT.Columns.Add(getworkingdays[j].TestingArea.ToLower() + " Test");
            }
            for (int i = 0; i < DT.Columns.Count; i++)
            {
                Db.Add(new columnname
                {
                    data = DT.Columns[i].Caption
                });
            }
            return Db;
        }
        public Dynamicarray Getsitelist(int regionid, int categoryid)
        {
            Array A;
          
            var DyObjectsList = new List<dynamic>();
            string category = "";
            string Region = "";
            DataTable DT = new DataTable();
            int index;
            DT.Columns.Add("Region");
            DT.Columns.Add("Category");
            DT.Columns.Add("Site");
            DT.Columns.Add("GeneralWorking Days");
            var getworkingdays1 = ctx.SiteTestingdays.Join(ctx.TestingArea, b => b.testingareaid, c => c.TestingAreaID, (b, c) => new { b, c })
                .GroupBy(g => g.b.testingareaid)
                 .Select(x => new
                 {

                     TestingArea = x.Max(s => s.c.AreaName),

                 }).ToList();
            for (int i = 0; i < getworkingdays1.Count; i++)
            {
                DT.Columns.Add(getworkingdays1[i].TestingArea.ToLower() + " Test");
            }
            
        var res = ctx.Site.Join(ctx.Region, b => b.regionid, c => c.RegionID, (b, c) => new { b, c })
            .Join(ctx.SiteCategory, e => e.b.CategoryID, f => f.CategoryID, (e, f) => new { e, f })           
            .Select(h => new 
            {
                SiteID = h.e.b.SiteID,
                SiteName = h.e.b.SiteName,
                Workingdays = h.e.b.WorkingDays,
                Regionname = h.e.c.RegionName,
                Categoryname = h.f.CategoryName,
                Categoryid=h.f.CategoryID,
                regionid=h.e.c.RegionID
            }).ToList();

            if (categoryid ==0 && regionid==0)
            {

            }
            else
            {
               res= res.Where(tc => tc.Categoryid == categoryid || tc.regionid == regionid).ToList();
            }

              
        
            //  Array A = new Array[res.Count];
            for (int i = 0; i < res.Count; i++)
            {
                DataRow Dr = DT.NewRow();

                Dr["Region"] = res[i].Regionname;    //==Region?"":res[i].Regionname;
                Dr["Category"] = res[i].Categoryname;     //== category?"":res[i].Categoryname;
                Dr["Site"] = res[i].SiteName;
                Dr["GeneralWorking Days"] = res[i].Workingdays;
                category = res[i].Categoryname;
                Region = res[i].Regionname;
                var getworkingdays = ctx.SiteTestingdays.Join(ctx.TestingArea, b => b.testingareaid, c => c.TestingAreaID, (b, c) => new { b, c })
                    .Where(x => x.b.siteid == res[i].SiteID).Select(x => new
                    {
                        TestingArea = x.c.AreaName,
                        workingDays = x.b.testingdays
                    }).ToList();


                string ab = "GeneralWorking Days";
                DyObjectsList.Add(new
                {
                    Region = res[i].Regionname,
                    Category = res[i].Categoryname,
                    Site = res[i].SiteName,
                    GeneralWorkingDays = res[i].Workingdays
                });


                for (int j = 0; j < getworkingdays.Count; j++)
                {
                    //string ss =getworkingdays[j].TestingArea.ToLower() + " Test";
                    //DyObjectsList.Append(getworkingdays[j].workingDays);
                    //DyObjectsList[i].Add(new{
                    //    ss = getworkingdays[j].workingDays

                    //    });
                    Dr[getworkingdays[j].TestingArea.ToLower() + " Test"] = getworkingdays[j].workingDays;
                }
                DT.Rows.Add(Dr);

            }






            var cc = DT.Rows.Cast<DataRow>().ToArray();
            var bb = DT.Select().ToArray();
            // cc[0].Table.ToArray();
            var aa = new Dynamicarray
            {
                data = DT,
                column = getcolumnname(),
                header = getdynamicctrl()
            };
            return aa;
        }
        public Array getdynamicctrl()
        {
            var getworkingdays = ctx.SiteTestingdays.Join(ctx.TestingArea, b => b.testingareaid, c => c.TestingAreaID, (b, c) => new { b, c })
                .GroupBy(g => g.b.testingareaid)
                 .Select(x => new
                 {

                     TestingArea = x.Max(s => s.c.AreaName),

                 }).ToArray();
            return getworkingdays;

        }
        public Array Getregionlist(int noofsites, string logic)
        {
            string[] param = logic.Split(",");
            Array A = new Array[ctx.Region.Count()];
            List<filterregionlist> filterregionlist = new List<filterregionlist>();
            if (param[0] == "")
            {
                filterregionlist = ctx.Region.Join(ctx.Site, b => b.RegionID, c => c.regionid, (b, c) => new { b, c })
                    .Join(ctx.Country,d=>d.b.CountryId,e=>e.Id, (d, e) => new {d, e})
                    .GroupBy(g => new { g.d.b.RegionName, g.d.b.ShortName })

                    .Select(h => new filterregionlist
                    {
                        ShortName = h.Key.ShortName,
                        RegionName = h.Key.RegionName,
                        NoofSites = h.Count(x => x.d.c.SiteID != 0),
                        countryid=h.Max(x=>x.d.b.CountryId),
                        countryname=h.Max(x => x.e.Name)
                    }).ToList();

                //A = res;
                //return A;

            }
            else if (param[0] == ">")
            {
                filterregionlist = ctx.Region.Join(ctx.Site, b => b.RegionID, c => c.regionid, (b, c) => new { b, c })
                      .Join(ctx.Country, d => d.b.CountryId, e => e.Id, (d, e) => new { d, e })
                 .GroupBy(g => new { g.d.b.RegionName, g.d.b.ShortName })
                 .Where(grp => grp.Count() > noofsites)
                 .Select(h => new filterregionlist
                 {
                     ShortName = h.Key.ShortName,
                     RegionName = h.Key.RegionName,
                     NoofSites = h.Count(x => x.d.c.SiteID != 0),
                     countryid = h.Max(x => x.d.b.CountryId),
                     countryname = h.Max(x => x.e.Name)
                 }).ToList();
                //A = res;
                //return A;
            }
            else if (param[0] == "<")
            {

                filterregionlist = ctx.Region.Join(ctx.Site, b => b.RegionID, c => c.regionid, (b, c) => new { b, c })
                       .Join(ctx.Country, d => d.b.CountryId, e => e.Id, (d, e) => new { d, e })
                 .GroupBy(g => new { g.d.b.RegionName, g.d.b.ShortName })
              .Where(grp => grp.Count() < noofsites)
              .Select(h => new filterregionlist
              {
                  ShortName = h.Key.ShortName,
                  RegionName = h.Key.RegionName,
                  NoofSites = h.Count(x => x.d.c.SiteID != 0),
                  countryid = h.Max(x => x.d.b.CountryId),
                  countryname = h.Max(x => x.e.Name)
              }).ToList();
                //A = res;
                //return A;
            }
            else if (param[0] == "=")
            {
                filterregionlist = ctx.Region.Join(ctx.Site, b => b.RegionID, c => c.regionid, (b, c) => new { b, c })
             .Join(ctx.Country, d => d.b.CountryId, e => e.Id, (d, e) => new { d, e })
                 .GroupBy(g => new { g.d.b.RegionName, g.d.b.ShortName })
            .Where(grp => grp.Count() == noofsites)
            .Select(h => new filterregionlist
            {
                ShortName = h.Key.ShortName,
                RegionName = h.Key.RegionName,
                NoofSites = h.Count(x => x.d.c.SiteID != 0),
                countryid = h.Max(x => x.d.b.CountryId),
                countryname = h.Max(x => x.e.Name)
            }).ToList();
                //A = res;
                //return A;
            }
            if (param[1]!="0")
            {
                filterregionlist = filterregionlist.Where(b => b.countryid == Convert.ToInt32(param[1])).ToList();
            }
            A = filterregionlist.ToArray();
            return A;
        }

        public Array Gettestlist(int areaid)
        {
            Array A;
           var res = ctx.Test.Join(ctx.TestingArea, b => b.TestingAreaID, c => c.TestingAreaID, (b, c) => new { b, c })           
                .Select(x => new
                {
                    area = x.c.AreaName,
                    test = x.b.TestName,
                    areaid=x.c.TestingAreaID
                }).ToList();
            if (areaid==0)
            {
                A = res.ToArray();
            }
            else
            {
                A = res.Where(e => e.areaid == areaid).ToArray();
            }
            return A;
        }
        public DataTable Getcomparisionsummarydata(string param)
        {
            DataTable DT = new DataTable();
            DT.Columns.Add("Methodology");
            DT.Columns.Add("ProductType");
            DT.Columns.Add("Cost");

            ForecastInfo forecastInfo = new ForecastInfo();
            string[] p1;
            p1 = param.Split(",");
            var getdistinctproducttype = ctx.ForecastedResult.GroupBy(x => x.ProductTypeId).Select(x => new { producttypename = x.Max(s => s.ProductType), producttypeid = x.Key }).ToList();

            var consumptionforecastsummary = ctx.ForecastedResult
             .Join(ctx.ForecastInfo, b => b.ForecastId, c => c.ForecastID, (b, c) => new { b, c })
             .Join(ctx.MasterProduct, e => e.b.ProductId, f => f.ProductID, (e, f) => new { e, f })
             .Where(x => x.e.b.ForecastId == Convert.ToInt16(p1[0]) && x.e.b.DurationDateTime >= x.e.c.StartDate && x.e.b.DurationDateTime < GetMaxForecastDate(x.e.c.Period, x.e.c.StartDate, x.e.c.Extension))
             .GroupBy(s => new { s.e.b.ProductType, s.e.b.Duration, s.e.b.DurationDateTime, s.e.c.StartDate })
             .Select(n => new
             {

                 ProductType = n.Key.ProductType,

                 NoofProduct = n.Sum(x => x.e.b.TotalValue),
                 PackQty = n.Sum(x => x.e.b.PackQty),
                 Price = n.Sum(x => x.e.b.PackPrice),
                 Duration = n.Key.Duration,
                 DurationDateTime = n.Key.DurationDateTime

             }).OrderBy(x => x.DurationDateTime).ToList();




            var serviceforecastsummary = ctx.ForecastedResult
                     .Join(ctx.ForecastInfo, b => b.ForecastId, c => c.ForecastID, (b, c) => new { b, c })
                     .Join(ctx.MasterProduct, e => e.b.ProductId, f => f.ProductID, (e, f) => new { e, f })
                     .Where(x => x.e.b.ForecastId == Convert.ToInt16(p1[1]) && x.e.b.DurationDateTime >= x.e.c.StartDate && x.e.b.DurationDateTime < GetMaxForecastDate(x.e.c.Period, x.e.c.StartDate, x.e.c.Extension))
                     .GroupBy(s => new { s.e.b.ProductType, s.e.b.Duration, s.e.b.DurationDateTime, s.e.c.StartDate })
                     .Select(n => new
                     {

                         ProductType = n.Key.ProductType,

                         NoofProduct = n.Sum(x => x.e.b.TotalValue),
                         PackQty = n.Sum(x => x.e.b.PackQty),
                         Price = n.Sum(x => x.e.b.PackPrice),
                         Duration = n.Key.Duration,
                         DurationDateTime = n.Key.DurationDateTime

                     }).OrderBy(x => x.DurationDateTime).ToList();




            if (consumptionforecastsummary != null)
            {    //////////getconsumption



                for (int j = 0; j < getdistinctproducttype.Count; j++)
                {
                    var getconsumption = consumptionforecastsummary.Where(x => x.ProductType == getdistinctproducttype[j].producttypename).Sum(x => x.Price);

                    if (getconsumption != 0)
                    {
                        DataRow Dr = DT.NewRow();

                        Dr["Methodology"] = "Consumption";
                        Dr["ProductType"] = getdistinctproducttype[j].producttypename;
                        Dr["Cost"] = Convert.ToInt32(getconsumption);
                        DT.Rows.Add(Dr);
                    }
                }

            }


            if (serviceforecastsummary != null)
            {    //////////getservice

                for (int j = 0; j < getdistinctproducttype.Count; j++)
                {
                    var getservice = serviceforecastsummary.Where(x => x.ProductType == getdistinctproducttype[j].producttypename).Sum(x => x.Price);
                    if (getservice != 0)
                    {

                        DataRow Dr = DT.NewRow();

                        Dr["Methodology"] = "Service Staistic";
                        Dr["ProductType"] = getdistinctproducttype[j].producttypename;
                        Dr["Cost"] = Convert.ToInt32(getservice);
                        DT.Rows.Add(Dr);
                    }
                }




            }


            forecastInfo = ctx.ForecastInfo.Where(b => b.ForecastID == Convert.ToInt16(p1[2])).FirstOrDefault();
            if (forecastInfo != null)
            {    //////////getservice
                for (int j = 0; j < getdistinctproducttype.Count; j++)
                {
                 
                    var demographicforecastsummary = ctx.ForecastedResult

                  .Where(x => x.ForecastId == forecastInfo.ForecastID && x.ProductTypeId == Convert.ToInt16(getdistinctproducttype[j].producttypeid))
                  .GroupBy(s => new { s.SiteId, s.ProductTypeId, s.ProductId })
                  .Select(n => new { n.Key.ProductTypeId, totalcost = Math.Round(Math.Ceiling(n.Sum(s => s.TotalValue) / n.Max(s => s.PackQty)) * n.Max(s => s.PackPrice), 2) }).ToList();
                    var demographicforecastsummary1 = demographicforecastsummary.GroupBy(s => new { s.ProductTypeId }).Select(g => g.Sum(x => x.totalcost)).FirstOrDefault();
                    if (demographicforecastsummary1 != 0)
                    {
                        DataRow Dr = DT.NewRow();

                        Dr["Methodology"] = "Demographic";
                        Dr["ProductType"] = getdistinctproducttype[j].producttypename;
                        Dr["Cost"] = Convert.ToInt32(demographicforecastsummary1);
                        DT.Rows.Add(Dr);
                    }
                }

            }
            return DT;
        }
        public Dynamicarray Getdemographicsummary(int id)
        {
            decimal existingpatienttest;
            decimal existingpatient = 0;
            decimal testPermonth;
            decimal totaltest;
            string[] siteids;
            decimal productused = 0;
            decimal getworkingdays = 0;
            decimal existingtestnumber=0;
            decimal TotalTestPerYear = 0;
            decimal tttest = 0;
            decimal finaltotalcost = 0;
            Dynamicarray dynamic = new Dynamicarray();
            List<int> distinctproductids = new List<int>();
            List<Productsummary> Productsummary = new List<Productsummary>();
            var _forecastinfo = ctx.ForecastInfo.Find(id);
       
            List<columnname> Db = new List<columnname>();
            string[] Header;
            DataTable DT = new DataTable();
            int index;
           DT.Columns.Add("product type");
            DT.Columns.Add("product name");
            ///////////getunique duration
         //   var getuniqueduration = ctx.TestByMonth.Where(b => b.ForeCastID == id).GroupBy(x => x.Month).OrderBy(x => x.Max(s => s.SNo)).Select(s => new { Duration = s.Key }).ToList();
            /////////////////getunique productid





            //for (int i = 0; i < getuniqueduration.Count; i++)
            //{
            //    DT.Columns.Add(getuniqueduration[i].Duration.ToLower());
            //}
            DT.Columns.Add("total quantity in pack");
         //   DT.Columns.Add("adjusted pack size");
            DT.Columns.Add("cost");
            DT.Columns.Add("total cost");
            Header = new string[DT.Columns.Count];
            for (int i = 0; i < DT.Columns.Count; i++)
            {
                Header[i] = Char.ToUpperInvariant(DT.Columns[i].Caption[0]) + DT.Columns[i].Caption.Substring(1);
                Db.Add(new columnname
                {
                    data = DT.Columns[i].Caption
                });
            }
            var distintsitecategoryids = ctx.TestByMonth.Where(b => b.ForeCastID == id).GroupBy(x => x.sitecategoryid).Select(x => x.Key).ToList();
            var occurrenceofsamemonth = ctx.TestByMonth.Where(b => b.ForeCastID == id).GroupBy(x => x.Month).Select(s => s.Key).Count();
            //if (_forecastinfo.ForecastType == "S")
            //{
            //    siteids = ctx.ForecastSiteInfo.Where(b => b.ForecastinfoID == id).Select(x => Convert.ToString(x.SiteID)).ToArray();
            //    existingpatient = Convert.ToDecimal(ctx.ForecastSiteInfo.Where(b => b.ForecastinfoID == id).Average(x => x.CurrentPatient));
            //}
            //else
            //{
            //    siteids = ctx.ForecastCategorySiteInfo.Where(b => b.ForecastInfoID == id).Select(x => Convert.ToString(x.SiteID)).ToArray();
            //    existingpatient = Convert.ToDecimal(ctx.ForecastCategoryInfo.Where(b => b.ForecastinfoID == id).Average(x => x.CurrentPatient));
            //}
            //getworkingdays = ctx.Site.Where(b => siteids.Contains(Convert.ToString(b.SiteID))).Average(x => Convert.ToDecimal(x.WorkingDays));


            //TotalTestPerYear = Convert.ToDecimal(ctx.TestingProtocol.Where(b => b.ForecastinfoID == id).Average(x => x.TotalTestPerYear));
            //existingpatienttest = existingpatient * testbymonth[0].Duration * TotalTestPerYear;
            //existingtestnumber = existingpatienttest / (testbymonth[0].Duration * 12);

            //TotalTestPerYear = Convert.ToDecimal(ctx.TestingProtocol.Where(b => b.ForecastinfoID == id).Average(x => x.TotalTestPerYear));
            //existingpatienttest = existingpatient * testbymonth[0].Duration * TotalTestPerYear;
            //existingtestnumber = existingpatienttest / (testbymonth[0].Duration * 12);
            for (int z = 0; z < distintsitecategoryids.Count; z++)
            {

                var testbymonth = ctx.TestByMonth.Where(b => b.ForeCastID == id && b.sitecategoryid == distintsitecategoryids[z]).ToList();

                if (_forecastinfo.ForecastType == "S")
                {
                    siteids = new string[1];
                    siteids[0] = distintsitecategoryids[z].ToString();
                    existingpatient = Convert.ToDecimal(ctx.ForecastSiteInfo.Where(b => b.ForecastinfoID == id && b.SiteID == distintsitecategoryids[z]).Select(x => x.CurrentPatient).FirstOrDefault());
                    getworkingdays = ctx.Site.Where(b => b.SiteID == distintsitecategoryids[z]).Select(x => Convert.ToDecimal(x.WorkingDays)).FirstOrDefault();
                }
                else
                {
                    existingpatient = Convert.ToDecimal(ctx.ForecastCategoryInfo.Where(b => b.ForecastinfoID == id && b.SiteCategoryId == distintsitecategoryids[z]).Select(x => x.CurrentPatient).FirstOrDefault());
                    siteids = ctx.ForecastCategorySiteInfo.Where(b => b.ForecastInfoID == id).Select(x => Convert.ToString(x.SiteID)).ToArray();
                    getworkingdays = ctx.Site.Where(b => siteids.Contains(Convert.ToString(b.SiteID))).Average(x => Convert.ToDecimal(x.WorkingDays));
                }



                TotalTestPerYear = Convert.ToDecimal(ctx.TestingProtocol.Where(b => b.ForecastinfoID == id).Average(x => x.TotalTestPerYear));
                existingpatienttest = existingpatient * testbymonth[0].Duration * TotalTestPerYear;
                existingtestnumber = existingpatienttest / (testbymonth[0].Duration * 12);

                for (int i = 0; i < testbymonth.Count; i++)
                {




                    testPermonth = Convert.ToDecimal(testbymonth[i].TstNo) * Convert.ToDecimal(testbymonth[i].NewPatient);
                  //  totaltest2 = totaltest2 + testPermonth;
                    totaltest = testPermonth + Math.Round(existingtestnumber, 2);

                    //newpatienttestnumber = totaltest + (totaltest * (Per / 100));
                    var getgeneralvariable = ctx.demographicMMGeneralAssumption.Where(b => b.Forecastid == _forecastinfo.ForecastID && b.Entity_type_id == 5 && !b.VariableName.Contains("Mon")).ToList();
                    foreach (demographicMMGeneralAssumption M in getgeneralvariable)
                    {

                        var getgeneralassumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.Forecastid == id && b.TestID == testbymonth[i].TestID && b.PatientGroupID == testbymonth[i].PGrp && b.Parameterid == M.Id).FirstOrDefault();
                        if (M.VariableEffect == true)
                        {
                            if (M.VariableDataType == 1)
                            {
                                totaltest = totaltest + getgeneralassumptionvalue.Parametervalue;
                            }
                            else
                            {
                                totaltest = totaltest + ((totaltest * getgeneralassumptionvalue.Parametervalue) / 100);
                            }
                        }
                        else

                        {
                            if (M.VariableDataType == 1)
                            {
                                totaltest = totaltest - getgeneralassumptionvalue.Parametervalue;
                            }
                            else
                            {
                                totaltest = totaltest - ((totaltest * getgeneralassumptionvalue.Parametervalue) / 100);
                            }
                        }

                    }

                    tttest = tttest + totaltest;
                    /////////////////getproductusage


                    var productusagebytest = ctx.ProductUsage.Join(ctx.MasterProduct, b => b.ProductId, c => c.ProductID, (b, c) => new { b, c })
                        .Join(ctx.ProductType, e => e.c.ProductTypeId, f => f.TypeID, (e, f) => new { e, f })
                        .Join(ctx.ProductPrice, g => g.e.c.ProductID, h => h.ProductId, (g, h) => new { g, h })
                        .Where(x => x.g.e.b.TestId == testbymonth[i].TestID && x.g.e.b.IsForControl == false).Select(x => new
                        {

                            producttypeid = x.g.f.TypeID,
                            producttypename = x.g.f.TypeName,
                            productid = x.g.e.c.ProductID,
                            productname = x.g.e.c.ProductName,
                            duration = testbymonth[i].Month,
                            quantity = getproductusagequantity(x.g.e.b.Rate, totaltest, ctx.siteinstrument.Where(b => b.InstrumentID == x.g.e.b.InstrumentId && siteids.Contains(Convert.ToString(b.SiteID))).FirstOrDefault() == null ? 0 : ctx.siteinstrument.Where(b => b.InstrumentID == x.g.e.b.InstrumentId && siteids.Contains(Convert.ToString(b.SiteID))).Average(s => s.TestRunPercentage)),
                            cost = x.h.Price,
                            packsize = x.h.PackSize


                        })
                        .ToList();
                    /////////////getcontrolusage
                    var controlusagerate = ctx.ProductUsage.Join(ctx.MasterProduct, b => b.ProductId, c => c.ProductID, (b, c) => new { b, c })
                         .Join(ctx.ProductType, e => e.c.ProductTypeId, f => f.TypeID, (e, f) => new { e, f })
                         .Join(ctx.ProductPrice, g => g.e.c.ProductID, h => h.ProductId, (g, h) => new { g, h })
                         .Where(x => x.g.e.b.TestId == testbymonth[i].TestID && x.g.e.b.IsForControl == true).Select(x => new
                         {

                             producttypeid = x.g.f.TypeID,
                             producttypename = x.g.f.TypeName,
                             productid = x.g.e.c.ProductID,
                             productname = x.g.e.c.ProductName,
                             duration = testbymonth[i].Month,
                             quantity = Getcontrolquantity(ctx.Instrument.Where(s => s.InstrumentID == x.g.e.b.InstrumentId).FirstOrDefault(), x.g.e.b.Rate, testbymonth[i].Duration, getworkingdays, occurrenceofsamemonth),
                             cost = x.h.Price,
                             packsize = x.h.PackSize


                         })
                         .ToList();

                    ////////getconsumableuusage

                    var consumableusage = ctx.ConsumableUsage.Join(ctx.MasterConsumable, b => b.ConsumableId, c => c.MasterCID, (b, c) => new { b, c })
                            .Join(ctx.MasterProduct, e => e.b.ProductId, f => f.ProductID, (e, f) => new { e, f })
                                 .Join(ctx.ProductType, g => g.f.ProductTypeId, h => h.TypeID, (g, h) => new { g, h })
                            .Join(ctx.ProductPrice, j => j.g.f.ProductID, k => k.ProductId, (j, k) => new { j, k })
                            .Where(x => x.j.g.e.c.TestId == testbymonth[i].TestID).Select(x => new
                            {

                                producttypeid = x.j.h.TypeID,
                                producttypename = x.j.h.TypeName,
                                productid = x.j.g.f.ProductID,
                                productname = x.j.g.f.ProductName,
                                duration = testbymonth[i].Month,
                                quantity = Getconsumablequantity(x.j.g.e.b, siteids, testbymonth[i].Duration, getworkingdays, totaltest, ctx.siteinstrument.Where(b => b.InstrumentID == x.j.g.e.b.InstrumentId && siteids.Contains(Convert.ToString(b.SiteID))).FirstOrDefault() != null ? ctx.siteinstrument.Where(b => b.InstrumentID == x.j.g.e.b.InstrumentId && siteids.Contains(Convert.ToString(b.SiteID))).Average(s => s.Quantity) : 0, occurrenceofsamemonth),
                                cost = x.k.Price,
                                packsize = x.k.PackSize


                            })
                            .ToList();


                    var getcompleteproductusage = productusagebytest.Union(controlusagerate).Union(consumableusage).GroupBy(x => new { x.duration, x.productid }).Select(x => new
                    {

                        producttypeid = x.Max(s => s.producttypeid),
                        producttypename = x.Max(s => s.producttypename),
                        productid = x.Key.productid,
                        productname = x.Max(s => s.productname),
                        duration = x.Key.duration,
                        quantity = x.Sum(s => s.quantity),
                        cost = x.Max(s => s.cost),
                        packsize = x.Max(s => s.packsize),
                        Sno = testbymonth[i].SNo


                    }).ToList();

                    foreach (var a in getcompleteproductusage)
                    {
                        Productsummary.Add(new Productsummary
                        {
                            producttypeid = a.producttypeid,
                            producttype = a.producttypename,
                            productid = a.productid,
                            productname = a.productname,
                            duration = a.duration,
                            quantity = a.quantity,
                            cost = a.cost,
                            packsize = a.packsize,
                            sitecategoryid = distintsitecategoryids[z],
                            sno = a.Sno

                        });

                    }



                }
            }
            var productsummary1 = Productsummary.GroupBy(x => new { x.sitecategoryid, x.productid, x.sno, }).Select(x => new
            {
                producttypeid = x.Max(s => s.producttypeid),
                producttype = x.Max(s => s.producttype),
                productid = x.Key.productid,
                productname = x.Max(s => s.productname),
                duration = x.Max(s => s.duration),
                quantity = x.Sum(s => s.quantity),
                cost = x.Max(a => a.cost),
                packsize = x.Max(a => a.packsize),
                sitecategoryid = x.Key.sitecategoryid,
                sno = x.Key.sno
            }).ToList();
            var forecastresult = ctx.ForecastedResult.Where(b => b.ForecastId == id).ToList();
            ctx.ForecastedResult.RemoveRange(forecastresult);
            ctx.SaveChanges();

            for (int i = 0; i < productsummary1.Count; i++)
            {
                ForecastedResult FR = new ForecastedResult();
                FR.ProductId = productsummary1[i].productid;
                FR.ProductTypeId = productsummary1[i].producttypeid;
                FR.ProductType = productsummary1[i].producttype;
                FR.TotalValue = productsummary1[i].quantity;
                FR.PackQty = Convert.ToInt32(productsummary1[i].packsize);
                FR.PackPrice = productsummary1[i].cost;
                FR.Duration = productsummary1[i].duration;
                FR.ForecastId = id;
                FR.DurationDateTime = DateTime.Now;
                FR.SiteId = productsummary1[i].sitecategoryid;
                FR.Sno = productsummary1[i].sno;
                ctx.ForecastedResult.Add(FR);
                ctx.SaveChanges();


            }



           

            var getuniqueproduct = ctx.ForecastedResult.Join(ctx.MasterProduct,b=>b.ProductId,c=>c.ProductID,(b,c)=>  new{b,c }).Where(g=>g.b.ForecastId==id).GroupBy(x => new { x.b.SiteId, x.b.ProductId })
                .Select(s => new { Productid = s.Key.ProductId , ProductType =s.Max(x=>x.b.ProductType), ProductName =s.Max(x=>x.c.ProductName),siteid=s.Key.SiteId}).ToList();
            for (int i = 0; i < getuniqueproduct.Count; i++)
            {
                DataRow Dr = DT.NewRow();
                decimal Totalproduct = 0;
                decimal Totalprice = 0;
                decimal packquantity = 0;
                if (_forecastinfo.ForecastType=="S")
                Dr["product type"] =ctx.Site.Where(b=>b.SiteID == getuniqueproduct[i].siteid).Select(c=>c.SiteName).FirstOrDefault();
                else
                 Dr["product type"] = ctx.SiteCategory.Where(b=>b.CategoryID== getuniqueproduct[i].siteid).Select(c => c.CategoryName).FirstOrDefault();
                Dr["product name"] = getuniqueproduct[i].ProductName;
                var demographicduration = ctx.ForecastedResult.Where(x => x.ProductId == getuniqueproduct[i].Productid && x.ForecastId==id ).Select(n => new
                {

                    ProductType = n.ProductType,
                 
                    NoofProduct = n.TotalValue,
                    PackQty = n.PackQty,
                    Price = n.PackPrice,
                    Duration = n.Duration

                }).ToList();
                for (int j = 0; j < demographicduration.Count; j++)
                {
                    //Dr[demographicduration[j].Duration.ToLower()] = Convert.ToInt32(demographicduration[j].NoofProduct);
                    Totalproduct = Totalproduct + demographicduration[j].NoofProduct;
                    Totalprice = demographicduration[j].Price;
                    packquantity = demographicduration[j].PackQty;
                }
          //      Dr["total quantity in pack"] = Totalproduct;
                Dr["total quantity in pack"] =Math.Ceiling( Totalproduct / packquantity);
                
                Dr["cost"]= String.Format("{0:n}", Math.Round(Totalprice,2));
                Dr["total cost"]= String.Format("{0:n}", Math.Round(Math.Ceiling(Totalproduct / packquantity)* Totalprice,2));
                if (Totalproduct != 0)
                {
                    DT.Rows.Add(Dr);
                    finaltotalcost= finaltotalcost+ Math.Round(Math.Ceiling(Totalproduct / packquantity) * Totalprice, 2);
                }
            }
          Array A = DT.Select("[total quantity in pack] <> 0").ToArray();
            var sum = tttest;
            dynamic.data = DT;
            dynamic.column = Db;
            dynamic.header = Header;
            dynamic.title = _forecastinfo.ForecastNo;
            dynamic.forecastperiod = _forecastinfo.StartDate.ToString("MMM") + "-" + _forecastinfo.StartDate.Year + "  " + _forecastinfo.ForecastDate.Value.ToString("MMM") + "-" + _forecastinfo.ForecastDate.Value.Year;
            dynamic.Finalcost = String.Format("{0:n}", finaltotalcost);
            return dynamic;

         
        }
        private decimal getproductusagequantity(decimal rate, decimal testno, decimal insquantity)
        {
            decimal quantity = 0;

            quantity = rate * testno * (insquantity / 100);
            return quantity;
        }
        private decimal Getconsumablequantity(ConsumableUsage CU, string[] siteids, decimal duration, decimal workingdays, decimal testno, double insquantity, int noofsite)
        {

            decimal quantity = 0;
            decimal monthinduration = 0;
            decimal weekinduration = 0;
            decimal workingdaysinduration = 0;
            decimal Quarterinduration = 0;

            monthinduration = duration * 12;
            Quarterinduration = monthinduration / 3;

            weekinduration = duration * 4;
            workingdaysinduration = duration * workingdays;


            if (CU.PerTest == true)
            {
                quantity = (testno / CU.NoOfTest) * CU.UsageRate;

            }
            else if (CU.PerPeriod == true)
            {


                if (CU.Period == "Daily")
                {
                    quantity = (workingdays * CU.UsageRate) / noofsite;
                }
                else if (CU.Period == "Monthly")
                {
                    quantity = (1 * CU.UsageRate) / noofsite;
                }
                else if (CU.Period == "Weekly")
                {
                    quantity = (4 * CU.UsageRate) / noofsite;
                }
                else if (CU.Period == "Yearly")
                {
                    quantity = ((1 / 12) * CU.UsageRate) / noofsite;
                }
            }
            else
            {

                if (CU.Period == "Daily")
                {
                    quantity = (workingdays * CU.UsageRate * Convert.ToDecimal(insquantity)) / noofsite;
                }
                else if (CU.Period == "Monthly")
                {
                    quantity = (1 * CU.UsageRate * Convert.ToDecimal(insquantity)) / noofsite;
                }
                else if (CU.Period == "Weekly")
                {
                    quantity = (4 * CU.UsageRate * Convert.ToDecimal(insquantity)) / noofsite;
                }
                else if (CU.Period == "Yearly")
                {
                    quantity = ((1 / 12) * CU.UsageRate * Convert.ToDecimal(insquantity)) / noofsite;
                }




            }
            return quantity;
        }
        private decimal Getcontrolquantity(Instrument I, decimal rate, decimal duration, decimal workingdays, int noofsite)
        {
            decimal quantity = 0;
            decimal monthinduration = 0;
            decimal weekinduration = 0;
            decimal workingdaysinduration = 0;
            decimal Quarterinduration = 0;


            monthinduration = duration * 12;
            Quarterinduration = duration / 3;

            weekinduration = duration * 4;
            workingdaysinduration = duration * workingdays;
            if (I.DailyCtrlTest > 0)
            {
                quantity = (rate * I.DailyCtrlTest * workingdays) / noofsite;

            }
            else if (I.WeeklyCtrlTest > 0)
            {
                quantity = (rate * I.WeeklyCtrlTest * 4) / noofsite;
            }
            else if (I.MonthlyCtrlTest > 0)
            {
                quantity = (rate * I.MonthlyCtrlTest * 1) / noofsite;
            }
            else if (I.QuarterlyCtrlTest > 0)
            {
                quantity = (rate * I.QuarterlyCtrlTest * (1 / 3)) / noofsite;
            }
            else

            {
                quantity = 0;
            }

            return quantity;
        }

        public Dynamicarray Getservicesummary(int id)
        {

            Dynamicarray dynamic = new Dynamicarray();
            var _forecastinfo = ctx.ForecastInfo.Find(id);
            var consumptionforecastsummary = ctx.ForecastedResult
                .Join(ctx.ForecastInfo, b => b.ForecastId, c => c.ForecastID, (b, c) => new { b, c })
                .Join(ctx.MasterProduct, e => e.b.ProductId, f => f.ProductID, (e, f) => new { e, f }) //x.e.b.ServiceConverted == true &&  && x.e.b.DurationDateTime >= x.e.c.StartDate && x.e.b.DurationDateTime < GetMaxForecastDate(x.e.c.Period, x.e.c.StartDate, x.e.c.Extension)
                .Where(x => x.e.b.ForecastId == id )
                .GroupBy(s => new { s.e.b.IsForGeneralConsumable, s.e.b.IsForControl, s.f.ProductName, s.e.b.Duration, s.e.b.DurationDateTime, s.e.b.ProductType, s.f.BasicUnit })
                .Select(n => new
                {

                    ProductType = n.Key.IsForControl == true ? n.Key.ProductType + "-Control" : n.Key.IsForGeneralConsumable == true ? n.Key.ProductType + "-General Consumable" : n.Key.ProductType,
                    ProductName = n.Key.ProductName,
                    NoofProduct = n.Sum(x => x.e.b.TotalValue),
                    PackQty = n.Sum(x => x.e.b.PackQty),
                    Price = n.Sum(x => x.e.b.PackPrice),
                    Duration = n.Key.Duration,
                    DurationDateTime = n.Key.DurationDateTime,
                    BasicUnit = n.Key.BasicUnit

                }).OrderBy(x => x.DurationDateTime).ToList();

            ////////////Getcolumnname
            decimal finaltotalcost = 0;
            List<columnname> Db = new List<columnname>();
            string[] Header;
            DataTable DT = new DataTable();
            int index;
            DT.Columns.Add("product type");
            DT.Columns.Add("product name");

            //var getuniqueduration = consumptionforecastsummary.GroupBy(x => x.Duration).OrderBy(x => x.Max(s => s.DurationDateTime)).Select(s => new { Duration = s.Key }).ToList();
            //for (int i = 0; i < getuniqueduration.Count; i++)
            //{
            //    DT.Columns.Add(getuniqueduration[i].Duration.ToLower());
            //}
            DT.Columns.Add("total no of product");
            DT.Columns.Add("total price");
            Header = new string[DT.Columns.Count];
            for (int i = 0; i < DT.Columns.Count; i++)
            {
                Header[i] = Char.ToUpperInvariant(DT.Columns[i].Caption[0]) + DT.Columns[i].Caption.Substring(1);
                Db.Add(new columnname
                {
                    data = DT.Columns[i].Caption
                });
            }
            var getuniqueproduct = consumptionforecastsummary.GroupBy(x => new { x.ProductType, x.ProductName }).OrderBy(x => x.Max(s => s.DurationDateTime)).Select(s => new { ProductType = s.Key.ProductType, ProductName = s.Key.ProductName }).ToList();
            for (int i = 0; i < getuniqueproduct.Count; i++)
            {
                DataRow Dr = DT.NewRow();
                decimal Totalproduct = 0;
                decimal Totalprice = 0;
                Dr["product type"] = getuniqueproduct[i].ProductType;
                Dr["product name"] = getuniqueproduct[i].ProductName;
                var consumptionduration = consumptionforecastsummary.Where(x => x.ProductName == getuniqueproduct[i].ProductName && x.ProductType == getuniqueproduct[i].ProductType).Select(n => new
                {

                    ProductType = n.ProductType,
                    ProductName = n.ProductName,
                    NoofProduct = n.NoofProduct,
                    PackQty = n.PackQty,
                    Price = n.Price,
                    Duration = n.Duration,
                    DurationDateTime = n.DurationDateTime

                }).OrderBy(x => x.DurationDateTime).ToList();
                for (int j = 0; j < consumptionduration.Count; j++)
                {
                   // Dr[consumptionduration[j].Duration.ToLower()] = consumptionduration[j].NoofProduct;
                    Totalproduct = Totalproduct + consumptionduration[j].NoofProduct;
                    Totalprice = Totalprice + consumptionduration[j].Price;
                }
                Dr["total no of product"] = Totalproduct;
                Dr["total price"] = Totalprice;
                DT.Rows.Add(Dr);
                if (Totalproduct != 0)
                {

                    finaltotalcost = finaltotalcost + Totalprice;
                }
            }
            DataView dv = new DataView(DT);
            dv.RowFilter = "[total price] <> '0.00'";
            DT = dv.ToTable();
  //          Array A = DT.Select("[total price] <> 0");
            dynamic.data = DT;
            dynamic.column = Db;
            dynamic.header = Header;
            dynamic.title = _forecastinfo.ForecastNo;
            dynamic.forecastperiod = _forecastinfo.StartDate.ToString("MMM") + "-" + _forecastinfo.StartDate.Year + "  " + _forecastinfo.ForecastDate.Value.ToString("MMM") + "-" + _forecastinfo.ForecastDate.Value.Year;
            dynamic.Finalcost = String.Format("{0:n}", finaltotalcost);
            return dynamic;
        }
        public Dynamicarray Getconsumptionsummary(int id)
       {
            decimal finaltotalcost = 0;
            Dynamicarray dynamic = new Dynamicarray();
            var _forecastinfo = ctx.ForecastInfo.Find(id);
            var consumptionforecastsummary = ctx.ForecastedResult
                .Join(ctx.ForecastInfo, b => b.ForecastId, c => c.ForecastID, (b, c) => new { b, c })
                .Join(ctx.MasterProduct, e => e.b.ProductId, f => f.ProductID, (e, f) => new { e, f })
               .Where(x => x.e.b.ForecastId == id)  //&& x.e.b.DurationDateTime >= x.e.c.StartDate && x.e.b.DurationDateTime < GetMaxForecastDate(x.e.c.Period, x.e.c.StartDate, x.e.c.Extension)
           .GroupBy(s => new { s.e.b.ProductId, s.e.b.Duration, s.e.b.DurationDateTime, s.e.b.ProductTypeId })
                .Select(n => new
                {

                   ProductType=n.Max(x=>x.e.b.ProductType)  ,
                   ProductName = n.Max(x => x.f.ProductName),
                  //  n.Key.ProductName,
                    NoofProduct = n.Sum(x => x.e.b.TotalValue),
                    PackQty = n.Sum(x => x.e.b.PackQty),
                    Price = n.Sum(x => x.e.b.PackPrice),
                    n.Key.Duration,
                  n.Key.DurationDateTime,
                   n.Key.ProductId,
                   n.Key.ProductTypeId

                }).OrderBy(x => x.DurationDateTime).ToList();

            ////////////Getcolumnname

            List<columnname> Db = new List<columnname>();
            string[] Header;
            DataTable DT = new DataTable();
            int index;
            DT.Columns.Add("product type");
            DT.Columns.Add("product name");

            //var getuniqueduration = consumptionforecastsummary.GroupBy(x => x.Duration).OrderBy(x => x.Max(s => s.DurationDateTime)).Select(s => new { Duration = s.Key }).ToList();
            //for (int i = 0; i < getuniqueduration.Count; i++)
            //{
            //    DT.Columns.Add(getuniqueduration[i].Duration.ToLower());
            //}

            DT.Columns.Add("total no of product");

            DT.Columns.Add("total price");
          //  DT.Columns.Add("Unit Cost");
         DT.Columns.Add("total priceo", typeof(decimal));
            Header = new string[DT.Columns.Count];
            for (int i = 0; i < DT.Columns.Count; i++)
            {
                Header[i] = Char.ToUpperInvariant(DT.Columns[i].Caption[0]) + DT.Columns[i].Caption.Substring(1);
                Db.Add(new columnname
                {
                    data = DT.Columns[i].Caption
                });
            }
            var getuniqueproduct = consumptionforecastsummary.GroupBy(x => new { x.ProductTypeId, x.ProductId }).OrderBy(x => x.Max(s => s.DurationDateTime)).Select(s => new { ProductType = s.Max(x=>x.ProductType), ProductName = s.Max(x=>x.ProductName),s.Key.ProductId }).ToList();
            for (int i = 0; i < getuniqueproduct.Count; i++)
            {
                DataRow Dr = DT.NewRow();
                decimal Totalproduct = 0;
                decimal Totalprice = 0;
                Dr["product type"] = getuniqueproduct[i].ProductType;
                Dr["product name"] = getuniqueproduct[i].ProductName;


                var productprice = ctx.ProductPrice.Where(b => b.ProductId == getuniqueproduct[i].ProductId);
                var consumptionduration = consumptionforecastsummary.Where(x => x.ProductName == getuniqueproduct[i].ProductName && x.ProductType == getuniqueproduct[i].ProductType).Select(n => new
                {

                 n.ProductType,
                 n.ProductName,
                 n.NoofProduct,
                  n.PackQty,
                    n.Price,
                    n.Duration,
                   n.DurationDateTime

                }).OrderBy(x => x.DurationDateTime).ToList();
                for (int j = 0; j < consumptionduration.Count; j++)
                {
                   // Dr[consumptionduration[j].Duration.ToLower()] = consumptionduration[j].NoofProduct;
                    Totalproduct = Totalproduct + consumptionduration[j].NoofProduct;
                    Totalprice = Totalprice + consumptionduration[j].Price;
                }
                Dr["total no of product"] = Totalproduct;
                Dr["total price"] = String.Format("{0:n}", Totalprice);

              Dr["total priceo"] = Totalprice ;
                DT.Rows.Add(Dr);
                if (Totalproduct != 0)
                {

                    finaltotalcost = finaltotalcost + Totalprice;
                }
            }
            DataView dv = new DataView(DT);
            dv.RowFilter = "[total priceo] <> '0.00'";
            dv.Sort = "[total priceo] desc";
       
            DT = dv.ToTable();
            // DT.DefaultView.Sort = "[total price] desc";
            DT.Columns.Remove("total priceo");
           
            dynamic.data = DT;
            dynamic.column = Db;
            dynamic.header = Header;
            dynamic.title = _forecastinfo.ForecastNo;
            dynamic.forecastperiod = _forecastinfo.StartDate.ToString("MMM") + "-" + _forecastinfo.StartDate.Year + "  " + _forecastinfo.ForecastDate.Value.ToString("MMM") + "-" + _forecastinfo.ForecastDate.Value.Year;
            dynamic.Finalcost = String.Format("{0:n}", finaltotalcost);
            return dynamic;
        }

        public Array Getconsumptionsummarynew(int id)
        {

            var consumptionforecastsummary = ctx.ForecastedResult
             .Join(ctx.ForecastInfo, b => b.ForecastId, c => c.ForecastID, (b, c) => new { b, c })
             .Join(ctx.MasterProduct, e => e.b.ProductId, f => f.ProductID, (e, f) => new { e, f })
            .Where(x => x.e.b.ForecastId == id)  //&& x.e.b.DurationDateTime >= x.e.c.StartDate && x.e.b.DurationDateTime < GetMaxForecastDate(x.e.c.Period, x.e.c.StartDate, x.e.c.Extension)
        .GroupBy(s => s.e.b.ProductId).OrderByDescending(i=>i.Sum(j=>j.e.b.PackPrice))
             .Select(n => new
             {

              
                 ProductName = n.Max(x => x.f.ProductName),
               
                  //  NoofProduct = n.Sum(x => x.e.b.TotalValue),
                 ForecastQty = n.Sum(x => x.e.b.PackQty),
                 Unitcost= GetActiveProductPrice(DateTime.Now, ctx.ProductPrice.Where(b => b.ProductId == n.Key).ToList()).packcost,
                 Price = String.Format("{0:n}", n.Sum(x => x.e.b.PackPrice)),
                

             }).ToArray();


            return consumptionforecastsummary;

        }


        public Array Getservicesummarynew(int id)
        {

            var consumptionforecastsummary = ctx.ForecastedResult
             .Join(ctx.Test, b => b.TestId, c => c.TestID, (b, c) => new { b, c })
                .Join(ctx.TestingArea, d => d.c.TestingAreaID, e => e.TestingAreaID, (d, e) => new { d, e })
                   .Where(s => s.d.b.ForecastId == id)  //&& x.e.b.DurationDateTime >= x.e.c.StartDate && x.e.b.DurationDateTime < GetMaxForecastDate(x.e.c.Period, x.e.c.StartDate, x.e.c.Extension)
        .GroupBy(x=>x.d.c.TestingAreaID)
             .Select(n => new
             {


                 TestName = n.Max(x => x.d.c.TestName),

                 //  NoofProduct = n.Sum(x => x.e.b.TotalValue),
                 ForecastQty = n.Sum(x => x.d.b.PackQty),
               // Unitcost = GetActiveProductPrice(DateTime.Now, ctx.ProductPrice.Where(b => b.ProductId == n.Key).ToList()).packcost,
                 Price = n.Sum(x => x.d.b.PackPrice),


             }).ToArray();


            return consumptionforecastsummary;

        }
        public  pricedetail GetActiveProductPrice(DateTime date,List<ProductPrice> _productPrices)
        {

            pricedetail activeProductPrice = null;
            DateTime? date1 = DateTime.Now;
            foreach (ProductPrice p in _productPrices)
            {
                if (p.FromDate <= date)
                {
                    if (activeProductPrice == null)
                    {
                        activeProductPrice = new pricedetail();
                        activeProductPrice.packcost = p.Price;
                        activeProductPrice.packsize = p.PackSize;
                        activeProductPrice.FromDate = p.FromDate;
                        date1 = p.FromDate;
                    }
                    else if (p.FromDate > activeProductPrice.FromDate)
                    {
                        activeProductPrice = new pricedetail();
                        activeProductPrice.packcost = p.Price;
                        activeProductPrice.packsize = p.PackSize;
                        activeProductPrice.FromDate = p.FromDate;
                        date1 = p.FromDate;
                    }
                    else if (_productPrices.Count == 1)
                    {
                        activeProductPrice = new pricedetail();
                        activeProductPrice.packcost = p.Price;
                        activeProductPrice.packsize = p.PackSize;
                        activeProductPrice.FromDate = p.FromDate;
                        date1 = p.FromDate;
                    }
                }
                else if (p.FromDate > date)
                {
                    activeProductPrice = new pricedetail();
                    activeProductPrice.packcost = p.Price;
                    activeProductPrice.packsize = p.PackSize;
                    activeProductPrice.FromDate = p.FromDate;
                    date1 = p.FromDate;
                }

            }
            return activeProductPrice;
        }
        public Array Getsiteinstruentlist(string param)
        {
            string[] p1;
            p1 = param.Split(",");

            Array A;
            var res = ctx.Site.Join(ctx.Region, b => b.regionid, c => c.RegionID, (b, c) => new { b, c })
                 .Join(ctx.SiteCategory, e => e.b.CategoryID, f => f.CategoryID, (e, f) => new { e, f })
                 .Join(ctx.siteinstrument, g => g.e.b.SiteID, h => h.SiteID, (g, h) => new { g, h })
                 .Join(ctx.Instrument, j => j.h.InstrumentID, k => k.InstrumentID, (j, k) => new { j, k })                
                 .Select(x => new
                 {

                     x.j.g.e.c.RegionName,
                      x.j.g.f.CategoryName,
                     regionid = x.j.g.e.c.RegionID,
                    x.j.g.f.CategoryID,
                    x.k.testingArea.TestingAreaID,
                  x.j.g.e.b.SiteName,
                  x.k.InstrumentName,
                     x.j.h.Quantity,
                      x.j.h.TestRunPercentage,
                     TestingArea = ctx.TestingArea.Where(v => v.TestingAreaID == x.k.testingArea.TestingAreaID).Select(s => s.AreaName).FirstOrDefault()
                 }).ToList();
            if (p1[0]=="0" && p1[1]=="0" && p1[2]=="0")
            {
                A = res.ToArray();
                
            }
            else
            {
                A = res.Where(l => l.CategoryID == Convert.ToInt32(p1[0]) || l.regionid == Convert.ToInt32(p1[1]) || l.TestingAreaID == Convert.ToInt32(p1[2])).ToArray();
            }
            
            return A;
        }
        private DateTime GetMaxForecastDate(string period, DateTime Startdate, int extension)
        {
            DateTime EndDate;
            int MonthAdded;
            MonthAdded = extension;


            if (period == "Bimonthly")
            {
                MonthAdded = extension * 2;
            }
            else if (period == "Quarterly")
            {
                MonthAdded = extension * 3;
            }
            else if (period == "Yearly")
            {
                MonthAdded = extension * 12;
            }


            EndDate = Startdate.AddMonths(MonthAdded);
            return EndDate;
        }


        public Dynamicarray Getnoofpatientsummary(int id)
        {
            Dynamicarray dynamic = new Dynamicarray();
            DataTable dtgrowth = new DataTable();
            List<columnname> Db = new List<columnname>();
            string[] Header;
            var forecastinfo = ctx.ForecastInfo.Where(b => b.ForecastID == id).FirstOrDefault();

            dtGetValue.Columns.Add("VariableName", typeof(string));
            dtGetValue.Columns.Add("VariableEffect", typeof(string));
            dtGetValue.Columns.Add("VariableDataType", typeof(string));
            var _mMGeneralAssumption = ctx.MMGeneralAssumption.Where(b => b.ProgramId == forecastinfo.ProgramId && b.Entity_type_id == 3);
            foreach (MMGeneralAssumption gAssumption in _mMGeneralAssumption)
            {

                DataRow dr = dtGetValue.NewRow();
                varids = varids + gAssumption.Id + ",";
                //varName += "Isnull([" + gAssumption.VariableName + "],0) As [" + gAssumption.VariableName + "],";
                //svarName += "" + gAssumption.VariableName + " " + ",";
                dr["VariableName"] = gAssumption.VariableName.ToString();
                dr["VariableEffect"] = gAssumption.VariableEffect.ToString();
                dr["VariableDataType"] = gAssumption.VariableDataType.ToString();

                dtGetValue.Rows.Add(dr);

                dtGetValue.AcceptChanges();
                dtValue.Columns.Add(gAssumption.VariableName.ToString());
            }

            dtgrowth = DisplayData(forecastinfo, _mMGeneralAssumption);
            Header = new string[dtgrowth.Columns.Count];
            for (int i = 0; i < dtgrowth.Columns.Count; i++)
            {
                Header[i] = Char.ToUpperInvariant(dtgrowth.Columns[i].Caption[0]) + dtgrowth.Columns[i].Caption.Substring(1);
                Db.Add(new columnname
                {
                    data = Char.ToLowerInvariant(dtgrowth.Columns[i].Caption[0]) + dtgrowth.Columns[i].Caption.Substring(1)
            
            });
            }
            dynamic.data = dtgrowth;
            dynamic.column = Db;
            dynamic.header = Header;
            return dynamic;
        }

        public Array GetForecastdescription(int id)
        {
            string [] A;
          
            string duration;
            var forecastinfo = ctx.ForecastInfo.Where(b => b.ForecastID == id).FirstOrDefault();

            DateTime StartDate1 = DateTime.Parse(forecastinfo.StartDate.ToShortDateString());
            DateTime EndDate1 = DateTime.Parse(forecastinfo.ForecastDate.Value.ToShortDateString());
           A= new string[2];

            duration = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(StartDate1.Month) + " " + StartDate1.Year + "-" + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(EndDate1.Month) + " " + EndDate1.Year;


               A[0] = forecastinfo.ForecastNo;
            A[1] = duration;
            return A;

        }
        private DataTable DisplayData(ForecastInfo b, IEnumerable<MMGeneralAssumption> f)
        {
            DataTable Dt = new DataTable();
            DateTime StartDate1 = DateTime.Parse(b.StartDate.ToShortDateString());
            DateTime EndDate1 = DateTime.Parse(b.ForecastDate.Value.ToShortDateString());
            DateTime month = new DateTime(StartDate1.Year, StartDate1.Month, 1); // StartDate1.Month;
                                                                                 //   StartDate1.Month = month.AddMonths(-1);
           string month1= CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(StartDate1.Month);                                                                 //  StartDate1.Month =StartDate1.AddMonths(StartDate1.Month
            StartDate1 = StartDate1.AddMonths(-1);


            int newpatient = 0;
           
            int targetpatient = 0;
            int totaltargetpatient = 0;
            int numberofslot = 0;
          
            int totalcurrentpatient = 0;
            int totalmonth = GetMonthDifference(StartDate1, EndDate1);
            Lstmonthname = MonthsBetween(StartDate1, EndDate1);
            dtlineargrowth.Columns.Add("sitecategoryname");

            DataTable dtpatient = new DataTable();
            dtpatient.Columns.Add("siteName");
            dtpatient.Columns.Add("currentpatient");
            dtpatient.Columns.Add("targetpatient");
            int reportingPeriod = 0;
            if (b.Period == "Bimonthly")
            {
                reportingPeriod = 2;
            }
            else if (b.Period == "Monthly")
            {
                reportingPeriod = 1;
            }
            else if (b.Period == "Quarterly")
            {
                reportingPeriod = 4;
            }
            else if (b.Period == "Yearly")
            {
                reportingPeriod = 12;
            }
            if (reportingPeriod != 0)
            {
                numberofslot = totalmonth / reportingPeriod;
            }


            int j=0;

            while (j < Lstmonthname.Count)
            {
              
                    dtlineargrowth.Columns.Add(Lstmonthname[j].ToString());
               j = j+ reportingPeriod;

            }



            if (b.ForecastType == "S")
            {
                var getpatient = ctx.ForecastSiteInfo.Where(c => c.ForecastinfoID == b.ForecastID).Join(ctx.Site,x=>x.SiteID,c=>c.SiteID,(x,c)=> new {x,c }).Select(s=>new {

                    siteName=s.c.SiteName,
                    currentpatient=s.x.CurrentPatient,
                    targetpatient=s.x.TargetPatient



                }).ToList();
                for (int i = 0; i < getpatient.Count; i++)
                {
                    DataRow Dr = dtpatient.NewRow();
                    Dr["siteName"] = getpatient[i].siteName;
                    Dr["currentpatient"] = getpatient[i].currentpatient;
                    Dr["targetpatient"] = getpatient[i].targetpatient;
                    dtpatient.Rows.Add(Dr);
                }

            }
            else
            {
                var getpatient = ctx.ForecastCategoryInfo.Where(c => c.ForecastinfoID == b.ForecastID).Join(ctx.SiteCategory, x => x.SiteCategoryId, c => c.CategoryID, (x, c) => new { x, c }).Select(s => new {

                    siteName = s.c.CategoryName,
                    currentpatient = s.x.CurrentPatient,
                    targetpatient = s.x.TargetPatient



                }).ToList();
                for (int i = 0; i < getpatient.Count; i++)
                {
                    DataRow Dr = dtpatient.NewRow();
                    Dr["siteName"] = getpatient[i].siteName;
                    Dr["currentpatient"] = getpatient[i].currentpatient;
                    Dr["targetpatient"] = getpatient[i].targetpatient;
                    dtpatient.Rows.Add(Dr);
                }
            }
            string[] arrids;
            int res = 0;
            arrids = varids.Trim(',').Split(',');
            for (int i = 0; i < dtpatient.Rows.Count; i++)
            {
               newpatient = Convert.ToInt32(dtpatient.Rows[i]["targetpatient"].ToString()) - Convert.ToInt32(dtpatient.Rows[i]["currentpatient"].ToString());
                //if (b.ForecastType == "S")
                //{
                //    DataRow Dr = dtValue.NewRow();
                //    foreach (MMGeneralAssumption gAssumption in f)
                //    {

                //        var mmgeneralassumptionvalue = ctx.MMGeneralAssumptionValue.Where(c => c.SiteID == Convert.ToInt32(dtpatient.Rows[i]["SiteCategoryID"].ToString()) && c.Forecastid == b.ForecastID && c.Parameterid == gAssumption.Id).FirstOrDefault();
                //        foreach (DataColumn dc in dtValue.Columns)
                //        {
                //            if (dc.Caption == gAssumption.VariableName)
                //            {
                //                if (mmgeneralassumptionvalue != null)
                //                    Dr[gAssumption.VariableName] = mmgeneralassumptionvalue.Parametervalue;
                //                else
                //                    Dr[gAssumption.VariableName] = 0;
                //            }
                //        }
                //    }
                //    dtValue.Rows.Add(Dr);

                //}
                //else
                //{
                //    DataRow Dr = dtValue.NewRow();
                //    foreach (MMGeneralAssumption gAssumption in f)
                //    {

                //        var mmgeneralassumptionvalue = ctx.MMGeneralAssumptionValue.Where(c => c.CategoryID == Convert.ToInt32(dtpatient.Rows[i]["SiteCategoryID"].ToString()) && c.Forecastid == b.ForecastID && c.Parameterid == gAssumption.Id).FirstOrDefault();
                //        foreach (DataColumn dc in dtValue.Columns)
                //        {
                //            if (dc.Caption == gAssumption.VariableName)
                //            {
                //                if (mmgeneralassumptionvalue != null)
                //                    Dr[gAssumption.VariableName] = mmgeneralassumptionvalue.Parametervalue;
                //                else
                //                    Dr[gAssumption.VariableName] = 0;
                //            }
                //        }
                //    }
                //    dtValue.Rows.Add(Dr);
                //}



                //newpatient =newpatient + (fPatient);

                //newpatient = newpatient - ((newpatient * HIVpositivewithoutfollowing) / 100);
                targetpatient = newpatient + Convert.ToInt32(dtpatient.Rows[i]["currentpatient"].ToString());

                totaltargetpatient += targetpatient;
                totalcurrentpatient += Convert.ToInt32(dtpatient.Rows[i]["currentpatient"].ToString());
               calculatelineargrowth(Convert.ToInt32(dtpatient.Rows[i]["currentpatient"].ToString()), targetpatient, numberofslot, 0, reportingPeriod, b.ForecastID, dtpatient.Rows[i]["siteName"].ToString());
                // calculatelineargrowth(Convert.ToInt32(Dt1.Rows[i]["ID"].ToString()), Dt1.Rows[i]["Name"].ToString(), Convert.ToInt32(Dt1.Rows[i]["CurrentPatient"].ToString()), Convert.ToInt32(Dt1.Rows[i]["TargetPatient"].ToString()), numberofslot,i);
            }
            Dt = dtlineargrowth;
            return Dt;
        }
        private void calculatelineargrowth(double currentpatient, double targetpatient, int numberslot, int k, int reportingPeriod, int forecastId,string sitename)//int ID,string sitename,
        {
            int count = numberslot;
            double slotvalue = 0;
            int colindex = 4;
         
            double diff = Convert.ToDouble(targetpatient - currentpatient);
            List<string> list1 = new List<string>();
            string List = "";
         
            DataRow dr = dtlineargrowth.NewRow();

            // dr["Id"] = Convert.ToString(ID);
            dr["sitecategoryname"] =sitename;
       
            //dr["Name"] = sitename;


            List += "," + Convert.ToString(currentpatient);
            while (count > 1)
            {
                slotvalue = currentpatient + (diff / numberslot);
                List += "," + Convert.ToString(Math.Round(slotvalue, 0));
                //  list1.Add(Convert.ToString(slotvalue));               
                count--;
                colindex++;
                currentpatient = slotvalue;
            }
            List += "," + Convert.ToString(targetpatient);
            List = List.TrimStart(',');
            string[] gridvalue = List.Split(',');
            int j = 0; int i = 0;

            while (i < Lstmonthname.Count)
            {
           
                dr[Lstmonthname[i].ToString()] = gridvalue[j].ToString();

                i = i + reportingPeriod;
                j++;
            }
            dtlineargrowth.AcceptChanges();
            dtlineargrowth.Rows.Add(dr);



        }
        private int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }
        private List<string> MonthsBetween(DateTime startDate, DateTime endDate)
        {
            DateTime iterator;
            DateTime limit;
            List<string> List = new List<string>();
            if (endDate > startDate)
            {
                iterator = new DateTime(startDate.Year, startDate.Month, 1);
                limit = endDate;
            }
            else
            {
                iterator = new DateTime(endDate.Year, endDate.Month, 1);
                limit = startDate;
            }

            var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
            while (iterator <= limit)
            {
                List.Add(dateTimeFormat.GetMonthName(iterator.Month).Substring(0, 3) + " " + iterator.Year.ToString());
                iterator = iterator.AddMonths(1);
            }
            return List;
        }


    }
}
