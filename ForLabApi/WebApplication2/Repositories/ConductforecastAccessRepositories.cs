using ForLabApi.DataInterface;
using ForLabApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ForLabApi.Repositories
{
    public class ConductforecastAccessRepositories : IConductforecast<Costclass, ConductforecastDasboard, ConductDashboardchartdata>
    {

        ForLabContext ctx;
        private List<ForecastedResult> _listFresult;
        private bool _forecastWithoutError;
        private DateTime lastEntryDate;
        private string[] param1;
        public ConductforecastAccessRepositories(ForLabContext c)
        {
            ctx = c;
            //return ctx;
        }
        public IList<ConductDashboardchartdata> GetProducttypecostratio(int id, int fid)
        {
            decimal total = 0;
            List<ConductDashboardchartdata> Db = new List<ConductDashboardchartdata>();
            var list = ctx.ForecastedResult.Join(ctx.ForecastInfo, b => b.ForecastId, c => c.ForecastID, (b, c) => new { b, c })
                .Where(s => s.b.PackPrice != 0 && s.b.ForecastId == fid && s.b.SiteId == id && s.b.DurationDateTime >= s.c.StartDate && s.b.DurationDateTime < getmaxdate(s.c.Period, s.c.StartDate, s.c.Extension))
                .GroupBy(f => f.b.ProductTypeId)
                .Select(g => new
                {
                    g.Key,
                    ProductType = g.Max(h => h.b.ProductType),
                    Totalprice = g.Sum(h => h.b.PackPrice)

                }).ToList();

            for (int i = 0; i < list.Count; i++)
            {
                total = total + list[i].Totalprice;
            }

            for (int i = 0; i < list.Count; i++)
            {

                Db.Add(new ConductDashboardchartdata
                {
                    name = list[i].ProductType,
                    y = Math.Round((list[i].Totalprice / total) * 100, 2)

                });


            }
            return Db;
        }



        public IList<ConductDashboardchartdata> GetProducttypecostratioNEW(int fid)
        {
            decimal total = 0;
            List<ConductDashboardchartdata> Db = new List<ConductDashboardchartdata>();
            var list = ctx.ForecastedResult.Join(ctx.ForecastInfo, b => b.ForecastId, c => c.ForecastID, (b, c) => new { b, c })
                .Where(s => s.b.PackPrice != 0 && s.b.ForecastId == fid) //&& s.b.SiteId == id && s.b.DurationDateTime >= s.c.StartDate && s.b.DurationDateTime < getmaxdate(s.c.Period, s.c.StartDate, s.c.Extension)
                .GroupBy(f => f.b.ProductTypeId)
                .Select(g => new
                {
                    g.Key,
                    ProductType = g.Max(h => h.b.ProductType),
                    Totalprice = g.Sum(h => h.b.PackPrice)

                }).ToList();

            for (int i = 0; i < list.Count; i++)
            {
                total = total + list[i].Totalprice;
            }

            for (int i = 0; i < list.Count; i++)
            {

                Db.Add(new ConductDashboardchartdata
                {
                    name = list[i].ProductType,
                    y = Math.Round((list[i].Totalprice / total) * 100, 2)

                });


            }
            return Db;
        }


        public IList<ConductDashboardchartdata> GetdemoProducttypecostratioNEW(int fid)
        {
            decimal total = 0;
            List<ConductDashboardchartdata> Db = new List<ConductDashboardchartdata>();


            var list = ctx.ForecastedResult.Where(x => x.ForecastId == fid && x.TotalValue != 0)
            .GroupBy(s => new { s.SiteId, s.ProductTypeId, s.ProductId })
            .Select(n => new
            {
                n.Key.ProductTypeId,
                n.Key.SiteId,
                ProductType = n.Max(s => s.ProductType),
                totalcost = Math.Round(Math.Ceiling(n.Sum(s => s.TotalValue) / n.Max(s => s.PackQty)) * n.Max(s => s.PackPrice), 2),
                productname = n.Max(s => s.ProductId)
            }).ToList();


            var list1 = list.GroupBy(s => s.ProductTypeId)
                .Select(g => new
                {
                    g.Key,
                    ProductType = g.Max(h => h.ProductType),
                    Totalprice = g.Sum(h => h.totalcost)

                }).ToList();

            for (int i = 0; i < list1.Count; i++)
            {
                total = total + list1[i].Totalprice;
            }

            for (int i = 0; i < list1.Count; i++)
            {

                Db.Add(new ConductDashboardchartdata
                {
                    name = list1[i].ProductType,
                    y = Math.Round((list1[i].Totalprice / total) * 100, 2)

                });


            }
            return Db;
        }


        public Costclass getcostparameter(int id)
        {
            Costclass cc = new Costclass();

            cc.totalcost = String.Format("{0:n}", ctx.ForecastedResult.Where(b => b.ForecastId == id).Sum(x => x.PackPrice));


            cc.qccost = String.Format("{0:n}", ctx.ForecastedResult.Where(b => b.ForecastId == id && b.IsForGeneralConsumable == false).Sum(x => x.PackPrice));
            cc.cccost = String.Format("{0:n}", ctx.ForecastedResult.Where(b => b.ForecastId == id && b.IsForGeneralConsumable == true).Sum(x => x.PackPrice));

            return cc;
        }

        public Costclass getdemocostparameter(int id)
        {
            Costclass cc = new Costclass();
            var list = ctx.ForecastedResult.Where(x => x.ForecastId == id && x.TotalValue != 0)
                .GroupBy(s => new { s.SiteId, s.ProductTypeId, s.ProductId })
                .Select(n => new
                {
                    n.Key.ProductTypeId,
                    n.Key.SiteId,
                    totalcost = Math.Round(Math.Ceiling(n.Sum(s => s.TotalValue) / n.Max(s => s.PackQty)) * n.Max(s => s.PackPrice), 2),
                    productname = n.Max(s => s.ProductId)
                }).ToList();

            cc.totalcost = String.Format("{0:n}", list.Sum(x => x.totalcost));

            var list1 = ctx.ForecastedResult.Where(x => x.ForecastId == id && x.TotalValue != 0 && x.IsForGeneralConsumable == false)
                       .GroupBy(s => new { s.SiteId, s.ProductTypeId, s.ProductId })
                       .Select(n => new
                       {
                           n.Key.ProductTypeId,
                           n.Key.SiteId,
                           totalcost = Math.Round(Math.Ceiling(n.Sum(s => s.TotalValue) / n.Max(s => s.PackQty)) * n.Max(s => s.PackPrice), 2),
                           productname = n.Max(s => s.ProductId)
                       }).ToList();
            cc.qccost = String.Format("{0:n}", list1.Sum(x => x.totalcost));
            var list2 = ctx.ForecastedResult.Where(x => x.ForecastId == id && x.TotalValue != 0 && x.IsForGeneralConsumable == true)
                         .GroupBy(s => new { s.SiteId, s.ProductTypeId, s.ProductId })
                         .Select(n => new
                         {
                             n.Key.ProductTypeId,
                             n.Key.SiteId,
                             totalcost = Math.Round(Math.Ceiling(n.Sum(s => s.TotalValue) / n.Max(s => s.PackQty)) * n.Max(s => s.PackPrice), 2),
                             productname = n.Max(s => s.ProductId)
                         }).ToList();
            cc.cccost = String.Format("{0:n}", list2.Sum(x => x.totalcost));

            return cc;
        }
        //  Math.Round(Math.Ceiling(n.Sum(s => s.TotalValue) / n.Max(s => s.PackQty)) * n.Max(s => s.PackPrice), 2),
        public IList<ConductDashboardchartdata> GettestingareacostratioNEW(int fid)
        {
            decimal total = 0;
            List<ConductDashboardchartdata> Db = new List<ConductDashboardchartdata>();
            var list = ctx.ForecastedResult.Join(ctx.Test, b => b.TestId, c => c.TestID, (b, c) => new { b, c })
                .Join(ctx.TestingArea, d => d.c.TestingAreaID, e => e.TestingAreaID, (d, e) => new { d, e })
                   .Where(s => s.d.b.ForecastId == fid) //&& s.b.SiteId == id && s.b.DurationDateTime >= s.c.StartDate && s.b.DurationDateTime < getmaxdate(s.c.Period, s.c.StartDate, s.c.Extension)
                .GroupBy(f => new { f.d.c.TestingAreaID })
                .Select(g => new
                {
                    g.Key,
                    TestingArea = g.Max(h => h.d.b.TestingArea),
                    Totalprice = g.Sum(h => h.d.b.PackPrice)

                }).ToList();

            for (int i = 0; i < list.Count; i++)
            {
                total = total + list[i].Totalprice;
            }

            for (int i = 0; i < list.Count; i++)
            {

                Db.Add(new ConductDashboardchartdata
                {
                    name = list[i].TestingArea,
                    y = Math.Round((list[i].Totalprice / total) * 100, 2)

                });


            }
            return Db;
        }
        public IList<ConductDashboardchartdata> GetProducttypecostratiocategory(int id, int fid)
        {
            decimal total = 0;
            List<ConductDashboardchartdata> Db = new List<ConductDashboardchartdata>();
            var list = ctx.ForecastedResult.Join(ctx.ForecastInfo, b => b.ForecastId, c => c.ForecastID, (b, c) => new { b, c })
                .Where(s => s.b.PackPrice != 0 && s.b.ForecastId == fid && s.b.CategoryId == id && s.b.DurationDateTime >= s.c.StartDate && s.b.DurationDateTime < getmaxdate(s.c.Period, s.c.StartDate, s.c.Extension))
                .GroupBy(f => f.b.ProductTypeId)
                .Select(g => new
                {
                    g.Key,
                    ProductType = g.Max(h => h.b.ProductType),
                    Totalprice = g.Sum(h => h.b.PackPrice)

                }).ToList();

            for (int i = 0; i < list.Count; i++)
            {
                total = total + list[i].Totalprice;
            }

            for (int i = 0; i < list.Count; i++)
            {

                Db.Add(new ConductDashboardchartdata
                {
                    name = list[i].ProductType,
                    y = Math.Round((list[i].Totalprice / total) * 100, 2)

                });


            }
            return Db;
        }

        private DateTime getmaxdate(string Period, DateTime startdate, int extension)
        {
            DateTime Enddate;
            int @MonthAdded;
            @MonthAdded = extension;


            if (Period == "Bimonthly")
                @MonthAdded = extension * 2;

            if (Period == "Quarterly")
                @MonthAdded = extension * 3;

            if (Period == "Yearly") ;
            @MonthAdded = extension * 12;


            Enddate = startdate.AddMonths(@MonthAdded);
            return Enddate;

        }
        public Array Getdistinctduration(int id, int fid)
        {
            var ss = ctx.ForecastedResult.Join(ctx.ForecastInfo, b => b.ForecastId, c => c.ForecastID, (b, c) => new { b, c })
                  .Where(s => s.b.ForecastId == fid && s.b.SiteId == id && s.b.DurationDateTime >= s.c.StartDate && s.b.DurationDateTime < getmaxdate(s.c.Period, s.c.StartDate, s.c.Extension))
               .GroupBy(f => new { f.b.ProductTypeId, f.b.Duration, f.b.DurationDateTime }).Select(x => new
               {
                   TotalPrice = x.Sum(g => g.b.PackPrice),
                   ProductType = x.Max(g => g.b.ProductType),
                   x.Key.Duration,
                   x.Key.DurationDateTime,
                   x.Key.ProductTypeId


               }).OrderBy(h => h.DurationDateTime);
            var distinctduration = ss.GroupBy(s => s.DurationDateTime).Select(x => new { Duration = x.Max(f => f.Duration) }).ToArray();
            return distinctduration;

        }
        public Array Getdistinctdurationnew(int fid)
        {
            var ss = ctx.ForecastedResult.Join(ctx.ForecastInfo, b => b.ForecastId, c => c.ForecastID, (b, c) => new { b, c })
                  .Where(s => s.b.ForecastId == fid) //&& s.b.SiteId == id && s.b.DurationDateTime >= s.c.StartDate && s.b.DurationDateTime < getmaxdate(s.c.Period, s.c.StartDate, s.c.Extension)
               .GroupBy(f => new { f.b.ProductTypeId, f.b.Duration, f.b.DurationDateTime }).Select(x => new
               {
                   TotalPrice = x.Sum(g => g.b.PackPrice),
                   ProductType = x.Max(g => g.b.ProductType),
                   x.Key.Duration,
                   x.Key.DurationDateTime,
                   x.Key.ProductTypeId


               }).OrderBy(h => h.DurationDateTime);
            var distinctduration = ss.GroupBy(s => s.DurationDateTime).Select(x => new { Duration = x.Max(f => f.Duration) }).ToArray();
            return distinctduration;

        }
        public Array Getdistinctdurationservice(int fid)
        {
            var ss = ctx.ForecastedResult.Join(ctx.Test, b => b.TestId, c => c.TestID, (b, c) => new { b, c })
                .Join(ctx.TestingArea, d => d.c.TestingAreaID, e => e.TestingAreaID, (d, e) => new { d, e })
                  .Where(s => s.d.b.ForecastId == fid) //&& s.b.SiteId == id && s.b.DurationDateTime >= s.c.StartDate && s.b.DurationDateTime < getmaxdate(s.c.Period, s.c.StartDate, s.c.Extension)
               .GroupBy(f => new { f.d.c.TestingAreaID, f.d.b.Duration, f.d.b.DurationDateTime }).Select(x => new
               {
                   TotalPrice = x.Sum(g => g.d.b.PackPrice),
                   ProductType = x.Max(g => g.d.b.TestingArea),
                   x.Key.Duration,
                   x.Key.DurationDateTime,
                   x.Key.TestingAreaID


               }).OrderBy(h => h.DurationDateTime);
            var distinctduration = ss.GroupBy(s => s.DurationDateTime).Select(x => new { Duration = x.Max(f => f.Duration) }).ToArray();
            return distinctduration;

        }
        public IList<ConductforecastDasboard> Getforecastsummarydurationforcategory(int id, int fid)
        {
            var ss = ctx.ForecastedResult.Join(ctx.ForecastInfo, b => b.ForecastId, c => c.ForecastID, (b, c) => new { b, c })
               .Where(s => s.b.ForecastId == fid && s.b.CategoryId == id && s.b.DurationDateTime >= s.c.StartDate && s.b.DurationDateTime < getmaxdate(s.c.Period, s.c.StartDate, s.c.Extension))
            .GroupBy(f => new { f.b.ProductTypeId, f.b.Duration, f.b.DurationDateTime }).Select(x => new
            {
                TotalPrice = x.Sum(g => g.b.PackPrice),
                ProductType = x.Max(g => g.b.ProductType),
                x.Key.Duration,
                x.Key.DurationDateTime,
                x.Key.ProductTypeId


            }).OrderBy(h => h.DurationDateTime).ToList();


            var distinctproducttype = ss.GroupBy(s => s.ProductTypeId)
                .Select(x => new { x.Key, producttypename = x.Max(f => f.ProductType) }).ToList();

            var distinctduration = ss.GroupBy(s => s.DurationDateTime).Select(x => new { Duration = x.Max(f => f.Duration) }).ToList();

            List<ConductforecastDasboard> Db = new List<ConductforecastDasboard>();
            if (distinctduration.Count == 0)
                return Db;


            decimal[] data1 = new decimal[distinctduration.Count];



            for (int j = 0; j < distinctproducttype.Count; j++)
            {

                data1 = new decimal[distinctduration.Count];
                for (int i = 0; i < distinctduration.Count; i++)
                {
                    data1[i] = ss.Where(b => b.ProductTypeId == distinctproducttype[j].Key && b.Duration == distinctduration[i].Duration).Select(c => c.TotalPrice).FirstOrDefault();

                }

                Db.Add(new ConductforecastDasboard
                {

                    name = distinctproducttype[j].producttypename,
                    data = data1

                });



            }
            return Db;

        }
        public IList<ConductforecastDasboard> Getforecastsummarydurationforsite(int id, int fid)
        {

            //var forecastinfo = ctx.ForecastInfo.Find(fid);


            var ss = ctx.ForecastedResult.Join(ctx.ForecastInfo, b => b.ForecastId, c => c.ForecastID, (b, c) => new { b, c })
                   .Where(s => s.b.PackPrice != 0 && s.b.ForecastId == fid && s.b.SiteId == id && s.b.DurationDateTime >= s.c.StartDate && s.b.DurationDateTime < getmaxdate(s.c.Period, s.c.StartDate, s.c.Extension))
                .GroupBy(f => new { f.b.ProductTypeId, f.b.Duration, f.b.DurationDateTime }).Select(x => new
                {
                    TotalPrice = x.Sum(g => g.b.PackPrice),
                    ProductType = x.Max(g => g.b.ProductType),
                    x.Key.Duration,
                    x.Key.DurationDateTime,
                    x.Key.ProductTypeId


                }).OrderBy(h => h.DurationDateTime).ToList();


            var distinctproducttype = ss.GroupBy(s => s.ProductTypeId)
                .Select(x => new { x.Key, producttypename = x.Max(f => f.ProductType) }).ToList();

            var distinctduration = ss.GroupBy(s => s.DurationDateTime).Select(x => new { Duration = x.Max(f => f.Duration) }).ToList();

            List<ConductforecastDasboard> Db = new List<ConductforecastDasboard>();
            if (distinctduration.Count == 0)
                return Db;


            decimal[] data1 = new decimal[distinctduration.Count];



            for (int j = 0; j < distinctproducttype.Count; j++)
            {

                data1 = new decimal[distinctduration.Count];
                for (int i = 0; i < distinctduration.Count; i++)
                {
                    data1[i] = ss.Where(b => b.ProductTypeId == distinctproducttype[j].Key && b.Duration == distinctduration[i].Duration).Select(c => c.TotalPrice).FirstOrDefault();

                }

                Db.Add(new ConductforecastDasboard
                {

                    name = distinctproducttype[j].producttypename,
                    data = data1

                });



            }
            return Db;

        }



        public IList<ConductforecastDasboard> Getforecastsummarydurationforsitenew(int fid)
        {

            //var forecastinfo = ctx.ForecastInfo.Find(fid);


            var ss = ctx.ForecastedResult.Join(ctx.ForecastInfo, b => b.ForecastId, c => c.ForecastID, (b, c) => new { b, c })
                   .Where(s => s.b.PackPrice != 0 && s.b.ForecastId == fid) //&& s.b.SiteId == id && s.b.DurationDateTime >= s.c.StartDate && s.b.DurationDateTime < getmaxdate(s.c.Period, s.c.StartDate, s.c.Extension)
                .GroupBy(f => new { f.b.ProductTypeId, f.b.Duration, f.b.DurationDateTime }).Select(x => new
                {
                    TotalPrice = x.Sum(g => g.b.PackPrice),
                    ProductType = x.Max(g => g.b.ProductType),
                    x.Key.Duration,
                    x.Key.DurationDateTime,
                    x.Key.ProductTypeId


                }).OrderBy(h => h.DurationDateTime).ToList();


            var distinctproducttype = ss.GroupBy(s => s.ProductTypeId)
                .Select(x => new { x.Key, producttypename = x.Max(f => f.ProductType) }).ToList();

            var distinctduration = ss.GroupBy(s => s.DurationDateTime).Select(x => new { Duration = x.Max(f => f.Duration) }).ToList();

            List<ConductforecastDasboard> Db = new List<ConductforecastDasboard>();
            if (distinctduration.Count == 0)
                return Db;


            decimal[] data1 = new decimal[distinctduration.Count];



            for (int j = 0; j < distinctproducttype.Count; j++)
            {

                data1 = new decimal[distinctduration.Count];
                for (int i = 0; i < distinctduration.Count; i++)
                {
                    data1[i] = ss.Where(b => b.ProductTypeId == distinctproducttype[j].Key && b.Duration == distinctduration[i].Duration).Select(c => c.TotalPrice).FirstOrDefault();

                }

                Db.Add(new ConductforecastDasboard
                {

                    name = distinctproducttype[j].producttypename,
                    data = data1

                });



            }
            return Db;

        }


        public IList<ConductforecastDasboard> Getforecastsummarydurationforsiteservice(int fid)
        {

            //var forecastinfo = ctx.ForecastInfo.Find(fid);


            var ss = ctx.ForecastedResult.Join(ctx.Test, b => b.TestId, c => c.TestID, (b, c) => new { b, c })
                .Join(ctx.TestingArea, d => d.c.TestingAreaID, e => e.TestingAreaID, (d, e) => new { d, e })
                   .Where(s => s.d.b.ForecastId == fid) //&& s.b.SiteId == id && s.b.DurationDateTime >= s.c.StartDate && s.b.DurationDateTime < getmaxdate(s.c.Period, s.c.StartDate, s.c.Extension)
                .GroupBy(f => new { f.d.c.TestingAreaID, f.d.b.Duration, f.d.b.DurationDateTime }).Select(x => new
                {
                    TotalPrice = x.Sum(g => g.d.b.PackPrice),
                    TestingArea = x.Max(g => g.d.b.TestingArea),
                    x.Key.Duration,
                    x.Key.DurationDateTime,
                    x.Key.TestingAreaID


                }).OrderBy(h => h.DurationDateTime);


            var distinctproducttype = ss.GroupBy(s => s.TestingAreaID)
                .Select(x => new { x.Key, TestingArea = x.Max(f => f.TestingArea) }).ToList();

            var distinctduration = ss.GroupBy(s => s.DurationDateTime).Select(x => new { Duration = x.Max(f => f.Duration) }).ToList();

            List<ConductforecastDasboard> Db = new List<ConductforecastDasboard>();
            if (distinctduration.Count == 0)
                return Db;


            decimal[] data1 = new decimal[distinctduration.Count];



            for (int j = 0; j < distinctproducttype.Count; j++)
            {

                data1 = new decimal[distinctduration.Count];
                for (int i = 0; i < distinctduration.Count; i++)
                {
                    data1[i] = ss.Where(b => b.TestingAreaID == distinctproducttype[j].Key && b.Duration == distinctduration[i].Duration).Select(c => c.TotalPrice).FirstOrDefault();

                }

                Db.Add(new ConductforecastDasboard
                {

                    name = distinctproducttype[j].TestingArea,
                    data = data1

                });



            }
            return Db;

        }

        public Array Getforecastsite(int id)
        {
            Array A;
            var forecastinfo = ctx.ForecastInfo.Find(id);
            if (forecastinfo.DataUsage == "DATA_USAGE1" || forecastinfo.DataUsage == "DATA_USAGE2")
            {
                A = ctx.ForecastSite.Join(ctx.Site, b => b.SiteId, c => c.SiteID, (b, c) => new { b, c }).Where(s => s.b.ForecastInfoId == id)
                    .Select(x => new
                    {

                        siteid = x.b.SiteId,
                        sitename = x.c.SiteName
                    }).ToArray();
            }
            else
            {
                A = ctx.ForecastCategory.Where(b => b.ForecastId == id).Select(x => new
                {
                    siteid = x.CategoryId,
                    sitename = x.CategoryName

                }).ToArray();
            }

            return A;
        }
        public Array Getforecastlist(string metho, string datausage, int userid)
        {
            var forcastlist = ctx.ForecastInfo.Where(b => b.Methodology == metho && b.DataUsage == datausage && b.UserId == userid).Select(x => new
            {
                x.ForecastID,
                x.ForecastNo
            }

            ).ToArray();
            return forcastlist;
        }
        public string Calculateforecast(int id, string MethodType)
        {
            string res = "";

            try

            {
                param1 = MethodType.TrimEnd(',').Split(",");
                var _forecastInfo = ctx.ForecastInfo.Find(id);
                if (_forecastInfo.Status != "OPEN")
                {
                    //if (
                    //    MessageBox.Show(
                    //        "This forecast is already forecasted.Are you sure do you want to overwrite this forecast?",
                    //        "Forecasting Process", MessageBoxButtons.YesNo, MessageBoxIcon.Question) !=
                    //    DialogResult.Yes)
                    //{
                    //    return;
                    //}
                    var forecastresult = ctx.ForecastedResult.Where(b => b.ForecastId == id).ToList();
                    ctx.ForecastedResult.RemoveRange(forecastresult);
                    ctx.SaveChanges();
                }



                _listFresult = new List<ForecastedResult>();

                if (_forecastInfo.DataUsage == "DATA_USAGE1" ||
                    _forecastInfo.DataUsage == "DATA_USAGE2")
                    ForcastSiteHistoricalData(_forecastInfo);
                else
                    ForcastCategoryHistoricalData(_forecastInfo);



                SaveBulkForecastedResult();

                _forecastWithoutError = true;
                if (_forecastWithoutError == true)
                {
                    _forecastInfo.Status = "CLOSED";
                    _forecastInfo.Method = param1[0];
                    _forecastInfo.ForecastDate = DateTime.Now;
                    _forecastInfo.Westage = Convert.ToDecimal(param1[1]);
                    _forecastInfo.Scaleup = Convert.ToDecimal(param1[2]);
                    ctx.SaveChanges();
                    res = "Forecasting process completed successfully";
                }


            }
            catch (Exception ex)
            {
                _forecastWithoutError = false;

                res = "";

            }

            return res;
        }
        public void ForcastCategoryHistoricalData(ForecastInfo FiFo)
        {

            var forecastcategory = ctx.ForecastCategory.Where(b => b.ForecastId == FiFo.ForecastID).ToList();

            foreach (ForecastCategory c in forecastcategory)
            {
                if (FiFo.Methodology == "CONSUMPTION")
                {
                    IList<int> products = ctx.ForecastCategoryProduct.Where(b => b.CategoryID == c.CategoryId).GroupBy(x => x.ProductID).Select(x => x.Key).ToList();
                    foreach (int p in products)
                    {
                        //   ClearChart1SeriesPoints();
                        IList<ForecastCategoryProduct> fcatProduct = ctx.ForecastCategoryProduct.Where(b => b.CategoryID == c.CategoryId && b.ProductID == p).OrderBy(x => x.DurationDateTime).ToList();
                        DataTable InputDs = new DataTable();
                        InputDs.Columns.Add("X");
                        InputDs.Columns.Add("Y");
                        foreach (ForecastCategoryProduct cp in fcatProduct)
                        {


                            DataRow Dr = InputDs.NewRow();
                            Dr["X"] = cp.DurationDateTime.Value;
                            Dr["Y"] = cp.Adjusted;
                            InputDs.Rows.Add(Dr);
                        }





                        DataTable ds = Calculateforecastmultiplemethod(InputDs, FiFo);

                        ReadDataset(FiFo, 0, c.CategoryId, p, 0, ds, lastEntryDate, InputDs);
                    }
                }
                else if (FiFo.Methodology == "SERVICE_STATISTIC")
                {
                    IList<int> tests = ctx.ForecastCategoryTest.Where(b => b.CategoryID == c.CategoryId).GroupBy(x => x.TestID).Select(x => x.Key).ToList();
                    foreach (int p in tests)
                    {

                        IList<ForecastCategoryTest> fcatTest = ctx.ForecastCategoryTest.Where(b => b.CategoryID == c.CategoryId && b.TestID == p).OrderBy(x => x.DurationDateTime).ToList();

                        DataTable InputDs = new DataTable();
                        InputDs.Columns.Add("X");
                        InputDs.Columns.Add("Y");
                        foreach (ForecastCategoryTest ft in fcatTest)
                        {
                            DataRow Dr = InputDs.NewRow();
                            Dr["X"] = ft.DurationDateTime.Value;
                            Dr["Y"] = ft.Adjusted;
                            InputDs.Rows.Add(Dr);
                        }
                        DataTable ds = Calculateforecastmultiplemethod(InputDs, FiFo);

                        ReadDatasetservice(FiFo, 0, c.CategoryId, p, 0, ds, lastEntryDate, InputDs);
                    }
                }
            }
        }

        private void SaveBulkForecastedResult()
        {
            //SqlConnection sqlConnection = ConnectionManager.GetInstance().GetSqlConnection();
            //DataTable fresultdt = GenericToDataTable.ConvertTo(_listFresult);
            //if (sqlConnection.State != ConnectionState.Open)
            //{
            //    sqlConnection.Open();
            //}
            //using (var bulkCopy = new SqlBulkCopy(sqlConnection))
            //{
            //    bulkCopy.DestinationTableName = "dbo.ForecastedResult";
            //    bulkCopy.WriteToServer(fresultdt);
            //}
            ctx.ForecastedResult.AddRange(_listFresult);
            ctx.SaveChanges();
        }
        private void ForcastSiteHistoricalData(ForecastInfo finfo)
        {
            var forecastsite = ctx.ForecastSite.Where(b => b.ForecastInfoId == finfo.ForecastID).ToList();
            foreach (ForecastSite s in forecastsite)
            {
                if (finfo.Methodology == "CONSUMPTION")
                {
                    IList<int> products = ctx.ForecastSiteProduct.Where(b => b.ForecastSiteID == s.Id).GroupBy(x => x.ProductID).Select(x => x.Key).ToList();


                    foreach (int p in products)
                    {

                        DataTable InputDs = new DataTable();
                        InputDs.Columns.Add("X");
                        InputDs.Columns.Add("Y");

                        IList<ForecastSiteProduct> fsiteProduct = ctx.ForecastSiteProduct.Where(b => b.ForecastSiteID == s.Id && b.ProductID == p).OrderBy(x => x.DurationDateTime).ToList();
                        foreach (ForecastSiteProduct sp in fsiteProduct)
                        {
                            DataRow Dr = InputDs.NewRow();
                            Dr["X"] = sp.DurationDateTime.Value;
                            Dr["Y"] = sp.Adjusted;
                            InputDs.Rows.Add(Dr);

                        }

                        //gets last Input date
                        lastEntryDate = fsiteProduct[fsiteProduct.Count - 1].DurationDateTime.Value;
                        DataTable ds = Calculateforecastmultiplemethod(InputDs, finfo);
                        //   CalculateChart1(finfo.Extension);

                        //   DataTable ds = chart1.DataManipulator.ExportSeriesValues("Forecasting");

                        //
                        ReadDataset(finfo, s.SiteId, 0, p, 0, ds, lastEntryDate, InputDs);

                    }
                }
                else if (finfo.Methodology == "SERVICE STATSTICS")
                {
                    IList<int> tests = ctx.ForecastSiteTest.Where(b => b.ForecastSiteID == s.Id).GroupBy(x => x.TestID).Select(f => f.Key).ToList();

                    foreach (int p in tests)
                    {

                        IList<ForecastSiteTest> fsiteTest = ctx.ForecastSiteTest.Where(b => b.ForecastSiteID == s.Id && b.TestID == p).OrderBy(f => f.DurationDateTime).ToList();


                        DataTable InputDs = new DataTable();
                        InputDs.Columns.Add("X");
                        InputDs.Columns.Add("Y");
                        foreach (ForecastSiteTest sp in fsiteTest)
                        {
                            DataRow Dr = InputDs.NewRow();
                            Dr["X"] = sp.DurationDateTime.Value;
                            Dr["Y"] = sp.Adjusted;
                            InputDs.Rows.Add(Dr);
                        }


                        lastEntryDate = fsiteTest[fsiteTest.Count - 1].DurationDateTime.Value;

                        DataTable ds = Calculateforecastmultiplemethod(InputDs, finfo);

                        //CalculateChart1(finfo.Extension); m,
                        //DataTable ds = chart1.DataManipulator.ExportSeriesValues("Forecasting");
                        ReadDatasetservice(finfo, s.SiteId, 0, 0, p, ds, lastEntryDate, InputDs);

                    }
                }
            }
        }

        private void ReadDataset(ForecastInfo finfo, int siteid, int catid, int proid, int testid, DataTable ds, DateTime lastDate, DataTable inputDs)
        {
            int period = 0;

            foreach (DataRow row in ds.Rows)
            {
                DateTime ddate = Convert.ToDateTime(row[0]);

                //if (ddate.Day > 29)
                //{
                //    if (ddate.Month == 12)
                //        ddate = new DateTime(ddate.Year + 1, 1, 1);
                //    ddate = new DateTime(ddate.Year, ddate.Month+1, 1);
                //}

                ForecastedResult fresult = new ForecastedResult();
                fresult.ForecastId = finfo.ForecastID;
                fresult.SiteId = siteid;
                fresult.CategoryId = catid;
                fresult.TestId = testid;
                fresult.ProductId = proid;
                fresult.DurationDateTime = Utility.Utility.CorrectDateTime(ddate.Date);//ddate.Date;

                if (Convert.ToDecimal(row[1]) < 0)
                    fresult.ForecastValue = 0;
                else
                    fresult.ForecastValue = Convert.ToDecimal(row[1]);


                if (ddate > lastDate)
                {
                    fresult.IsHistory = false;
                    fresult.HistoricalValue = 0;
                }
                else
                {
                    DataRow rowinput = inputDs.Rows[period];
                    fresult.HistoricalValue = Convert.ToDecimal(rowinput[1]);
                    fresult.IsHistory = true;
                }

                if (finfo.Period == "Monthly" || finfo.Period == "Bimonthly")
                    fresult.Duration = String.Format("{0}-{1}", Utility.Utility.Months[fresult.DurationDateTime.Month - 1], fresult.DurationDateTime.Year);
                else if (finfo.Period == "Quarterly")
                    fresult.Duration = String.Format("Qua{0}-{1}", Utility.Utility.GetQuarter(fresult.DurationDateTime), fresult.DurationDateTime.Year);
                else
                    fresult.Duration = String.Format("{0}", fresult.DurationDateTime.Year);

                //fresult.TotalValue = fresult.ForecastValue + (fresult.ForecastValue * (Convert.ToInt32(param1[1]) / 100));
                //fresult.TotalValue += prevValue * (Convert.ToInt32(param1[2]) / 100);
                //prevValue = fresult.TotalValue;

                fresult.ServiceConverted = false;

                Site fsite = null;
                if (fresult.SiteId > 0)
                    fsite = ctx.Site.Find(fresult.SiteId);

                ForecastCategory fc = null;
                if (fresult.CategoryId > 0)
                    fc = ctx.ForecastCategory.Where(b => b.CategoryId == fresult.CategoryId).FirstOrDefault();


                int workingdays;
                #region Forecast Period Conversion
                if (finfo.DataUsage == "DATA_USAGE3")
                {
                    int[] sitesid = ctx.ForecastCategorySite.Where(c => c.CategoryID == catid).Select(s => s.SiteID).ToArray();
                    workingdays = Convert.ToInt32(ctx.Site.Where(b => sitesid.Contains(b.SiteID)).Average(s => s.WorkingDays));


                }
                else
                {
                    workingdays = fsite.WorkingDays;

                }
                string fprd = finfo.Period;
                decimal fPinDay = 0, fPinWeek = 0, fPinMonth = 0, fPinQuarter = 0, fPinYear = 0;
                if (fprd == "Yearly")
                {
                    fPinDay = workingdays * 12;
                    fPinMonth = 12;
                    fPinWeek = (workingdays / 4) * 12;
                    fPinQuarter = 4;
                    fPinYear = 1;
                }
                if (fprd == "Quarterly")
                {
                    fPinDay = workingdays * 3;
                    fPinMonth = 3;
                    fPinWeek = (workingdays / 4) * 3;
                    fPinQuarter = 1;
                    fPinYear = 1 / 4;
                }
                if (fprd == "Bimonthly")
                {
                    fPinDay = workingdays * 2;
                    fPinMonth = 2;
                    fPinWeek = (workingdays / 4) * 2;
                    fPinQuarter = 2 / 3;
                    fPinYear = 1 / 6;
                }
                if (fprd == "Monthly")
                {
                    fPinDay = workingdays;
                    fPinMonth = 1;
                    fPinWeek = (workingdays / 4);
                    fPinQuarter = 1 / 3;
                    fPinYear = 1 / 12;
                }
                #endregion




                //get TestingArea
                if (fresult.TestId > 0)
                {
                    Test test = ctx.Test.Find(fresult.TestId);
                    fresult.TestingArea = ctx.TestingArea.Find(test.TestingAreaID).AreaName;

                }

                int Nopack = int.Parse(decimal.Round(fresult.ForecastValue, 0).ToString());

                if (Nopack < fresult.ForecastValue)
                    Nopack = Nopack + 1;
                if (Nopack == 0)
                    Nopack = 0;
                fresult.TotalValue = Nopack;
                //get packQty
                if (fresult.ProductId > 0)
                {
                    MasterProduct p = ctx.MasterProduct.Find(fresult.ProductId);
                    p._productPrices = ctx.ProductPrice.Where(b => b.ProductId == p.ProductID).ToList();
                    //converting quantity to packsize commented out
                    //int packSize = p.GetActiveProductPrice(fresult.DurationDateTime).PackSize;
                    //fresult.PackQty = GetNoofPackage(packSize, fresult.TotalValue);

                    //rounding forecasted pack quantity

                    fresult.PackQty = Nopack;

                    fresult.PackPrice = fresult.PackQty * p.GetActiveProductPrice(fresult.DurationDateTime).packcost;

                    fresult.ProductTypeId = p.ProductTypeId;
                    fresult.ProductType = ctx.ProductType.Find(p.ProductTypeId).TypeName;
                    _listFresult.Add(fresult);

                }



                //test to product
                if (fresult.TestId > 0)
                {
                    Test test = ctx.Test.Find(fresult.TestId);

                    #region Forecast General Consumables


                    IList<ForecastedResult> _consumablesDailyFlist = new List<ForecastedResult>();


                    foreach (ConsumableUsage cusage in GetAllConsumableUsageByTestId(fresult.TestId))//DataRepository.GetConsumableUsageByTestId(fresult.TestId))
                    {
                        //
                        ForecastedResult consumableFresult = new ForecastedResult();
                        //copyvalues
                        consumableFresult.ForecastId = fresult.ForecastId;
                        consumableFresult.TestId = fresult.TestId;
                        consumableFresult.DurationDateTime = fresult.DurationDateTime;
                        consumableFresult.SiteId = fresult.SiteId;
                        consumableFresult.CategoryId = fresult.CategoryId;
                        consumableFresult.Duration = fresult.Duration;
                        consumableFresult.IsHistory = fresult.IsHistory;
                        consumableFresult.TestingArea = fresult.TestingArea;
                        consumableFresult.ForecastValue = fresult.ForecastValue;
                        consumableFresult.TotalValue = fresult.TotalValue;
                        //endcopy
                        decimal Qty = 0;


                        if (cusage.PerInstrument)
                        {
                            if (finfo.DataUsage == "DATA_USAGE1" || finfo.DataUsage == "DATA_USAGE2")
                            {
                                if (fsite != null)
                                {
                                    SiteInstrument siteins = ctx.siteinstrument.Where(b => b.InstrumentID == cusage.InstrumentId && b.SiteID == fsite.SiteID).FirstOrDefault();
                                    if (siteins != null)
                                    {
                                        if (cusage.Period == "Daily")
                                        {
                                            Qty = cusage.UsageRate * fPinDay;
                                        }
                                        if (cusage.Period == "Weekly")
                                        {
                                            Qty = cusage.UsageRate * fPinWeek;
                                        }
                                        if (cusage.Period == "Monthly")
                                        {
                                            Qty = cusage.UsageRate * fPinMonth;
                                        }
                                        if (cusage.Period == "Quarterly")
                                        {
                                            Qty = cusage.UsageRate * fPinQuarter;
                                        }
                                        if (cusage.Period == "Yearly")
                                        {
                                            Qty = cusage.UsageRate * fPinYear;
                                        }
                                        Qty = Qty * siteins.Quantity;
                                    }
                                }
                            }
                        }
                        if (cusage.PerPeriod)
                        {
                            if (finfo.DataUsage == "DATA_USAGE1" || finfo.DataUsage == "DATA_USAGE2")
                            {
                                if (fsite != null)
                                {
                                    if (cusage.Period == "Daily")
                                    {
                                        Qty = cusage.UsageRate * fPinDay;
                                    }
                                    if (cusage.Period == "Weekly")
                                    {
                                        Qty = cusage.UsageRate * fPinWeek;
                                    }
                                    if (cusage.Period == "Monthly")
                                    {
                                        Qty = cusage.UsageRate * fPinMonth;
                                    }
                                    if (cusage.Period == "Quarterly")
                                    {
                                        Qty = cusage.UsageRate * fPinQuarter;
                                    }
                                    if (cusage.Period == "Yearly")
                                    {
                                        Qty = cusage.UsageRate * fPinYear;
                                    }
                                }
                            }
                        }
                        if (cusage.PerTest)
                        {
                            if (finfo.DataUsage == "DATA_USAGE1" || finfo.DataUsage == "DATA_USAGE2")
                            {
                                if (fsite != null)
                                {
                                    Qty = cusage.UsageRate * (fresult.TotalValue / cusage.NoOfTest);
                                }
                            }
                        }

                        consumableFresult.TotalValue = Qty;
                        MasterProduct MP = new MasterProduct();
                        MP = ctx.MasterProduct.Find(cusage.ProductId);
                        MP._productPrices = ctx.ProductPrice.Where(b => b.ProductId == MP.ProductID).ToList();
                        int packSize = MP.GetActiveProductPrice(fresult.DurationDateTime).packsize;
                        consumableFresult.ProductId = cusage.ProductId;
                        consumableFresult.PackQty = GetNoofPackage(packSize, Qty);
                        consumableFresult.PackPrice = consumableFresult.PackQty * MP.GetActiveProductPrice(fresult.DurationDateTime).packcost;
                        consumableFresult.ProductTypeId = MP.ProductTypeId;
                        consumableFresult.ProductType = ctx.ProductType.Find(MP.ProductTypeId).TypeName;
                        consumableFresult.IsForGeneralConsumable = true;
                        consumableFresult.ServiceConverted = true;
                        _listFresult.Add(consumableFresult);


                    }



                    #endregion

                    #region Forecast Control Test

                    if (finfo.DataUsage == "DATA_USAGE1" || finfo.DataUsage == "DATA_USAGE2")
                    {
                        if (fsite != null)
                        {
                            SiteInstrument siteins = ctx.siteinstrument.Join(ctx.Instrument, b => b.InstrumentID, c => c.InstrumentID, (b, c) => new { b, c }).Where(x => x.c.testingArea.TestingAreaID == test.TestingAreaID && x.b.SiteID == fsite.SiteID).GroupBy(s => s.c.testingArea.TestingAreaID)
                                .Select(s => new SiteInstrument
                                {

                                    ID = s.Max(x => x.b.ID),
                                    InstrumentID = s.Max(x => x.b.InstrumentID)
                                }).FirstOrDefault();
                            //fsite.GetSiteInstrumentByTA(test.TestingArea.Id);
                            if (siteins != null)
                            {
                                if (ctx.Instrument.Where(s => s.InstrumentID == siteins.InstrumentID).Select(s => s.DailyCtrlTest).FirstOrDefault() > 0)
                                {
                                    fresult.ControlTest = fPinDay * siteins.Quantity * ctx.Instrument.Where(s => s.InstrumentID == siteins.InstrumentID).Select(s => s.DailyCtrlTest).FirstOrDefault();
                                }
                                if (ctx.Instrument.Where(s => s.InstrumentID == siteins.InstrumentID).Select(s => s.WeeklyCtrlTest).FirstOrDefault() > 0)
                                {
                                    fresult.ControlTest = fPinWeek * siteins.Quantity * ctx.Instrument.Where(s => s.InstrumentID == siteins.InstrumentID).Select(s => s.WeeklyCtrlTest).FirstOrDefault();
                                }
                                if (ctx.Instrument.Where(s => s.InstrumentID == siteins.InstrumentID).Select(s => s.MonthlyCtrlTest).FirstOrDefault() > 0)
                                {
                                    fresult.ControlTest = fPinMonth * siteins.Quantity * ctx.Instrument.Where(s => s.InstrumentID == siteins.InstrumentID).Select(s => s.MonthlyCtrlTest).FirstOrDefault();

                                }
                                if (ctx.Instrument.Where(s => s.InstrumentID == siteins.InstrumentID).Select(s => s.QuarterlyCtrlTest).FirstOrDefault() > 0)
                                {
                                    fresult.ControlTest = fPinQuarter * siteins.Quantity * ctx.Instrument.Where(s => s.InstrumentID == siteins.InstrumentID).Select(s => s.QuarterlyCtrlTest).FirstOrDefault();
                                }
                                if (ctx.Instrument.Where(s => s.InstrumentID == siteins.InstrumentID).Select(s => s.MaxTestBeforeCtrlTest).FirstOrDefault() > 0)
                                {
                                    fresult.ControlTest = ((fresult.TotalValue * (siteins.TestRunPercentage / 100)) / ctx.Instrument.Where(s => s.InstrumentID == siteins.InstrumentID).Select(s => s.MaxTestBeforeCtrlTest).FirstOrDefault());
                                }
                            }
                        }
                    }

                    #endregion

                    #region Test Test to Product
                    var productusage = ctx.ProductUsage.Where(b => b.TestId == test.TestID && b.IsForControl == false).ToList();
                    foreach (ProductUsage pu in productusage) //change on aug 22.2014 (ProductUsage pu in test.ProductUsages)
                    {
                        ForecastedResult cfresult = new ForecastedResult();
                        //copyvalues
                        cfresult.ForecastId = fresult.ForecastId;
                        cfresult.TestId = fresult.TestId;
                        cfresult.DurationDateTime = fresult.DurationDateTime;
                        cfresult.SiteId = fresult.SiteId;
                        cfresult.CategoryId = fresult.CategoryId;
                        cfresult.Duration = fresult.Duration;
                        cfresult.IsHistory = fresult.IsHistory;
                        cfresult.TestingArea = fresult.TestingArea;
                        //endcopy

                        if (finfo.DataUsage == "DATA_USAGE1" || finfo.DataUsage == "DATA_USAGE2")
                        {
                            if (fsite != null)
                            {
                                SiteInstrument siteins = ctx.siteinstrument.Where(b => b.InstrumentID == pu.InstrumentId && b.SiteID == fsite.SiteID).FirstOrDefault(); if (siteins != null)
                                {
                                    decimal Qty = pu.Rate * fresult.TotalValue * siteins.TestRunPercentage / 100;
                                    cfresult.TotalValue = Qty;
                                    MasterProduct p = ctx.MasterProduct.Find(pu.ProductId);
                                    p._productPrices = ctx.ProductPrice.Where(b => b.ProductId == p.ProductID).ToList();
                                    int packSize = p.GetActiveProductPrice(fresult.DurationDateTime).packsize;
                                    cfresult.ProductId = pu.ProductId;
                                    cfresult.PackQty = GetNoofPackage(packSize, Qty);
                                    cfresult.PackPrice = cfresult.PackQty * p.GetActiveProductPrice(fresult.DurationDateTime).packcost;

                                    cfresult.ProductTypeId = ctx.MasterProduct.Find(pu.ProductId).ProductTypeId;
                                    cfresult.ProductType = ctx.ProductType.Find(ctx.MasterProduct.Find(pu.ProductId).ProductTypeId).TypeName;
                                    cfresult.ServiceConverted = true;
                                    _listFresult.Add(cfresult);
                                }
                            }
                        }
                        else
                        {
                            //if (fsite != null)
                            //{
                            //    ForecastCategoryInstrument fcins = DataRepository.GetForecastCategoryInstrumentById(pu.Instrument.Id);

                            //    if (fcins != null)
                            //    {
                            //        decimal Qty = pu.Rate * fresult.TotalValue * fcins.TestRunPercentage;
                            //        cfresult.TotalValue = Qty;
                            //        int packSize = pu.Product.GetActiveProductPrice(fresult.DurationDateTime).PackSize;
                            //        cfresult.ProductId = pu.Product.Id;
                            //        cfresult.PackQty = GetNoofPackage(packSize, Qty);
                            //        cfresult.PackPrice = cfresult.PackQty * pu.Product.GetActiveProductPrice(fresult.DurationDateTime).Price;

                            //        cfresult.ProductTypeId = pu.Product.ProductType.Id;
                            //        cfresult.ProductType = pu.Product.ProductType.TypeName;
                            //        cfresult.ServiceConverted = true;
                            //        _listFresult.Add(cfresult);
                            //    }
                            //}
                        }
                    }
                    ///////// 
                    #endregion

                    #region Control Test to Product



                    foreach (ProductUsage pu in test.GetProductUsageByType(true)) //change on aug 22.2014 (ProductUsage pu in test.ProductUsages)
                    {
                        ForecastedResult cfresult = new ForecastedResult();
                        //copyvalues
                        cfresult.ForecastId = fresult.ForecastId;
                        cfresult.TestId = fresult.TestId;
                        cfresult.DurationDateTime = fresult.DurationDateTime;
                        cfresult.SiteId = fresult.SiteId;
                        cfresult.CategoryId = fresult.CategoryId;
                        cfresult.Duration = fresult.Duration;
                        cfresult.IsHistory = fresult.IsHistory;
                        cfresult.TestingArea = fresult.TestingArea;

                        cfresult.IsForControl = true;
                        //endcopy

                        if (finfo.DataUsage == "DATA_USAGE1" || finfo.DataUsage == "DATA_USAGE2")
                        {
                            if (fsite != null)
                            {
                                SiteInstrument siteins = ctx.siteinstrument.Where(b => b.InstrumentID == pu.InstrumentId && b.SiteID == fsite.SiteID).FirstOrDefault();
                                if (siteins != null)
                                {


                                    decimal Qty = 0;
                                    if (ctx.Instrument.Where(s => s.InstrumentID == siteins.InstrumentID).Select(s => s.DailyCtrlTest).FirstOrDefault() > 0)
                                    {
                                        Qty = pu.Rate * fPinDay * siteins.Quantity * ctx.Instrument.Where(s => s.InstrumentID == siteins.InstrumentID).Select(s => s.DailyCtrlTest).FirstOrDefault();
                                        // cfresult.ForecastValue = fPinDay * siteins.Quantity * siteins.Instrument.DailyCtrlTest;
                                    }
                                    if (ctx.Instrument.Where(s => s.InstrumentID == siteins.InstrumentID).Select(s => s.WeeklyCtrlTest).FirstOrDefault() > 0)
                                    {
                                        Qty = pu.Rate * fPinWeek * siteins.Quantity * ctx.Instrument.Where(s => s.InstrumentID == siteins.InstrumentID).Select(s => s.WeeklyCtrlTest).FirstOrDefault();
                                        // cfresult.ForecastValue = fPinWeek * siteins.Quantity * siteins.Instrument.WeeklyCtrlTest;
                                    }
                                    if (ctx.Instrument.Where(s => s.InstrumentID == siteins.InstrumentID).Select(s => s.MonthlyCtrlTest).FirstOrDefault() > 0)
                                    {
                                        Qty = pu.Rate * fPinMonth * siteins.Quantity * ctx.Instrument.Where(s => s.InstrumentID == siteins.InstrumentID).Select(s => s.MonthlyCtrlTest).FirstOrDefault();
                                        // cfresult.ForecastValue = fPinMonth * siteins.Quantity * siteins.Instrument.MonthlyCtrlTest;

                                    }
                                    if (ctx.Instrument.Where(s => s.InstrumentID == siteins.InstrumentID).Select(s => s.QuarterlyCtrlTest).FirstOrDefault() > 0)
                                    {
                                        Qty = pu.Rate * fPinQuarter * siteins.Quantity * ctx.Instrument.Where(s => s.InstrumentID == siteins.InstrumentID).Select(s => s.QuarterlyCtrlTest).FirstOrDefault();
                                        //cfresult.ForecastValue = fPinQuarter * siteins.Quantity * siteins.Instrument.QuarterlyCtrlTest;
                                    }
                                    if (ctx.Instrument.Where(s => s.InstrumentID == siteins.InstrumentID).Select(s => s.MaxTestBeforeCtrlTest).FirstOrDefault() > 0)
                                    {
                                        Qty = pu.Rate * ((fresult.TotalValue * (siteins.TestRunPercentage / 100)) / ctx.Instrument.Where(s => s.InstrumentID == siteins.InstrumentID).Select(s => s.MaxTestBeforeCtrlTest).FirstOrDefault());
                                        // cfresult.ForecastValue = ((fresult.TotalValue * (siteins.TestRunPercentage / 100)) / siteins.Instrument.MaxTestBeforeCtrlTest);
                                    }


                                    //decimal Qty = pu.Rate * fresult.TotalValue * siteins.TestRunPercentage / 100;

                                    cfresult.TotalValue = Qty;
                                    MasterProduct p = ctx.MasterProduct.Find(pu.ProductId);
                                    p._productPrices = ctx.ProductPrice.Where(b => b.ProductId == p.ProductID).ToList();
                                    int packSize = p.GetActiveProductPrice(fresult.DurationDateTime).packsize;
                                    cfresult.ProductId = pu.ProductId;
                                    cfresult.PackQty = GetNoofPackage(packSize, Qty);
                                    cfresult.PackPrice = cfresult.PackQty * p.GetActiveProductPrice(fresult.DurationDateTime).packcost;

                                    cfresult.ProductTypeId = ctx.MasterProduct.Find(pu.ProductId).ProductTypeId;
                                    cfresult.ProductType = ctx.ProductType.Find(ctx.MasterProduct.Find(pu.ProductId).ProductTypeId).TypeName;
                                    cfresult.ServiceConverted = true;
                                    _listFresult.Add(cfresult);
                                }
                            }
                        }
                        else
                        {
                            //if (fsite != null)
                            //{
                            //    ForecastCategoryInstrument fcins = DataRepository.GetForecastCategoryInstrumentById(pu.Instrument.Id);

                            //    if (fcins != null)
                            //    {
                            //        decimal Qty = 0;
                            //        if (fcins.Instrument.DailyCtrlTest > 0)
                            //        {
                            //            Qty = pu.Rate * fPinDay * fcins.Instrument.DailyCtrlTest;
                            //            //cfresult.ForecastValue = fPinDay * fcins.Instrument.DailyCtrlTest;
                            //        }
                            //        if (fcins.Instrument.WeeklyCtrlTest > 0)
                            //        {
                            //            Qty = pu.Rate * fPinWeek * fcins.Instrument.WeeklyCtrlTest;
                            //            //cfresult.ForecastValue = fPinWeek * fcins.Instrument.WeeklyCtrlTest;
                            //        }
                            //        if (fcins.Instrument.MonthlyCtrlTest > 0)
                            //        {
                            //            Qty = pu.Rate * fPinMonth * fcins.Instrument.MonthlyCtrlTest;
                            //            //cfresult.ForecastValue = fPinMonth * fcins.Instrument.MonthlyCtrlTest;
                            //        }
                            //        if (fcins.Instrument.QuarterlyCtrlTest > 0)
                            //        {
                            //            Qty = pu.Rate * fPinQuarter * fcins.Instrument.QuarterlyCtrlTest;
                            //            //cfresult.ForecastValue = fPinQuarter * fcins.Instrument.QuarterlyCtrlTest;
                            //        }
                            //        if (fcins.Instrument.MaxTestBeforeCtrlTest > 0)
                            //        {
                            //            Qty = pu.Rate * ((fresult.TotalValue * (fcins.TestRunPercentage / 100)) / fcins.Instrument.MaxTestBeforeCtrlTest);
                            //            //cfresult.ForecastValue = ((fresult.TotalValue * (fcins.TestRunPercentage / 100)) / fcins.Instrument.MaxTestBeforeCtrlTest);
                            //        }

                            //        //decimal Qty = pu.Rate * fresult.TotalValue * fcins.TestRunPercentage;
                            //        cfresult.TotalValue = Qty;
                            //        int packSize = pu.Product.GetActiveProductPrice(fresult.DurationDateTime).PackSize;
                            //        cfresult.ProductId = pu.Product.Id;
                            //        cfresult.PackQty = GetNoofPackage(packSize, Qty);
                            //        cfresult.PackPrice = cfresult.PackQty * pu.Product.GetActiveProductPrice(fresult.DurationDateTime).Price;

                            //        cfresult.ProductTypeId = pu.Product.ProductType.Id;
                            //        cfresult.ProductType = pu.Product.ProductType.TypeName;
                            //        cfresult.ServiceConverted = true;
                            //        cfresult.IsForControl = true;
                            //        _listFresult.Add(cfresult);
                            //    }
                            //}
                        }
                    }
                    ///////// 
                    #endregion

                }
                //end test to product

                period++;
            }
        }


        private void ReadDatasetservice(ForecastInfo finfo, int siteid, int catid, int proid, int testid, DataTable ds, DateTime lastDate, DataTable inputDs)
        {
            int period = 0;

            foreach (DataRow row in ds.Rows)
            {
                DateTime ddate = Convert.ToDateTime(row[0]);

                //if (ddate.Day > 29)
                //{
                //    if (ddate.Month == 12)
                //        ddate = new DateTime(ddate.Year + 1, 1, 1);
                //    ddate = new DateTime(ddate.Year, ddate.Month+1, 1);
                //}

                ForecastedResult fresult = new ForecastedResult();
                fresult.ForecastId = finfo.ForecastID;
                fresult.SiteId = siteid;
                fresult.CategoryId = catid;
                fresult.TestId = testid;
                fresult.ProductId = proid;
                fresult.DurationDateTime = Utility.Utility.CorrectDateTime(ddate.Date);//ddate.Date;

                if (Convert.ToDecimal(row[1]) < 0)
                    fresult.ForecastValue = 0;
                else
                    fresult.ForecastValue = Convert.ToDecimal(row[1]);


                if (ddate > lastDate)
                {
                    fresult.IsHistory = false;
                    fresult.HistoricalValue = 0;
                }
                else
                {
                    DataRow rowinput = inputDs.Rows[period];
                    fresult.HistoricalValue = Convert.ToDecimal(rowinput[1]);
                    fresult.IsHistory = true;
                }

                if (finfo.Period == "Monthly" || finfo.Period == "Bimonthly")
                    fresult.Duration = String.Format("{0}-{1}", Utility.Utility.Months[fresult.DurationDateTime.Month - 1], fresult.DurationDateTime.Year);
                else if (finfo.Period == "Quarterly")
                    fresult.Duration = String.Format("Qua{0}-{1}", Utility.Utility.GetQuarter(fresult.DurationDateTime), fresult.DurationDateTime.Year);
                else
                    fresult.Duration = String.Format("{0}", fresult.DurationDateTime.Year);

                //fresult.TotalValue = fresult.ForecastValue + (fresult.ForecastValue * (Convert.ToInt32(param1[1]) / 100));
                //fresult.TotalValue += prevValue * (Convert.ToInt32(param1[2]) / 100);
                //prevValue = fresult.TotalValue;

                fresult.ServiceConverted = false;

                Site fsite = null;
                if (fresult.SiteId > 0)
                    fsite = ctx.Site.Find(fresult.SiteId);

                ForecastCategory fc = null;
                if (fresult.CategoryId > 0)
                    fc = ctx.ForecastCategory.Where(b => b.CategoryId == fresult.CategoryId).FirstOrDefault();


                int workingdays;
                #region Forecast Period Conversion
                if (finfo.DataUsage == "DATA_USAGE3")
                {
                    int[] sitesid = ctx.ForecastCategorySite.Where(c => c.CategoryID == catid).Select(s => s.SiteID).ToArray();
                    workingdays = Convert.ToInt32(ctx.Site.Where(b => sitesid.Contains(b.SiteID)).Average(s => s.WorkingDays));


                }
                else
                {
                    workingdays = fsite.WorkingDays;

                }
                string fprd = finfo.Period;
                decimal fPinDay = 0, fPinWeek = 0, fPinMonth = 0, fPinQuarter = 0, fPinYear = 0;
                if (fprd == "Yearly")
                {
                    fPinDay = workingdays * 12;
                    fPinMonth = 12;
                    fPinWeek = (workingdays / 4) * 12;
                    fPinQuarter = 4;
                    fPinYear = 1;
                }
                if (fprd == "Quarterly")
                {
                    fPinDay = workingdays * 3;
                    fPinMonth = 3;
                    fPinWeek = (workingdays / 4) * 3;
                    fPinQuarter = 1;
                    fPinYear = 1 / 4;
                }
                if (fprd == "Bimonthly")
                {
                    fPinDay = workingdays * 2;
                    fPinMonth = 2;
                    fPinWeek = (workingdays / 4) * 2;
                    fPinQuarter = 2 / 3;
                    fPinYear = 1 / 6;
                }
                if (fprd == "Monthly")
                {
                    fPinDay = workingdays;
                    fPinMonth = 1;
                    fPinWeek = (workingdays / 4);
                    fPinQuarter = 1 / 3;
                    fPinYear = 1 / 12;
                }
                #endregion




                //get TestingArea
                if (fresult.TestId > 0)
                {
                    Test test = ctx.Test.Find(fresult.TestId);
                    fresult.TestingArea = ctx.TestingArea.Find(test.TestingAreaID).AreaName;

                }

                int Nopack = int.Parse(decimal.Round(fresult.ForecastValue, 0).ToString());

                if (Nopack < fresult.ForecastValue)
                    Nopack = Nopack + 1;
                if (Nopack == 0)
                    Nopack = 0;
                fresult.TotalValue = Nopack;
                //get packQty
                if (fresult.ProductId > 0)
                {
                    MasterProduct p = ctx.MasterProduct.Find(fresult.ProductId);
                    p._productPrices = ctx.ProductPrice.Where(b => b.ProductId == p.ProductID).ToList();
                    //converting quantity to packsize commented out
                    //int packSize = p.GetActiveProductPrice(fresult.DurationDateTime).PackSize;
                    //fresult.PackQty = GetNoofPackage(packSize, fresult.TotalValue);

                    //rounding forecasted pack quantity

                    fresult.PackQty = Nopack;

                    fresult.PackPrice = fresult.PackQty * p.GetActiveProductPrice(fresult.DurationDateTime).packcost;

                    fresult.ProductTypeId = p.ProductTypeId;
                    fresult.ProductType = ctx.ProductType.Find(p.ProductTypeId).TypeName;
                    _listFresult.Add(fresult);

                }



                //test to product
                if (fresult.TestId > 0)
                {
                    Test test = ctx.Test.Find(fresult.TestId);

                    #region Forecast General Consumables


                    IList<ForecastedResult> _consumablesDailyFlist = new List<ForecastedResult>();


                    foreach (ForecastConsumableUsage cusage in GetAllserviceConsumableUsageByTestId(fresult.TestId, fresult.ForecastId))//DataRepository.GetConsumableUsageByTestId(fresult.TestId))
                    {
                        //
                        ForecastedResult consumableFresult = new ForecastedResult();
                        //copyvalues
                        consumableFresult.ForecastId = fresult.ForecastId;
                        consumableFresult.TestId = fresult.TestId;
                        consumableFresult.DurationDateTime = fresult.DurationDateTime;
                        consumableFresult.SiteId = fresult.SiteId;
                        consumableFresult.CategoryId = fresult.CategoryId;
                        consumableFresult.Duration = fresult.Duration;
                        consumableFresult.IsHistory = fresult.IsHistory;
                        consumableFresult.TestingArea = fresult.TestingArea;
                        consumableFresult.ForecastValue = fresult.ForecastValue;
                        consumableFresult.TotalValue = fresult.TotalValue;
                        //endcopy
                        decimal Qty = 0;


                        if (cusage.PerInstrument)
                        {
                            if (finfo.DataUsage == "DATA_USAGE1" || finfo.DataUsage == "DATA_USAGE2")
                            {
                                if (fsite != null)
                                {
                                    ForecastIns siteins = ctx.ForecastIns.Where(b => b.InsID == cusage.InstrumentId && b.forecastID == fresult.ForecastId).FirstOrDefault();
                                    if (siteins != null)
                                    {
                                        if (cusage.Period == "Daily")
                                        {
                                            Qty = cusage.UsageRate * fPinDay;
                                        }
                                        if (cusage.Period == "Weekly")
                                        {
                                            Qty = cusage.UsageRate * fPinWeek;
                                        }
                                        if (cusage.Period == "Monthly")
                                        {
                                            Qty = cusage.UsageRate * fPinMonth;
                                        }
                                        if (cusage.Period == "Quarterly")
                                        {
                                            Qty = cusage.UsageRate * fPinQuarter;
                                        }
                                        if (cusage.Period == "Yearly")
                                        {
                                            Qty = cusage.UsageRate * fPinYear;
                                        }
                                        Qty = Qty * siteins.Quantity;
                                    }
                                }
                            }
                        }
                        if (cusage.PerPeriod)
                        {
                            if (finfo.DataUsage == "DATA_USAGE1" || finfo.DataUsage == "DATA_USAGE2")
                            {
                                if (fsite != null)
                                {
                                    if (cusage.Period == "Daily")
                                    {
                                        Qty = cusage.UsageRate * fPinDay;
                                    }
                                    if (cusage.Period == "Weekly")
                                    {
                                        Qty = cusage.UsageRate * fPinWeek;
                                    }
                                    if (cusage.Period == "Monthly")
                                    {
                                        Qty = cusage.UsageRate * fPinMonth;
                                    }
                                    if (cusage.Period == "Quarterly")
                                    {
                                        Qty = cusage.UsageRate * fPinQuarter;
                                    }
                                    if (cusage.Period == "Yearly")
                                    {
                                        Qty = cusage.UsageRate * fPinYear;
                                    }
                                }
                            }
                        }
                        if (cusage.PerTest)
                        {
                            if (finfo.DataUsage == "DATA_USAGE1" || finfo.DataUsage == "DATA_USAGE2")
                            {
                                if (fsite != null)
                                {
                                    Qty = cusage.UsageRate * (fresult.TotalValue / cusage.NoOfTest);
                                }
                            }
                        }

                        consumableFresult.TotalValue = Qty;
                        MasterProduct MP = new MasterProduct();
                        MP = ctx.MasterProduct.Find(cusage.ProductId);
                        MP._productPrices = ctx.ProductPrice.Where(b => b.ProductId == MP.ProductID).ToList();
                        int packSize = MP.GetActiveProductPrice(fresult.DurationDateTime).packsize;
                        consumableFresult.ProductId = cusage.ProductId;
                        consumableFresult.PackQty = GetNoofPackage(packSize, Qty);
                        consumableFresult.PackPrice = consumableFresult.PackQty * MP.GetActiveProductPrice(fresult.DurationDateTime).packcost;
                        consumableFresult.ProductTypeId = MP.ProductTypeId;
                        consumableFresult.ProductType = ctx.ProductType.Find(MP.ProductTypeId).TypeName;
                        consumableFresult.IsForGeneralConsumable = true;
                        consumableFresult.ServiceConverted = true;
                        _listFresult.Add(consumableFresult);


                    }



                    #endregion

                    #region Forecast Control Test

                    if (finfo.DataUsage == "DATA_USAGE1" || finfo.DataUsage == "DATA_USAGE2")
                    {
                        if (fsite != null)
                        {
                            ForecastIns siteins = ctx.ForecastIns.Join(ctx.Instrument, b => b.InsID, c => c.InstrumentID, (b, c) => new { b, c })
                                .Where(x => x.c.testingArea.TestingAreaID == test.TestingAreaID && x.b.forecastID == fresult.ForecastId).GroupBy(s => s.c.testingArea.TestingAreaID)
                                .Select(s => new ForecastIns
                                {

                                    ID = s.Max(x => x.b.ID),
                                    InsID = s.Max(x => x.b.InsID)
                                }).FirstOrDefault();
                            //fsite.GetSiteInstrumentByTA(test.TestingArea.Id);
                            if (siteins != null)
                            {
                                if (ctx.Instrument.Where(s => s.InstrumentID == siteins.InsID).Select(s => s.DailyCtrlTest).FirstOrDefault() > 0)
                                {
                                    fresult.ControlTest = fPinDay * siteins.Quantity * ctx.Instrument.Where(s => s.InstrumentID == siteins.InsID).Select(s => s.DailyCtrlTest).FirstOrDefault();
                                }
                                if (ctx.Instrument.Where(s => s.InstrumentID == siteins.InsID).Select(s => s.WeeklyCtrlTest).FirstOrDefault() > 0)
                                {
                                    fresult.ControlTest = fPinWeek * siteins.Quantity * ctx.Instrument.Where(s => s.InstrumentID == siteins.InsID).Select(s => s.WeeklyCtrlTest).FirstOrDefault();
                                }
                                if (ctx.Instrument.Where(s => s.InstrumentID == siteins.InsID).Select(s => s.MonthlyCtrlTest).FirstOrDefault() > 0)
                                {
                                    fresult.ControlTest = fPinMonth * siteins.Quantity * ctx.Instrument.Where(s => s.InstrumentID == siteins.InsID).Select(s => s.MonthlyCtrlTest).FirstOrDefault();

                                }
                                if (ctx.Instrument.Where(s => s.InstrumentID == siteins.InsID).Select(s => s.QuarterlyCtrlTest).FirstOrDefault() > 0)
                                {
                                    fresult.ControlTest = fPinQuarter * siteins.Quantity * ctx.Instrument.Where(s => s.InstrumentID == siteins.InsID).Select(s => s.QuarterlyCtrlTest).FirstOrDefault();
                                }
                                if (ctx.Instrument.Where(s => s.InstrumentID == siteins.InsID).Select(s => s.MaxTestBeforeCtrlTest).FirstOrDefault() > 0)
                                {
                                    fresult.ControlTest = ((fresult.TotalValue * (siteins.TestRunPercentage / 100)) / ctx.Instrument.Where(s => s.InstrumentID == siteins.InsID).Select(s => s.MaxTestBeforeCtrlTest).FirstOrDefault());
                                }
                            }
                        }
                    }

                    #endregion

                    #region Test Test to Product
                    var productusage = ctx.ForecastProductUsage.Where(b => b.TestId == test.TestID && b.IsForControl == false && b.Forecastid == fresult.ForecastId).ToList();
                    foreach (ForecastProductUsage pu in productusage) //change on aug 22.2014 (ProductUsage pu in test.ProductUsages)
                    {
                        ForecastedResult cfresult = new ForecastedResult();
                        //copyvalues
                        cfresult.ForecastId = fresult.ForecastId;
                        cfresult.TestId = fresult.TestId;
                        cfresult.DurationDateTime = fresult.DurationDateTime;
                        cfresult.SiteId = fresult.SiteId;
                        cfresult.CategoryId = fresult.CategoryId;
                        cfresult.Duration = fresult.Duration;
                        cfresult.IsHistory = fresult.IsHistory;
                        cfresult.TestingArea = fresult.TestingArea;
                        //endcopy

                        if (finfo.DataUsage == "DATA_USAGE1" || finfo.DataUsage == "DATA_USAGE2")
                        {
                            if (fsite != null)
                            {
                                ForecastIns siteins = ctx.ForecastIns.Where(b => b.InsID == pu.InstrumentId && b.forecastID == fresult.ForecastId).FirstOrDefault();
                                if (siteins != null)
                                {
                                    decimal Qty = pu.Rate * fresult.TotalValue * siteins.TestRunPercentage / 100;
                                    cfresult.TotalValue = Qty;
                                    MasterProduct p = ctx.MasterProduct.Find(pu.ProductId);
                                    p._productPrices = ctx.ProductPrice.Where(b => b.ProductId == p.ProductID).ToList();
                                    int packSize = p.GetActiveProductPrice(fresult.DurationDateTime).packsize;
                                    cfresult.ProductId = pu.ProductId;
                                    cfresult.PackQty = GetNoofPackage(packSize, Qty);
                                    cfresult.PackPrice = cfresult.PackQty * p.GetActiveProductPrice(fresult.DurationDateTime).packcost;

                                    cfresult.ProductTypeId = ctx.MasterProduct.Find(pu.ProductId).ProductTypeId;
                                    cfresult.ProductType = ctx.ProductType.Find(ctx.MasterProduct.Find(pu.ProductId).ProductTypeId).TypeName;
                                    cfresult.ServiceConverted = true;
                                    _listFresult.Add(cfresult);
                                }
                            }
                        }
                        else
                        {
                            //if (fsite != null)
                            //{
                            //    ForecastCategoryInstrument fcins = DataRepository.GetForecastCategoryInstrumentById(pu.Instrument.Id);

                            //    if (fcins != null)
                            //    {
                            //        decimal Qty = pu.Rate * fresult.TotalValue * fcins.TestRunPercentage;
                            //        cfresult.TotalValue = Qty;
                            //        int packSize = pu.Product.GetActiveProductPrice(fresult.DurationDateTime).PackSize;
                            //        cfresult.ProductId = pu.Product.Id;
                            //        cfresult.PackQty = GetNoofPackage(packSize, Qty);
                            //        cfresult.PackPrice = cfresult.PackQty * pu.Product.GetActiveProductPrice(fresult.DurationDateTime).Price;

                            //        cfresult.ProductTypeId = pu.Product.ProductType.Id;
                            //        cfresult.ProductType = pu.Product.ProductType.TypeName;
                            //        cfresult.ServiceConverted = true;
                            //        _listFresult.Add(cfresult);
                            //    }
                            //}
                        }
                    }
                    ///////// 
                    #endregion

                    #region Control Test to Product


                    var cproductusage = ctx.ForecastProductUsage.Where(b => b.TestId == test.TestID && b.IsForControl == true && b.Forecastid == fresult.ForecastId).ToList();
                    foreach (ForecastProductUsage pu in cproductusage) //change on aug 22.2014 (ProductUsage pu in test.ProductUsages)
                    {
                        ForecastedResult cfresult = new ForecastedResult();
                        //copyvalues
                        cfresult.ForecastId = fresult.ForecastId;
                        cfresult.TestId = fresult.TestId;
                        cfresult.DurationDateTime = fresult.DurationDateTime;
                        cfresult.SiteId = fresult.SiteId;
                        cfresult.CategoryId = fresult.CategoryId;
                        cfresult.Duration = fresult.Duration;
                        cfresult.IsHistory = fresult.IsHistory;
                        cfresult.TestingArea = fresult.TestingArea;

                        cfresult.IsForControl = true;
                        //endcopy

                        if (finfo.DataUsage == "DATA_USAGE1" || finfo.DataUsage == "DATA_USAGE2")
                        {
                            if (fsite != null)
                            {
                                ForecastIns siteins = ctx.ForecastIns.Where(b => b.InsID == pu.InstrumentId && b.forecastID == fresult.ForecastId).FirstOrDefault();
                                if (siteins != null)
                                {


                                    decimal Qty = 0;
                                    if (ctx.Instrument.Where(s => s.InstrumentID == siteins.InsID).Select(s => s.DailyCtrlTest).FirstOrDefault() > 0)
                                    {
                                        Qty = pu.Rate * fPinDay * siteins.Quantity * ctx.Instrument.Where(s => s.InstrumentID == siteins.InsID).Select(s => s.DailyCtrlTest).FirstOrDefault();
                                        // cfresult.ForecastValue = fPinDay * siteins.Quantity * siteins.Instrument.DailyCtrlTest;
                                    }
                                    if (ctx.Instrument.Where(s => s.InstrumentID == siteins.InsID).Select(s => s.WeeklyCtrlTest).FirstOrDefault() > 0)
                                    {
                                        Qty = pu.Rate * fPinWeek * siteins.Quantity * ctx.Instrument.Where(s => s.InstrumentID == siteins.InsID).Select(s => s.WeeklyCtrlTest).FirstOrDefault();
                                        // cfresult.ForecastValue = fPinWeek * siteins.Quantity * siteins.Instrument.WeeklyCtrlTest;
                                    }
                                    if (ctx.Instrument.Where(s => s.InstrumentID == siteins.InsID).Select(s => s.MonthlyCtrlTest).FirstOrDefault() > 0)
                                    {
                                        Qty = pu.Rate * fPinMonth * siteins.Quantity * ctx.Instrument.Where(s => s.InstrumentID == siteins.InsID).Select(s => s.MonthlyCtrlTest).FirstOrDefault();
                                        // cfresult.ForecastValue = fPinMonth * siteins.Quantity * siteins.Instrument.MonthlyCtrlTest;

                                    }
                                    if (ctx.Instrument.Where(s => s.InstrumentID == siteins.InsID).Select(s => s.QuarterlyCtrlTest).FirstOrDefault() > 0)
                                    {
                                        Qty = pu.Rate * fPinQuarter * siteins.Quantity * ctx.Instrument.Where(s => s.InstrumentID == siteins.InsID).Select(s => s.QuarterlyCtrlTest).FirstOrDefault();
                                        //cfresult.ForecastValue = fPinQuarter * siteins.Quantity * siteins.Instrument.QuarterlyCtrlTest;
                                    }
                                    if (ctx.Instrument.Where(s => s.InstrumentID == siteins.InsID).Select(s => s.MaxTestBeforeCtrlTest).FirstOrDefault() > 0)
                                    {
                                        Qty = pu.Rate * ((fresult.TotalValue * (siteins.TestRunPercentage / 100)) / ctx.Instrument.Where(s => s.InstrumentID == siteins.InsID).Select(s => s.MaxTestBeforeCtrlTest).FirstOrDefault());
                                        // cfresult.ForecastValue = ((fresult.TotalValue * (siteins.TestRunPercentage / 100)) / siteins.Instrument.MaxTestBeforeCtrlTest);
                                    }


                                    //decimal Qty = pu.Rate * fresult.TotalValue * siteins.TestRunPercentage / 100;

                                    cfresult.TotalValue = Qty;
                                    MasterProduct p = ctx.MasterProduct.Find(pu.ProductId);
                                    p._productPrices = ctx.ProductPrice.Where(b => b.ProductId == p.ProductID).ToList();
                                    int packSize = p.GetActiveProductPrice(fresult.DurationDateTime).packsize;
                                    cfresult.ProductId = pu.ProductId;
                                    cfresult.PackQty = GetNoofPackage(packSize, Qty);
                                    cfresult.PackPrice = cfresult.PackQty * p.GetActiveProductPrice(fresult.DurationDateTime).packcost;

                                    cfresult.ProductTypeId = ctx.MasterProduct.Find(pu.ProductId).ProductTypeId;
                                    cfresult.ProductType = ctx.ProductType.Find(ctx.MasterProduct.Find(pu.ProductId).ProductTypeId).TypeName;
                                    cfresult.ServiceConverted = true;
                                    _listFresult.Add(cfresult);
                                }
                            }
                        }
                        else
                        {
                            //if (fsite != null)
                            //{
                            //    ForecastCategoryInstrument fcins = DataRepository.GetForecastCategoryInstrumentById(pu.Instrument.Id);

                            //    if (fcins != null)
                            //    {
                            //        decimal Qty = 0;
                            //        if (fcins.Instrument.DailyCtrlTest > 0)
                            //        {
                            //            Qty = pu.Rate * fPinDay * fcins.Instrument.DailyCtrlTest;
                            //            //cfresult.ForecastValue = fPinDay * fcins.Instrument.DailyCtrlTest;
                            //        }
                            //        if (fcins.Instrument.WeeklyCtrlTest > 0)
                            //        {
                            //            Qty = pu.Rate * fPinWeek * fcins.Instrument.WeeklyCtrlTest;
                            //            //cfresult.ForecastValue = fPinWeek * fcins.Instrument.WeeklyCtrlTest;
                            //        }
                            //        if (fcins.Instrument.MonthlyCtrlTest > 0)
                            //        {
                            //            Qty = pu.Rate * fPinMonth * fcins.Instrument.MonthlyCtrlTest;
                            //            //cfresult.ForecastValue = fPinMonth * fcins.Instrument.MonthlyCtrlTest;
                            //        }
                            //        if (fcins.Instrument.QuarterlyCtrlTest > 0)
                            //        {
                            //            Qty = pu.Rate * fPinQuarter * fcins.Instrument.QuarterlyCtrlTest;
                            //            //cfresult.ForecastValue = fPinQuarter * fcins.Instrument.QuarterlyCtrlTest;
                            //        }
                            //        if (fcins.Instrument.MaxTestBeforeCtrlTest > 0)
                            //        {
                            //            Qty = pu.Rate * ((fresult.TotalValue * (fcins.TestRunPercentage / 100)) / fcins.Instrument.MaxTestBeforeCtrlTest);
                            //            //cfresult.ForecastValue = ((fresult.TotalValue * (fcins.TestRunPercentage / 100)) / fcins.Instrument.MaxTestBeforeCtrlTest);
                            //        }

                            //        //decimal Qty = pu.Rate * fresult.TotalValue * fcins.TestRunPercentage;
                            //        cfresult.TotalValue = Qty;
                            //        int packSize = pu.Product.GetActiveProductPrice(fresult.DurationDateTime).PackSize;
                            //        cfresult.ProductId = pu.Product.Id;
                            //        cfresult.PackQty = GetNoofPackage(packSize, Qty);
                            //        cfresult.PackPrice = cfresult.PackQty * pu.Product.GetActiveProductPrice(fresult.DurationDateTime).Price;

                            //        cfresult.ProductTypeId = pu.Product.ProductType.Id;
                            //        cfresult.ProductType = pu.Product.ProductType.TypeName;
                            //        cfresult.ServiceConverted = true;
                            //        cfresult.IsForControl = true;
                            //        _listFresult.Add(cfresult);
                            //    }
                            //}
                        }
                    }
                    ///////// 
                    #endregion

                }
                //end test to product

                period++;
            }
        }


        private int GetNoofPackage(int packSize, decimal noofproduct)
        {
            int Nopack;
            decimal Result;
            if (packSize == 0)
                Result = noofproduct;
            else
                Result = noofproduct / packSize;

            Nopack = int.Parse(decimal.Round(Result, 0).ToString());

            if (Nopack < Result)
                Nopack = Nopack + 1;
            if (Nopack == 0)
                Nopack = 0;

            return Nopack;
        }
        public IList<ConsumableUsage> GetAllConsumableUsageByTestId(int testId)
        {
            IList<MasterConsumable> _mainConsumableList = ctx.MasterConsumable.ToList();
            IList<ConsumableUsage> _cUsageList = new List<ConsumableUsage>();
            foreach (MasterConsumable mc in _mainConsumableList)
            {

                if (mc.TestId == testId)
                {
                    var ConsumableUsages = ctx.ConsumableUsage.Where(b => b.ConsumableId == mc.MasterCID).ToList();
                    foreach (ConsumableUsage cu in ConsumableUsages)
                        _cUsageList.Add(cu);
                }

            }
            return _cUsageList;
        }




        public IList<ForecastConsumableUsage> GetAllserviceConsumableUsageByTestId(int testId, int forecastid)
        {
            //  IList<MasterConsumable> _mainConsumableList = ctx.MasterConsumable.ToList();
            IList<ForecastConsumableUsage> _cUsageList = new List<ForecastConsumableUsage>();

            var ConsumableUsages = ctx.ForecastConsumableUsage.Where(b => b.TestId == testId && b.Forecastid == forecastid).ToList();
            foreach (ForecastConsumableUsage cu in ConsumableUsages)
                _cUsageList.Add(cu);

            return _cUsageList;
        }
        public DataTable Calculateforecastmultiplemethod(DataTable inputds, ForecastInfo forecastInfo)
        {
            DataTable ds = new DataTable();
            double a = 0;
            double b = 0;
            double sigmaxy = 0;
            double averagex = 0;
            double averagey = 0;
            double xsqr = 0;
            double totalx = 0;
            double totaly = 0;
            ds.Columns.Add("0");
            ds.Columns.Add("1");
            if (param1[0] == "Specifiedpercentagegrowth")
            {
                for (int i = 0; i < inputds.Rows.Count; i++)
                {
                    DataRow Dr = ds.NewRow();
                    Dr[0] = Convert.ToDateTime(inputds.Rows[i][0]);
                    Dr[1] = Convert.ToDecimal(inputds.Rows[i][1]) + (Convert.ToDecimal(inputds.Rows[i][1]) * (Convert.ToDecimal(param1[2]) / 100));
                    ds.Rows.Add(Dr);
                }

            }
            else if (param1[0] == "Simplemovingaverage")
            {
                if (inputds.Rows.Count > 3)
                {
                    for (int i = 0; i < inputds.Rows.Count; i++)
                    {
                        DataRow Dr = ds.NewRow();
                        Dr[0] = Convert.ToDateTime(inputds.Rows[i][0]);
                        if (param1[3] == "3")
                            Dr[1] = (Convert.ToDecimal(inputds.Rows[inputds.Rows.Count - 1][1]) + Convert.ToDecimal(inputds.Rows[inputds.Rows.Count - 2][1]) + Convert.ToDecimal(inputds.Rows[inputds.Rows.Count - 3][1])) / 3;
                        else
                            Dr[1] = (Convert.ToDecimal(inputds.Rows[inputds.Rows.Count - 1][1]) + Convert.ToDecimal(inputds.Rows[inputds.Rows.Count - 2][1]) + Convert.ToDecimal(inputds.Rows[inputds.Rows.Count - 3][1]) + Convert.ToDecimal(inputds.Rows[inputds.Rows.Count - 4][1]) + Convert.ToDecimal(inputds.Rows[inputds.Rows.Count - 5][1])) / 5;
                        ds.Rows.Add(Dr);
                    }
                }
                else
                {
                    ds = inputds;
                }

            }

            else if (param1[0] == "Weightedmovingaverage")
            {
                if (inputds.Rows.Count > 3)
                {
                    for (int i = 0; i < inputds.Rows.Count; i++)
                    {
                        DataRow Dr = ds.NewRow();
                        Dr[0] = Convert.ToDateTime(inputds.Rows[i][0]);
                        if (param1[3] == "3")
                            Dr[1] = ((Convert.ToDouble(inputds.Rows[inputds.Rows.Count - 1][1]) * 0.6) + (Convert.ToDouble(inputds.Rows[inputds.Rows.Count - 2][1]) * 0.3) + (Convert.ToDouble(inputds.Rows[inputds.Rows.Count - 3][1]) * 0.1));
                        else
                            Dr[1] = ((Convert.ToDouble(inputds.Rows[inputds.Rows.Count - 1][1]) * 0.5) + (Convert.ToDouble(inputds.Rows[inputds.Rows.Count - 2][1]) * 0.2) + (Convert.ToDouble(inputds.Rows[inputds.Rows.Count - 3][1]) * 0.1) + (Convert.ToDouble(inputds.Rows[inputds.Rows.Count - 4][1]) * 0.1) + (Convert.ToDouble(inputds.Rows[inputds.Rows.Count - 5][1]) * 0.1));
                        ds.Rows.Add(Dr);
                    }
                }
                else
                {
                    ds = inputds;
                }
            }
            else if (param1[0] == "Simplelinearregression")
            {
                double xy = 0;
                for (int i = 0; i < inputds.Rows.Count; i++)
                {
                    //DataRow Dr = ds.NewRow();
                    //Dr[0] = Convert.ToDateTime(inputds.Rows[i][0]);
                    //Dr[1] = Convert.ToDouble(inputds.Rows[i][1]);
                    //ds.Rows.Add(Dr);

                    xy = (Convert.ToDateTime(inputds.Rows[i][0]).Month) * Convert.ToDouble(inputds.Rows[i][1]);
                    sigmaxy = sigmaxy + xy;

                    totalx = totalx + Convert.ToDateTime(inputds.Rows[i][0]).Month;
                    totaly = totaly + Convert.ToDouble(inputds.Rows[i][1]);
                    xsqr = xsqr + (Convert.ToDateTime(inputds.Rows[i][0]).Month * Convert.ToDateTime(inputds.Rows[i][0]).Month);
                    //if (param1[3] == "3")

                    //    Dr[1] = ((Convert.ToDouble(inputds.Rows[inputds.Rows.Count - 1][1]) * 0.6) + (Convert.ToDouble(inputds.Rows[inputds.Rows.Count - 2][1]) * 0.3) + (Convert.ToDouble(inputds.Rows[inputds.Rows.Count - 3][1]) * 0.1));
                    //else
                    //    Dr[1] = ((Convert.ToDouble(inputds.Rows[inputds.Rows.Count - 1][1]) * 0.5) + (Convert.ToDouble(inputds.Rows[inputds.Rows.Count - 2][1]) * 0.2) + (Convert.ToDouble(inputds.Rows[inputds.Rows.Count - 3][1]) * 0.1) + (Convert.ToDouble(inputds.Rows[inputds.Rows.Count - 4][1]) * 0.1) + (Convert.ToDouble(inputds.Rows[inputds.Rows.Count - 5][1]) * 0.1));

                }
                if (inputds.Rows.Count > 0)
                {
                    totalx = totalx / inputds.Rows.Count;
                    totaly = totaly / inputds.Rows.Count;
                    b = (sigmaxy - (inputds.Rows.Count * totalx * totaly)) / (xsqr - (inputds.Rows.Count * totalx * totalx));
                    a = totaly - (b * totalx);
                }
                for (int i = 0; i < inputds.Rows.Count; i++)
                {
                    DataRow Dr = ds.NewRow();
                    Dr[0] = Convert.ToDateTime(inputds.Rows[i][0]);
                    Dr[1] = Math.Round(a + Convert.ToDateTime(inputds.Rows[i][0]).Month * b, 2);
                    ds.Rows.Add(Dr);
                }
            }

            //  ds = inputds;
            int ext;

            int interval = 0;
            if (forecastInfo.Period == "Monthly")
            {
                interval = 1;
            }
            else if (forecastInfo.Period == "Bimonthly")
            {
                interval = 2;
            }
            else if (forecastInfo.Period == "Quarterly")
            {
                interval = 3;
            }
            else
            {

                interval = 12;
            }
            ext = forecastInfo.Extension;
            if (param1[0] == "Specifiedpercentagegrowth")
            {
                while (ext > 0)
                {
                    DataRow Dr = ds.NewRow();
                    Dr[0] = Convert.ToDateTime(ds.Rows[ds.Rows.Count - 1][0]).AddMonths(interval);
                    Dr[1] = Convert.ToDecimal(ds.Rows[ds.Rows.Count - 1][1]) + (Convert.ToDecimal(ds.Rows[ds.Rows.Count - 1][1]) * (Convert.ToDecimal(param1[2]) / 100));
                    ds.Rows.Add(Dr);
                    ext--;
                }
            }
            else if (param1[0] == "Simplemovingaverage")
            {
                while (ext > 0)
                {
                    DataRow Dr = ds.NewRow();
                    Dr[0] = Convert.ToDateTime(ds.Rows[ds.Rows.Count - 1][0]).AddMonths(interval);
                    if (param1[3] == "3")
                        Dr[1] = (Convert.ToDecimal(ds.Rows[ds.Rows.Count - 1][1]) + Convert.ToDecimal(ds.Rows[ds.Rows.Count - 2][1]) + Convert.ToDecimal(ds.Rows[ds.Rows.Count - 3][1])) / 3;
                    else
                        Dr[1] = (Convert.ToDecimal(ds.Rows[ds.Rows.Count - 1][1]) + Convert.ToDecimal(ds.Rows[ds.Rows.Count - 2][1]) + Convert.ToDecimal(ds.Rows[ds.Rows.Count - 3][1]) + Convert.ToDecimal(ds.Rows[ds.Rows.Count - 4][1]) + Convert.ToDecimal(ds.Rows[ds.Rows.Count - 5][1])) / 3;
                    ds.Rows.Add(Dr);
                    ext--;
                }
            }
            else if (param1[0] == "Simplelinearregression")
            {
                while (ext > 0)
                {
                    DataRow Dr = ds.NewRow();
                    Dr[0] = Convert.ToDateTime(ds.Rows[ds.Rows.Count - 1][0]).AddMonths(interval);
                    // if (param1[3] == "3")
                    Dr[1] = Math.Round(a + (ds.Rows.Count+interval) * b, 2); //(Convert.ToDecimal(ds.Rows[ds.Rows.Count - 1][1]) + Convert.ToDecimal(ds.Rows[ds.Rows.Count - 2][1]) + Convert.ToDecimal(ds.Rows[ds.Rows.Count - 3][1])) / 3;
                    //else
                    //    Dr[1] = (Convert.ToDecimal(ds.Rows[ds.Rows.Count - 1][1]) + Convert.ToDecimal(ds.Rows[ds.Rows.Count - 2][1]) + Convert.ToDecimal(ds.Rows[ds.Rows.Count - 3][1]) + Convert.ToDecimal(ds.Rows[ds.Rows.Count - 4][1]) + Convert.ToDecimal(ds.Rows[ds.Rows.Count - 5][1])) / 3;
                    ds.Rows.Add(Dr);
                    ext--;
                }
            }
            else if (param1[0] == "Weightedmovingaverage")
            {
                while (ext > 0)
                {
                    DataRow Dr = ds.NewRow();
                    Dr[0] = Convert.ToDateTime(ds.Rows[ds.Rows.Count - 1][0]).AddMonths(interval);
                    if (param1[3] == "3")
                        Dr[1] = ((Convert.ToDouble(ds.Rows[ds.Rows.Count - 1][1]) * 0.6) + (Convert.ToDouble(ds.Rows[ds.Rows.Count - 2][1]) * 0.3) + (Convert.ToDouble(ds.Rows[ds.Rows.Count - 3][1]) * 0.1));
                    else
                        Dr[1] = ((Convert.ToDouble(ds.Rows[ds.Rows.Count - 1][1]) * 0.5) + (Convert.ToDouble(ds.Rows[ds.Rows.Count - 2][1]) * 0.2) + (Convert.ToDouble(ds.Rows[ds.Rows.Count - 3][1]) * 0.1) + (Convert.ToDouble(ds.Rows[ds.Rows.Count - 4][1]) * 0.1) + (Convert.ToDouble(ds.Rows[ds.Rows.Count - 5][1]) * 0.1));
                    ds.Rows.Add(Dr);
                    ext--;
                }
            }

            return ds;
        }

    }
}
