using ForLabApi.DataInterface;
using ForLabApi.Models;
using ForLabApi.Utility;
using LQT.Core.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Repositories
{
    public class ConsumptionAccessRepositories : IConsumption<Datausagewithcontrol, siteproductno, adjustedvolume>
    {
        ForLabContext ctx;
        int refid = 0;
        ForecastInfo _forecastInfo;
        IList<ForecastCategoryProduct> productcatList = null;
        IList<ForecastSiteProduct> productsiteList = null;
        int _noOfPeriod;
        //IList<ForecastCategoryTest> testcatList = null;
        IList<ForecastSiteTest> testsiteList = null;
        IList<ForecastCategoryTest> testcatList = null;

        int productListunsorted = 0;
        int testListunsorted = 0;
        public ConsumptionAccessRepositories(ForLabContext c)
        {
            ctx = c;
            //return ctx;
        }
        public siteproductno Gettotalsiteandproduct(int id)
        {
            siteproductno A = new siteproductno();
            var totalsite = ctx.ForecastSite.Where(b => b.ForecastInfoId == id).Select(x => x.Id).ToArray();
            var forecastinfo = ctx.ForecastInfo.Find(id);
            if (forecastinfo.Methodology == "SERVICE_STATISTIC")
            {
                var totalproduct = ctx.ForecastSiteTest.Where(b => totalsite.Contains(b.ForecastSiteID)).GroupBy(x => x.TestID).Select(s => s.Key).ToList();
               
                A.totalproductno = totalproduct.Count;
            }
            else
            {
                var totalproduct = ctx.ForecastSiteProduct.Where(b => totalsite.Contains(b.ForecastSiteID)).GroupBy(x => x.ProductID).Select(s => s.Key).ToList();
                
                A.totalproductno = totalproduct.Count;
            }
            A.totalsiteno = totalsite.Length;
            return A;
        }
        public Array Getforecastsite(int forecastid)
        {
            var forecastsite = ctx.ForecastSite.Join(ctx.Site, b => b.SiteId, c => c.SiteID, (b, c) => new { b, c })
                .Where(i => i.b.ForecastInfoId == forecastid && i.b.ReportedSiteId == 0).Select(x => new
                {
                    SiteID = x.b.SiteId,
                    SiteName = x.c.SiteName

                }).ToArray();

            return forecastsite;
        }
        public Datausagewithcontrol Bindforecastsiteproduct(int id, int siteorcatid)
        {
            List<string> list = new List<string>();
            List<Datausage> DU = new List<Datausage>();
            List<string> plist = new List<string>();
            List<int> plistId = new List<int>();
            List<string> columnlst = new List<string>();

            _forecastInfo = ctx.ForecastInfo.Find(id);
            if (_forecastInfo.DataUsage == "DATA_USAGE3")
            {

                refid = ctx.ForecastCategory.Where(b => b.ForecastId == id && b.CategoryId == siteorcatid).Select(x => x.CategoryId).FirstOrDefault();
            }
            else
            {
                refid = ctx.ForecastSite.Where(b => b.ForecastInfoId == id && b.SiteId == siteorcatid).Select(x => x.Id).FirstOrDefault();
            }
            Datausagewithcontrol DC = new Datausagewithcontrol();
            DC = Bindforecastproductortest();

            return DC;
        }
        public Datausagewithcontrol Addactualconsumption(int id, string param)
        {

            Datausagewithcontrol DC = new Datausagewithcontrol();

            if (AddDurationDatausage(id, param))
            {

                DC = Bindforecastproductortest();
            }

            return DC;
        }
        public Datausagewithcontrol GetDataUasge(int id, string param)
        {
            Datausagewithcontrol DC = new Datausagewithcontrol();

            if (AddDatausage(id, param))
            {

                DC = Bindforecastproductortest();
            }

            return DC;
        }
        public string Removedatausagefromsite(int id, string param)
        {
            ForecastCategoryProduct productcat = null;
            ForecastSiteProduct productsite = null;
            IList list;
            int _noOfPeriod;
            //IList<ForecastCategoryTest> testcatList = null;
            ForecastSiteTest testsite = null;
            ForecastCategoryTest testcat = null;
            string str = "";
            string[] param1 = param.TrimEnd(',').Split(",");
            _forecastInfo = ctx.ForecastInfo.Find(Convert.ToInt32(param));

            if (_forecastInfo.Methodology == "CONSUMPTION")
            {
                if (_forecastInfo.DataUsage == DataUsageEnum.DATA_USAGE3.ToString())
                {
                    productcat = (ForecastCategoryProduct)GetDataUsage(id);

                    productcatList = (IList<ForecastCategoryProduct>)GetListOfDataUsagesproducttest(productcat.ProductID, productcat.CategoryID);
                    if (productcatList.Count >= 4)
                    {
                        if (productcat.CDuration == productcatList[0].CDuration)
                        {
                            ctx.ForecastCategoryProduct.Remove(productcat);
                            ctx.SaveChanges();
                            str = "";
                        }
                        else
                        {
                            str = "Can Not Delete Record in middle";
                        }
                    }
                    else
                    {
                        str = "Can Not Delete Record, Minimum 3 record is required";
                    }

                }
                else
                {
                    productsite = (ForecastSiteProduct)GetDataUsage(id);

                    productsiteList = (IList<ForecastSiteProduct>)GetListOfDataUsagesproducttest(productsite.ProductID, productsite.ForecastSiteID);
                    if (productsiteList.Count >= 4)
                    {
                        if (productsite.CDuration == productsiteList[0].CDuration)
                        {
                            ctx.ForecastSiteProduct.Remove(productsite);
                            ctx.SaveChanges();
                            str = "";
                        }
                        else
                        {
                            str = "Can Not Delete Record in middle";
                        }
                    }
                    else
                    {
                        str = "Can Not Delete Record, Minimum 3 record is required";
                    }



                }


            }
            else
            {
                if (_forecastInfo.DataUsage == DataUsageEnum.DATA_USAGE3.ToString())
                {
                    testcat = (ForecastCategoryTest)GetDataUsage(id);

                    testcatList = (IList<ForecastCategoryTest>)GetListOfDataUsagesproducttest(testcat.TestID, testcat.CategoryID);
                    if (testcatList.Count >= 4)
                    {
                        if (testcat.CDuration == testcatList[0].CDuration)
                        {
                            ctx.ForecastCategoryTest.Remove(testcat);
                            ctx.SaveChanges();
                            str = "";
                        }
                        else
                        {
                            str = "Can Not Delete Record in middle";
                        }
                    }
                    else
                    {
                        str = "Can Not Delete Record, Minimum 3 record is required";
                    }

                }
                else
                {
                    testsite = (ForecastSiteTest)GetDataUsage(id);

                    testsiteList = (IList<ForecastSiteTest>)GetListOfDataUsagesproducttest(testsite.TestID, testsite.ForecastSiteID);
                    if (testsiteList.Count >= 4)
                    {
                        if (testsite.CDuration == testsiteList[0].CDuration)
                        {
                            ctx.ForecastSiteTest.Remove(testsite);
                            ctx.SaveChanges();
                            str = "";
                        }
                        else
                        {
                            str = "Can Not Delete Record in middle";
                        }
                    }
                    else
                    {
                        str = "Can Not Delete Record, Minimum 3 record is required";
                    }



                }

            }

            return str;
        }
        public Array Getforecastnonreportedsite(int forecastid, int siteid)
        {
            Array A;
            var forecaststie = ctx.ForecastSite.Where(b => b.ForecastInfoId == forecastid && b.SiteId == siteid).FirstOrDefault();
            if (forecaststie != null)
            {
                A = ctx.ForecastNRSite.Join(ctx.Site, b => b.NReportedSiteId, c => c.SiteID, (b, c) => new { b, c })
                     .Where(x => x.b.ForecastSiteId == forecaststie.Id).Select(x => new
                     {

                         SiteName = x.c.SiteName,
                         SiteID = x.b.NReportedSiteId
                     }).ToArray();
                return A;
            }
            A = new Array[0];
            return A;

        }




        public string removestefromdatausage(int id, int siteid)
        {

            string str = "";

            _forecastInfo = ctx.ForecastInfo.Find(id);
            var forecastsite = ctx.ForecastSite.Where(b => b.SiteId == siteid && b.ForecastInfoId == id).FirstOrDefault();
            if (forecastsite != null)
            {

                var forecastnonreportedsites = ctx.ForecastNRSite.Where(b => b.NReportedSiteId == siteid && b.ForecastSiteId == forecastsite.ReportedSiteId).ToList();
                if (_forecastInfo.Methodology == "CONSUMPTION")
                {

                    var forecastsiteproduct = ctx.ForecastSiteProduct.Where(b => b.ForecastSiteID == forecastsite.Id).ToList();
                    ctx.ForecastSiteProduct.RemoveRange(forecastsiteproduct);
                    ctx.SaveChanges();
                    ctx.ForecastNRSite.RemoveRange(forecastnonreportedsites);
                    ctx.SaveChanges();
                    ctx.ForecastSite.Remove(forecastsite);
                    ctx.SaveChanges();
                    str = "Record deleted";
                }
                else
                {
                    var forecastsitetest = ctx.ForecastSiteTest.Where(b => b.ForecastSiteID == forecastsite.Id).ToList();
                    ctx.ForecastSiteTest.RemoveRange(forecastsitetest);
                    ctx.SaveChanges();
                    ctx.ForecastNRSite.RemoveRange(forecastnonreportedsites);
                    ctx.SaveChanges();
                    ctx.ForecastSite.Remove(forecastsite);
                    ctx.SaveChanges();
                    str = "Record deleted";

                }
            }

            return str;
        }
        public string Addsiteincategory(int id, string param)
        {
            string res = "";
            string[] arrsiteids = param.TrimEnd(',').Split(",");
            _forecastInfo = ctx.ForecastInfo.Find(id);
            int reportcategoryid = Convert.ToInt32(arrsiteids[arrsiteids.Length - 1]);

            for (int i = 0; i < arrsiteids.Length - 1; i++)
            {
                var issiteexist = ctx.ForecastCategorySite.Where(b => b.CategoryID == reportcategoryid && b.SiteID == Convert.ToInt32(arrsiteids[i])).FirstOrDefault();
                if (issiteexist == null)
                {
                    ForecastCategorySite FS = new ForecastCategorySite();
                    FS.CategoryID = reportcategoryid;
                    FS.SiteID = Convert.ToInt32(arrsiteids[i]);
                    ctx.ForecastCategorySite.Add(FS);
                    ctx.SaveChanges();
                    res = "Site added";
                }
            }
            return res;

        }
        public string deletecategorydatausage(int id)
        {
            string res = "";
            ///Delete for datausage 3
            var forecastcategory = ctx.ForecastCategory.Find(id);
            if (forecastcategory !=null)
            {
                var forcaseinfo = ctx.ForecastInfo.Find(forecastcategory.ForecastId);
                if (forcaseinfo.DataUsage== "DATA_USAGE3"  && forcaseinfo.Methodology== "SERVICE_STATISTIC")
                {
                    var categorytest = ctx.ForecastCategoryTest.Where(x => x.CategoryID == id).ToList();
                    if (categorytest != null)
                    {
                        ctx.ForecastCategoryTest.RemoveRange(categorytest);
                        ctx.SaveChanges();
                        var categorysite = ctx.ForecastCategorySite.Where(b => b.CategoryID == id).ToList();
                        if (categorysite != null)
                        {
                            ctx.ForecastCategorySite.RemoveRange(categorysite);
                            ctx.SaveChanges();
                            ctx.ForecastCategory.Remove(ctx.ForecastCategory.Find(id));
                            ctx.SaveChanges();
                        }
                        res = "category deleted";
                        return res;
                    }
                }
                else
                {
                    var categoryproduct = ctx.ForecastCategoryProduct.Where(x => x.CategoryID == id).ToList();
                    if (categoryproduct != null)
                    {
                        ctx.ForecastCategoryProduct.RemoveRange(categoryproduct);
                        ctx.SaveChanges();
                        var categorysite = ctx.ForecastCategorySite.Where(b => b.CategoryID == id).ToList();
                        if (categorysite != null)
                        {
                            ctx.ForecastCategorySite.RemoveRange(categorysite);
                            ctx.SaveChanges();
                            ctx.ForecastCategory.Remove(ctx.ForecastCategory.Find(id));
                            ctx.SaveChanges();
                        }
                        res = "category deleted";
                        return res;
                    }
                }



            }




         
            return res;
        }
        public string Addcategory(int id, string param)
        {
            string res = "";
            var category = ctx.ForecastCategory.Where(b => b.ForecastId == id && b.CategoryName == param).FirstOrDefault();
            if (category == null)
            {
                ForecastCategory FC = new ForecastCategory();
                FC.CategoryName = param;
                FC.ForecastId = id;
                ctx.ForecastCategory.Add(FC);
                ctx.SaveChanges();
                res = Convert.ToString(FC.CategoryId);

            }
            else
            {
                res = "Duplicate data";
            }
            return res;
        }
        public string deletesiteformcategory(int id, string param)
        {
            string res = "";
            var categorysite = ctx.ForecastCategorySite.Where(b => b.CategoryID == id && b.SiteID == Convert.ToInt32(param)).FirstOrDefault();
            if (categorysite != null)
            {
                ctx.ForecastCategorySite.Remove(categorysite);
                ctx.SaveChanges();
                res = "site deleted";
            }
            return res;

        }
        public string Deleteservicestatistic(int id)
        {
            string res = "";
            var forecastinfo = ctx.ForecastInfo.Find(id);
            if (forecastinfo.Methodology == "SERVICE_STATISTIC")
            {
                if (forecastinfo.DataUsage == "DATA_USAGE3")
                {
                    var forecastcategory = ctx.ForecastCategory.Where(x => x.ForecastId == id).ToList();
                    if (forecastcategory != null)
                    {
                        for (int i = 0; i < forecastcategory.Count; i++)
                        {
                            var forecastcategorysite = ctx.ForecastCategorySite.Where(x => x.CategoryID == forecastcategory[i].CategoryId).ToList();
                            if (forecastcategorysite != null)
                            {
                                ctx.ForecastCategorySite.RemoveRange(forecastcategorysite);
                                ctx.SaveChanges();
                            }
                            var forecastcategorytest = ctx.ForecastCategoryTest.Where(x => x.CategoryID == forecastcategory[i].CategoryId).ToList();
                            if (forecastcategorytest != null)
                            {
                                ctx.ForecastCategoryTest.RemoveRange(forecastcategorytest);
                                ctx.SaveChanges();
                            }
                        }
                        ctx.ForecastCategory.RemoveRange(forecastcategory);
                        ctx.SaveChanges();
                        ctx.ForecastInfo.Remove(forecastinfo);
                        ctx.SaveChanges();
                        res = "Deleted Successfully";
                    }
                    else
                    {
                        ctx.ForecastInfo.Remove(forecastinfo);
                        ctx.SaveChanges();
                        res = "Deleted Successfully";
                    }

                }
                else if (forecastinfo.DataUsage == "DATA_USAGE2")
                {
                    var forecastsite = ctx.ForecastSite.Where(x => x.ForecastInfoId == id).ToList();
                    if (forecastsite != null)
                    {
                        for (int i = 0; i < forecastsite.Count; i++)
                        {
                            var forecastNRSite = ctx.ForecastNRSite.Where(x => x.ForecastSiteId == forecastsite[i].Id).ToList();
                            if (forecastNRSite != null)
                            {
                                ctx.ForecastNRSite.RemoveRange(forecastNRSite);
                                ctx.SaveChanges();
                            }
                            var ForecastSiteTest = ctx.ForecastSiteTest.Where(x => x.ForecastSiteID == forecastsite[i].Id).ToList();
                            if (ForecastSiteTest != null)
                            {
                                ctx.ForecastSiteTest.RemoveRange(ForecastSiteTest);
                                ctx.SaveChanges();
                            }
                        }
                        ctx.ForecastSite.RemoveRange(forecastsite);
                        ctx.SaveChanges();
                        ctx.ForecastInfo.Remove(forecastinfo);
                        ctx.SaveChanges();
                        res = "Deleted Successfully";
                    }
                    else
                    {
                        ctx.ForecastInfo.Remove(forecastinfo);
                        ctx.SaveChanges();
                        res = "Deleted Successfully";
                    }
                }
                else
                {
                    var forecastsite = ctx.ForecastSite.Where(x => x.ForecastInfoId == id).ToList();
                    if (forecastsite != null)
                    {
                        for (int i = 0; i < forecastsite.Count; i++)
                        {

                            var ForecastSiteTest = ctx.ForecastSiteTest.Where(x => x.ForecastSiteID == forecastsite[i].Id).ToList();
                            if (ForecastSiteTest != null)
                            {
                                ctx.ForecastSiteTest.RemoveRange(ForecastSiteTest);
                                ctx.SaveChanges();
                            }
                        }
                        ctx.ForecastSite.RemoveRange(forecastsite);
                        ctx.SaveChanges();
                        ctx.ForecastInfo.Remove(forecastinfo);
                        ctx.SaveChanges();
                        res = "Deleted Successfully";
                    }
                    else
                    {
                        ctx.ForecastInfo.Remove(forecastinfo);
                        ctx.SaveChanges();
                        res = "Deleted Successfully";
                    }
                }
            }
            return res;
        }
        public string Deleteconsumption(int id)
        {
            string res = "";
            var forecastinfo = ctx.ForecastInfo.Find(id);
            if (forecastinfo.Methodology== "CONSUMPTION")
            {
                if (forecastinfo.DataUsage== "DATA_USAGE3")
                {
                    var forecastcategory = ctx.ForecastCategory.Where(x => x.ForecastId == id).ToList();
                    if(forecastcategory!=null)
                    {
                        for (int i = 0; i < forecastcategory.Count; i++)
                        {
                            var forecastcategorysite = ctx.ForecastCategorySite.Where(x => x.CategoryID == forecastcategory[i].CategoryId).ToList();
                            if (forecastcategorysite !=null)
                            {
                                ctx.ForecastCategorySite.RemoveRange(forecastcategorysite);
                                ctx.SaveChanges();
                            }
                            var forecastcategoryproduct = ctx.ForecastCategoryProduct.Where(x => x.CategoryID == forecastcategory[i].CategoryId).ToList();
                            if (forecastcategoryproduct!=null)
                            {
                                ctx.ForecastCategoryProduct.RemoveRange(forecastcategoryproduct);
                                ctx.SaveChanges();
                            }
                        }
                        ctx.ForecastCategory.RemoveRange(forecastcategory);
                        ctx.SaveChanges();
                        ctx.ForecastInfo.Remove(forecastinfo);
                        ctx.SaveChanges();
                        res = "Deleted Successfully";
                    }
                    else
                    {
                        ctx.ForecastInfo.Remove(forecastinfo);
                        ctx.SaveChanges();
                        res = "Deleted Successfully";
                    }

                }
                else if(forecastinfo.DataUsage == "DATA_USAGE2")
                {
                    var forecastsite = ctx.ForecastSite.Where(x => x.ForecastInfoId == id).ToList();
                    if (forecastsite != null)
                    {
                        for (int i = 0; i < forecastsite.Count; i++)
                        {
                            var forecastNRSite = ctx.ForecastNRSite.Where(x => x.ForecastSiteId == forecastsite[i].Id).ToList();
                            if (forecastNRSite != null)
                            {
                                ctx.ForecastNRSite.RemoveRange(forecastNRSite);
                                ctx.SaveChanges();
                            }
                            var forecastsiteproduct = ctx.ForecastSiteProduct.Where(x => x.ForecastSiteID == forecastsite[i].Id).ToList();
                            if (forecastsiteproduct != null)
                            {
                                ctx.ForecastSiteProduct.RemoveRange(forecastsiteproduct);
                                ctx.SaveChanges();
                            }
                        }
                        ctx.ForecastSite.RemoveRange(forecastsite);
                        ctx.SaveChanges();
                        ctx.ForecastInfo.Remove(forecastinfo);
                        ctx.SaveChanges();
                        res = "Deleted Successfully";
                    }
                    else
                    {
                        ctx.ForecastInfo.Remove(forecastinfo);
                        ctx.SaveChanges();
                        res = "Deleted Successfully";
                    }
                }
                else
                {
                    var forecastsite = ctx.ForecastSite.Where(x => x.ForecastInfoId == id).ToList();
                    if (forecastsite != null)
                    {
                        for (int i = 0; i < forecastsite.Count; i++)
                        {
                           
                            var forecastsiteproduct = ctx.ForecastSiteProduct.Where(x => x.ForecastSiteID == forecastsite[i].Id).ToList();
                            if (forecastsiteproduct != null)
                            {
                                ctx.ForecastSiteProduct.RemoveRange(forecastsiteproduct);
                                ctx.SaveChanges();
                            }
                        }
                        ctx.ForecastSite.RemoveRange(forecastsite);
                        ctx.SaveChanges();
                        ctx.ForecastInfo.Remove(forecastinfo);
                        ctx.SaveChanges();
                        res = "Deleted Successfully";
                    }
                    else
                    {
                        ctx.ForecastInfo.Remove(forecastinfo);
                        ctx.SaveChanges();
                        res = "Deleted Successfully";
                    }
                }
            }
            return res;
        }
        public string Addnonrportedsites(int id, string param)
        {
            string res = "";
            string[] arrsiteids = param.TrimEnd(',').Split(",");
            _forecastInfo = ctx.ForecastInfo.Find(id);
            int reportsiteid = Convert.ToInt32(arrsiteids[arrsiteids.Length - 1]);
            var _activeFSite = ctx.ForecastSite.Where(b => b.ForecastInfoId == id && b.SiteId == reportsiteid).FirstOrDefault();
            if (_activeFSite != null)
            {
                for (int i = 0; i < arrsiteids.Length - 1; i++)
                {
                    var nrsite = ctx.ForecastNRSite.Where(b => b.NReportedSiteId == Convert.ToInt32(arrsiteids[i]) && b.ForecastSiteId == _activeFSite.Id).FirstOrDefault();
                    if (nrsite == null)
                    {
                        ForecastNRSite fc = new ForecastNRSite();
                        fc.NReportedSiteId = Convert.ToInt32(arrsiteids[i]);
                        fc.ForecastSiteId = _activeFSite.Id;
                        ctx.ForecastNRSite.Add(fc);
                        ctx.SaveChanges();

                        ForecastSite fs = new ForecastSite();
                        fs.SiteId = fc.NReportedSiteId;
                        fs.ForecastInfoId = id;
                        fs.ReportedSiteId = _activeFSite.Id;
                        ctx.ForecastSite.Add(fs);
                        ctx.SaveChanges();

                        CopySiteProduct(_activeFSite, fs);
                    }
                    res = "non reported site added";

                }
            }
            else
            {
                res = "false";
            }

            return res;

        }

        public Array Getcategorysite(int catid)
        {
            var categorysite = ctx.ForecastCategorySite.Join(ctx.Site, b => b.SiteID, c => c.SiteID, (b, c) => new { b, c })
                .Where(x => x.b.CategoryID == catid).Select(x => new
                {
                    siteID = x.b.SiteID,
                    siteName = x.c.SiteName
                }).ToArray();

            return categorysite;
        }
        public Datausagewithcontrol GetAdjustedVolume(int id, adjustedvolume A)
        {
            int workingdays = 22;
            _forecastInfo = ctx.ForecastInfo.Find(id);
            IBaseDataUsage Fc = null;
            Datausagewithcontrol DU = new Datausagewithcontrol();
            if (_forecastInfo.Methodology == "CONSUMPTION")
            {
                if(_forecastInfo.DataUsage=="DATA_USAGE3")
                {
                    Fc = (IBaseDataUsage)ctx.ForecastCategoryProduct.Find(A.Id); 
                    var fs = (ForecastCategoryProduct)Fc;
                    refid = fs.CategoryID;
                }
                else
                {
                    Fc = (IBaseDataUsage)ctx.ForecastSiteProduct.Find(A.Id);
                    var fs = (ForecastSiteProduct)Fc;
                    refid = fs.ForecastSiteID;
                    int siteid = ctx.ForecastSite.Where(b => b.Id == fs.ForecastSiteID).Select(x => x.SiteId).FirstOrDefault();
                    workingdays = ctx.Site.Where(b => b.SiteID == siteid).Select(x => x.WorkingDays).FirstOrDefault();
                }

            }
            else
            {
                if (_forecastInfo.DataUsage == "DATA_USAGE3")
                {
                    Fc = (IBaseDataUsage)ctx.ForecastCategoryTest.Find(A.Id); 
                    var fs = (ForecastCategoryTest)Fc;
                    refid = fs.CategoryID;
                }
                else
                {
                    Fc = (IBaseDataUsage)ctx.ForecastSiteTest.Find(A.Id);
                    var fs = (ForecastSiteTest)Fc;
                    refid = fs.ForecastSiteID;
                    int siteid = ctx.ForecastSite.Where(b => b.Id == fs.ForecastSiteID).Select(x => x.SiteId).FirstOrDefault();
                    workingdays = ctx.Site.Where(b => b.SiteID == siteid).Select(x => x.WorkingDays).FirstOrDefault();
                }
            }
            if(A.type== "StockOut")
            {
                Fc.StockOut = A.value;
            }
            else if (A.type== "InstrumentDowntime")
            {
                Fc.InstrumentDowntime = A.value;
            }
            else if(A.type== "ProductUsed/TestPerformed")
            {
                Fc.AmountUsed = A.value;
            }
            Fc.Adjusted = Utility.Utility.GetAdjustedVolume(Fc.AmountUsed, Fc.StockOut + Fc.InstrumentDowntime, _forecastInfo.Period, workingdays);
        ctx.SaveChanges();
           
            DU = Bindforecastproductortest();
            return DU;

        }

        public Array GetcategoryList(int forecastid)
        {
            var categorylist = ctx.ForecastCategory.Where(b => b.ForecastId == forecastid).Select(x => new
            {

                categoryid = x.CategoryId,
                categoryname = x.CategoryName
            }).ToArray();
            return categorylist;
        }
        private void CopySiteProduct(ForecastSite source, ForecastSite destination)
        {
            if (_forecastInfo.Methodology == "CONSUMPTION")
            {
                var siteproducts = ctx.ForecastSiteProduct.Where(b => b.ForecastSiteID == source.Id).ToList();
                foreach (ForecastSiteProduct fp in siteproducts)
                {
                    ForecastSiteProduct fc = (ForecastSiteProduct)GetCloneForecastSiteProduct((IBaseDataUsage)fp);
                    fc.ForecastSiteID = destination.Id;
                    fc.ProductID = fp.ProductID;
                    ctx.ForecastSiteProduct.Add(fc);
                    ctx.SaveChanges();
                }
            }
            else
            {
                var siteTests = ctx.ForecastSiteTest.Where(b => b.ForecastSiteID == source.Id).ToList();
                foreach (ForecastSiteTest fp in siteTests)
                {
                    ForecastSiteTest ft = (ForecastSiteTest)GetCloneForecastSiteProduct((IBaseDataUsage)fp);
                    ft.ForecastSiteID = destination.Id;
                    ft.TestID = fp.TestID;
                    ctx.ForecastSiteTest.Add(ft);
                    ctx.SaveChanges();
                }
            }
        }

        private Datausagewithcontrol Bindforecastproductortest()
        {
            List<Durationwithids> list = new List<Durationwithids>();
            List<Datausage> DU = new List<Datausage>();
            List<string> plist = new List<string>();
            List<int> plistId = new List<int>();
            List<string> columnlst = new List<string>();
            if (refid != 0) //_activeCategory != null ||
            {
                int index = 0;
                int productcount = 0;


                if (GetListOfDataUsages(refid).Count > 0)
                {
                    if (_forecastInfo.Methodology == MethodologyEnum.CONSUMPTION.ToString())
                    {
                        if (_forecastInfo.DataUsage == DataUsageEnum.DATA_USAGE3.ToString())
                        {
                            productcatList = (IList<ForecastCategoryProduct>)GetListOfDataUsages(refid);
                            productListunsorted = productcatList[0].Id;
                            productcatList.Sort(delegate (ForecastCategoryProduct p1, ForecastCategoryProduct p2) { return p1.DurationDateTime.Value.Date.CompareTo(p2.DurationDateTime.Value.Date); });
                            productcount = productcatList.Count;
                            foreach (ForecastCategoryProduct p in productcatList)
                            {
                                if (list.Find(b => b.Duration.ToLower() == p.CDuration.ToLower()) == null)
                                {
                                    list.Add(new Durationwithids { Duration = Char.ToLowerInvariant(p.CDuration[0]) + p.CDuration.Substring(1), id = p.Id });
                                }
                                if (plist.Contains(ctx.MasterProduct.Where(b => b.ProductID == p.ProductID).Select(x => x.ProductName).FirstOrDefault()) != true)
                                {
                                    plist.Add(ctx.MasterProduct.Where(b => b.ProductID == p.ProductID).Select(x => x.ProductName).FirstOrDefault());
                                    plistId.Add(p.ProductID);
                                }
                                if (productListunsorted < p.Id)
                                    productListunsorted = p.Id;

                            }
                        }
                        else
                        {
                            productsiteList = (IList<ForecastSiteProduct>)GetListOfDataUsages(refid);
                            productListunsorted = productsiteList[0].Id;
                            productsiteList.Sort(delegate (ForecastSiteProduct p1, ForecastSiteProduct p2) { return p1.DurationDateTime.Value.Date.CompareTo(p2.DurationDateTime.Value.Date); });
                            productcount = productsiteList.Count;


                            foreach (ForecastSiteProduct p in productsiteList)
                            {
                                if (list.Find(b => b.Duration.ToLower() == p.CDuration.ToLower()) == null)
                                {
                                    list.Add(new Durationwithids { Duration = Char.ToLowerInvariant(p.CDuration[0]) + p.CDuration.Substring(1), id = p.Id });
                                }
                                if (plist.Contains(ctx.MasterProduct.Where(b => b.ProductID == p.ProductID).Select(x => x.ProductName).FirstOrDefault()) != true)
                                {
                                    plist.Add(ctx.MasterProduct.Where(b => b.ProductID == p.ProductID).Select(x => x.ProductName).FirstOrDefault());
                                    plistId.Add(p.ProductID);
                                }
                                if (productListunsorted < p.Id)
                                    productListunsorted = p.Id;

                            }


                        }
                    }
                    else
                    {
                        if (_forecastInfo.DataUsage == DataUsageEnum.DATA_USAGE3.ToString())
                        {
                            testcatList = (IList<ForecastCategoryTest>)GetListOfDataUsages(refid);
                            testListunsorted = testcatList[0].Id;
                            testcatList.Sort(delegate (ForecastCategoryTest t1, ForecastCategoryTest t2) { return t1.DurationDateTime.Value.Date.CompareTo(t2.DurationDateTime.Value.Date); });
                            productcount = testcatList.Count;
                            foreach (ForecastCategoryTest t in testcatList)
                            {
                                if (list.Find(b => b.Duration.ToLower() == t.CDuration.ToLower()) == null)
                                {
                                    list.Add(new Durationwithids { Duration = Char.ToLowerInvariant(t.CDuration[0]) + t.CDuration.Substring(1), id = t.Id });
                                }
                                if (plist.Contains(ctx.Test.Where(b => b.TestID == t.TestID).Select(x => x.TestName).FirstOrDefault()) != true)
                                {
                                    plist.Add(ctx.Test.Where(b => b.TestID == t.TestID).Select(x => x.TestName).FirstOrDefault());
                                    plistId.Add(t.TestID);
                                }
                                if (testListunsorted < t.Id)
                                    testListunsorted = t.Id;

                            }


                            //testcatList = (IList<ForecastCategoryTest>)_forecastInfo.GetListOfDataUsages(siteOrcatid, activIndex);
                            //testListunsorted = testcatList[0].Id;
                            //testcatList.Sort(delegate (ForecastCategoryTest t1, ForecastCategoryTest t2) { return t1.DurationDateTime.Value.Date.CompareTo(t2.DurationDateTime.Value.Date); });
                            //productcount = testcatList.Count;



                            //foreach (ForecastCategoryTest t in testcatList)
                            //{
                            //    if (list.Contains(t.CDuration) != true)
                            //    {
                            //        list.Add(t.CDuration);
                            //    }
                            //    if (plist.Contains(t.Test.TestName) != true)
                            //    {
                            //        plist.Add(t.Test.TestName);
                            //        plistId.Add(t.Test.Id);
                            //    }
                            //    if (testListunsorted < t.Id)
                            //        testListunsorted = t.Id;

                            //}
                        }
                        else
                        {
                            testsiteList = (IList<ForecastSiteTest>)GetListOfDataUsages(refid);
                            testListunsorted = testsiteList[0].Id;
                            testsiteList.Sort(delegate (ForecastSiteTest t1, ForecastSiteTest t2) { return t1.DurationDateTime.Value.Date.CompareTo(t2.DurationDateTime.Value.Date); });
                            productcount = testsiteList.Count;


                            foreach (ForecastSiteTest t in testsiteList)
                            {
                                if (list.Find(b => b.Duration.ToLower() == t.CDuration.ToLower()) == null)
                                {
                                    list.Add(new Durationwithids { Duration = Char.ToLowerInvariant(t.CDuration[0]) + t.CDuration.Substring(1), id = t.Id });
                                }
                                if (plist.Contains(ctx.Test.Where(b => b.TestID == t.TestID).Select(x => x.TestName).FirstOrDefault()) != true)
                                {
                                    plist.Add(ctx.Test.Where(b => b.TestID == t.TestID).Select(x => x.TestName).FirstOrDefault());
                                    plistId.Add(t.TestID);
                                }
                                if (testListunsorted < t.Id)
                                    testListunsorted = t.Id;

                            }
                        }
                    }


                    if (_forecastInfo.Period == "YEARLY")
                        list.Sort();
                    int count = list.Count;

                    if (_forecastInfo.Period == ForecastPeriodEnum.Yearly.ToString())
                    {
                        for (int i = count; i >= 1; i--)
                        {

                            columnlst.Add(list[i - 1].ToString());

                        }

                    }

                    if (_forecastInfo.Period == ForecastPeriodEnum.Monthly.ToString())
                    {
                        DateTime last = _forecastInfo.StartDate;
                        for (int i = 1; i <= count; i++)
                        {
                            columnlst.Add(list[i - 1].ToString());

                        }


                    }
                    if (_forecastInfo.Period == ForecastPeriodEnum.Bimonthly.ToString())
                    {
                        DateTime last = _forecastInfo.StartDate;
                        for (int i = count; i >= 1; i--)
                        {
                            // ColumnHeader c = new ColumnHeader();
                            last = last.AddMonths(-2);
                            columnlst.Add(list[i - 1].ToString());

                        }

                    }
                    if (_forecastInfo.Period == ForecastPeriodEnum.Quarterly.ToString())
                    {
                        int quar = Utility.Utility.GetQuarter(_forecastInfo.StartDate);
                        int year = _forecastInfo.StartDate.Year;
                        for (int i = count; i >= 1; i--)
                        {
                            if (quar == 1)
                            {
                                quar = 4;
                                year--;
                            }
                            else
                                quar--;
                            columnlst.Add(list[i - 1].ToString());

                        }
                    }



                    int j = 0, k = 1;

                    for (int z = 0; z < plistId.Count; z++)
                    {
                        bool flag = false;
                        bool flag1 = false;
                        string productname = "";
                        string testname = "";
                        int proid = 0;

                        int insert = 1, rem = 0;
                        j = 0;
                        DataTable DT = new DataTable();
                        DT.Columns.Add("column1");

                        for (int i = 0; i < list.Count; i++)
                        {
                            DT.Columns.Add(list[i].Duration);
                            DT.Columns.Add(list[i].Duration + "-id");
                        }
                        if (_forecastInfo.Methodology == "CONSUMPTION")
                        {
                            if (_forecastInfo.DataUsage == DataUsageEnum.DATA_USAGE3.ToString())
                            {

                                // ForecastCategoryProduct Fcp = (ForecastCategoryProduct)p;
                                // proid = ctx.MasterProduct.Where(b => b.ProductName == plist[z]).Select(x => x.ProductID).FirstOrDefault();
                                proid = plistId[z];
                            }
                            else
                            {
                                //  ForecastSiteProduct Fcp = (ForecastSiteProduct)p;
                                //proid = ctx.MasterProduct.Where(b => b.ProductName == plist[z]).Select(x => x.ProductID).FirstOrDefault();
                                proid = plistId[z];
                            }

                        }
                        else
                        {
                            //if (_forecastInfo.DataUsage == DataUsageEnum.DATA_USAGE3.ToString())
                            //{

                            //    //ForecastCategoryProduct Fcp = (ForecastCategoryProduct)p;
                            //    //productname = ctx.MasterProduct.Where(b => b.ProductID == Fcp.ProductID).Select(x => x.ProductName).FirstOrDefault();
                            //}
                            //else
                            //{
                            //  ForecastSiteTest Fcp = (ForecastSiteTest)p;
                          //  proid = ctx.Test.Where(b => b.TestName == plist[z]).Select(x => x.TestID).FirstOrDefault();
                            proid = plistId[z];
                            //}
                        }

                        if (GetListOfDataUsages_data(refid, proid) != null)
                        {
                            DataRow Dr = DT.NewRow();
                            Dr[0] = "ProductUsed/TestPerformed";
                            IList pp = GetListOfDataUsages_data(refid, proid);
                            for (int i = 0; i < list.Count; i++)
                            {
                                foreach (IBaseDataUsage p in pp)
                                {

                                    if (list[i].Duration.ToLower() == p.CDuration.ToLower())
                                    {
                                        // Dr[list[i].Duration + "-id"] = p.Id;
                                        Dr[list[i].Duration + "-id"] = p.Id;
                                        Dr[list[i].Duration] = p.AmountUsed;
                                        break;
                                    }
                                    else
                                    {
                                        // Dr[list[i].Duration + "-id"] = 0;
                                        Dr[list[i].Duration + "-id"] = p.Id;
                                        Dr[list[i].Duration] = "-";
                                    }

                                }

                            }
                            DT.Rows.Add(Dr);

                            DataRow Dr1 = DT.NewRow();
                            Dr1[0] = "StockOut";

                            for (int i = 0; i < list.Count; i++)
                            {

                                foreach (IBaseDataUsage p in pp)
                                {

                                    if (list[i].Duration.ToLower() == p.CDuration.ToLower())
                                    {
                                        //   Dr[list[i].Duration + "-id"] = p.Id;
                                        Dr1[list[i].Duration + "-id"] = p.Id;
                                        Dr1[list[i].Duration] = p.StockOut;
                                        break;
                                    }
                                    else
                                    {
                                        //  Dr[list[i].Duration + "-id"] = 0;
                                        Dr1[list[i].Duration + "-id"] = p.Id;
                                        Dr1[list[i].Duration] = "-";
                                    }
                                    //   Dr1[list[i].Duration + "-id"] = p.Id;
                                }

                            }
                            DT.Rows.Add(Dr1);


                            DataRow Dr2 = DT.NewRow();
                            Dr2[0] = "InstrumentDowntime";
                            for (int i = 0; i < list.Count; i++)
                            {
                                foreach (IBaseDataUsage p in pp)
                                {


                                    if (list[i].Duration.ToLower() == p.CDuration.ToLower())
                                    {

                                        Dr2[list[i].Duration] = p.InstrumentDowntime;
                                        Dr2[list[i].Duration + "-id"] = p.Id;
                                        break;
                                    }
                                    else
                                    {
                                        Dr2[list[i].Duration + "-id"] = p.Id;

                                        Dr2[list[i].Duration] = "-";
                                    }

                                }

                            }
                            DT.Rows.Add(Dr2);


                            DataRow Dr3 = DT.NewRow();
                            Dr3[0] = "Adjusted";
                            for (int i = 0; i < list.Count; i++)
                            {
                                foreach (IBaseDataUsage p in pp)
                                {

                                    if (list[i].Duration.ToLower() == p.CDuration.ToLower())
                                    {
                                        Dr3[list[i].Duration + "-id"] = p.Id;

                                        Dr3[list[i].Duration] = p.Adjusted;
                                        break;
                                    }
                                    else
                                    {
                                        Dr3[list[i].Duration + "-id"] = p.Id;
                                        //   Dr[list[i].Duration + "-id"] = 0;
                                        Dr3[list[i].Duration] = "-";
                                    }

                                }

                            }
                            DT.Rows.Add(Dr3);

                            DataRow Dr4 = DT.NewRow();
                            Dr4[0] = "Note";

                            for (int i = 0; i < list.Count; i++)
                            {
                                foreach (IBaseDataUsage p in GetListOfDataUsages_data(refid, proid))
                                {
                                    if (list[i].Duration.ToLower() == p.CDuration.ToLower())
                                    {
                                        if (p.AmountUsed != p.Adjusted)
                                        {
                                            Dr4[list[i].Duration + "-id"] = p.Id;
                                            Dr4[p.CDuration] = "Adjusted";
                                            break;

                                        }
                                        else
                                        {
                                            Dr4[list[i].Duration + "-id"] = p.Id;
                                            Dr4[p.CDuration] = "-";
                                        }
                                    }
                                   

                                }
                            }
                            DT.Rows.Add(Dr4);
                        }
                        else
                        {
                            DataRow Dr = DT.NewRow();
                            Dr[0] = "ProductUsed/TestPerformed";

                            for (int i = 0; i < list.Count; i++)
                            {


                                Dr[list[i].Duration] = "-";
                            }
                            DT.Rows.Add(Dr);

                            DataRow Dr1 = DT.NewRow();
                            Dr1[0] = "StockOut";

                            for (int i = 0; i < list.Count; i++)
                            {


                                Dr1[list[i].Duration] = "-";
                            }
                            DT.Rows.Add(Dr1);


                            DataRow Dr2 = DT.NewRow();
                            Dr2[0] = "InstrumentDowntime";

                            for (int i = 0; i < list.Count; i++)
                            {


                                Dr2[list[i].Duration] = "-";
                            }
                            DT.Rows.Add(Dr2);


                            DataRow Dr3 = DT.NewRow();
                            Dr3[0] = "Adjusted";

                            for (int i = 0; i < list.Count; i++)
                            {


                                Dr3[list[i].Duration] = "-";
                            }
                            DT.Rows.Add(Dr3);

                            DataRow Dr4 = DT.NewRow();
                            Dr4[0] = "Note";

                            for (int i = 0; i < list.Count; i++)
                            {


                                Dr4[list[i].Duration] = "-";
                            }
                            DT.Rows.Add(Dr4);
                        }

                        DU.Add(new Datausage
                        {
                            productid = proid,
                            productname = plist[z],

                            value = DT
                        });


                    }

                }



            }



            Datausagewithcontrol DC = new Datausagewithcontrol();
            DC.controls = list;
            DC.Datausage = DU;
            return DC;

        }

        private IBaseDataUsage GetDataUsage(int id)
        {
            IBaseDataUsage idusage = null;

            if (_forecastInfo.Methodology == "CONSUMPTION")
            {
                switch (_forecastInfo.DataUsage)
                {
                    case "DATA_USAGE1":
                    case "DATA_USAGE2":

                        idusage = ctx.ForecastSiteProduct.Find(id);
                        break;
                    case "DATA_USAGE3":

                        idusage = ctx.ForecastCategoryProduct.Find(id);

                        break;
                }
            }
            else
            {
                switch (_forecastInfo.DataUsage)
                {
                    case "DATA_USAGE1":
                    case "DATA_USAGE2":

                        idusage = ctx.ForecastSiteTest.Find(id);
                        break;
                    case "DATA_USAGE3":


                        idusage = ctx.ForecastCategoryTest.Find(id);

                        break;
                }
            }

            return idusage;
        }

        private IList GetListOfDataUsagesproducttest(int proOrtestid, int sitecategoryid)
        {
            IList result = new ArrayList();
            if (_forecastInfo.Methodology == "CONSUMPTION")
            {
                if (_forecastInfo.DataUsage == "DATA_USAGE3")
                    result = (IList)ctx.ForecastCategoryProduct.Where(b => b.CategoryID == sitecategoryid && b.ProductID == proOrtestid).OrderByDescending(s => s.DurationDateTime).ToList();

                else
                    result = (IList)ctx.ForecastSiteProduct.Where(b => b.ForecastSiteID == sitecategoryid && b.ProductID == proOrtestid).OrderByDescending(s => s.DurationDateTime).ToList();
            }
            else
            {
                if (_forecastInfo.DataUsage == "DATA_USAGE3")
                    result = (IList)ctx.ForecastCategoryTest.Where(b => b.CategoryID == sitecategoryid && b.TestID == proOrtestid).OrderByDescending(s => s.DurationDateTime).ToList();
                else
                    result = (IList)ctx.ForecastSiteTest.Where(b => b.ForecastSiteID == sitecategoryid && b.TestID == proOrtestid).OrderByDescending(s => s.DurationDateTime).ToList();
            }
            return result;
        }
        private bool AddDurationDatausage(int id, string param)
        {

            string[] param1 = param.TrimEnd(',').Split(",");
            MasterProduct product = null;
            Test test = null;
            IList list = new ArrayList();
            _forecastInfo = ctx.ForecastInfo.Find(Convert.ToInt32(param1[0]));
            int forecastsite = 0;
            //id for p
            if (_forecastInfo.Methodology == "CONSUMPTION")
            {
                if (_forecastInfo.DataUsage == "DATA_USAGE3")
                {
                    product = ctx.ForecastCategoryProduct.Join(ctx.MasterProduct, b => b.ProductID, c => c.ProductID, (b, c) => new { b, c })
                        .Where(x => x.b.Id == id).Select(x => x.c).FirstOrDefault();

                    refid = ctx.ForecastCategoryProduct.Where(x => x.Id == id).Select(s => s.CategoryID).FirstOrDefault();

                }
                else
                {
                    product = ctx.ForecastSiteProduct.Join(ctx.MasterProduct, b => b.ProductID, c => c.ProductID, (b, c) => new { b, c })
                    .Where(x => x.b.Id == id).Select(x => x.c).FirstOrDefault();
                    // forecastsite = ctx.ForecastSiteProduct.Join(ctx.ForecastSite, b => b.ForecastSiteID, c => c.Id, (b, c) => new { b, c }).Where(x => x.b.Id == id).Select(x => x.c.SiteId).FirstOrDefault();
                    refid = ctx.ForecastSiteProduct.Where(x => x.Id == id).Select(s => s.ForecastSiteID).FirstOrDefault();
                    // forecastsite = ctx.ForecastSite.Where(b => b.Id == refid).Select(x=>x.SiteId).FirstOrDefault();
                }
                list = GetListOfDataUsagesproducttest(product.ProductID, refid);
            }
            else
            {
                if (_forecastInfo.DataUsage == "DATA_USAGE3")
                {
                    test = ctx.ForecastCategoryTest.Join(ctx.Test, b => b.TestID, c => c.TestID, (b, c) => new { b, c })
                        .Where(x => x.b.Id == id).Select(x => x.c).FirstOrDefault();

                    refid = ctx.ForecastCategoryTest.Where(x => x.Id == id).Select(s => s.CategoryID).FirstOrDefault();
                }
                else
                {
                    test = ctx.ForecastSiteTest.Join(ctx.Test, b => b.TestID, c => c.TestID, (b, c) => new { b, c })
                        .Where(x => x.b.Id == id).Select(x => x.c).FirstOrDefault();

                    refid = ctx.ForecastSiteTest.Where(x => x.Id == id).Select(s => s.ForecastSiteID).FirstOrDefault();
                    //  forecastsite = ctx.ForecastSiteTest.Join(ctx.ForecastSite, b => b.ForecastSiteID, c => c.Id, (b, c) => new { b, c }).Where(x => x.b.Id == id).Select(x => x.c.SiteId).FirstOrDefault();
                }
                list = GetListOfDataUsagesproducttest(test.TestID, refid);
            }

            int direction = 1;
            DateTime lastd = _forecastInfo.StartDate;
            if (list.Count > 0)
            {
                if (direction == -1)
                    lastd = ((IBaseDataUsage)list[list.Count - 1]).DurationDateTime.Value;
                else
                    lastd = ((IBaseDataUsage)list[0]).DurationDateTime.Value;
            }

            int year = lastd.Year;
            int quar = Utility.Utility.GetQuarter(lastd);
            ////

            _noOfPeriod = Convert.ToInt32(param1[1]);
            //if (_forecastInfo.DataUsage == "DATA_USAGE1" && direction == -1)
            //{
            //    if (_forecastInfo.Methodology == "CONSUMPTION")
            //    {
            //        if (CheckHistoryData(product.Id, lastd))
            //            AddForecastProductHistory(product.Id, _activeFSite.Site.Id, lastd);
            //    }
            //    else
            //    {
            //        if (CheckHistoryData(test.Id, lastd))
            //            AddForecastTestHistory(test.Id, _activeFSite.Site.Id, lastd);
            //    }
            //}
            //int len = list.Count + _noOfPeriod;

            //if (_forecastInfo.Period == "Bimonthly")
            //{
            //    lastd = lastd.AddMonths(direction * _noOfHistData * 2);
            //}
            //else if (_forecastInfo.PeriodEnum == ForecastPeriodEnum.Monthly)
            //{
            //    lastd = lastd.AddMonths(direction * _noOfHistData);
            //}
            //else if (_forecastInfo.PeriodEnum == ForecastPeriodEnum.Quarterly)
            //{
            //    for (int i = 0; i < _noOfHistData; i++)
            //    {
            //        // lastd = lastd.AddMonths(-3);
            //    }
            //}
            //else
            //{
            //    lastd = lastd.AddYears(direction * _noOfHistData);
            //}
            int count;
            int last;

            //if (direction == -1)
            //{
            //    count = list.Count + _noOfHistData + 1;
            //    last = len;
            //}
            //else
            // {
            count = 1;
            last = Convert.ToInt32(param1[1]);
            //  }


            ////

            for (int i = count; i <= last; i++) //for (int i = 1; i <= frm.NoPeriod; i++)
            {
                IBaseDataUsage fc = null;
                if (_forecastInfo.Methodology == "CONSUMPTION")
                {
                    fc = GetNewDataUsage(refid, product.ProductID);
                }
                else
                {
                    fc = GetNewDataUsage(refid, test.TestID);
                }

                fc.AmountUsed = 1;
                fc.Adjusted = 1;

                if (_forecastInfo.Period == "Bimonthly")
                {
                    lastd = lastd.AddMonths(direction * 2);
                    fc.CDuration = String.Format("{0}-{1}", Utility.Utility.Months[lastd.Month - 1], lastd.Year);
                }
                else if (_forecastInfo.Period == "Monthly")
                {
                    lastd = lastd.AddMonths(direction * 1);
                    fc.CDuration = String.Format("{0}-{1}", Utility.Utility.Months[lastd.Month - 1], lastd.Year);
                }
                else if (_forecastInfo.Period == "Quarterly")
                {
                    lastd = lastd.AddMonths(direction * 3);
                    if (direction == -1)
                    {
                        if (quar == 1)
                        {
                            quar = 4;
                            year--;
                        }
                        else
                            quar--;
                    }
                    else
                    {
                        if (quar == 4)
                        {
                            quar = 1;
                            year++;
                        }
                        else
                            quar++;
                    }
                    fc.CDuration = String.Format("Qua{0}-{1}", quar, year);
                }
                else
                {
                    lastd = lastd.AddYears(direction * 1);
                    fc.CDuration = lastd.Year.ToString();
                }
                fc.DurationDateTime = Utility.Utility.DurationToDateTime(fc.CDuration);
                AddDataUsageToSiteOrCat(fc);
            }

            if (_forecastInfo.Status == "CLOSED")
                _forecastInfo.Status = ForecastStatusEnum.REOPEN.ToString();



            return true;
        }




        private bool AddDatausage(int id, string param)
        {
            int siteid;
            int Period;
            string[] param1 = param.TrimEnd(',').Split(",");

            siteid = Convert.ToInt32(param1[param1.Length - 2]);
            _forecastInfo = ctx.ForecastInfo.Find(id);
            // int referid = 0;
            if (_forecastInfo.DataUsage == "DATA_USAGE3")
            {

                var ForecastCategory = ctx.ForecastCategory.Where(b => b.ForecastId == _forecastInfo.ForecastID && b.CategoryId == siteid).FirstOrDefault();
                if (ForecastCategory != null)
                {
                    refid = ForecastCategory.CategoryId;
                }
                else
                {
                    ForecastCategory FS = new ForecastCategory();
                    FS.ForecastId = _forecastInfo.ForecastID;
                    FS.CategoryId = siteid;
                    ctx.ForecastCategory.Add(FS);
                    ctx.SaveChanges();
                    refid = FS.CategoryId;
                }

            }
            else
            {
                var forecastsite = ctx.ForecastSite.Where(b => b.ForecastInfoId == _forecastInfo.ForecastID && b.SiteId == siteid).FirstOrDefault();
                if (forecastsite != null)
                {
                    refid = forecastsite.Id;
                }
                else
                {
                    ForecastSite FS = new ForecastSite();
                    FS.ForecastInfoId = _forecastInfo.ForecastID;
                    FS.SiteId = siteid;
                    ctx.ForecastSite.Add(FS);
                    ctx.SaveChanges();
                    refid = FS.Id;
                }

            }

            Period = Convert.ToInt32(param1[param1.Length - 1]);
            //   IList<int> selectedIds = GetSelectedProOrTestId();

            for (int i = 0; i < param1.Length - 2; i++)

            {
                int protestid = 0;
                int year = _forecastInfo.StartDate.Year;
                int quar = Utility.Utility.GetQuarter(_forecastInfo.StartDate);
                DateTime lastd = _forecastInfo.StartDate;

                MasterProduct product = null;
                Test test = null;
                if (_forecastInfo.Methodology == "CONSUMPTION")
                {
                    product = ctx.MasterProduct.Find(Convert.ToInt32(param1[i]));
                    protestid = product.ProductID;
                }
                else
                {
                    test = ctx.Test.Find(Convert.ToInt32(param1[i]
                        ));

                    protestid = test.TestID;
                }




                for (int x = 1; x <= Period; x++)
                {
                    IBaseDataUsage fc = GetNewDataUsage(refid, protestid);
                    //fc.ProductID = product.ProductID;
                    //fc.TestID = test==null?0: test.TestID;
                    fc.AmountUsed = 1;
                    fc.Adjusted = 1;

                    if (_forecastInfo.Period == "Bimonthly")
                    {
                        lastd = lastd.AddMonths(-2);
                        fc.CDuration = String.Format("{0}-{1}", Utility.Utility.Months[lastd.Month - 1], lastd.Year);
                    }
                    else if (_forecastInfo.Period == "Monthly")
                    {
                        lastd = lastd.AddMonths(-1);
                        fc.CDuration = String.Format("{0}-{1}", Utility.Utility.Months[lastd.Month - 1], lastd.Year);
                    }
                    else if (_forecastInfo.Period == "Quarterly")
                    {
                        if (quar == 1)
                        {
                            quar = 4;
                            year--;
                        }
                        else
                            quar--;

                        fc.CDuration = String.Format("Qua{0}-{1}", quar, year);
                    }
                    else
                    {
                        year--;
                        fc.CDuration = year.ToString();
                    }

                    fc.DurationDateTime = Utility.Utility.DurationToDateTime(fc.CDuration);

                    AddDataUsageToSiteOrCat(fc);

                    if (_forecastInfo.DataUsage == "DATA_USAGE2")
                        AddProductToNReportedSite(refid, fc, protestid);
                }
            }
            if (_forecastInfo.Status == "CLOSED")
                _forecastInfo.Status = "REOPEN";

            //   OnForecastInfoDataChanged();
            return true;



        }
        private IBaseDataUsage GetCloneForecastSiteProduct(IBaseDataUsage fsp)
        {
            IBaseDataUsage dusage;
            if (_forecastInfo.Methodology == "CONSUMPTION")
                dusage = new ForecastSiteProduct();
            else
                dusage = new ForecastSiteTest();


            dusage.AmountUsed = fsp.AmountUsed;
            dusage.Adjusted = fsp.Adjusted;
            dusage.CDuration = fsp.CDuration;
            dusage.DurationDateTime = fsp.DurationDateTime;

            return dusage;
        }

        private void AddProductToNReportedSite(int sourceid, IBaseDataUsage fc, int protestid)
        {
            foreach (ForecastSite fs in _forecastInfo.GetNoneReportedForecastSite(sourceid))
            {
                if (_forecastInfo.Method == "CONSUMPTION")
                {
                    ForecastSiteProduct fsp = (ForecastSiteProduct)GetCloneForecastSiteProduct(fc);
                    fsp.ForecastSiteID = sourceid;
                    fsp.ProductID = protestid;
                    ctx.ForecastSiteProduct.Add(fsp);
                    ctx.SaveChanges();
                }
                else
                {
                    ForecastSiteTest fst = (ForecastSiteTest)GetCloneForecastSiteProduct(fc);
                    fst.ForecastSiteID = sourceid;
                    fst.TestID = protestid;
                    ctx.ForecastSiteTest.Add(fst);
                    ctx.SaveChanges();
                }
            }
            if (_forecastInfo.Status == "CLOSED")
                _forecastInfo.Status = "REOPEN";
        }
        private void AddDataUsageToSiteOrCat(IBaseDataUsage dusage)
        {
            if (_forecastInfo.Methodology == "CONSUMPTION")
            {
                if (_forecastInfo.DataUsage == "DATA_USAGE3")
                {
                    ctx.ForecastCategoryProduct.Add((ForecastCategoryProduct)dusage);
                    ctx.SaveChanges();

                }
                else
                {
                    ctx.ForecastSiteProduct.Add((ForecastSiteProduct)dusage);
                    ctx.SaveChanges();
                }



            }
            else
            {
                if (_forecastInfo.DataUsage == "DATA_USAGE3")
                {
                    ctx.ForecastCategoryTest.Add((ForecastCategoryTest)dusage);
                    ctx.SaveChanges();
                }

                else
                {
                    ctx.ForecastSiteTest.Add((ForecastSiteTest)dusage);
                    ctx.SaveChanges();
                }

            }
            if (_forecastInfo.Status == "CLOSED")
            {
                _forecastInfo.Status = "REOPEN";
                ctx.SaveChanges();

            }
        }
        private IBaseDataUsage GetNewDataUsage(int id, int protestid)
        {
            IBaseDataUsage dusage = null;
            if (_forecastInfo.Methodology == "CONSUMPTION")
            {
                switch (_forecastInfo.DataUsage)
                {
                    case "DATA_USAGE1":
                    case "DATA_USAGE2":
                        dusage = new ForecastSiteProduct() { ForecastSiteID = id, ProductID = protestid };

                        break;
                    case "DATA_USAGE3":
                        dusage = new ForecastCategoryProduct() { CategoryID = id, ProductID = protestid };
                        break;
                }
            }
            else
            {
                switch (_forecastInfo.DataUsage)
                {
                    case "DATA_USAGE1":
                    case "DATA_USAGE2":
                        dusage = new ForecastSiteTest() { ForecastSiteID = id, TestID = protestid };
                        break;
                    case "DATA_USAGE3":
                        dusage = new ForecastCategoryTest() { CategoryID = id, TestID = protestid };
                        break;
                }
            }
            return dusage;
        }


        private IList GetListOfDataUsages(int siteOrcatid)
        {
            IList list = null;
            int index = 0;



            if (_forecastInfo.Methodology == "CONSUMPTION")
            {
                switch (_forecastInfo.DataUsage)
                {
                    case "DATA_USAGE1":
                    case "DATA_USAGE2":
                        if (siteOrcatid > 0)
                            list = (IList)ctx.ForecastSiteProduct.Where(b => b.ForecastSiteID == refid).ToList();

                        break;
                    case "DATA_USAGE3":
                        if (siteOrcatid > 0)
                            list = (IList)ctx.ForecastCategoryProduct.Where(b => b.CategoryID == refid).ToList();
                        break;
                        //if (siteOrcatid > 0)
                        //    list = (IList)GetForecastCategory(siteOrcatid).CategoryProducts;
                        //else
                        //    list = (IList)ForecastCategories[index].CategoryProducts;
                        //break;
                }
            }
            else
            {
                switch (_forecastInfo.DataUsage)
                {
                    case "DATA_USAGE1":
                    case "DATA_USAGE2":
                        if (siteOrcatid > 0)
                            list = (IList)ctx.ForecastSiteTest.Where(b => b.ForecastSiteID == refid).ToList();

                        break;
                    case "DATA_USAGE3":
                        if (siteOrcatid > 0)
                            list = (IList)ctx.ForecastCategoryTest.Where(b => b.CategoryID == refid).ToList();
                        break;
                        //if (siteOrcatid > 0)
                        //    list = (IList)GetForecastCategory(siteOrcatid).CategoryProducts;
                        //else
                        //    list = (IList)ForecastCategories[index].CategoryProducts;
                        //break;
                }
            }
            return list;
        }


        private IList GetListOfDataUsages_data(int siteOrcatid, int id)
        {
            IList list = null;
            int index = 0;



            if (_forecastInfo.Methodology == "CONSUMPTION")
            {
                switch (_forecastInfo.DataUsage)
                {
                    case "DATA_USAGE1":
                    case "DATA_USAGE2":
                        if (siteOrcatid > 0)
                            list = (IList)ctx.ForecastSiteProduct.Where(b => b.ForecastSiteID == refid && b.ProductID == id).ToList();

                        break;
                    case "DATA_USAGE3":
                        if (siteOrcatid > 0)
                            list = (IList)ctx.ForecastCategoryProduct.Where(b => b.CategoryID == refid && b.ProductID == id).ToList();
                        break;
                        //if (siteOrcatid > 0)
                        //    list = (IList)GetForecastCategory(siteOrcatid).CategoryProducts;
                        //else
                        //    list = (IList)ForecastCategories[index].CategoryProducts;
                        //break;
                }
            }
            else
            {
                switch (_forecastInfo.DataUsage)
                {
                    case "DATA_USAGE1":
                    case "DATA_USAGE2":
                        if (siteOrcatid > 0)
                            list = (IList)ctx.ForecastSiteTest.Where(b => b.ForecastSiteID == refid && b.TestID == id).ToList();

                        break;
                    case "DATA_USAGE3":
                        if (siteOrcatid > 0)
                            list = (IList)ctx.ForecastCategoryTest.Where(b => b.CategoryID == refid && b.TestID == id).ToList();
                        break;
                        //if (siteOrcatid > 0)
                        //    list = (IList)GetForecastCategory(siteOrcatid).CategoryProducts;
                        //else
                        //    list = (IList)ForecastCategories[index].CategoryProducts;
                        //break;
                }
            }
            return list;
        }
    }
}
