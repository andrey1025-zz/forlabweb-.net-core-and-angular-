using ForLabApi.DataInterface;
using ForLabApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ForLabApi.Repositories
{
    public class DashboardAccessRepositories : IDashboard<Dashboard, Dashboardchartdata>
    {

        DataTable DTProduct = new DataTable();
        ForLabContext ctx;
        int k = 0;
        public DashboardAccessRepositories(ForLabContext c)
        {
            ctx = c;
            //return ctx;
        }
        public IList<Dashboard> Getforecastcomparision(string param)
        {
            int[] data1 = new int[3];
            List<Dashboard> Db = new List<Dashboard>();
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

            for (int j = 0; j < getdistinctproducttype.Count; j++)
            {
                data1 = new int[3];

                if (consumptionforecastsummary != null)
                {    //////////getconsumption


                    var getconsumption = consumptionforecastsummary.Where(x => x.ProductType == getdistinctproducttype[j].producttypename).Sum(x => x.Price);
                    data1[0] = Convert.ToInt32(getconsumption);
                    //////////getservice

                }
                else
                {
                    data1[0] = 0;
                }

                if (serviceforecastsummary != null)
                {    //////////getservice


                    var getservice = serviceforecastsummary.Where(x => x.ProductType == getdistinctproducttype[j].producttypename).Sum(x => x.Price);
                    data1[1] = Convert.ToInt32(getservice);




                }
                else
                {
                    data1[1] = 0;
                }

                forecastInfo = ctx.ForecastInfo.Where(b => b.ForecastID == Convert.ToInt16(p1[2])).FirstOrDefault();
                if (forecastInfo != null)
                {    //////////getservice


                    var demographicforecastsummary = ctx.ForecastedResult

                  .Where(x => x.ForecastId == forecastInfo.ForecastID && x.ProductTypeId == Convert.ToInt16(getdistinctproducttype[j].producttypeid))
                  .GroupBy(s => new { s.SiteId, s.ProductTypeId, s.ProductId })
                  .Select(n => new { n.Key.ProductTypeId, totalcost = Math.Round(Math.Ceiling(n.Sum(s => s.TotalValue) / n.Max(s => s.PackQty)) * n.Max(s => s.PackPrice), 2) }).ToList();

                    var demographicforecastsummary1 = demographicforecastsummary.GroupBy(s => new { s.ProductTypeId }).Select(g => g.Sum(x => x.totalcost)).FirstOrDefault();
                    data1[2] = Convert.ToInt32(demographicforecastsummary1);




                }
                else
                {
                    data1[2] = 0;
                }

                Db.Add(new Dashboard
                {

                    name = getdistinctproducttype[j].producttypename,
                    data = data1

                });



            }

            return Db;
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
        public IList<Dashboard> Getnoofpatientpermonth(int id)
        {
            List<forecastsitedata> FS = new List<forecastsitedata>();
            var forecastinfo = ctx.ForecastInfo.Find(id);
            if (forecastinfo.ForecastType == "S")
            {
                FS = ctx.ForecastSiteInfo.Join(ctx.Site, b => b.SiteID, c => c.SiteID, (b, c) => new { b, c }).Where(x => x.b.ForecastinfoID == id).Select(g => new forecastsitedata
                {
                    name = g.c.SiteName,
                    id = g.c.SiteID
                }).ToList();
            }
            else
            {
                FS = ctx.ForecastCategoryInfo.Join(ctx.SiteCategory, b => b.SiteCategoryId, c => c.CategoryID, (b, c) => new { b, c }).Where(x => x.b.ForecastinfoID == id).Select(g => new forecastsitedata
                {
                    name = g.c.CategoryName,
                    id = g.c.CategoryID
                }).ToList();
            }
            //  var getallcategory = ctx.SiteCategory.ToList();
            List<Dashboard> Db = new List<Dashboard>();

            var month = ctx.PatientNumberDetail.Where(b => b.ForeCastId == id).GroupBy(g => new { g.Columnname }).OrderBy(f => f.Max(g => g.ID)).Select(x => x.Key).ToList();
            if (month.Count == 0)
                return Db;

            int[] data1 = new int[month.Count];

            var list = ctx.PatientNumberDetail.Where(b => b.ForeCastId == id).GroupBy(x => new { x.Columnname, x.SiteCategoryId }).OrderBy(f => f.Max(g => g.ID)).Select(g => new
            {
                g.Key.Columnname,
                g.Key.SiteCategoryId,
                count = g.Max(f => f.Serial)

            }).ToList();

            //  getallcategory = getallcategory.Where(b => list.Select(c => c.categoryID).Contains(b.CategoryID)).ToList();
            for (int j = 0; j < FS.Count; j++)
            {

                data1 = new int[month.Count];
                for (int i = 0; i < month.Count; i++)
                {
                    data1[i] = list.Where(b => b.SiteCategoryId == FS[j].id && b.Columnname == month[i].Columnname).Select(c => Convert.ToInt32(c.count)).FirstOrDefault();
                }

                Db.Add(new Dashboard
                {

                    name = FS[j].name,
                    data = data1

                });



            }
            return Db;
        }
        public IList<Dashboard> Getnoofsiteperregion(int id)
        {

            var getallcategory = ctx.SiteCategory.ToList();
            var region = ctx.Region.OrderByDescending(b => b.RegionID).ToList();
            int[] data1 = new int[region.Count];
            List<Dashboard> Db = new List<Dashboard>();
            var list = ctx.Site.Join(ctx.Region, b => b.regionid, c => c.RegionID, (b, c) => new { b, c })

                          .GroupBy(x => new { x.b.regionid, x.b.CategoryID }).Select(g => new
                          {
                              g.Key.regionid,
                              categoryID = g.Max(x => x.b.CategoryID),
                              countryid = g.Max(x => x.b.CountryId),
                              count = g.Count()

                          }).OrderByDescending(b => b.regionid).ToList();
            if (id != 0)
            {
                list = list.Where(b => b.countryid == id).ToList();
                region = region.Where(b => b.CountryId == id).ToList();
            }
            getallcategory = getallcategory.Where(b => list.Select(c => c.categoryID).Contains(b.CategoryID)).ToList();
            for (int j = 0; j < getallcategory.Count; j++)
            {

                data1 = new int[region.Count];
                for (int i = 0; i < region.Count; i++)
                {
                    data1[i] = list.Where(b => b.categoryID == getallcategory[j].CategoryID && b.regionid == region[i].RegionID).Select(c => c.count).FirstOrDefault();
                }

                Db.Add(new Dashboard
                {

                    name = getallcategory[j].CategoryName,
                    data = data1

                });



            }
            return Db;
        }
        public IList<Dashboardchartdata> Getnoofinsperarea(int userid)
        {
            List<Dashboardchartdata> ListDash = new List<Dashboardchartdata>();
            //var list = ctx.Test.Join(ctx.TestingArea, b => b.TestingAreaID, c => c.TestingAreaID, (b, c) => new { b, c })
            //  .GroupBy(x => new { x.b.TestingAreaID }).Select(g => new 
            //  {

            //      name = g.Max(x => x.c.AreaName),
            //      y = g.Count()


            //  }).ToList();


            var Roles = ctx.User.Where(b => b.Id == userid).Select(x => x.Role).FirstOrDefault();
            if (Roles == "admin")
            {
                ListDash = ctx.Instrument.Join(ctx.TestingArea, b => b.testingArea.TestingAreaID, c => c.TestingAreaID, (b, c) => new { b, c })
              .GroupBy(x => new { x.b.testingArea.TestingAreaID }).Select(g => new Dashboardchartdata
              {

                  name = g.Max(x => x.c.AreaName),
                  y = g.Count()


              }).ToList();

            }
            else
            {
                var adminuserid = ctx.User.Where(b => b.Role == "admin").Select(x =>

                  x.Id

             ).FirstOrDefault();
                ListDash = ctx.Instrument.Join(ctx.TestingArea, b => b.testingArea.TestingAreaID, c => c.TestingAreaID, (b, c) => new { b, c })
                    .Where(g => g.b.UserId == userid || g.b.UserId==adminuserid ||g.b.Isapprove==true)
              .GroupBy(x => new { x.b.testingArea.TestingAreaID }).Select(g => new Dashboardchartdata
              {

                  name = g.Max(x => x.c.AreaName),
                  y = g.Count()


              }).ToList();

            }
            return ListDash;
        }
        public IList<Dashboardchartdata> Getnooftestperarea(int userid)
        {
            List<Dashboardchartdata> ListDash = new List<Dashboardchartdata>();
            //var list = ctx.Test.Join(ctx.TestingArea, b => b.TestingAreaID, c => c.TestingAreaID, (b, c) => new { b, c })
            //  .GroupBy(x => new { x.b.TestingAreaID }).Select(g => new 
            //  {

            //      name = g.Max(x => x.c.AreaName),
            //      y = g.Count()


            //  }).ToList();


            var Roles = ctx.User.Where(b => b.Id == userid).Select(x => x.Role).FirstOrDefault();
            if (Roles == "admin")
            {
                ListDash = ctx.Test.Join(ctx.TestingArea, b => b.TestingAreaID, c => c.TestingAreaID, (b, c) => new { b, c })
              .GroupBy(x => new { x.b.TestingAreaID }).Select(g => new Dashboardchartdata
              {

                  name = g.Max(x => x.c.AreaName),
                  y = g.Count()


              }).ToList();

            }
            else
            {
                var adminuserid = ctx.User.Where(b => b.Role == "admin").Select(x =>

                 x.Id

            ).FirstOrDefault();
                ListDash = ctx.Test.Join(ctx.TestingArea, b => b.TestingAreaID, c => c.TestingAreaID, (b, c) => new { b, c })
                    .Where(g => g.b.UserId == userid || g.b.UserId == adminuserid || g.b.Isapprove == true)
              .GroupBy(x => new { x.b.TestingAreaID }).Select(g => new Dashboardchartdata
              {

                  name = g.Max(x => x.c.AreaName),
                  y = g.Count()


              }).ToList();

            }
            return ListDash;
        }
        public IList<Dashboardchartdata> Getnoofproductpertype(int userid)
        {
            List<Dashboardchartdata> ListDash = new List<Dashboardchartdata>();
            //   var list = ctx.MasterProduct.Join(ctx.ProductType, b => b.ProductTypeId, c => c.TypeID, (b, c) => new { b, c })
            //.GroupBy(x => new { x.b.ProductTypeId }).Select(g => new Dashboardchartdata
            //{

            //    name = g.Max(x => x.c.TypeName),
            //    y = g.Count()

            //}).ToList();


            var Roles = ctx.User.Where(b => b.Id == userid).Select(x => x.Role).FirstOrDefault();
            if (Roles == "admin")
            {
                ListDash = ctx.MasterProduct.Join(ctx.ProductType, b => b.ProductTypeId, c => c.TypeID, (b, c) => new { b, c })
         .GroupBy(x => new { x.b.ProductTypeId }).Select(g => new Dashboardchartdata
         {

             name = g.Max(x => x.c.TypeName),
             y = g.Count()

         }).ToList();

            }
            else
            {
                ListDash = ctx.MasterProduct.Join(ctx.ProductType, b => b.ProductTypeId, c => c.TypeID, (b, c) => new { b, c })
         .Where(g => g.b.UserId == userid)
                    .GroupBy(x => new { x.b.ProductTypeId }).Select(g => new Dashboardchartdata
                    {

                        name = g.Max(x => x.c.TypeName),
                        y = g.Count()

                    }).ToList();

            }
            return ListDash;


        }
        public Array gettstname(int id)
        {
            Array A;
            A = ctx.ForecastedTestByTest.Where(b => b.ForeCastID == id).Join(ctx.Test, c => c.Tst, d => d.TestID, (c, d) => new { c, d }).GroupBy(g => g.c.Tst).Select(x => x.Max(f => f.d.TestName)).ToArray();
            return A;
        }
        public Array getproducttype(int id)
        {
            var producttype = ctx.ForecastedResult.Where(b => b.ForecastId == id && b.TotalValue != 0).GroupBy(g => g.ProductTypeId).Select(x => x.Max(g => g.ProductType)).ToArray();
            return producttype;
        }
        public Array Getmonthbyforecast(int id)
        {
            Array A;
            A = ctx.PatientNumberDetail.Where(b => b.ForeCastId == id).GroupBy(g => new { g.Columnname }).OrderBy(f => f.Max(g => g.ID)).Select(x => x.Key).ToArray();
            return A;
        }
        public IList<Dashboardchartdata> Getnoofsitespercategory(int id)
        {
            List<Dashboardchartdata> list = new List<Dashboardchartdata>();
            if (id != 0)
            {
                list = ctx.Site.Join(ctx.SiteCategory, b => b.CategoryID, c => c.CategoryID, (b, c) => new { b, c })
                    .Join(ctx.Country, d => d.b.CountryId, e => e.Id, (d, e) => new { d, e })
                    .Where(g => g.e.Id == id)
         .GroupBy(x => new { x.d.b.CategoryID }).Select(g => new Dashboardchartdata
         {

             name = g.Max(x => x.d.c.CategoryName),
             y = g.Count()

         }).ToList();
            }
            else
            {
                list = ctx.Site.Join(ctx.SiteCategory, b => b.CategoryID, c => c.CategoryID, (b, c) => new { b, c })
            .GroupBy(x => new { x.b.CategoryID }).Select(g => new Dashboardchartdata
            {

                name = g.Max(x => x.c.CategoryName),
                y = g.Count()

            }).ToList();
            }
            return list;
        }

        public IList<Dashboardchartdata> Getratiobytestarea(int id)
        {
            DataTable Ratiotestarea = new DataTable();
            Ratiotestarea.Columns.Add("Areaname");
            Ratiotestarea.Columns.Add("Tst");
            List<Dashboardchartdata> Db = new List<Dashboardchartdata>();
            var testchart = ctx.ForecastedTestByTest.Join(ctx.Test, b => b.Tst, c => c.TestID, (b, c) => new { b, c })
                .Join(ctx.TestingArea, f => f.c.TestingAreaID, h => h.TestingAreaID, (f, h) => new { f, h })
                .Where(g => g.f.b.ForeCastID == id).GroupBy(j => j.h.TestingAreaID).Select(f => new
                {
                    Tst = f.Sum(s => s.f.b.TotalTst),
                    Areaname = f.Max(s => s.h.AreaName)
                }).ToList();
            for (int i = 0; i < testchart.Count; i++)
            {
                DataRow Dr = Ratiotestarea.NewRow();
                Dr["Areaname"] = testchart[i].Areaname;
                Dr["Tst"] = testchart[i].Tst;
                Ratiotestarea.Rows.Add(Dr);

            }
            Ratiotestarea.Columns.Add("Percentage");

            if (Ratiotestarea.Rows.Count >= 1)
            {
                int tot = 0;
                for (int l2 = 0; l2 < Ratiotestarea.Rows.Count; l2++)
                {
                    if (!string.IsNullOrEmpty(Ratiotestarea.Rows[l2][1].ToString()))
                    {
                        tot = tot + Convert.ToInt32(Math.Round(Convert.ToDecimal(Ratiotestarea.Rows[l2][1].ToString())));
                    }

                }
                for (int l2 = 0; l2 < Ratiotestarea.Rows.Count; l2++)
                {
                    if (tot != 0)
                    {
                        Ratiotestarea.Rows[l2][2] = Math.Round(Math.Round(Convert.ToDecimal(Ratiotestarea.Rows[l2][1])) * 100 / tot, 2);

                    }
                }
                // chart26.DataSource = TestArea;
            }

            DataView dv = Ratiotestarea.DefaultView;
            dv.Sort = "Areaname asc";
            DataTable sortedDT = dv.ToTable();
            for (int i = 0; i < sortedDT.Rows.Count; i++)
            {
                if (sortedDT.Rows[i][2].ToString() != "")
                {
                    Db.Add(new Dashboardchartdata
                    {
                        name = sortedDT.Rows[i][0].ToString(),
                        y = Convert.ToInt32(Math.Round(Convert.ToDecimal(sortedDT.Rows[i][2].ToString())))

                    });
                }

            }
            return Db;
        }
        public IList<Dashboard> GetChartProductprice(int id)
        {
            DataTable DT = new DataTable();


            Calculateproductprice(id);

            List<forecastsitedata> FS = new List<forecastsitedata>();
            var forecastinfo = ctx.ForecastInfo.Find(id);
            if (forecastinfo.ForecastType == "S")
            {
                FS = ctx.ForecastSiteInfo.Join(ctx.Site, b => b.SiteID, c => c.SiteID, (b, c) => new { b, c }).Where(x => x.b.ForecastinfoID == id).Select(g => new forecastsitedata
                {
                    name = g.c.SiteName,
                    id = g.c.SiteID
                }).ToList();
            }
            else
            {
                FS = ctx.ForecastCategoryInfo.Join(ctx.SiteCategory, b => b.SiteCategoryId, c => c.CategoryID, (b, c) => new { b, c }).Where(x => x.b.ForecastinfoID == id).Select(g => new forecastsitedata
                {
                    name = g.c.CategoryName,
                    id = g.c.CategoryID
                }).ToList();
            }
            //  var getallcategory = ctx.SiteCategory.ToList();


            var producttype = ctx.ForecastedResult.Where(b => b.ForecastId == id && b.TotalValue != 0).GroupBy(g => g.ProductTypeId).Select(x => x.Key).ToList();
            int[] data1 = new int[producttype.Count];
            List<Dashboard> Db = new List<Dashboard>();

            var list = ctx.ForecastedResult

       .Where(x => x.ForecastId == id && x.TotalValue != 0)
       .GroupBy(s => new { s.SiteId, s.ProductTypeId, s.ProductId })
       .Select(n => new
       {
           n.Key.ProductTypeId,
           n.Key.SiteId,
           totalcost = Math.Round(Math.Ceiling(n.Sum(s => s.TotalValue) / n.Max(s => s.PackQty)) * n.Max(s => s.PackPrice), 2),
           productname = n.Max(s => s.ProductId)
       }).ToList();


            var list1 = list.GroupBy(s => new { s.SiteId, s.ProductTypeId }).Select(n => new
            {

                ProductTypeId = n.Key.ProductTypeId,
                SiteId = n.Key.SiteId,
                totalcost = n.Sum(x => x.totalcost)
            });

            for (int j = 0; j < FS.Count; j++)
            {

                data1 = new int[producttype.Count];
                for (int i = 0; i < producttype.Count; i++)
                {
                    data1[i] = list1.Where(b => b.SiteId == FS[j].id && b.ProductTypeId == producttype[i]).Select(c => Convert.ToInt32(c.totalcost)).FirstOrDefault();
                }

                Db.Add(new Dashboard
                {

                    name = FS[j].name,
                    data = data1

                });



            }

           ;


            return Db;


        }
        private void Calculateproductprice(int id)
        {
            decimal existingpatient = 0;
            decimal testPermonth;
            decimal totaltest;
            decimal totaltest2 = 0;
            string[] siteids;
            decimal getworkingdays = 0;
            decimal existingtestnumber = 0;
            decimal TotalTestPerYear = 0;
            decimal tttest = 0;
            List<Dashboardchartdata> Db = new List<Dashboardchartdata>();

            List<Productsummary> Productsummary = new List<Productsummary>();
            var _forecastinfo = ctx.ForecastInfo.Find(id);


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




                for (int i = 0; i < testbymonth.Count; i++)
                {


                    TotalTestPerYear = Convert.ToDecimal(ctx.TestingProtocol.Where(b => b.ForecastinfoID == id && b.PatientGroupID == testbymonth[i].PGrp && b.TestID == testbymonth[i].TestID).Select(x => x.TotalTestPerYear).FirstOrDefault());
                    existingtestnumber = testbymonth[i].ExistingPatient * (TotalTestPerYear / 12);



                    testPermonth = Convert.ToDecimal(testbymonth[i].TstNo) * Convert.ToDecimal(testbymonth[i].NewPatient);
                    totaltest2 = totaltest2 + testPermonth;
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
                    var product1 = ctx.ProductUsage.Where(x => x.TestId == testbymonth[i].TestID && x.IsForControl == false).ToList();



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
                            quantity = getproductusagequantity(x.g.e.b.Rate, Math.Round(totaltest, 2), (ctx.ForecastIns.Where(b => b.InsID == x.g.e.b.InstrumentId ).Select(s => s.TestRunPercentage).FirstOrDefault())),//&& siteids.Contains(Convert.ToString(b.SiteID))
                            cost = x.h.Price,
                            packsize = x.h.PackSize,
                            Isforcontol=false,
                            Isforconsumble=false


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
                             packsize = x.h.PackSize,
                               Isforcontol = true,
                             Isforconsumble = false


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
                                quantity = Getconsumablequantity(x.j.g.e.b, siteids, testbymonth[i].Duration, getworkingdays, Math.Round(totaltest, 2), ctx.ForecastIns.Where(b => b.InsID == x.j.g.e.b.InstrumentId ).FirstOrDefault() != null ? ctx.ForecastIns.Where(b => b.InsID == x.j.g.e.b.InstrumentId).Average(s => s.Quantity) : 0, occurrenceofsamemonth),
                                cost = x.k.Price,
                                packsize = x.k.PackSize,
                                Isforcontol = false,
                                Isforconsumble = true


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
                        Sno = testbymonth[i].SNo,

                        Isforconsumble=  x.Max(s => s.Isforconsumble),
                        Isforcontol=  x.Max(s => s.Isforcontol),

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
                            sno = a.Sno,
                            isforcontrol=a.Isforcontol,
                            isforconumbles=a.Isforconsumble

                        });

                    }



                }
            }
            var productsummary1 = Productsummary.GroupBy(x => new { x.sitecategoryid, x.productid, x.sno }).Select(x => new
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
                sno = x.Key.sno,
                isforcontrol= x.Max(s => s.isforcontrol),
                isforconsumble = x.Max(s => s.isforconumbles),
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
                FR.IsForControl = productsummary1[i].isforcontrol;
                FR.IsForGeneralConsumable = productsummary1[i].isforconsumble;
                ctx.ForecastedResult.Add(FR);
                ctx.SaveChanges();


            }


            // var res = ctx.ForecastedResult

            //.Where(x => x.ForecastId == _forecastinfo.ForecastID)
            //.GroupBy(s => s.ProductTypeId)
            //.Select(n => new
            //{

            //    totalcost = (n.Sum(s => s.TotalValue) / n.Max(s => s.PackQty)) * n.Max(s => s.PackPrice),
            //    productname = n.Max(s => s.ProductType)
            //}).ToList();
            // res = res.OrderBy(s => s.productname).ToList();
            // for (int i = 0; i < res.Count; i++)
            // {
            //     Db.Add(new Dashboardchartdata
            //     {
            //         name = res[i].productname,
            //         y = Convert.ToInt32(res[i].totalcost)

            //     });
            // }

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
        private class getprod
        {
            public int typeid { get; set; }
            public string typename { get; set; }
            public decimal totalcost { get; set; }
        }
        private void calculation(double testnumber1, int testid, string Month, int j, int period, int id)
        {
            var getforecastinfo = ctx.ForecastInfo.Where(b => b.ForecastID == id).Select(g => new
            {
                ForecastType = g.ForecastType == null ? "" : g.ForecastType,
                Title = g.ForecastNo,
                programid = g.ProgramId

            }).FirstOrDefault();
            DataTable DTProductneed = new DataTable();


            DataTable dtPParameterValue = new DataTable();
            DataTable dtGetValue = new DataTable();
            dtGetValue.Columns.Add("VariableName", typeof(string));
            dtGetValue.Columns.Add("VariableEffect", typeof(string));
            dtGetValue.Columns.Add("VariableDataType", typeof(string));


            string[] arrids;

            var _mPMGeneralAssumption = ctx.MMGeneralAssumption.Where(b => b.Entity_type_id == 4 && b.ProgramId == getforecastinfo.programid).ToList();



            foreach (MMGeneralAssumption gAssumption in _mPMGeneralAssumption)
            {

                DataRow dr = dtGetValue.NewRow();

                dr["VariableName"] = gAssumption.VariableName.ToString();
                dr["VariableEffect"] = gAssumption.VariableEffect.ToString();
                dr["VariableDataType"] = gAssumption.VariableDataType.ToString();
                dtGetValue.Rows.Add(dr);

                dtGetValue.AcceptChanges();
            }

            double siteworkingdays = 0;
            if (getforecastinfo.ForecastType == "S")
            {

                siteworkingdays = ctx.ForecastSiteInfo.Join(ctx.Site, b => b.SiteID, c => c.SiteID, (b, c) => new { b, c }).Where(g => g.b.ForecastinfoID == id).Average(f => f.c.WorkingDays);


            }
            else
            {
                siteworkingdays = ctx.ForecastCategorySiteInfo.Join(ctx.Site, b => b.SiteID, c => c.SiteID, (b, c) => new { b, c }).Where(g => g.b.ForecastInfoID == id).Average(f => f.c.WorkingDays);

            }
            if (getforecastinfo.ForecastType == "S")
            {
                var q1 = ctx.ForecastSiteInfo.Where(b => b.ForecastinfoID == id).Select(c => c.SiteID).ToList();
                arrids = new string[q1.Count];
                for (int i = 0; i < q1.Count; i++)
                {
                    arrids[i] = q1[i].ToString();
                }
            }
            else
            {
                var q1 = ctx.ForecastCategorySiteInfo.Where(b => b.ForecastInfoID == id).Select(c => c.SiteID).ToList();
                arrids = new string[q1.Count];
                for (int i = 0; i < q1.Count; i++)
                {
                    arrids[i] = q1[i].ToString();
                }
            }
            int _period = (period * 12) / 3;

            ///for product Usage
            var q2 = ctx.siteinstrument.Join(ctx.Instrument, b => b.InstrumentID, c => c.InstrumentID, (b, c) => new { b, c })
                .Join(ctx.TestingArea, g => g.c.testingArea.TestingAreaID, f => f.TestingAreaID, (g, f) => new { g, f })
                .Where(h => arrids.Contains(Convert.ToString(h.g.b.SiteID))).Select(k => new
                {
                    SiteID = k.g.b.SiteID,
                    testingArea = k.f.AreaName,
                    instrumentname = k.g.c.InstrumentName,
                    Quantity = k.g.b.Quantity,
                    Percentage = k.g.b.TestRunPercentage,
                    InstrumentID = k.g.b.InstrumentID,
                    TestingAreaID = k.f.TestingAreaID

                }).ToList();

            var q3 = q2.GroupBy(g => new { g.TestingAreaID, g.InstrumentID }).Select(h => new
            {
                testingArea = h.Max(x => x.testingArea),
                instrumentname = h.Max(x => x.instrumentname),
                Quantity = h.Average(x => x.Quantity),
                Percentage = h.Average(x => x.Percentage),
                InstrumentID = h.Key.InstrumentID,
                TestingAreaID = h.Key.TestingAreaID
            }).ToList();

            string[] insids;

            insids = new string[q3.Count];


            for (int i = 0; i < q3.Count; i++)
            {
                insids[i] = q3[i].InstrumentID.ToString();
            }


            //main query
            var q4 = ctx.ProductUsage.Join(ctx.MasterProduct, b => b.ProductId, c => c.ProductID, (b, c) => new { b, c })
                .Join(q3, k => k.b.InstrumentId, h => h.InstrumentID, (k, h) => new { k, h })
               .Where(t => insids.Contains(Convert.ToString(t.k.b.InstrumentId)) && t.k.b.TestId == testid && t.k.b.IsForControl == false).Select(x => new
               {

                   ProductId = x.k.b.ProductId,
                   ProductName = x.k.c.ProductName,
                   TestId = x.k.b.TestId,
                   testnumber = testnumber1,
                   Productneed = ((x.k.b.Rate * Convert.ToDecimal(0.28)) * x.h.Percentage / 100),
                   month1 = Month

               }).ToList();


            ///for Control Usage
            var q5 = ctx.ProductUsage.Join(ctx.Instrument, b => b.InstrumentId, c => c.InstrumentID, (b, c) => new { b, c })
                 .Join(ctx.MasterProduct, f => f.b.ProductId, h => h.ProductID, (f, h) => new { f, h }).Where(g => g.f.b.IsForControl == true && g.f.b.TestId == testid)
                 .Select(x => new
                 {
                     ProductId = x.f.b.ProductId,
                     ProductName = x.h.ProductName,
                     TestId = x.f.b.TestId,
                     testnumber = testnumber1,
                     Productneed = getproductneed(x.f.c.DailyCtrlTest, x.f.c.MaxTestBeforeCtrlTest, x.f.c.WeeklyCtrlTest, x.f.c.MonthlyCtrlTest, x.f.c.QuarterlyCtrlTest, x.f.b.Rate, siteworkingdays),
                     month1 = Month
                 }).ToList();


            ///for Consumable Usage
            ///

            var q6 = ctx.ConsumableUsage.Join(ctx.MasterConsumable, b => b.ConsumableId, c => c.MasterCID, (b, c) => new { b, c })
                .Join(ctx.MasterProduct, g => g.b.ProductId, h => h.ProductID, (g, h) => new { g, h })
                .Join(ctx.Instrument, k => k.g.b.InstrumentId, l => l.InstrumentID, (k, l) => new { k, l })
                .Where(s => s.k.g.c.TestId == testid).Select(x => new
                {
                    ProductId = x.k.g.b.ProductId,
                    ProductName = x.k.h.ProductName,
                    TestId = x.k.g.c.TestId,
                    testnumber = testnumber1,
                    Productneed = getconsumableproductneed(x.k.g.b.UsageRate, x.k.g.b.NoOfTest, x.k.g.b.PerTest, period, _period, testnumber1, x.k.g.b.Period, siteworkingdays, arrids, x.l.InstrumentID),
                    month1 = Month
                }).ToList();
            List<decimal> dynamicparameter = new List<decimal>();
            var testingprotocol1 = ctx.MMGeneralAssumption.Where(b => b.ProgramId == getforecastinfo.programid && b.Entity_type_id == 4).ToList();

            var q7 = q4.Union(q5).Union(q6).Join(ctx.MasterProduct, b => b.ProductId, c => c.ProductID, (b, c) => new { b, c })
            .Join(ctx.ProductType, f => f.c.ProductTypeId, g => g.TypeID, (f, g) => new { f, g })
            .Join(ctx.ProductPrice, l => l.f.b.ProductId, k => k.ProductId, (l, k) => new { l, k }).ToList();

            //for (int k = 0; k < q7.Count; k++)
            //{
            //    for (int i = 0; i < testingprotocol1.Count; i++)
            //    {

            //        var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.ProductTypeID == q7[i].l.g.TypeID && b.Forecastid == id && b.Parameterid == testingprotocol1[i].Id).FirstOrDefault();

            //        if (MMGeneralAssumptionvalue != null)
            //            dynamicparameter.Add(MMGeneralAssumptionvalue.Parametervalue);
            //        //  Dr[testingprotocol1[i].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
            //        else
            //            dynamicparameter.Add(0);

            //    }
            //}


            var final = q4.Union(q5).Union(q6).Join(ctx.MasterProduct, b => b.ProductId, c => c.ProductID, (b, c) => new { b, c })
                    .Join(ctx.ProductType, f => f.c.ProductTypeId, g => g.TypeID, (f, g) => new { f, g })
                    .Join(ctx.ProductPrice, l => l.f.b.ProductId, k => k.ProductId, (l, k) => new { l, k })
                        .GroupBy(g => new { g.l.f.b.month1, g.l.f.c.ProductID }).Select(x => new
                        {
                            ProductId = x.Key.ProductID,
                            TypeID = x.Max(s => s.l.g.TypeID),
                            TypeName = x.Max(s => s.l.g.TypeName),
                            ProductName = x.Max(s => s.l.f.c.ProductName),
                            month1 = x.Key.month1,
                            testnumber = x.Sum(s => s.l.f.b.testnumber),
                            Productneed = x.Sum(s => s.l.f.b.Productneed),
                            Packsize = x.Max(s => s.k.PackSize),
                            Price = x.Max(s => s.k.Price)
                        }).ToList();

            DTProductneed.Columns.Add("ProductId");
            DTProductneed.Columns.Add("TypeID");
            DTProductneed.Columns.Add("TypeName");
            DTProductneed.Columns.Add("ProductName");
            DTProductneed.Columns.Add("month1");
            DTProductneed.Columns.Add("testnumber");
            DTProductneed.Columns.Add("Productneed");
            DTProductneed.Columns.Add("Packsize");
            DTProductneed.Columns.Add("Price");
            decimal per = 0;
            decimal num = 0;
            for (int i = 0; i < final.Count; i++)
            {
                DataRow Dr = DTProductneed.NewRow();
                Dr["ProductId"] = final[i].ProductId;
                Dr["TypeID"] = final[i].TypeID;
                Dr["TypeName"] = final[i].TypeName;
                Dr["ProductName"] = final[i].ProductName;
                Dr["month1"] = final[i].month1;
                Dr["testnumber"] = final[i].testnumber;
                Dr["Productneed"] = final[i].Productneed;
                Dr["Packsize"] = final[i].Packsize;
                Dr["Price"] = final[i].Price;

                for (int l = 0; l < testingprotocol1.Count; l++)
                {

                    var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.ProductTypeID == final[i].TypeID && b.Forecastid == id && b.Parameterid == testingprotocol1[l].Id).FirstOrDefault();

                    if (MMGeneralAssumptionvalue != null)
                    {
                        if (testingprotocol1[l].VariableEffect == true && testingprotocol1[l].VariableDataType == 2)
                        {
                            per = per + MMGeneralAssumptionvalue.Parametervalue;
                        }
                        else if (testingprotocol1[l].VariableEffect == false && testingprotocol1[l].VariableDataType == 2)
                        {
                            per = per - MMGeneralAssumptionvalue.Parametervalue;
                        }
                        else if (testingprotocol1[l].VariableEffect == true && testingprotocol1[l].VariableDataType == 1)
                        {
                            num = num + MMGeneralAssumptionvalue.Parametervalue;
                        }
                        else if (testingprotocol1[l].VariableEffect == false && testingprotocol1[l].VariableDataType == 1)
                        {
                            num = num - MMGeneralAssumptionvalue.Parametervalue;
                        }
                    }


                }

                DTProductneed.Rows.Add(Dr);
            }
            DTProductneed.Columns.Add("Adjustedpacksize", typeof(decimal));
            for (int i = 0; i < DTProductneed.Rows.Count; i++)
            {
                DTProductneed.Rows[i]["Adjustedpacksize"] = (Math.Ceiling(Convert.ToDecimal(DTProductneed.Rows[i]["Productneed"].ToString()) / Convert.ToDecimal(DTProductneed.Rows[i]["Packsize"].ToString())));
            }
            DTProductneed.Columns.Add("totalquantityinpack", typeof(decimal));
            for (int i = 0; i < DTProductneed.Rows.Count; i++)
            {
                DTProductneed.Rows[i]["totalquantityinpack"] = (Math.Ceiling(Convert.ToDecimal(DTProductneed.Rows[i]["Adjustedpacksize"].ToString()) + (Convert.ToDecimal(DTProductneed.Rows[i]["Adjustedpacksize"].ToString()) * (per / 100)) + (num)));
            }





            DTProductneed.Columns.Add("totalCost", typeof(decimal));
            for (int i4 = 0; i4 < DTProductneed.Rows.Count; i4++)
            {
                DTProductneed.Rows[i4]["totalCost"] = Math.Ceiling(Convert.ToDecimal(DTProductneed.Rows[i4]["totalquantityinpack"].ToString()) * Convert.ToDecimal(DTProductneed.Rows[i4]["Price"].ToString()));

            }


            if (j == 0)
            {
                DTProduct = DTProductneed;
            }
            else
            {

                foreach (DataRow dr in DTProductneed.Rows)
                {
                    DataRow[] rows = DTProduct.Select("ProductId='" + dr["ProductId"] + "' and month1='" + dr["month1"] + "' And TypeID='" + dr["TypeID"] + "'");
                    if (rows.Length > 0)
                    {
                        rows[0]["Productneed"] = Convert.ToDecimal(rows[0]["Productneed"]) + Convert.ToDecimal(dr["Productneed"]);
                        rows[0]["totalquantityinpack"] = Convert.ToDecimal(rows[0]["totalquantityinpack"]) + Convert.ToDecimal(dr["totalquantityinpack"]);
                        rows[0]["totalCost"] = Convert.ToDecimal(rows[0]["totalCost"]) + Convert.ToDecimal(dr["totalCost"]);
                    }
                    else
                    {
                        DTProduct.ImportRow(dr);
                    }
                }
            }
            // return DTProduct;

        }
        private decimal getconsumableproductneed(decimal Usagerate, int NoOfTest, bool Pertest, int period, int _period, double testnumber, string period1, double workingdays, string[] siteids, int insid)
        {
            decimal productneed;





            if (Pertest == true && testnumber > 0)
            {
                productneed = (Convert.ToDecimal(testnumber) / NoOfTest) * Usagerate;
            }
            else if (period1 == "Daily")
            {
                productneed = 1 * Convert.ToDecimal(workingdays) * Usagerate;
            }
            else if (period1 == "Yearly")
            {
                productneed = period * Usagerate;
            }
            else if (period1 == "Quarterly")
            {
                productneed = _period * Usagerate;
            }
            else if (period1 == "Weekly")
            {
                productneed = 1 * 4 * Convert.ToDecimal(ctx.siteinstrument.Where(b => siteids.Contains(Convert.ToString(b.SiteID)) && b.InstrumentID == insid).Average(c => c.Quantity));
            }
            else
            {
                productneed = 0;
            }
            return productneed;
        }
        private decimal getproductneed(int DailyCtrlTest, int MaxTestBeforeCtrlTest, int WeeklyCtrlTest, int MonthlyCtrlTest, int QuarterlyCtrlTest, decimal Rate, double workingdays)
        {
            decimal productneed;
            if (DailyCtrlTest > 0)
            {
                productneed = Convert.ToDecimal(workingdays) * 1 * Rate * DailyCtrlTest;
            }
            else if (WeeklyCtrlTest > 0)
            {
                productneed = 1 * 4 * Rate * WeeklyCtrlTest;
            }
            else if (MonthlyCtrlTest > 0)
            {
                productneed = 1 * Rate * MonthlyCtrlTest;
            }
            else if (QuarterlyCtrlTest > 0)
            {
                productneed = (1 / 4) * Rate * QuarterlyCtrlTest;
            }
            else
            {
                productneed = 0;
            }
            return productneed;
        }
        public IList<Dashboardchartdata> GetChartPatient(int id)
        {
            var patientdetail = ctx.PatientNumberDetail.Where(b => b.ForeCastId == id).Select(g => new Dashboardchartdata
            {
                name = g.Columnname,
                y = Convert.ToInt32(g.Serial)
            }).ToList();
            return patientdetail;
        }
        public IList<Dashboard> GetChartNooftestpertest(int id)
        {
            DataTable DT = new DataTable();

            fillChartTectNoByTest(id);

            List<forecastsitedata> FS = new List<forecastsitedata>();
            var forecastinfo = ctx.ForecastInfo.Find(id);
            if (forecastinfo.ForecastType == "S")
            {
                FS = ctx.ForecastSiteInfo.Join(ctx.Site, b => b.SiteID, c => c.SiteID, (b, c) => new { b, c }).Where(x => x.b.ForecastinfoID == id).Select(g => new forecastsitedata
                {
                    name = g.c.SiteName,
                    id = g.c.SiteID
                }).ToList();
            }
            else
            {
                FS = ctx.ForecastCategoryInfo.Join(ctx.SiteCategory, b => b.SiteCategoryId, c => c.CategoryID, (b, c) => new { b, c }).Where(x => x.b.ForecastinfoID == id).Select(g => new forecastsitedata
                {
                    name = g.c.CategoryName,
                    id = g.c.CategoryID
                }).ToList();
            }
            //  var getallcategory = ctx.SiteCategory.ToList();


            var test = ctx.ForecastedTestByTest.Where(b => b.ForeCastID == id).GroupBy(g => g.Tst).Select(x => x.Key).ToList();
            int[] data1 = new int[test.Count];
            List<Dashboard> Db = new List<Dashboard>();
            var list = ctx.ForecastedTestByTest.Join(ctx.Test, b => b.Tst, c => c.TestID, (b, c) => new { b, c }).Where(g => g.b.ForeCastID == id).GroupBy(x => new { x.c.TestID, x.b.sitecategoryid }).Select(f => new
            {
                f.Key.TestID,
                f.Key.sitecategoryid,
                Tst = f.Sum(s => s.b.TotalTst),
                TestName = f.Max(s => s.c.TestName)
            }).ToList();

            //  getallcategory = getallcategory.Where(b => list.Select(c => c.categoryID).Contains(b.CategoryID)).ToList();
            for (int j = 0; j < FS.Count; j++)
            {

                data1 = new int[test.Count];
                for (int i = 0; i < test.Count; i++)
                {
                    data1[i] = list.Where(b => b.sitecategoryid == FS[j].id && b.TestID == test[i]).Select(c => Convert.ToInt32(c.Tst)).FirstOrDefault();
                }

                Db.Add(new Dashboard
                {

                    name = FS[j].name,
                    data = data1

                });



            }

           ;


            return Db;

        }
        private void fillChartTectNoByTest12(int id)
        {
            DataTable period = new DataTable();
            DataTable months = new DataTable();
            months.Columns.Add("columnname");
            months.Columns.Add("serial");
            //     DataTable Duration = new DataTable();
            DataTable Percentage = new DataTable();
            Percentage.Columns.Add("id");
            Percentage.Columns.Add("patientgroupname");
            Percentage.Columns.Add("patientpercentage");

            double mmyear;

            DataTable TestingProtocal = new DataTable();
            DataTable Calculate = new DataTable();
            DataTable TestName = new DataTable();
            DataTable TestArea = new DataTable();
            DataTable ratiowise = new DataTable();
            DataTable TestChart = new DataTable();
            DataTable BaseLine = new DataTable();
            DataTable yr = new DataTable(); DataTable margins = new DataTable();
            margins.Columns.Add("num");
            margins.Columns.Add("valu");


            DataTable Percentag = new DataTable();
            Percentag.Columns.Add("testn");
            Percentag.Columns.Add("pgrpid");
            Percentag.Columns.Add("pernew");
            Percentag.Columns.Add("perold");
            Percentag.Columns.Add("TotalTestPerYear");


            DataTable TestNoChart = new DataTable();
            DataTable TestByMonth = new System.Data.DataTable(); TestByMonth.Columns.Add("FOreCastID"); TestByMonth.Columns.Add("TestID");
            TestByMonth.Columns.Add("Month"); TestByMonth.Columns.Add("Tests"); TestByMonth.Columns.Add("PGrp"); TestByMonth.Columns.Add("SNo");
            TestByMonth.Columns.Add("NewPatient"); TestByMonth.Columns.Add("TestPerYear");

            var forecastinfo = ctx.ForecastInfo.Where(b => b.ForecastID == id).FirstOrDefault();

            var mmprogram = ctx.MMProgram.Where(b => b.Id == forecastinfo.ProgramId).FirstOrDefault();
            var MMGeneralAssumption1 = ctx.MMGeneralAssumption.Where(b => b.Entity_type_id == 5).ToList();


            var testmonth = ctx.TestByMonth.Where(b => b.ForeCastID == id).ToList();
            ctx.TestByMonth.RemoveRange(testmonth);
            ctx.SaveChanges();

            var mMGAbyType = MMGeneralAssumption1.Where(c => !c.VariableName.Contains("Mon") && c.ProgramId == forecastinfo.ProgramId).ToList();
            for (int i = 0; i < mMGAbyType.Count; i++)
            {
                TestByMonth.Columns.Add(mMGAbyType[i].VariableName);
            }

            var forecasttestbytest = ctx.ForecastedTestByTest.Where(c => c.ForeCastID == id).ToList();
            ctx.ForecastedTestByTest.RemoveRange(forecasttestbytest);
            ctx.SaveChanges();
            int[] sitecategoryids;


            if (forecastinfo.ForecastType == "S")
            {

                sitecategoryids = ctx.ForecastSiteInfo.Where(b => b.ForecastinfoID == id).Select(x => x.SiteID).ToArray();
            }
            else
            {
                sitecategoryids = ctx.ForecastCategoryInfo.Where(b => b.ForecastinfoID == id).Select(x => x.SiteCategoryId).ToArray();
            }

            for (int z = 0; z < sitecategoryids.Length; z++)
            {


                var months1 = ctx.PatientNumberDetail.Where(c => c.ForeCastId == id && c.SiteCategoryId == sitecategoryids[z]).ToList();

                for (int i = 0; i < months1.Count; i++)
                {
                    DataRow Dr = months.NewRow();
                    Dr["columnname"] = months1[i].Columnname;
                    Dr["serial"] = months1[i].Serial;
                    months.Rows.Add(Dr);
                }
                if (months.Rows.Count < 12)
                {
                    mmyear = Convert.ToDouble(Convert.ToDecimal(months.Rows.Count) / 12);
                }
                else
                {
                    mmyear = mmprogram.NoofYear;
                }
                double yrs = 0;
                if (forecastinfo.Period == "Monthly")
                {
                    yrs = Convert.ToDouble((Convert.ToDecimal(months.Rows.Count - 1) * 1) / 12);
                }
                if (forecastinfo.Period == "Bimonthly")
                {
                    yrs = Convert.ToDouble((Convert.ToDecimal(months.Rows.Count - 1) * 2) / 12);
                }
                if (forecastinfo.Period == "Quarterly")
                {
                    yrs = Convert.ToDouble((Convert.ToDecimal(months.Rows.Count - 1) * 4) / 12);
                }
                if (z == 0)
                {
                    months.Columns.Add("NewPatient");
                }

                var Duration = ctx.PatientNumberDetail.Where(c => c.ForeCastId == id && c.SiteCategoryId == sitecategoryids[z]).Count();
                for (int i = 0; i < Duration - 1; i++)
                {
                    months.Rows[i + 1][2] = (Convert.ToDecimal(months.Rows[i + 1][1]) - Convert.ToDecimal(months.Rows[i][1])).ToString();

                }
                var patientgroup = ctx.PatientGroup.Where(c => c.ForecastinfoID == id).ToList();

                for (int i = 0; i < patientgroup.Count; i++)
                {
                    DataRow Dr = Percentage.NewRow();
                    Dr["id"] = patientgroup[i].GroupID;
                    Dr["patientgroupname"] = patientgroup[i].PatientGroupName;
                    Dr["patientpercentage"] = patientgroup[i].PatientPercentage;
                    Percentage.Rows.Add(Dr);
                }

                decimal newP = 0;
                if (months.Rows.Count > 1)
                    newP = Convert.ToDecimal(months.Rows[1][2]);
                else newP = 0;
                decimal OldP = Convert.ToDecimal(months.Rows[0][1]);
                if (z == 0)
                {
                    Percentage.Columns.Add("PercentageCal");
                    Percentage.Columns.Add("PercentageCalOld");
                }
                for (int i1 = 0; i1 < Percentage.Rows.Count; i1++)
                {
                    Percentage.Rows[i1][3] = ((Convert.ToDecimal(Percentage.Rows[i1][2]) * newP) / 100).ToString();
                    Percentage.Rows[i1][4] = ((Convert.ToDecimal(Percentage.Rows[i1][2]) * OldP) / 100).ToString();
                }
                TestingProtocal = GettestAssumption(forecastinfo, sitecategoryids[z]);


                if (TestingProtocal.Rows.Count > 0)
                {

                    TestingProtocal.Columns.Add("Percentage1"); TestingProtocal.Columns.Add("Percentageold");

                    for (int i2 = 0; i2 < Percentage.Rows.Count; i2++)
                    {
                        for (int i3 = 0; i3 < TestingProtocal.Rows.Count; i3++)
                        {
                            if (Convert.ToInt32(Percentage.Rows[i2][0]) == Convert.ToInt32(TestingProtocal.Rows[i3][1]))
                            {

                                TestingProtocal.Rows[i3]["Percentage1"] = (Convert.ToDecimal(Convert.ToDecimal(Percentage.Rows[i2][3]) * Convert.ToDecimal(TestingProtocal.Rows[i3]["PercentagePanel"])) / 100).ToString();
                                TestingProtocal.Rows[i3]["Percentageold"] = Convert.ToDecimal(Percentage.Rows[i2][4]).ToString();

                            }

                        }
                    }

                    if (z == 0)
                    {
                        TestChart.Columns.Add("Total");
                        TestChart.Columns.Add("ForeCastId");
                        TestChart.Columns.Add("BaseLine");
                        TestChart.Columns.Add("TestID");
                        TestChart.Columns.Add("PatientGroupID");
                        TestChart.Columns.Add("TestPerYear");
                        foreach (var item in mMGAbyType)
                        {
                            TestChart.Columns.Add(item.VariableName);
                        }


                        //for (int k4 = 1; k4 < Convert.ToInt32(Duration.Rows[0][0]); k4++)

                        for (int k4 = 1; k4 <= yrs * 12; k4++)
                        {
                            TestChart.Columns.Add(k4.ToString());
                        }

                    }
                    TestingProtocal.Columns.Add("Total0"); TestingProtocal.Columns.Add("TotalNo"); TestingProtocal.Columns.Add("TotalGap");
                    var per1 = ctx.PercentageVal.ToList();
                    ctx.PercentageVal.RemoveRange(per1);
                    ctx.SaveChanges();




                    for (int k3 = 0; k3 < TestingProtocal.Rows.Count; k3++)
                    {
                        int k6 = 0;
                        for (int k5 = 0; k5 < mmyear * 12; k5++) //here 12 replce with number oy years
                        {
                            if (TestingProtocal.Rows[k3][7 + k5].ToString() == "0")
                            {
                                k6 = k6 + 1;
                            }
                        }
                        int no = 0;
                        TestingProtocal.Rows[k3]["Total0"] = k6;//replace 24 and 25 with name
                        TestingProtocal.Rows[k3]["TotalNo"] = mmyear * 12 - k6;//here 12 replce with number oy years
                        if (Convert.ToInt32(TestingProtocal.Rows[k3]["TotalNo"]) > 0)//replace 24 and 25 with name
                        {
                            no = 12 / Convert.ToInt32(TestingProtocal.Rows[k3]["TotalNo"]);//here 12 replce with number oy years
                        }
                        TestingProtocal.Rows[k3]["TotalGap"] = no.ToString();//replace0 26 with name

                        string _inserValuesPV = "";


                        _inserValuesPV += "'" + TestingProtocal.Rows[k3][0].ToString().Trim() + "','" + TestingProtocal.Rows[k3][1].ToString().Trim() + "','" + TestingProtocal.Rows[k3]["Percentage1"].ToString().Trim() + "','" + TestingProtocal.Rows[k3]["Percentageold"].ToString().Trim() + "','" + TestingProtocal.Rows[k3]["TotalTestPerYear"].ToString().Trim() + "' ,";
                        PercentageVal PV = new PercentageVal();
                        PV.TestN = Convert.ToInt32(TestingProtocal.Rows[k3][0].ToString().Trim());
                        PV.PGrpID = Convert.ToInt32(TestingProtocal.Rows[k3][1].ToString().Trim());
                        PV.PerNew = TestingProtocal.Rows[k3]["Percentage1"].ToString() == "" ? 0 : Convert.ToDecimal(TestingProtocal.Rows[k3]["Percentage1"].ToString());
                        PV.PerOld = TestingProtocal.Rows[k3]["Percentageold"].ToString() == "" ? 0 : Convert.ToDecimal(TestingProtocal.Rows[k3]["Percentageold"].ToString());
                        PV.TotalTestPerYear = Convert.ToInt32(TestingProtocal.Rows[k3]["TotalTestPerYear"].ToString().Trim());

                        PV.sitecategoryid = sitecategoryids[z];
                        ctx.PercentageVal.Add(PV);
                        ctx.SaveChanges();
                    }
                    if (z == 0)
                    {
                        BaseLine.Columns.Add("Test"); BaseLine.Columns.Add("PGrp"); BaseLine.Columns.Add("No"); BaseLine.Columns.Add("Value");
                    }

                    for (int k9 = 0; k9 < TestingProtocal.Rows.Count; k9++)
                    {
                        for (int k8 = 0; k8 < mmyear * 12; k8++)  //rename 12 with no of year
                        {

                            if (Convert.ToInt32(TestingProtocal.Rows[k9][k8 + 7]) != 0)
                            {
                                BaseLine.Rows.Add();
                                int a1 = BaseLine.Rows.Count;
                                BaseLine.Rows[a1 - 1][0] = TestingProtocal.Rows[k9][0];
                                BaseLine.Rows[a1 - 1][1] = TestingProtocal.Rows[k9][1];

                                BaseLine.Rows[a1 - 1][2] = k8 + 1;
                                BaseLine.Rows[a1 - 1][3] = TestingProtocal.Rows[k9][k8 + 7].ToString();

                            }

                        }
                    }
                    var tem = ctx.Temptbl1.ToList();
                    ctx.Temptbl1.RemoveRange(tem);
                    ctx.SaveChanges();

                    for (int i = 0; i < BaseLine.Rows.Count; i++)
                    {
                        Temptbl1 Tb = new Temptbl1();
                        Tb.Tst = Convert.ToInt32(BaseLine.Rows[i][0].ToString().Trim());
                        Tb.PGrp = Convert.ToInt32(BaseLine.Rows[i][1].ToString().Trim());
                        Tb.Num = Convert.ToInt32(BaseLine.Rows[i][2].ToString().Trim());
                        Tb.Valu = Convert.ToInt32(BaseLine.Rows[i][3].ToString().Trim());
                        Tb.sitecategoryid = sitecategoryids[z];
                        ctx.Temptbl1.Add(Tb);
                        ctx.SaveChanges();

                    }
                    var testingprotocol = MMGeneralAssumption1.Where(c => c.VariableName.Contains("Mon")).ToList();
                    var testingprotocol1 = MMGeneralAssumption1.Where(c => !c.VariableName.Contains("Mon") && c.ProgramId == forecastinfo.ProgramId).ToList();
                    if (z == 0)
                    {
                        yr.Columns.Add("Baseline");


                        if (mmyear == 2)
                        {
                            for (int i = 0; i < 24; i++)
                            {
                                yr.Columns.Add(testingprotocol[i].VariableName, typeof(Int32));
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 12; i++)
                            {
                                yr.Columns.Add(testingprotocol[i].VariableName, typeof(Int32));
                            }
                        }
                        yr.Columns.Add("TotalTestPerYear", typeof(Int32));

                        for (int i = 0; i < testingprotocol1.Count; i++)
                        {
                            yr.Columns.Add(testingprotocol1[i].VariableName, typeof(Int32));
                        }



                        for (int i = 0; i < testingprotocol1.Count; i++)
                        {
                            Percentag.Columns.Add(testingprotocol1[i].VariableName, typeof(Int32));
                        }
                    }

                    ////delete test by month
                    for (int k3 = 0; k3 < TestingProtocal.Rows.Count; k3++)
                    {
                        TestChart.Clear(); TestByMonth.Clear(); margins.Clear(); Percentag.Clear();





                        var TestingprotocolEnt = ctx.TestingProtocol.Where(b => b.TestID == Convert.ToInt32(TestingProtocal.Rows[k3][0]) && b.ForecastinfoID == id && b.PatientGroupID == Convert.ToInt32(TestingProtocal.Rows[k3][1])).FirstOrDefault();

                        DataRow Dr = yr.NewRow();

                        Dr["Baseline"] = TestingprotocolEnt.Baseline;
                        if (mmyear == 2)
                        {
                            for (int i = 0; i < 24; i++)
                            {
                                var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == Convert.ToInt32(TestingProtocal.Rows[k3][0]) && b.Forecastid == id && b.Parameterid == testingprotocol[i].Id && b.PatientGroupID == Convert.ToInt32(TestingProtocal.Rows[k3][1])).FirstOrDefault();

                                if (MMGeneralAssumptionvalue != null)
                                    Dr[testingprotocol[i].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                                else
                                    Dr[testingprotocol[i].VariableName] = 0;


                            }
                        }
                        else
                        {
                            for (int i = 0; i < mmyear * 12; i++)
                            {
                                var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == Convert.ToInt32(TestingProtocal.Rows[k3][0]) && b.Forecastid == id && b.Parameterid == testingprotocol[i].Id && b.PatientGroupID == Convert.ToInt32(TestingProtocal.Rows[k3][1])).FirstOrDefault();

                                if (MMGeneralAssumptionvalue != null)
                                    Dr[testingprotocol[i].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                                else
                                    Dr[testingprotocol[i].VariableName] = 0;
                            }
                        }
                        Dr["TotalTestPerYear"] = TestingprotocolEnt.TotalTestPerYear;


                        for (int i = 0; i < testingprotocol1.Count; i++)
                        {

                            var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == Convert.ToInt32(TestingProtocal.Rows[k3][0]) && b.Forecastid == id && b.Parameterid == testingprotocol1[i].Id && b.PatientGroupID == Convert.ToInt32(TestingProtocal.Rows[k3][1])).FirstOrDefault();

                            if (MMGeneralAssumptionvalue != null)
                                Dr[testingprotocol1[i].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                            else
                                Dr[testingprotocol1[i].VariableName] = 0;

                        }
                        yr.Rows.Add(Dr);



                        var margin1 = ctx.Temptbl1.Where(c => c.Tst == Convert.ToInt32(TestingProtocal.Rows[k3][0]) && c.PGrp == Convert.ToInt32(TestingProtocal.Rows[k3][1])).ToList();
                        //Changes in dynamic month with Parameter
                        for (int i = 0; i < margin1.Count; i++)
                        {
                            DataRow Dr1 = margins.NewRow();
                            Dr1["num"] = margin1[i].Num;
                            Dr1["valu"] = margin1[i].Valu;
                            margins.Rows.Add(Dr1);
                        }

                        //  int y = yrs * 12 / Convert.ToInt32(yr.Rows[0][0]);

                        for (int k7 = 0; k7 < mmyear * 12; k7++) //no of year in 12  
                        {
                            TestChart.Rows.Add();
                            TestChart.Rows[k7][1] = id.ToString();
                            TestChart.Rows[k7][2] = TestingProtocal.Rows[k3][6].ToString();
                            TestChart.Rows[k7][3] = TestingProtocal.Rows[k3][0].ToString();
                            TestChart.Rows[k7][4] = TestingProtocal.Rows[k3][1].ToString();
                            TestChart.Rows[k7][5] = TestingProtocal.Rows[k3][3].ToString();
                            TestChart.Rows[k7][6] = TestingProtocal.Rows[k3][4].ToString();
                            foreach (var item in mMGAbyType)
                            {
                                TestChart.Rows[k7][item.VariableName] = TestingProtocal.Rows[k3][item.VariableName].ToString();
                            }
                            //TestChart.Rows[k7][6] = TestingProtocal.Rows[k3][4].ToString();
                            //TestChart.Rows[k7][7] = TestingProtocal.Rows[k3][5].ToString();

                            int columnIndex = TestChart.Columns.IndexOf("1");

                            for (int k07 = 0; k07 < yrs * 12; k07++)
                            {
                                if (k07 == 0 && k7 == 0)
                                {
                                    TestChart.Rows[k7][columnIndex + k07] = Convert.ToInt32(yr.Rows[k3][0]) + Convert.ToInt32(yr.Rows[k3][1]);
                                }
                                else if (k07 != 0 && k7 == 0)
                                {
                                    if (k07 < 12)
                                    {
                                        if (yr.Rows[k3][k07 + 1] != System.DBNull.Value)
                                        {
                                            TestChart.Rows[k7][columnIndex + k07] = Convert.ToInt32(yr.Rows[k3][k07 + 1]);
                                        }
                                    }
                                    else
                                    {
                                        int kj = k07 % 12;
                                        TestChart.Rows[k7][columnIndex + k07] = Convert.ToInt32(yr.Rows[0][kj + 1]);
                                    }
                                }
                                else
                                {
                                    if (columnIndex + k07 + k7 < TestChart.Columns.Count)
                                    {
                                        TestChart.Rows[k7][columnIndex + k07 + k7] = TestChart.Rows[k7 - 1][columnIndex + k07 + k7 - 1];
                                    }

                                }
                            }


                            int tot = 0;
                            for (int jj = 0; jj < TestChart.Columns.Count - columnIndex; jj++)
                            {
                                if (!string.IsNullOrEmpty(TestChart.Rows[k7][jj + columnIndex].ToString()))
                                {
                                    tot = tot + Convert.ToInt32(TestChart.Rows[k7][jj + columnIndex]);
                                }

                            }
                            TestChart.Rows[k7][0] = tot;


                        }
                        int totalTest = 0; decimal aAmount = 0;//, per=0;
                        for (int jj = 0; jj < TestChart.Rows.Count; jj++)
                        {

                            totalTest = totalTest + Convert.ToInt32(TestChart.Rows[jj][0]);

                        }

                        var perc = ctx.PercentageVal.Where(c => c.TestN == Convert.ToInt32(TestChart.Rows[0][3]) && c.PGrpID == Convert.ToInt32(TestChart.Rows[0][4])).ToList();


                        for (int j = 0; j < perc.Count; j++)
                        {

                            DataRow Dr3 = Percentag.NewRow();
                            Dr3["testn"] = perc[j].TestN;
                            Dr3["pgrpid"] = perc[j].PGrpID;
                            Dr3["pernew"] = perc[j].PerNew;
                            Dr3["perold"] = perc[j].PerOld;
                            Dr3["TotalTestPerYear"] = perc[j].TotalTestPerYear;
                            for (int i = 0; i < testingprotocol1.Count; i++)
                            {

                                var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == perc[j].TestN && b.Forecastid == id && b.Parameterid == testingprotocol1[i].Id && b.PatientGroupID == perc[j].PGrpID).FirstOrDefault();

                                if (MMGeneralAssumptionvalue != null)
                                    Dr3[testingprotocol1[i].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                                else
                                    Dr3[testingprotocol1[i].VariableName] = 0;

                            }
                            Percentag.Rows.Add(Dr3);
                        }
                        //if (_variableName != "") _query = "Select testn,pgrpid,pernew,perold,TotalTestPerYear ," + _variableName.TrimEnd(',') + "from percentageval where testn='" + TestChart.Rows[0][3] + "' and pgrpid='" + TestChart.Rows[0][4] + "'";
                        //else if (_variableName == "") _query = "Select testn,pgrpid,pernew,perold,TotalTestPerYear from percentageval where testn='" + TestChart.Rows[0][3] + "' and pgrpid='" + TestChart.Rows[0][4] + "'";
                        //SqlDataAdapter adapt13 = new SqlDataAdapter(_query, con);

                        //Changes parameter according to MMProgram

                        decimal totalcal = (totalTest * Convert.ToDecimal(Percentag.Rows[0][2])) + (Convert.ToDecimal(Percentag.Rows[0][3]) * (Convert.ToDecimal(yrs) * Convert.ToDecimal(Percentag.Rows[0][4])));
                        //Decimal per = Convert.ToDecimal(Percentag.Rows[0][5]) + Convert.ToDecimal(Percentag.Rows[0][6]);
                        Decimal per = 0;
                        for (int ki = 0; ki < Percentag.Rows.Count; ki++)
                        {
                            foreach (DataColumn dc in Percentag.Columns)
                            {
                                foreach (MMGeneralAssumption MM in testingprotocol1)
                                {
                                    string v1 = MM.VariableName;
                                    string v2 = dc.ColumnName.ToString();
                                    if (v1.Equals(v2))
                                    {
                                        //bool variableEffect = gAssumption.Where(x => x.VariableName == item.ToString()).SingleOrDefault().VariableEffect;
                                        //int variableDatatype = _mMGeneralAssumption.Where(x => x.VariableName == item.ToString()).SingleOrDefault().VariableDataType;
                                        //if(variableDatatype==2)aAmount=aAmount+(newpatient*(Convert.ToDecimal(dtValue.Rows[ki][""+item.ToString()+""]))/100);
                                        //else if (variableDatatype == 2) aAmount = aAmount + (newpatient * (Convert.ToDecimal(dtValue.Rows[ki]["" + item.ToString() + ""])) / 100);
                                        decimal vvalue = Convert.ToDecimal(Percentag.Rows[ki]["" + v2 + ""]);
                                        aAmount = ((vvalue) / 100);
                                        if (MM.VariableEffect == true) per = per + aAmount;
                                        if (MM.VariableEffect == false) per = per - aAmount;

                                    }

                                }
                            }

                        }
                        //check parameter should add or subtract

                        decimal finalCal = totalcal + (totalcal * per);  //correct
                        ForecastedTestByTest FT = new ForecastedTestByTest();
                        FT.ForeCastID = id;
                        FT.Tst = Convert.ToInt32(TestChart.Rows[0][3].ToString().Trim());
                        FT.PGrp = Convert.ToInt32(TestChart.Rows[0][4].ToString().Trim());
                        FT.TotalTst = finalCal;
                        FT.sitecategoryid = sitecategoryids[z];
                        ctx.ForecastedTestByTest.Add(FT);
                        ctx.SaveChanges();

                        int columnIndex1 = TestChart.Columns.IndexOf("1");
                        var startmonth = forecastinfo.StartDate.Month;

                        for (int i = 0; i < TestChart.Columns.Count - columnIndex1; i++)
                        {
                            int tot = 0; int j = 0;
                            TestByMonth.Rows.Add();
                            TestByMonth.Rows[i][0] = TestChart.Rows[0][1];
                            TestByMonth.Rows[i][1] = TestChart.Rows[0][3];
                            if (mmyear > 1)
                            {
                                for (j = 0; j < 12; j++)
                                {
                                    if (!string.IsNullOrEmpty(TestChart.Rows[j][i + columnIndex1].ToString()))
                                    {
                                        tot = tot + Convert.ToInt32(TestChart.Rows[j][i + columnIndex1]);
                                    }
                                    TestByMonth.Rows[i][7] = TestChart.Rows[j][5];
                                    foreach (var item in mMGAbyType)
                                    {
                                        TestByMonth.Rows[i][item.VariableName] = TestChart.Rows[j][item.VariableName];
                                    }
                                    //TestByMonth.Rows[i][8] = TestChart.Rows[j][6];
                                    //TestByMonth.Rows[i][9] = TestChart.Rows[j][7];
                                }
                            }
                            else
                            {
                                for (j = 0; j < 11; j++)
                                {
                                    if (!string.IsNullOrEmpty(TestChart.Rows[j][i + columnIndex1].ToString()))
                                    {
                                        tot = tot + Convert.ToInt32(TestChart.Rows[j][i + columnIndex1]);
                                    }
                                    TestByMonth.Rows[i][7] = TestChart.Rows[j][5];
                                    foreach (var item in mMGAbyType)
                                    {
                                        TestByMonth.Rows[i][item.VariableName] = TestChart.Rows[j][item.VariableName];
                                    }
                                    //TestByMonth.Rows[i][8] = TestChart.Rows[j][6];
                                    //TestByMonth.Rows[i][9] = TestChart.Rows[j][7];
                                }
                            }


                            string mnth = "";
                            if ((startmonth) % 12 == 1)
                            {
                                mnth = "Jan";
                            }
                            if ((startmonth) % 12 == 2)
                            {
                                mnth = "Feb";
                            }
                            if ((startmonth) % 12 == 3)
                            {
                                mnth = "Mar";
                            }
                            if ((startmonth) % 12 == 4)
                            {
                                mnth = "Apr";
                            }
                            if ((startmonth) % 12 == 5)
                            {
                                mnth = "May";
                            }
                            if ((startmonth) % 12 == 6)
                            {
                                mnth = "Jun";
                            }
                            if ((startmonth) % 12 == 7)
                            {
                                mnth = "Jul";
                            }
                            if ((startmonth) % 12 == 8)
                            {
                                mnth = "Aug";
                            }
                            if ((startmonth) % 12 == 9)
                            {
                                mnth = "Sep";
                            }
                            if ((startmonth) % 12 == 10)
                            {
                                mnth = "Oct";
                            }
                            if ((startmonth) % 12 == 11)
                            {
                                mnth = "Nov";
                            }
                            if ((startmonth) % 12 == 0)
                            {
                                mnth = "Dec";
                            }
                            string m = months.Rows[0][0].ToString();

                            string ys = (m.Length > 3) ? m.Substring(m.Length - 4, 4) : m;
                            string years = "";
                            if (i + 1 <= 12)
                            {
                                years = ys;
                            }

                            if (i + 1 > 12 && i + 1 <= 24)
                            {
                                years = (Convert.ToInt32(ys) + 1).ToString();
                            }
                            if (i + 1 > 24 && i + 1 <= 36)
                            {
                                years = (Convert.ToInt32(ys) + 2).ToString();
                            }
                            if (i + 1 > 36 && i + 1 <= 48)
                            {
                                years = (Convert.ToInt32(ys) + 3).ToString();
                            }
                            if (i + 1 > 48 && i + 1 <= 60)
                            {
                                years = (Convert.ToInt32(ys) + 4).ToString();
                            }


                            TestByMonth.Rows[i][2] = string.Concat(mnth, " ", years);
                            TestByMonth.Rows[i][3] = tot.ToString();
                            TestByMonth.Rows[i][4] = TestChart.Rows[0][4];
                            TestByMonth.Rows[i][5] = (Convert.ToInt32(i) + 1).ToString();
                            startmonth++;

                        }


                        for (int ij1 = 0; ij1 < TestByMonth.Rows.Count; ij1++)
                        {


                            string _inserValuesTBM = "";

                            _inserValuesTBM += "'" + TestByMonth.Rows[ij1][0].ToString().Trim() + "','" + TestByMonth.Rows[ij1][1].ToString().Trim() + "','" + TestByMonth.Rows[ij1][2].ToString().Trim() + "','" + TestByMonth.Rows[ij1][3].ToString().Trim() + "','" + TestByMonth.Rows[ij1][4].ToString().Trim() + "','" + TestByMonth.Rows[ij1][5].ToString() + "'," + TestingProtocal.Rows[k3]["Percentage1"].ToString() + "," + TestByMonth.Rows[ij1][7].ToString() + " ,";
                            foreach (var item in mMGAbyType)
                            {
                                _inserValuesTBM += "" + TestingProtocal.Rows[k3][item.VariableName].ToString().Trim() + " ,";
                            }
                            _inserValuesTBM += "" + TestingProtocal.Rows[k3]["Percentageold"].ToString().Trim() + "," + yrs + "";

                            TestByMonth TM = new TestByMonth();
                            TM.ForeCastID = Convert.ToInt32(TestByMonth.Rows[ij1][0].ToString());
                            TM.TestID = Convert.ToInt32(TestByMonth.Rows[ij1][1].ToString());
                            TM.Month = TestByMonth.Rows[ij1][2].ToString();
                            TM.TstNo = Convert.ToDecimal(TestByMonth.Rows[ij1][3].ToString());


                            TM.PGrp = Convert.ToInt32(TestByMonth.Rows[ij1][4].ToString());
                            TM.SNo = Convert.ToInt32(TestByMonth.Rows[ij1][5].ToString());
                            TM.NewPatient = Convert.ToDecimal(TestingProtocal.Rows[k3]["Percentage1"].ToString());
                            TM.TotalTestPerYear = Convert.ToInt32(TestByMonth.Rows[ij1][7].ToString());
                            TM.ExistingPatient = Convert.ToDecimal(TestingProtocal.Rows[k3]["Percentageold"].ToString());
                            TM.Duration = Convert.ToDecimal(yrs);
                            TM.sitecategoryid = sitecategoryids[z];
                            ctx.TestByMonth.Add(TM);
                            ctx.SaveChanges();
                            //if (_variableName != "") _query1 = "insert into Testbymonth (ForeCastID, TestID, Month,TstNo,PGrp,SNo,NewPatient,TotalTestPerYear," + _variableName.TrimEnd(',') + ",ExistingPatient,Duration) values(" + _inserValuesTBM.TrimEnd(',') + ")";
                            //else if (_variableName == "") _query1 = "insert into Testbymonth (ForeCastID, TestID, Month,TstNo,PGrp,SNo,NewPatient,TotalTestPerYear,ExistingPatient,Duration) values(" + _inserValuesTBM.TrimEnd(',') + ")";
                            ////cm.CommandText = "insert into Testbymonth (ForeCastID, TestID, Month,TstNo,PGrp,SNo,NewPatient,TotalTestPerYear," + _variableName.TrimEnd(',') + ",ExistingPatient,Duration) values(" + _inserValuesTBM.TrimEnd(',') + ")";
                            //cm.CommandText = _query1;

                            //here we should add name TestPerYear in place of TestByMonth.Rows[ij1][7],RepeatTest in place of TestByMonth.Rows[ij1][8] and Symptomtest in place of TestByMonth.Rows[ij1][9]
                            //also create column according to Program in Testbymonth table




                            //cmd1.CommandText = "insert into PercentageVal (TestN, PGrpID, PerNew,PerOld,TotalTestPerYear," + _columnName.TrimEnd(',') + ") values(" + _inserValuesPV + ")";


                        }

                    }


                    //if (z == 0)
                    //{

                    //    TestNoChart.Columns.Add("TestName");
                    //    TestNoChart.Columns.Add("tst");
                    //}

                }
                months.Clear();
            }


            //var testchart = ctx.ForecastedTestByTest.Join(ctx.Test, b => b.Tst, c => c.TestID, (b, c) => new { b, c }).Where(g => g.b.ForeCastID == id).GroupBy(x => x.c.TestID).Select(f => new
            //{
            //    Tst = f.Sum(s => s.b.TotalTst),
            //    TestName = f.Max(s => s.c.TestName)
            //}).ToList();

            //for (int i = 0; i < testchart.Count; i++)
            //{
            //    DataRow Dr = TestNoChart.NewRow();
            //    Dr["TestName"] = testchart[i].TestName;
            //    Dr["tst"] = testchart[i].Tst;
            //    TestNoChart.Rows.Add(Dr);

            //}

            //return TestNoChart;
        }
        private void fillChartTectNoByTest(int id)
        {
            DataTable period = new DataTable();
            DataTable months = new DataTable();
            months.Columns.Add("columnname");
            months.Columns.Add("serial");
            //     DataTable Duration = new DataTable();
            DataTable Percentage = new DataTable();
            Percentage.Columns.Add("id");
            Percentage.Columns.Add("patientgroupname");
            Percentage.Columns.Add("patientpercentage");



            DataTable TestingProtocal = new DataTable();
            DataTable Calculate = new DataTable();
            DataTable TestName = new DataTable();
            DataTable TestArea = new DataTable();
            DataTable ratiowise = new DataTable();
            DataTable TestChart = new DataTable();
            DataTable BaseLine = new DataTable();
            DataTable yr = new DataTable(); DataTable margins = new DataTable();
            margins.Columns.Add("num");
            margins.Columns.Add("valu");


            DataTable Percentag = new DataTable();
            Percentag.Columns.Add("testn");
            Percentag.Columns.Add("pgrpid");
            Percentag.Columns.Add("pernew");
            Percentag.Columns.Add("perold");
            Percentag.Columns.Add("TotalTestPerYear");


            DataTable TestNoChart = new DataTable();
            DataTable TestByMonth = new System.Data.DataTable(); TestByMonth.Columns.Add("FOreCastID"); TestByMonth.Columns.Add("TestID");
            TestByMonth.Columns.Add("Month"); TestByMonth.Columns.Add("Tests"); TestByMonth.Columns.Add("PGrp"); TestByMonth.Columns.Add("SNo"); TestByMonth.Columns.Add("NewPatient"); TestByMonth.Columns.Add("TestPerYear");

            var forecastinfo = ctx.ForecastInfo.Where(b => b.ForecastID == id).FirstOrDefault();
            var mmprogram = ctx.MMProgram.Where(b => b.Id == forecastinfo.ProgramId).FirstOrDefault();
            var MMGeneralAssumption1 = ctx.demographicMMGeneralAssumption.Where(b => b.Entity_type_id == 5 && b.Forecastid == id).ToList();


            var testmonth = ctx.TestByMonth.Where(b => b.ForeCastID == id).ToList();
            ctx.TestByMonth.RemoveRange(testmonth);
            ctx.SaveChanges();

            var mMGAbyType = MMGeneralAssumption1.Where(c => !c.VariableName.Contains("Mon") && c.Forecastid == id).ToList();
            for (int i = 0; i < mMGAbyType.Count; i++)
            {
                TestByMonth.Columns.Add(mMGAbyType[i].VariableName);
            }

            var forecasttestbytest = ctx.ForecastedTestByTest.Where(c => c.ForeCastID == id).ToList();
            ctx.ForecastedTestByTest.RemoveRange(forecasttestbytest);
            ctx.SaveChanges();
            int[] sitecategoryids;


            if (forecastinfo.ForecastType == "S")
            {

                sitecategoryids = ctx.ForecastSiteInfo.Where(b => b.ForecastinfoID == id).Select(x => x.SiteID).ToArray();
            }
            else
            {
                sitecategoryids = ctx.ForecastCategoryInfo.Where(b => b.ForecastinfoID == id).Select(x => x.SiteCategoryId).ToArray();
            }

            for (int z = 0; z < sitecategoryids.Length; z++)
            {
                var months1 = ctx.PatientNumberDetail.Where(c => c.ForeCastId == id && c.SiteCategoryId == sitecategoryids[z]).ToList();
                for (int i = 0; i < months1.Count; i++)
                {
                    DataRow Dr = months.NewRow();
                    Dr["columnname"] = months1[i].Columnname;
                    Dr["serial"] = months1[i].Serial;
                    months.Rows.Add(Dr);
                }
                //  int yrs = 0;
                //if (forecastinfo.Period == "Monthly")
                //{
                //    yrs = ((months.Rows.Count - 1) * 1) / 12;
                //}
                //if (forecastinfo.Period == "Bimonthly")
                //{
                //    yrs = ((months.Rows.Count - 1) * 2) / 12;
                //}
                //if (forecastinfo.Period == "Quarterly")
                //{
                //    yrs = ((months.Rows.Count - 1) * 4) / 12;
                //}
                double yrs;
                yrs = Convert.ToDouble((Convert.ToDecimal(months.Rows.Count)) / 12);
                if(yrs==0)
                {
                    switch (forecastinfo.months)
                    {
                        case 13:
                            yrs = 12 / 12;

                            break;
                        case 12:
                            yrs = 12 / 12;
                            break;
                        case 18:
                            yrs = 18 / 12;
                            break;
                        case 24:
                            yrs = 24 / 12;
                            break;
                    }

                }

                if (z == 0)
                {
                    months.Columns.Add("NewPatient");
                }

                var Duration = ctx.PatientNumberDetail.Where(c => c.ForeCastId == id && c.SiteCategoryId == sitecategoryids[z]).Count();
                for (int i = 0; i < Duration - 1; i++)
                {
                    months.Rows[i + 1][2] = (Convert.ToDecimal(months.Rows[i + 1][1]) - Convert.ToDecimal(months.Rows[i][1])).ToString();

                }
                var patientgroup = ctx.PatientGroup.Where(c => c.ForecastinfoID == id).ToList();

                for (int i = 0; i < patientgroup.Count; i++)
                {
                    DataRow Dr = Percentage.NewRow();
                    Dr["id"] = patientgroup[i].GroupID;
                    Dr["patientgroupname"] = patientgroup[i].PatientGroupName;
                    Dr["patientpercentage"] = patientgroup[i].PatientPercentage;
                    Percentage.Rows.Add(Dr);
                }

                decimal newP = 0;
                if (months.Rows.Count > 1)
                    newP = Convert.ToDecimal(months.Rows[1][2]);
                else newP = 0;
                decimal OldP;
                if (months.Rows.Count > 1)
                    OldP = Convert.ToDecimal(months.Rows[0][1]);
                else OldP = 0;

                if (z == 0)
                {
                    Percentage.Columns.Add("PercentageCal");
                    Percentage.Columns.Add("PercentageCalOld");
                }
                for (int i1 = 0; i1 < Percentage.Rows.Count; i1++)
                {
                    Percentage.Rows[i1][3] = ((Convert.ToDecimal(Percentage.Rows[i1][2]) * newP) / 100).ToString();
                    Percentage.Rows[i1][4] = ((Convert.ToDecimal(Percentage.Rows[i1][2]) * OldP) / 100).ToString();
                }
                TestingProtocal = GettestAssumption(forecastinfo, sitecategoryids[z]);
                if (TestingProtocal.Rows.Count > 0)
                {
                    TestingProtocal.Columns.Add("Percentage1"); TestingProtocal.Columns.Add("Percentageold");
                    for (int i2 = 0; i2 < Percentage.Rows.Count; i2++)
                    {
                        for (int i3 = 0; i3 < TestingProtocal.Rows.Count; i3++)
                        {
                            if (Convert.ToInt32(Percentage.Rows[i2][0]) == Convert.ToInt32(TestingProtocal.Rows[i3][1]))
                            {

                                TestingProtocal.Rows[i3]["Percentage1"] = (Convert.ToDecimal(Convert.ToDecimal(Percentage.Rows[i2][3]) * Convert.ToDecimal(TestingProtocal.Rows[i3]["PercentagePanel"])) / 100).ToString();
                                TestingProtocal.Rows[i3]["Percentageold"] = Convert.ToDecimal(Percentage.Rows[i2][4]).ToString();

                            }

                        }
                    }


                    if (TestChart.Columns.Count == 0)
                    {
                        TestChart.Columns.Add("Total");
                        TestChart.Columns.Add("ForeCastId");
                        TestChart.Columns.Add("BaseLine");
                        TestChart.Columns.Add("TestID");
                        TestChart.Columns.Add("PatientGroupID");
                        TestChart.Columns.Add("TestPerYear");
                        foreach (var item in mMGAbyType)
                        {
                            TestChart.Columns.Add(item.VariableName);
                        }
                        for (int k4 = 1; k4 <= yrs * 12; k4++)
                        {
                            TestChart.Columns.Add(k4.ToString());
                        }
                    }
                    TestingProtocal.Columns.Add("Total0"); TestingProtocal.Columns.Add("TotalNo"); TestingProtocal.Columns.Add("TotalGap");

                    if (z == 0)
                    {
                        //for (int k4 = 1; k4 < Convert.ToInt32(Duration.Rows[0][0]); k4++)

                    }

                    var per1 = ctx.PercentageVal.ToList();
                    ctx.PercentageVal.RemoveRange(per1);
                    ctx.SaveChanges();




                    for (int k3 = 0; k3 < TestingProtocal.Rows.Count; k3++)
                    {
                        int k6 = 0;
                        //for (int k5 = 0; k5 < mmprogram.NoofYear * 12; k5++) //here 12 replce with number oy years
                        //{
                        //    if (TestingProtocal.Rows[k3][7 + k5].ToString() == "0")
                        //    {
                        //        k6 = k6 + 1;
                        //    }
                        //}


                        switch (forecastinfo.months)
                        {
                            case 13:
                                for (int k5 = 0; k5 < 12; k5++)
                                {
                                    if (TestingProtocal.Rows[k3][7 + k5].ToString() == "0")
                                    {
                                        k6 = k6 + 1;
                                    }
                                }
                              
                                break;
                            case 12:
                                for (int k5 = 0; k5 < 12; k5++)
                                {
                                    if (TestingProtocal.Rows[k3][7 + k5].ToString() == "0")
                                    {
                                        k6 = k6 + 1;
                                    }
                                }
                                break;
                            case 18:
                                for (int k5 = 0; k5 < 18; k5++)
                                {
                                    if (TestingProtocal.Rows[k3][7 + k5].ToString() == "0")
                                    {
                                        k6 = k6 + 1;
                                    }
                                }
                                break;
                            case 24:
                                for (int k5 = 0; k5 < 24; k5++)
                                {
                                    if (TestingProtocal.Rows[k3][7 + k5].ToString() == "0")
                                    {
                                        k6 = k6 + 1;
                                    }
                                }
                                break;
                        }





                        int no = 0;
                        TestingProtocal.Rows[k3]["Total0"] = k6;//replace 24 and 25 with name
                        TestingProtocal.Rows[k3]["TotalNo"] = mmprogram.NoofYear * 12 - k6;//here 12 replce with number oy years
                        if (Convert.ToInt32(TestingProtocal.Rows[k3]["TotalNo"]) > 0)//replace 24 and 25 with name
                        {
                            no = 12 / Convert.ToInt32(TestingProtocal.Rows[k3]["TotalNo"]);//here 12 replce with number oy years
                        }
                        TestingProtocal.Rows[k3]["TotalGap"] = no.ToString();//replace0 26 with name

                        string _inserValuesPV = "";


                        _inserValuesPV += "'" + TestingProtocal.Rows[k3][0].ToString().Trim() + "','" + TestingProtocal.Rows[k3][1].ToString().Trim() + "','" + TestingProtocal.Rows[k3]["Percentage1"].ToString().Trim() + "','" + TestingProtocal.Rows[k3]["Percentageold"].ToString().Trim() + "','" + TestingProtocal.Rows[k3]["TotalTestPerYear"].ToString().Trim() + "' ,";
                        PercentageVal PV = new PercentageVal();
                        PV.TestN = Convert.ToInt32(TestingProtocal.Rows[k3][0].ToString().Trim());
                        PV.PGrpID = Convert.ToInt32(TestingProtocal.Rows[k3][1].ToString().Trim());
                        PV.PerNew = TestingProtocal.Rows[k3]["Percentage1"].ToString() == "" ? 0 : Convert.ToDecimal(TestingProtocal.Rows[k3]["Percentage1"].ToString());
                        PV.PerOld = TestingProtocal.Rows[k3]["Percentageold"].ToString() == "" ? 0 : Convert.ToDecimal(TestingProtocal.Rows[k3]["Percentageold"].ToString());
                        PV.TotalTestPerYear = Convert.ToInt32(TestingProtocal.Rows[k3]["TotalTestPerYear"].ToString().Trim());
                        ctx.PercentageVal.Add(PV);
                        ctx.SaveChanges();
                    }
                    if (BaseLine.Columns.Count == 0)
                    {
                        BaseLine.Columns.Add("Test"); BaseLine.Columns.Add("PGrp"); BaseLine.Columns.Add("No"); BaseLine.Columns.Add("Value");
                    }

                    for (int k9 = 0; k9 < TestingProtocal.Rows.Count; k9++)
                    {


                        switch (forecastinfo.months)
                        {
                            case 13:
                                for (int k8 = 0; k8 < 12; k8++)
                                {
                                    if (Convert.ToInt32(TestingProtocal.Rows[k9][k8 + 7]) != 0)
                                    {
                                        BaseLine.Rows.Add();
                                        int a1 = BaseLine.Rows.Count;
                                        BaseLine.Rows[a1 - 1][0] = TestingProtocal.Rows[k9][0];
                                        BaseLine.Rows[a1 - 1][1] = TestingProtocal.Rows[k9][1];

                                        BaseLine.Rows[a1 - 1][2] = k8 + 1;
                                        BaseLine.Rows[a1 - 1][3] = TestingProtocal.Rows[k9][k8 + 7].ToString();

                                    }
                                }

                                break;
                            case 12:
                                for (int k8 = 0; k8 < 12; k8++)
                                {
                                    if (Convert.ToInt32(TestingProtocal.Rows[k9][k8 + 7]) != 0)
                                    {
                                        BaseLine.Rows.Add();
                                        int a1 = BaseLine.Rows.Count;
                                        BaseLine.Rows[a1 - 1][0] = TestingProtocal.Rows[k9][0];
                                        BaseLine.Rows[a1 - 1][1] = TestingProtocal.Rows[k9][1];

                                        BaseLine.Rows[a1 - 1][2] = k8 + 1;
                                        BaseLine.Rows[a1 - 1][3] = TestingProtocal.Rows[k9][k8 + 7].ToString();

                                    }
                                }
                                break;
                            case 18:
                                for (int k8 = 0; k8 < 18; k8++)
                                {
                                    if (Convert.ToInt32(TestingProtocal.Rows[k9][k8 + 7]) != 0)
                                    {
                                        BaseLine.Rows.Add();
                                        int a1 = BaseLine.Rows.Count;
                                        BaseLine.Rows[a1 - 1][0] = TestingProtocal.Rows[k9][0];
                                        BaseLine.Rows[a1 - 1][1] = TestingProtocal.Rows[k9][1];

                                        BaseLine.Rows[a1 - 1][2] = k8 + 1;
                                        BaseLine.Rows[a1 - 1][3] = TestingProtocal.Rows[k9][k8 + 7].ToString();

                                    }
                                }
                                break;
                            case 24:
                                for (int k8 = 0; k8 < 24; k8++)
                                {
                                    if (Convert.ToInt32(TestingProtocal.Rows[k9][k8 + 7]) != 0)
                                    {
                                        BaseLine.Rows.Add();
                                        int a1 = BaseLine.Rows.Count;
                                        BaseLine.Rows[a1 - 1][0] = TestingProtocal.Rows[k9][0];
                                        BaseLine.Rows[a1 - 1][1] = TestingProtocal.Rows[k9][1];

                                        BaseLine.Rows[a1 - 1][2] = k8 + 1;
                                        BaseLine.Rows[a1 - 1][3] = TestingProtocal.Rows[k9][k8 + 7].ToString();

                                    }
                                }
                                break;
                        }

                        //for (int k8 = 0; k8 < mmprogram.NoofYear * 12; k8++)  //rename 12 with no of year
                        //{

                        //    if (Convert.ToInt32(TestingProtocal.Rows[k9][k8 + 7]) != 0)
                        //    {
                        //        BaseLine.Rows.Add();
                        //        int a1 = BaseLine.Rows.Count;
                        //        BaseLine.Rows[a1 - 1][0] = TestingProtocal.Rows[k9][0];
                        //        BaseLine.Rows[a1 - 1][1] = TestingProtocal.Rows[k9][1];

                        //        BaseLine.Rows[a1 - 1][2] = k8 + 1;
                        //        BaseLine.Rows[a1 - 1][3] = TestingProtocal.Rows[k9][k8 + 7].ToString();

                        //    }

                        //}
                    }
                    var tem = ctx.Temptbl1.ToList();
                    ctx.Temptbl1.RemoveRange(tem);
                    ctx.SaveChanges();

                    for (int i = 0; i < BaseLine.Rows.Count; i++)
                    {
                        Temptbl1 Tb = new Temptbl1();
                        Tb.Tst = Convert.ToInt32(BaseLine.Rows[i][0].ToString().Trim());
                        Tb.PGrp = Convert.ToInt32(BaseLine.Rows[i][1].ToString().Trim());
                        Tb.Num = Convert.ToInt32(BaseLine.Rows[i][2].ToString().Trim());
                        Tb.Valu = Convert.ToInt32(BaseLine.Rows[i][3].ToString().Trim());
                        Tb.sitecategoryid = sitecategoryids[z];
                        ctx.Temptbl1.Add(Tb);
                        ctx.SaveChanges();

                    }
                    var testingprotocol = MMGeneralAssumption1.Where(c => c.VariableName.Contains("Mon")).ToList();
                    var testingprotocol1 = MMGeneralAssumption1.Where(c => !c.VariableName.Contains("Mon") && c.Forecastid == forecastinfo.ForecastID).ToList();
                    if (yr.Columns.Count == 0)
                    {
                        yr.Columns.Add("Baseline");



                        switch (forecastinfo.months)
                        {
                            case 13:
                                for (int i = 0; i < 12; i++)
                                {
                                    yr.Columns.Add(testingprotocol[i].VariableName, typeof(Int32));
                                }
                                yr.Columns.Add("Total Test Per Year");
                                break;
                            case 12:
                                for (int i = 0; i < 12; i++)
                                {
                                    yr.Columns.Add(testingprotocol[i].VariableName, typeof(Int32));
                                }
                                break;
                            case 18:
                                for (int i = 0; i < 18; i++)
                                {
                                    yr.Columns.Add(testingprotocol[i].VariableName, typeof(Int32));
                                }
                                break;
                            case 24:
                                for (int i = 0; i < 24; i++)
                                {
                                    yr.Columns.Add(testingprotocol[i].VariableName, typeof(Int32));
                                }
                                break;
                        }

                        //if (mmprogram.NoofYear == 2)
                        //{

                        //}
                        //else
                        //{

                        //}
                        //yr.Columns.Add("TotalTestPerYear", typeof(Int32));

                        for (int i = 0; i < testingprotocol1.Count; i++)
                        {
                            yr.Columns.Add(testingprotocol1[i].VariableName, typeof(Int32));
                        }



                        for (int i = 0; i < testingprotocol1.Count; i++)
                        {
                            Percentag.Columns.Add(testingprotocol1[i].VariableName, typeof(Int32));
                        }


                    }
                    // yr.Clear();
                    ////delete test by month
                    for (int k3 = 0; k3 < TestingProtocal.Rows.Count; k3++)
                    {
                        TestChart.Clear(); TestByMonth.Clear(); margins.Clear(); Percentag.Clear();





                        var TestingprotocolEnt = ctx.TestingProtocol.Where(b => b.TestID == Convert.ToInt32(TestingProtocal.Rows[k3][0]) && b.ForecastinfoID == id && b.PatientGroupID == Convert.ToInt32(TestingProtocal.Rows[k3][1])).FirstOrDefault();

                        DataRow Dr = yr.NewRow();

                        Dr["Baseline"] = TestingprotocolEnt.Baseline;


                        switch (forecastinfo.months)
                        {
                            case 13:
                                for (int i = 0; i < 12; i++)
                                {
                                    var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == Convert.ToInt32(TestingProtocal.Rows[k3][0]) && b.Forecastid == id && b.Parameterid == testingprotocol[i].Id && b.PatientGroupID == Convert.ToInt32(TestingProtocal.Rows[k3][1])).FirstOrDefault();

                                    if (MMGeneralAssumptionvalue != null)
                                        Dr[testingprotocol[i].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                                    else
                                        Dr[testingprotocol[i].VariableName] = 0;

                                }
                                Dr["TotalTestPerYear"] = TestingprotocolEnt.TotalTestPerYear;
                                break;
                            case 12:
                                for (int i = 0; i < 12; i++)
                                {
                                    var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == Convert.ToInt32(TestingProtocal.Rows[k3][0]) && b.Forecastid == id && b.Parameterid == testingprotocol[i].Id && b.PatientGroupID == Convert.ToInt32(TestingProtocal.Rows[k3][1])).FirstOrDefault();

                                    if (MMGeneralAssumptionvalue != null)
                                        Dr[testingprotocol[i].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                                    else
                                        Dr[testingprotocol[i].VariableName] = 0;

                                }
                                break;
                            case 18:
                                for (int i = 0; i < 18; i++)
                                {
                                    var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == Convert.ToInt32(TestingProtocal.Rows[k3][0]) && b.Forecastid == id && b.Parameterid == testingprotocol[i].Id && b.PatientGroupID == Convert.ToInt32(TestingProtocal.Rows[k3][1])).FirstOrDefault();

                                    if (MMGeneralAssumptionvalue != null)
                                        Dr[testingprotocol[i].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                                    else
                                        Dr[testingprotocol[i].VariableName] = 0;

                                }
                                break;
                            case 24:
                                for (int i = 0; i < 24; i++)
                                {
                                    var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == Convert.ToInt32(TestingProtocal.Rows[k3][0]) && b.Forecastid == id && b.Parameterid == testingprotocol[i].Id && b.PatientGroupID == Convert.ToInt32(TestingProtocal.Rows[k3][1])).FirstOrDefault();

                                    if (MMGeneralAssumptionvalue != null)
                                        Dr[testingprotocol[i].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                                    else
                                        Dr[testingprotocol[i].VariableName] = 0;

                                }
                                break;
                        }


                        //if (mmprogram.NoofYear == 2)
                        //{
                        //    for (int i = 0; i < 24; i++)
                        //    {
                        //        var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == Convert.ToInt32(TestingProtocal.Rows[k3][0]) && b.Forecastid == id && b.Parameterid == testingprotocol[i].Id && b.PatientGroupID == Convert.ToInt32(TestingProtocal.Rows[k3][1])).FirstOrDefault();

                        //        if (MMGeneralAssumptionvalue != null)
                        //            Dr[testingprotocol[i].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                        //        else
                        //            Dr[testingprotocol[i].VariableName] = 0;


                        //    }
                        //}
                        //else
                        //{
                        //    for (int i = 0; i < 12; i++)
                        //    {
                        //        var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == Convert.ToInt32(TestingProtocal.Rows[k3][0]) && b.Forecastid == id && b.Parameterid == testingprotocol[i].Id && b.PatientGroupID == Convert.ToInt32(TestingProtocal.Rows[k3][1])).FirstOrDefault();

                        //        if (MMGeneralAssumptionvalue != null)
                        //            Dr[testingprotocol[i].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                        //        else
                        //            Dr[testingprotocol[i].VariableName] = 0;
                        //    }
                        //}
                        //Dr["TotalTestPerYear"] = TestingprotocolEnt.TotalTestPerYear;


                        for (int i = 0; i < testingprotocol1.Count; i++)
                        {

                            var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == Convert.ToInt32(TestingProtocal.Rows[k3][0]) && b.Forecastid == id && b.Parameterid == testingprotocol1[i].Id && b.PatientGroupID == Convert.ToInt32(TestingProtocal.Rows[k3][1])).FirstOrDefault();

                            if (MMGeneralAssumptionvalue != null)
                                Dr[testingprotocol1[i].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                            else
                                Dr[testingprotocol1[i].VariableName] = 0;

                        }
                        yr.Rows.Add(Dr);



                        var margin1 = ctx.Temptbl1.Where(c => c.Tst == Convert.ToInt32(TestingProtocal.Rows[k3][0]) && c.PGrp == Convert.ToInt32(TestingProtocal.Rows[k3][1])).ToList();
                        //Changes in dynamic month with Parameter
                        for (int i = 0; i < margin1.Count; i++)
                        {
                            DataRow Dr1 = margins.NewRow();
                            Dr1["num"] = margin1[i].Num;
                            Dr1["valu"] = margin1[i].Valu;
                            margins.Rows.Add(Dr1);
                        }

                        //  int y = yrs * 12 / Convert.ToInt32(yr.Rows[0][0]);


                        switch (forecastinfo.months)
                        {
                            case 13:
                                for (int k7 = 0; k7 < 12; k7++)
                                {
                                    TestChart.Rows.Add();
                                    TestChart.Rows[k7][1] = id.ToString();
                                    TestChart.Rows[k7][2] = TestingProtocal.Rows[k3][6].ToString();
                                    TestChart.Rows[k7][3] = TestingProtocal.Rows[k3][0].ToString();
                                    TestChart.Rows[k7][4] = TestingProtocal.Rows[k3][1].ToString();
                                    TestChart.Rows[k7][5] = TestingProtocal.Rows[k3][3].ToString();
                                    TestChart.Rows[k7][6] = TestingProtocal.Rows[k3][4].ToString();
                                    foreach (var item in mMGAbyType)
                                    {
                                        TestChart.Rows[k7][item.VariableName] = TestingProtocal.Rows[k3][item.VariableName].ToString();
                                    }
                                    //TestChart.Rows[k7][6] = TestingProtocal.Rows[k3][4].ToString();
                                    //TestChart.Rows[k7][7] = TestingProtocal.Rows[k3][5].ToString();

                                    int columnIndex = TestChart.Columns.IndexOf("1");
                                    int tot = 0;
                                    if (columnIndex >= 0)
                                    {
                                        for (int k07 = 0; k07 < yrs * 12; k07++)
                                        {
                                            if (k07 == 0 && k7 == 0)
                                            {
                                                TestChart.Rows[k7][columnIndex + k07] = Convert.ToInt32(yr.Rows[k3][0]) + Convert.ToInt32(yr.Rows[k3][1]);
                                            }
                                            else if (k07 != 0 && k7 == 0)
                                            {
                                                if (k07 < 12)
                                                {
                                                    TestChart.Rows[k7][columnIndex + k07] = Convert.ToInt32(yr.Rows[k3][k07 + 1]);
                                                }
                                                else
                                                {
                                                    int kj = k07 % 12;
                                                    TestChart.Rows[k7][columnIndex + k07] = Convert.ToInt32(yr.Rows[0][kj + 1]);
                                                }
                                            }
                                            else
                                            {
                                                if (columnIndex + k07 + k7 < TestChart.Columns.Count)
                                                {
                                                    TestChart.Rows[k7][columnIndex + k07 + k7] = TestChart.Rows[k7 - 1][columnIndex + k07 + k7 - 1];
                                                }

                                            }
                                        }



                                        for (int jj = 0; jj < TestChart.Columns.Count - columnIndex; jj++)
                                        {
                                            if (!string.IsNullOrEmpty(TestChart.Rows[k7][jj + columnIndex].ToString()))
                                            {
                                                tot = tot + Convert.ToInt32(TestChart.Rows[k7][jj + columnIndex]);
                                            }

                                        }
                                    }
                                    TestChart.Rows[k7][0] = tot;


                                }

                                break;
                            case 12:
                                for (int k7 = 0; k7 < 12; k7++)
                                {

                                    TestChart.Rows.Add();
                                    TestChart.Rows[k7][1] = id.ToString();
                                    TestChart.Rows[k7][2] = TestingProtocal.Rows[k3][6].ToString();
                                    TestChart.Rows[k7][3] = TestingProtocal.Rows[k3][0].ToString();
                                    TestChart.Rows[k7][4] = TestingProtocal.Rows[k3][1].ToString();
                                    TestChart.Rows[k7][5] = TestingProtocal.Rows[k3][3].ToString();
                                    TestChart.Rows[k7][6] = TestingProtocal.Rows[k3][4].ToString();
                                    foreach (var item in mMGAbyType)
                                    {
                                        TestChart.Rows[k7][item.VariableName] = TestingProtocal.Rows[k3][item.VariableName].ToString();
                                    }
                                    //TestChart.Rows[k7][6] = TestingProtocal.Rows[k3][4].ToString();
                                    //TestChart.Rows[k7][7] = TestingProtocal.Rows[k3][5].ToString();

                                    int columnIndex = TestChart.Columns.IndexOf("1");
                                    int tot = 0;
                                    if (columnIndex >= 0)
                                    {
                                        for (int k07 = 0; k07 < yrs * 12; k07++)
                                        {
                                            if (k07 == 0 && k7 == 0)
                                            {
                                                TestChart.Rows[k7][columnIndex + k07] = Convert.ToInt32(yr.Rows[k3][0]) + Convert.ToInt32(yr.Rows[k3][1]);
                                            }
                                            else if (k07 != 0 && k7 == 0)
                                            {
                                                if (k07 < 12)
                                                {
                                                    TestChart.Rows[k7][columnIndex + k07] = Convert.ToInt32(yr.Rows[k3][k07 + 1]);
                                                }
                                                else
                                                {
                                                    int kj = k07 % 12;
                                                    TestChart.Rows[k7][columnIndex + k07] = Convert.ToInt32(yr.Rows[0][kj + 1]);
                                                }
                                            }
                                            else
                                            {
                                                if (columnIndex + k07 + k7 < TestChart.Columns.Count)
                                                {
                                                    TestChart.Rows[k7][columnIndex + k07 + k7] = TestChart.Rows[k7 - 1][columnIndex + k07 + k7 - 1];
                                                }

                                            }
                                        }



                                        for (int jj = 0; jj < TestChart.Columns.Count - columnIndex; jj++)
                                        {
                                            if (!string.IsNullOrEmpty(TestChart.Rows[k7][jj + columnIndex].ToString()))
                                            {
                                                tot = tot + Convert.ToInt32(TestChart.Rows[k7][jj + columnIndex]);
                                            }

                                        }
                                    }
                                    TestChart.Rows[k7][0] = tot;
                                }
                                break;
                            case 18:
                                for (int k7 = 0; k7 < 18; k7++)
                                {
                                    TestChart.Rows.Add();
                                    TestChart.Rows[k7][1] = id.ToString();
                                    TestChart.Rows[k7][2] = TestingProtocal.Rows[k3][6].ToString();
                                    TestChart.Rows[k7][3] = TestingProtocal.Rows[k3][0].ToString();
                                    TestChart.Rows[k7][4] = TestingProtocal.Rows[k3][1].ToString();
                                    TestChart.Rows[k7][5] = TestingProtocal.Rows[k3][3].ToString();
                                    TestChart.Rows[k7][6] = TestingProtocal.Rows[k3][4].ToString();
                                    foreach (var item in mMGAbyType)
                                    {
                                        TestChart.Rows[k7][item.VariableName] = TestingProtocal.Rows[k3][item.VariableName].ToString();
                                    }
                                    //TestChart.Rows[k7][6] = TestingProtocal.Rows[k3][4].ToString();
                                    //TestChart.Rows[k7][7] = TestingProtocal.Rows[k3][5].ToString();

                                    int columnIndex = TestChart.Columns.IndexOf("1");
                                    int tot = 0;
                                    if (columnIndex >= 0)
                                    {
                                        for (int k07 = 0; k07 < yrs * 12; k07++)
                                        {
                                            if (k07 == 0 && k7 == 0)
                                            {
                                                TestChart.Rows[k7][columnIndex + k07] = Convert.ToInt32(yr.Rows[k3][0]) + Convert.ToInt32(yr.Rows[k3][1]);
                                            }
                                            else if (k07 != 0 && k7 == 0)
                                            {
                                                if (k07 < 12)
                                                {
                                                    TestChart.Rows[k7][columnIndex + k07] = Convert.ToInt32(yr.Rows[k3][k07 + 1]);
                                                }
                                                else
                                                {
                                                    int kj = k07 % 12;
                                                    TestChart.Rows[k7][columnIndex + k07] = Convert.ToInt32(yr.Rows[0][kj + 1]);
                                                }
                                            }
                                            else
                                            {
                                                if (columnIndex + k07 + k7 < TestChart.Columns.Count)
                                                {
                                                    TestChart.Rows[k7][columnIndex + k07 + k7] = TestChart.Rows[k7 - 1][columnIndex + k07 + k7 - 1];
                                                }

                                            }
                                        }



                                        for (int jj = 0; jj < TestChart.Columns.Count - columnIndex; jj++)
                                        {
                                            if (!string.IsNullOrEmpty(TestChart.Rows[k7][jj + columnIndex].ToString()))
                                            {
                                                tot = tot + Convert.ToInt32(TestChart.Rows[k7][jj + columnIndex]);
                                            }

                                        }
                                    }
                                    TestChart.Rows[k7][0] = tot;

                                }
                                break;
                            case 24:
                                for (int k7 = 0; k7 < 24; k7++)
                                {

                                    TestChart.Rows.Add();
                                    TestChart.Rows[k7][1] = id.ToString();
                                    TestChart.Rows[k7][2] = TestingProtocal.Rows[k3][6].ToString();
                                    TestChart.Rows[k7][3] = TestingProtocal.Rows[k3][0].ToString();
                                    TestChart.Rows[k7][4] = TestingProtocal.Rows[k3][1].ToString();
                                    TestChart.Rows[k7][5] = TestingProtocal.Rows[k3][3].ToString();
                                    TestChart.Rows[k7][6] = TestingProtocal.Rows[k3][4].ToString();
                                    foreach (var item in mMGAbyType)
                                    {
                                        TestChart.Rows[k7][item.VariableName] = TestingProtocal.Rows[k3][item.VariableName].ToString();
                                    }
                                    //TestChart.Rows[k7][6] = TestingProtocal.Rows[k3][4].ToString();
                                    //TestChart.Rows[k7][7] = TestingProtocal.Rows[k3][5].ToString();

                                    int columnIndex = TestChart.Columns.IndexOf("1");
                                    int tot = 0;
                                    if (columnIndex >= 0)
                                    {
                                        for (int k07 = 0; k07 < yrs * 12; k07++)
                                        {
                                            if (k07 == 0 && k7 == 0)
                                            {
                                                TestChart.Rows[k7][columnIndex + k07] = Convert.ToInt32(yr.Rows[k3][0]) + Convert.ToInt32(yr.Rows[k3][1]);
                                            }
                                            else if (k07 != 0 && k7 == 0)
                                            {
                                                if (k07 < 12)
                                                {
                                                    TestChart.Rows[k7][columnIndex + k07] = Convert.ToInt32(yr.Rows[k3][k07 + 1]);
                                                }
                                                else
                                                {
                                                    int kj = k07 % 12;
                                                    TestChart.Rows[k7][columnIndex + k07] = Convert.ToInt32(yr.Rows[0][kj + 1]);
                                                }
                                            }
                                            else
                                            {
                                                if (columnIndex + k07 + k7 < TestChart.Columns.Count)
                                                {
                                                    TestChart.Rows[k7][columnIndex + k07 + k7] = TestChart.Rows[k7 - 1][columnIndex + k07 + k7 - 1];
                                                }

                                            }
                                        }



                                        for (int jj = 0; jj < TestChart.Columns.Count - columnIndex; jj++)
                                        {
                                            if (!string.IsNullOrEmpty(TestChart.Rows[k7][jj + columnIndex].ToString()))
                                            {
                                                tot = tot + Convert.ToInt32(TestChart.Rows[k7][jj + columnIndex]);
                                            }

                                        }
                                    }
                                    TestChart.Rows[k7][0] = tot;
                                }
                                break;
                        }


                        //for (int k7 = 0; k7 < mmprogram.NoofYear * 12; k7++) //no of year in 12  
                        //{
                        //    TestChart.Rows.Add();
                        //    TestChart.Rows[k7][1] = id.ToString();
                        //    TestChart.Rows[k7][2] = TestingProtocal.Rows[k3][6].ToString();
                        //    TestChart.Rows[k7][3] = TestingProtocal.Rows[k3][0].ToString();
                        //    TestChart.Rows[k7][4] = TestingProtocal.Rows[k3][1].ToString();
                        //    TestChart.Rows[k7][5] = TestingProtocal.Rows[k3][3].ToString();
                        //    TestChart.Rows[k7][6] = TestingProtocal.Rows[k3][4].ToString();
                        //    foreach (var item in mMGAbyType)
                        //    {
                        //        TestChart.Rows[k7][item.VariableName] = TestingProtocal.Rows[k3][item.VariableName].ToString();
                        //    }
                        //    //TestChart.Rows[k7][6] = TestingProtocal.Rows[k3][4].ToString();
                        //    //TestChart.Rows[k7][7] = TestingProtocal.Rows[k3][5].ToString();

                        //    int columnIndex = TestChart.Columns.IndexOf("1");
                        //    int tot = 0;
                        //    if (columnIndex >= 0)
                        //    {
                        //        for (int k07 = 0; k07 < yrs * 12; k07++)
                        //        {
                        //            if (k07 == 0 && k7 == 0)
                        //            {
                        //                TestChart.Rows[k7][columnIndex + k07] = Convert.ToInt32(yr.Rows[k3][0]) + Convert.ToInt32(yr.Rows[k3][1]);
                        //            }
                        //            else if (k07 != 0 && k7 == 0)
                        //            {
                        //                if (k07 < 12)
                        //                {
                        //                    TestChart.Rows[k7][columnIndex + k07] = Convert.ToInt32(yr.Rows[k3][k07 + 1]);
                        //                }
                        //                else
                        //                {
                        //                    int kj = k07 % 12;
                        //                    TestChart.Rows[k7][columnIndex + k07] = Convert.ToInt32(yr.Rows[0][kj + 1]);
                        //                }
                        //            }
                        //            else
                        //            {
                        //                if (columnIndex + k07 + k7 < TestChart.Columns.Count)
                        //                {
                        //                    TestChart.Rows[k7][columnIndex + k07 + k7] = TestChart.Rows[k7 - 1][columnIndex + k07 + k7 - 1];
                        //                }

                        //            }
                        //        }



                        //        for (int jj = 0; jj < TestChart.Columns.Count - columnIndex; jj++)
                        //        {
                        //            if (!string.IsNullOrEmpty(TestChart.Rows[k7][jj + columnIndex].ToString()))
                        //            {
                        //                tot = tot + Convert.ToInt32(TestChart.Rows[k7][jj + columnIndex]);
                        //            }

                        //        }
                        //    }
                        //    TestChart.Rows[k7][0] = tot;


                        //}
                        int totalTest = 0; decimal aAmount = 0;//, per=0;
                        for (int jj = 0; jj < TestChart.Rows.Count; jj++)
                        {

                            totalTest = totalTest + Convert.ToInt32(TestChart.Rows[jj][0]);

                        }

                        var perc = ctx.PercentageVal.Where(c => c.TestN == Convert.ToInt32(TestChart.Rows[0][3]) && c.PGrpID == Convert.ToInt32(TestChart.Rows[0][4])).ToList();


                        for (int j = 0; j < perc.Count; j++)
                        {

                            DataRow Dr3 = Percentag.NewRow();
                            Dr3["testn"] = perc[j].TestN;
                            Dr3["pgrpid"] = perc[j].PGrpID;
                            Dr3["pernew"] = perc[j].PerNew;
                            Dr3["perold"] = perc[j].PerOld;
                            Dr3["TotalTestPerYear"] = perc[j].TotalTestPerYear;
                            for (int i = 0; i < testingprotocol1.Count; i++)
                            {

                                var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == perc[j].TestN && b.Forecastid == id && b.Parameterid == testingprotocol1[i].Id && b.PatientGroupID == perc[j].PGrpID).FirstOrDefault();

                                if (MMGeneralAssumptionvalue != null)
                                    Dr3[testingprotocol1[i].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                                else
                                    Dr3[testingprotocol1[i].VariableName] = 0;

                            }
                            Percentag.Rows.Add(Dr3);
                        }
                        //if (_variableName != "") _query = "Select testn,pgrpid,pernew,perold,TotalTestPerYear ," + _variableName.TrimEnd(',') + "from percentageval where testn='" + TestChart.Rows[0][3] + "' and pgrpid='" + TestChart.Rows[0][4] + "'";
                        //else if (_variableName == "") _query = "Select testn,pgrpid,pernew,perold,TotalTestPerYear from percentageval where testn='" + TestChart.Rows[0][3] + "' and pgrpid='" + TestChart.Rows[0][4] + "'";
                        //SqlDataAdapter adapt13 = new SqlDataAdapter(_query, con);

                        //Changes parameter according to MMProgram

                        decimal totalcal = (totalTest * Convert.ToDecimal(Percentag.Rows[0][2])) + (Convert.ToDecimal(Percentag.Rows[0][3]) * (Convert.ToDecimal(yrs) * Convert.ToDecimal(Percentag.Rows[0][4])));
                        //Decimal per = Convert.ToDecimal(Percentag.Rows[0][5]) + Convert.ToDecimal(Percentag.Rows[0][6]);
                        Decimal per = 0;
                        for (int ki = 0; ki < Percentag.Rows.Count; ki++)
                        {
                            foreach (DataColumn dc in Percentag.Columns)
                            {
                                foreach (demographicMMGeneralAssumption MM in testingprotocol1)
                                {
                                    string v1 = MM.VariableName;
                                    string v2 = dc.ColumnName.ToString();
                                    if (v1.Equals(v2))
                                    {
                                        //bool variableEffect = gAssumption.Where(x => x.VariableName == item.ToString()).SingleOrDefault().VariableEffect;
                                        //int variableDatatype = _mMGeneralAssumption.Where(x => x.VariableName == item.ToString()).SingleOrDefault().VariableDataType;
                                        //if(variableDatatype==2)aAmount=aAmount+(newpatient*(Convert.ToDecimal(dtValue.Rows[ki][""+item.ToString()+""]))/100);
                                        //else if (variableDatatype == 2) aAmount = aAmount + (newpatient * (Convert.ToDecimal(dtValue.Rows[ki]["" + item.ToString() + ""])) / 100);
                                        decimal vvalue = Convert.ToDecimal(Percentag.Rows[ki]["" + v2 + ""]);
                                        aAmount = ((vvalue) / 100);
                                        if (MM.VariableEffect == true) per = per + aAmount;
                                        if (MM.VariableEffect == false) per = per - aAmount;

                                    }

                                }
                            }

                        }
                        //check parameter should add or subtract

                        decimal finalCal = totalcal + (totalcal * per);  //correct
                        ForecastedTestByTest FT = new ForecastedTestByTest();
                        FT.ForeCastID = id;
                        FT.Tst = Convert.ToInt32(TestChart.Rows[0][3].ToString().Trim());
                        FT.PGrp = Convert.ToInt32(TestChart.Rows[0][4].ToString().Trim());
                        FT.TotalTst = finalCal;
                        FT.sitecategoryid = sitecategoryids[z];
                        ctx.ForecastedTestByTest.Add(FT);
                        ctx.SaveChanges();


                        int columnIndex1 = TestChart.Columns.IndexOf("1");
                        var startmonth = forecastinfo.StartDate.Month;

                        for (int i = 0; i < TestChart.Columns.Count - columnIndex1; i++)
                        {
                            int tot = 0; int j = 0;
                            TestByMonth.Rows.Add();
                            TestByMonth.Rows[i][0] = TestChart.Rows[0][1];
                            TestByMonth.Rows[i][1] = TestChart.Rows[0][3];
                            for (j = 0; j < 12; j++)
                            {
                                if (!string.IsNullOrEmpty(TestChart.Rows[j][i + columnIndex1].ToString()))
                                {
                                    tot = tot + Convert.ToInt32(TestChart.Rows[j][i + columnIndex1]);
                                }
                                TestByMonth.Rows[i][7] = TestChart.Rows[j][5];
                                foreach (var item in mMGAbyType)
                                {
                                    TestByMonth.Rows[i][item.VariableName] = TestChart.Rows[j][item.VariableName];
                                }
                                //TestByMonth.Rows[i][8] = TestChart.Rows[j][6];
                                //TestByMonth.Rows[i][9] = TestChart.Rows[j][7];
                            }


                            string mnth = "";
                            if ((startmonth) % 12 == 1)
                            {
                                mnth = "Jan";
                            }
                            if ((startmonth) % 12 == 2)
                            {
                                mnth = "Feb";
                            }
                            if ((startmonth) % 12 == 3)
                            {
                                mnth = "Mar";
                            }
                            if ((startmonth) % 12 == 4)
                            {
                                mnth = "Apr";
                            }
                            if ((startmonth) % 12 == 5)
                            {
                                mnth = "May";
                            }
                            if ((startmonth) % 12 == 6)
                            {
                                mnth = "Jun";
                            }
                            if ((startmonth) % 12 == 7)
                            {
                                mnth = "Jul";
                            }
                            if ((startmonth) % 12 == 8)
                            {
                                mnth = "Aug";
                            }
                            if ((startmonth) % 12 == 9)
                            {
                                mnth = "Sep";
                            }
                            if ((startmonth) % 12 == 10)
                            {
                                mnth = "Oct";
                            }
                            if ((startmonth) % 12 == 11)
                            {
                                mnth = "Nov";
                            }
                            if ((startmonth) % 12 == 0)
                            {
                                mnth = "Dec";
                            }
                            string m = months.Rows[0][0].ToString();

                            string ys = (m.Length > 3) ? m.Substring(m.Length - 4, 4) : m;
                            string years = "";
                            if (startmonth <= 12)
                            {
                                years = ys;
                            }

                            if (startmonth > 12 && startmonth <= 24)
                            {
                                years = (Convert.ToInt32(ys) + 1).ToString();
                            }
                            if (startmonth > 24 && startmonth <= 36)
                            {
                                years = (Convert.ToInt32(ys) + 2).ToString();
                            }
                            if (startmonth > 36 && startmonth <= 48)
                            {
                                years = (Convert.ToInt32(ys) + 3).ToString();
                            }
                            if (startmonth > 48 && startmonth <= 60)
                            {
                                years = (Convert.ToInt32(ys) + 4).ToString();
                            }


                            TestByMonth.Rows[i][2] = string.Concat(mnth, " ", years);
                            TestByMonth.Rows[i][3] = tot.ToString();
                            TestByMonth.Rows[i][4] = TestChart.Rows[0][4];
                            TestByMonth.Rows[i][5] = (Convert.ToInt32(i) + 1).ToString();

                            if (forecastinfo.Period == "Monthly")
                            {
                                startmonth = startmonth + 1;
                            }
                            if (forecastinfo.Period == "Bimonthly")
                            {
                                startmonth = startmonth + 2;
                            }
                            if (forecastinfo.Period == "Quarterly")
                            {
                                startmonth = startmonth + 4;
                            }
                            if (forecastinfo.Period == "Yearly")
                            {
                                startmonth = startmonth + 12;
                            }


                        }


                        for (int ij1 = 0; ij1 < TestByMonth.Rows.Count; ij1++)
                        {


                            string _inserValuesTBM = "";

                            _inserValuesTBM += "'" + TestByMonth.Rows[ij1][0].ToString().Trim() + "','" + TestByMonth.Rows[ij1][1].ToString().Trim() + "','" + TestByMonth.Rows[ij1][2].ToString().Trim() + "','" + TestByMonth.Rows[ij1][3].ToString().Trim() + "','" + TestByMonth.Rows[ij1][4].ToString().Trim() + "','" + TestByMonth.Rows[ij1][5].ToString() + "'," + TestingProtocal.Rows[k3]["Percentage1"].ToString() + "," + TestByMonth.Rows[ij1][7].ToString() + " ,";
                            foreach (var item in mMGAbyType)
                            {
                                _inserValuesTBM += "" + TestingProtocal.Rows[k3][item.VariableName].ToString().Trim() + " ,";
                            }
                            _inserValuesTBM += "" + TestingProtocal.Rows[k3]["Percentageold"].ToString().Trim() + "," + yrs + "";

                            TestByMonth TM = new TestByMonth();
                            TM.ForeCastID = Convert.ToInt32(TestByMonth.Rows[ij1][0].ToString());
                            TM.TestID = Convert.ToInt32(TestByMonth.Rows[ij1][1].ToString());
                            TM.Month = TestByMonth.Rows[ij1][2].ToString();
                            TM.TstNo = Convert.ToDecimal(TestByMonth.Rows[ij1][3].ToString());


                            TM.PGrp = Convert.ToInt32(TestByMonth.Rows[ij1][4].ToString());
                            TM.SNo = Convert.ToInt32(TestByMonth.Rows[ij1][5].ToString());
                            TM.NewPatient = Convert.ToDecimal(TestingProtocal.Rows[k3]["Percentage1"].ToString());
                            TM.TotalTestPerYear = Convert.ToInt32(TestByMonth.Rows[ij1][7].ToString());
                            TM.ExistingPatient = Convert.ToDecimal(TestingProtocal.Rows[k3]["Percentageold"].ToString());
                            TM.sitecategoryid = sitecategoryids[z];
                            TM.Duration = Convert.ToDecimal(yrs);

                            ctx.TestByMonth.Add(TM);
                            ctx.SaveChanges();
                            //if (_variableName != "") _query1 = "insert into Testbymonth (ForeCastID, TestID, Month,TstNo,PGrp,SNo,NewPatient,TotalTestPerYear," + _variableName.TrimEnd(',') + ",ExistingPatient,Duration) values(" + _inserValuesTBM.TrimEnd(',') + ")";
                            //else if (_variableName == "") _query1 = "insert into Testbymonth (ForeCastID, TestID, Month,TstNo,PGrp,SNo,NewPatient,TotalTestPerYear,ExistingPatient,Duration) values(" + _inserValuesTBM.TrimEnd(',') + ")";
                            ////cm.CommandText = "insert into Testbymonth (ForeCastID, TestID, Month,TstNo,PGrp,SNo,NewPatient,TotalTestPerYear," + _variableName.TrimEnd(',') + ",ExistingPatient,Duration) values(" + _inserValuesTBM.TrimEnd(',') + ")";
                            //cm.CommandText = _query1;

                            //here we should add name TestPerYear in place of TestByMonth.Rows[ij1][7],RepeatTest in place of TestByMonth.Rows[ij1][8] and Symptomtest in place of TestByMonth.Rows[ij1][9]
                            //also create column according to Program in Testbymonth table




                            //cmd1.CommandText = "insert into PercentageVal (TestN, PGrpID, PerNew,PerOld,TotalTestPerYear," + _columnName.TrimEnd(',') + ") values(" + _inserValuesPV + ")";


                        }

                    }

                }
                months.Clear();
            }
            //TestNoChart.Columns.Add("TestName");
            //TestNoChart.Columns.Add("tst");

            //var testchart = ctx.ForecastedTestByTest.Join(ctx.Test, b => b.Tst, c => c.TestID, (b, c) => new { b, c }).Where(g => g.b.ForeCastID == id).GroupBy(x => x.c.TestID).Select(f => new
            //{
            //    Tst = f.Sum(s => s.b.TotalTst),
            //    TestName = f.Max(s => s.c.TestName)
            //}).ToList();
            //for (int i = 0; i < testchart.Count; i++)
            //{
            //    DataRow Dr = TestNoChart.NewRow();
            //    Dr["TestName"] = testchart[i].TestName;
            //    Dr["tst"] = testchart[i].Tst;
            //    TestNoChart.Rows.Add(Dr);

            //}
            //return TestNoChart;
        }


        private DataTable GettestAssumption(ForecastInfo ForecastInfo, int sitecategoryid)
        {

            DataTable DT = new DataTable();

            int testingareaid = 0;
            DT.Columns.Add("TestID");
            DT.Columns.Add("PatientGroupID");

            DT.Columns.Add("PercentagePanel");
            DT.Columns.Add("TotalTestPerYear");

            DT.Columns.Add("TestRepeatPerYear");
            DT.Columns.Add("SymptomTestPerYear");
            DT.Columns.Add("Baseline");



            var mmprogram = ctx.MMProgram.Where(b => b.Id == ForecastInfo.ProgramId).FirstOrDefault();
            var MMGeneralAssumption1 = ctx.demographicMMGeneralAssumption.Where(b => b.Entity_type_id == 5 && b.Forecastid == ForecastInfo.ForecastID).ToList();

            int[] siteids;

            if (ForecastInfo.ForecastType == "S")
            {
                siteids = new int[1];
                siteids[0] = sitecategoryid;
            }
            else
            {
                siteids = ctx.ForecastCategorySiteInfo.Where(b => b.ForecastInfoID == ForecastInfo.ForecastID && b.CategoryID == sitecategoryid).Select(x => x.SiteID).ToArray();
            }

            var testingprotocol = MMGeneralAssumption1.Where(c => c.VariableName.Contains("Mon")).ToList();

            //  Dr["TotalTestPerYear"] = TestingprotocolEnt.TotalTestPerYear;
            switch (ForecastInfo.months)
            {
                case 13:
                    for (int i = 0; i < 12; i++)
                    {
                        DT.Columns.Add(testingprotocol[i].VariableName, typeof(Int32));
                    }
                    DT.Columns.Add("TotalTestPerYear1", typeof(Int32));
                    break;
                case 12:
                    for (int i = 0; i < 12; i++)
                    {
                        DT.Columns.Add(testingprotocol[i].VariableName, typeof(Int32));
                    }
                    break;
                case 18:
                    for (int i = 0; i < 18; i++)
                    {
                        DT.Columns.Add(testingprotocol[i].VariableName, typeof(Int32));
                    }
                    break;
                case 24:
                    for (int i = 0; i < 24; i++)
                    {
                        DT.Columns.Add(testingprotocol[i].VariableName, typeof(Int32));
                    }
                    break;
            }

            //if (mmprogram.NoofYear == 2)
            //{
            //    for (int i = 0; i < 24; i++)
            //    {
            //        DT.Columns.Add(testingprotocol[i].VariableName, typeof(Int32));
            //    }
            //}
            //else
            //{
            //    for (int i = 0; i < 12; i++)
            //    {
            //        DT.Columns.Add(testingprotocol[i].VariableName, typeof(Int32));
            //    }
            //}

            var testingprotocol1 = MMGeneralAssumption1.Where(c => !c.VariableName.Contains("Mon") && c.Forecastid == ForecastInfo.ForecastID).ToList();
            for (int i = 0; i < testingprotocol1.Count; i++)
            {
                DT.Columns.Add(testingprotocol1[i].VariableName, typeof(Int32));
            }
            //  DT.Columns.Add("TotalTestPerYear1", typeof(Int32));
            var mmgroup = ctx.DemographicMMGroup.Where(c => c.Forecastid == ForecastInfo.ForecastID && c.IsActive == true).ToList();
            var testids = ctx.TestingProtocol.Where(c => c.ForecastinfoID == ForecastInfo.ForecastID)
    .GroupBy(x => x.TestID)
    .Select(group => group.First()).ToList();
            for (int i = 0; i < testids.Count; i++)
            {
                testingareaid = ctx.Test.Where(b => b.TestID == Convert.ToInt32(testids[i].TestID)).Select(x => x.TestingAreaID).FirstOrDefault();
                for (int z = 0; z < siteids.Length; z++)
                {


                    //int count = ctx.siteinstrument.Join(ctx.Instrument, b => b.InstrumentID, c => c.InstrumentID, (b, c) => new { b, c }).Where(x => x.b.SiteID == siteids[0] && x.c.testingArea.TestingAreaID == testingareaid).Count();
                    //if (count == 0)
                    //{
                    //    continue;
                    //}
                    for (int j = 0; j < mmgroup.Count; j++)
                    {
                        var TestingprotocolEnt = ctx.TestingProtocol.Where(b => b.TestID == testids[i].TestID && b.ForecastinfoID == ForecastInfo.ForecastID && b.PatientGroupID == mmgroup[j].Id).FirstOrDefault();

                        DataRow Dr = DT.NewRow();

                        Dr["TestID"] = TestingprotocolEnt.TestID;
                        Dr["PatientGroupID"] = mmgroup[j].Id;

                        Dr["PercentagePanel"] = TestingprotocolEnt.PercentagePanel;
                        Dr["TotalTestPerYear"] = TestingprotocolEnt.TotalTestPerYear;
                        Dr["TestRepeatPerYear"] = 0;
                        Dr["SymptomTestPerYear"] = 0;
                        Dr["Baseline"] = TestingprotocolEnt.Baseline;







                        switch (ForecastInfo.months)
                        {
                            case 13:
                                for (int k = 0; k < 12; k++)
                                {
                                    var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == testids[i].TestID && b.Forecastid == ForecastInfo.ForecastID && b.Parameterid == testingprotocol[k].Id && b.PatientGroupID == TestingprotocolEnt.PatientGroupID).FirstOrDefault();

                                    if (MMGeneralAssumptionvalue != null)
                                        Dr[testingprotocol[k].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                                    else
                                        Dr[testingprotocol[k].VariableName] = 0;
                                }
                                Dr["TotalTestPerYear1"] = TestingprotocolEnt.TotalTestPerYear;
                                break;
                            case 12:
                                for (int k = 0; k < 12; k++)
                                {
                                    var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == testids[i].TestID && b.Forecastid == ForecastInfo.ForecastID && b.Parameterid == testingprotocol[k].Id && b.PatientGroupID == TestingprotocolEnt.PatientGroupID).FirstOrDefault();

                                    if (MMGeneralAssumptionvalue != null)
                                        Dr[testingprotocol[k].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                                    else
                                        Dr[testingprotocol[k].VariableName] = 0;
                                }
                                break;
                            case 18:
                                for (int k = 0; k < 18; k++)
                                {
                                    var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == testids[i].TestID && b.Forecastid == ForecastInfo.ForecastID && b.Parameterid == testingprotocol[k].Id && b.PatientGroupID == TestingprotocolEnt.PatientGroupID).FirstOrDefault();

                                    if (MMGeneralAssumptionvalue != null)
                                        Dr[testingprotocol[k].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                                    else
                                        Dr[testingprotocol[k].VariableName] = 0;
                                }
                                break;
                            case 24:
                                for (int k = 0; k < 24; k++)
                                {
                                    var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == testids[i].TestID && b.Forecastid == ForecastInfo.ForecastID && b.Parameterid == testingprotocol[k].Id && b.PatientGroupID == TestingprotocolEnt.PatientGroupID).FirstOrDefault();

                                    if (MMGeneralAssumptionvalue != null)
                                        Dr[testingprotocol[k].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                                    else
                                        Dr[testingprotocol[k].VariableName] = 0;
                                }
                                break;
                        }
                        //if (mmprogram.NoofYear == 2)
                        //{
                        //    for (int k = 0; k < 24; k++)
                        //    {
                        //        var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == testids[i].TestID && b.Forecastid == ForecastInfo.ForecastID && b.Parameterid == testingprotocol[k].Id && b.PatientGroupID == TestingprotocolEnt.PatientGroupID).FirstOrDefault();

                        //        if (MMGeneralAssumptionvalue != null)
                        //            Dr[testingprotocol[k].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                        //        else
                        //            Dr[testingprotocol[k].VariableName] = 0;


                        //    }
                        //}
                        //else
                        //{
                        //    for (int k = 0; k < 12; k++)
                        //    {
                        //        var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == testids[i].TestID && b.Forecastid == ForecastInfo.ForecastID && b.Parameterid == testingprotocol[k].Id && b.PatientGroupID == TestingprotocolEnt.PatientGroupID).FirstOrDefault();

                        //        if (MMGeneralAssumptionvalue != null)
                        //            Dr[testingprotocol[k].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                        //        else
                        //            Dr[testingprotocol[k].VariableName] = 0;
                        //    }
                        //}


                        for (int k = 0; k < testingprotocol1.Count; k++)
                        {

                            var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == testids[i].TestID && b.Forecastid == ForecastInfo.ForecastID && b.Parameterid == testingprotocol1[k].Id && b.PatientGroupID == TestingprotocolEnt.PatientGroupID).FirstOrDefault();

                            if (MMGeneralAssumptionvalue != null)
                                Dr[testingprotocol1[k].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                            else
                                Dr[testingprotocol1[k].VariableName] = 0;

                        }


                        DT.Rows.Add(Dr);


                    }

                }

            }
            return DT;
        }
    }
}

