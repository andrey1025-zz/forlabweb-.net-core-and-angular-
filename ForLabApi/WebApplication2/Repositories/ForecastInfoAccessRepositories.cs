using ForLabApi.DataInterface;
using ForLabApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Repositories
{
    public class ForecastInfoAccessRepositories : IForcastInfo<Siteinsvalidation, ForecastInfo, ForecastSiteInfoList, ForecastSiteInfonew, ForecastCategoryInfo, ForecastCategoryInfoList, ForecastCategorySiteInfo, PatientGroup, Test, ForecastCategorySiteInfolist, ForecastInstrumentlist, ForecastProductUsageDetail, ForecastConsumableUsageDetail, forecastusagesmodel>
    {
        ForLabContext ctx;
        public ForecastInfoAccessRepositories(ForLabContext c)
        {
            ctx = c;
            //return ctx;
        }
        public string lockforecastinfo(int id)
        {
            string res = "";
            var forecastinfo = ctx.ForecastInfo.Find(id);
            if(forecastinfo.Forecastlock==true)
            {
                forecastinfo.Forecastlock = false;
                ctx.SaveChanges();
                res = "Forecastinfo unlock successfully";
            }
            else
            {
                forecastinfo.Forecastlock = true;
                ctx.SaveChanges();
                res = "Forecastinfo lock successfully";
            }
            return res;
        }
        public int saveforecastinfo(ForecastInfo b)
        {
            var ForecastInfo = ctx.ForecastInfo.FirstOrDefault(c => c.ForecastID == b.ForecastID);
            int res = 0;
            var startDATE = new DateTime(b.StartDate.Year, b.StartDate.Month, 1);
            var forecastDATE = new DateTime(b.ForecastDate.Value.Year, b.ForecastDate.Value.Month, 1);
                b.StartDate = startDATE;
            if (b.Method == "MORBIDITY")
                b.ForecastDate = forecastDATE;
            
            if (ForecastInfo != null)
            {
                //if (ForecastInfo.Forecastlock == false)
                //{
                    ForecastInfo.StartDate = b.StartDate;
                    ForecastInfo.ForecastNo = b.ForecastNo;
                    ForecastInfo.ForecastDate = b.ForecastDate;
                    ForecastInfo.Period = b.Period;
                    ForecastInfo.ScopeOfTheForecast = b.ScopeOfTheForecast;
                    ForecastInfo.Extension = b.Extension;
                    ForecastInfo.Countryid = b.Countryid;
                    ctx.SaveChanges();
                //}
                res = b.ForecastID;
                return res;
            }
            else
            {
                var ForecastInfo1 = ctx.ForecastInfo.FirstOrDefault(c => c.ForecastNo == b.ForecastNo && c.UserId==b.UserId);
                if (ForecastInfo1 != null)
                {
                    return res;
                }
                else
                {
                    b.months = 0;
                    b.Forecastlock = false;
                    ctx.ForecastInfo.Add(b);
                    ctx.SaveChanges();
                    res = b.ForecastID;
                    return res;
                }
            }

        }

        public int getprogramid(int id)
        {
            var forecastinfo = ctx.ForecastInfo.Where(b => b.ForecastID == id).FirstOrDefault();
            return forecastinfo.ProgramId;
        }


        public int updateprogram(int id ,int programid)
        {
            var ForecastInfo = ctx.ForecastInfo.Find(id);
            if(ForecastInfo !=null)
            {
                ForecastInfo.ProgramId = programid;
                ctx.SaveChanges();
            }
            return id;
        }
        public ForecastInfo Getbyid(int id)
        {
            var forecastinfo = ctx.ForecastInfo.Find(id);
            return forecastinfo;
        }
        public IEnumerable<Siteinsvalidation> Validationforsiteinstrument(int id)
        {
            var forecastinfo = ctx.ForecastInfo.Find(id);
            int[] siteids;
            int[] testids;
            string testname = "";
            List<Siteinsvalidation> SV = new List<Siteinsvalidation>();
            if (forecastinfo.ForecastType == "S")
            {
                siteids = ctx.ForecastSiteInfo.Where(x => x.ForecastinfoID == id).Select(b => b.SiteID).ToArray();
            }
            else
            {
                siteids = ctx.ForecastCategorySiteInfo.Where(b => b.ForecastInfoID == id).Select(x => x.SiteID).ToArray();
            }
            testids = ctx.TestingProtocol.Where(b => b.ForecastinfoID == id).GroupBy(x => x.TestID).Select(s => s.Key).ToArray();

            for (int i = 0; i < siteids.Length; i++)
            {
                for (int j = 0; j < testids.Length; j++)
                {
                    var testareaid = ctx.Test.Where(b => b.TestID == testids[j]).FirstOrDefault();

                    int count = ctx.siteinstrument.Join(ctx.Instrument, b => b.InstrumentID, c => c.InstrumentID, (b, c) => new { b, c }).Where(x => x.b.SiteID == siteids[i] && x.c.testingArea.TestingAreaID == testareaid.TestingAreaID).Count();
                    if (count == 0)
                    {
                        testname = testname + "," + testareaid.TestName;
                        continue;
                    }
                }
                if (testname !="")
                {
                    SV.Add(new Siteinsvalidation
                    {
                        Sitename = ctx.Site.Where(b => b.SiteID == siteids[i]).Select(x => x.SiteName).FirstOrDefault(),
                        Testname = testname.Trim(',')
                    });
                }
                testname = "";
            }


            return SV;



        }

        public int delforecastsiteinfo(string ids)
        {
            string[] arrids;
            int res = 0;
            arrids = ids.Trim(',').Split(',');

            var getsiteids = ctx.ForecastSiteInfo.Where(b => arrids.Contains(Convert.ToString(b.ID))).Select(x => new


            {
                siteid = x.SiteID,
                forecaseid = x.ForecastinfoID
            }).ToList();
            var forecastsiteinfo = ctx.ForecastSiteInfo.Where(b => arrids.Contains(Convert.ToString(b.ID))).ToList();
            if (forecastsiteinfo.Count > 0)
            {
                var forecastinfo = ctx.ForecastInfo.Find(forecastsiteinfo[0].ForecastinfoID);

                if (forecastinfo.Forecastlock != true)
                {
                    ctx.ForecastSiteInfo.RemoveRange(forecastsiteinfo);
                    res = ctx.SaveChanges();



                    for (int i = 0; i < getsiteids.Count; i++)
                    {
                        var patientassumptin = ctx.PatientAssumption.Where(b => b.ForecastinfoID == getsiteids[i].forecaseid && b.SiteID == getsiteids[i].siteid).FirstOrDefault();
                        ctx.PatientAssumption.Remove(patientassumptin);
                        ctx.SaveChanges();
                    }
                }
            }
            return res;

        }
        public int delforecastcategoryinfo(string ids)
        {
            string[] arrids;
            int res = 0;
            arrids = ids.Trim(',').Split(',');


            var ForecastCategoryInfo1 = ctx.ForecastCategoryInfo.Where(b => arrids.Contains(Convert.ToString(b.ID))).ToList();
            var forecastinfo = ctx.ForecastInfo.Find(ForecastCategoryInfo1[0].ForecastinfoID);
            if (forecastinfo.Forecastlock != true)
            {
                foreach (ForecastCategoryInfo FC in ForecastCategoryInfo1)
                {
                    var forecastsite = ctx.ForecastCategorySiteInfo.Where(c => c.ForecastInfoID == FC.ForecastinfoID && c.CategoryID==FC.SiteCategoryId).ToList();
                    if (forecastsite != null)
                    {

                        ctx.ForecastCategorySiteInfo.RemoveRange(forecastsite);
                        ctx.SaveChanges();
                    }
                }
            
                ctx.ForecastCategoryInfo.RemoveRange(ForecastCategoryInfo1);
                res = ctx.SaveChanges();
            }
            return res;
        }

        public int delforecastcategorysiteinfo(string ids)
        {
            string[] arrids;
            int res = 0;
            arrids = ids.Trim(',').Split(',');


            var ForecastCategorySiteInfo = ctx.ForecastCategorySiteInfo.Where(b => arrids.Contains(Convert.ToString(b.ID))).ToList();
            if (ForecastCategorySiteInfo.Count > 0)
            {
                var forecastinfo = ctx.ForecastInfo.Find(Convert.ToInt32(ForecastCategorySiteInfo[0].ForecastInfoID));
                if (forecastinfo.Forecastlock != true)
                {
                    ctx.ForecastCategorySiteInfo.RemoveRange(ForecastCategorySiteInfo);
                    res = ctx.SaveChanges();
                }

            }
            return res;
        }
        public IEnumerable<ForecastSiteInfonew> Getbyforecastid(int id)
        {

            var forecastsiteinfo = ctx.ForecastSiteInfo.Join(ctx.Site, b => b.SiteID, c => c.SiteID, (b, c) => new { b, c }).Where(f => f.b.ForecastinfoID == id).Select(g => new ForecastSiteInfonew
            {
                SiteID = g.b.SiteID,
                SiteName = g.c.SiteName,
                CurrentPatient = g.b.CurrentPatient,
                TargetPatient = g.b.TargetPatient,
                ForecastinfoID = g.b.ForecastinfoID,
                PopulationNumber = g.b.PopulationNumber,
                PrevalenceRate = g.b.PrevalenceRate,
                ID = g.b.ID

            }).ToList();
            return forecastsiteinfo;
        }

        public IEnumerable<PatientGroup> Getpatientgroupbyforecastid(int id, string programid)
        {

            int cnt = 0;
            decimal totaltargetpatient = 0;
            cnt = ctx.PatientGroup.Where(b => b.ForecastinfoID == id).Count();
            if (cnt > 0)
            {
                totaltargetpatient= Gettotaltargetpatient(id, programid);
                var patientgroup = ctx.PatientGroup.Join(ctx.MMGroup, b => b.GroupID, c => c.Id, (b, c) => new { b, c })
                    .Where(g => g.c.ProgramId == Convert.ToInt32(programid) && g.c.IsActive == true).Where(j => j.b.ForecastinfoID == id).Select(z => new PatientGroup
                    {
                        ID = z.b.ID,
                        ForecastinfoID = z.b.ForecastinfoID,
                        PatientGroupName = z.b.PatientGroupName,
                        PatientPercentage = z.b.PatientPercentage,
                        PatientRatio = z.b.PatientRatio,
                        GroupID = z.b.GroupID
                    }).ToList();
                for (int i = 0; i < patientgroup.Count; i++)
                {
                    patientgroup[i].PatientRatio = (patientgroup[i].PatientPercentage * totaltargetpatient) / 100;
                }
                return patientgroup;
            }
            else
            {
                var patientgroup = ctx.MMGroup
                  .Where(g => g.ProgramId == Convert.ToInt32(programid) && g.IsActive == true).Select(z => new PatientGroup
                  {
                      ID = 0,
                      ForecastinfoID = id,
                      PatientGroupName = z.GroupName,
                      PatientPercentage = 0,
                      PatientRatio = 0,
                      GroupID = z.Id

                  }).ToList();
                return patientgroup;
            }
        }

        public int Isdataimported(int id)
        {
            int i = 0;
           i= ctx.ForecastSite.Where(b => b.ForecastInfoId == id).Count();
           i = i + ctx.ForecastCategory.Where(b => b.ForecastId == id).Count();
            return i;
        }
        public IEnumerable<PatientGroup> Getpatientgroupbydemoforecastid(int id, string programid)
        {

            int cnt = 0;
            decimal totaltargetpatient = 0;
            cnt = ctx.PatientGroup.Where(b => b.ForecastinfoID == id).Count();
            if (cnt > 0)
            {
                totaltargetpatient = Gettotaltargetpatient(id, programid);
                var patientgroup = ctx.PatientGroup.Join(ctx.DemographicMMGroup, b => b.GroupID, c => c.Id, (b, c) => new { b, c })
                    .Where(g => g.c.ProgramId == Convert.ToInt32(programid) && g.c.IsActive == true).Where(j => j.b.ForecastinfoID == id).Select(z => new PatientGroup
                    {
                        ID = z.b.ID,
                        ForecastinfoID = z.b.ForecastinfoID,
                        PatientGroupName = z.b.PatientGroupName,
                        PatientPercentage = z.b.PatientPercentage,
                        PatientRatio = z.b.PatientRatio,
                        GroupID = z.b.GroupID
                    }).ToList();
                for (int i = 0; i < patientgroup.Count; i++)
                {
                    patientgroup[i].PatientRatio = (patientgroup[i].PatientPercentage * totaltargetpatient) / 100;
                }
                return patientgroup;
            }
            else
            {
                var patientgroup = ctx.DemographicMMGroup
                  .Where(g => g.ProgramId == Convert.ToInt32(programid) && g.Forecastid==id && g.IsActive == true).Select(z => new PatientGroup
                  {
                      ID = 0,
                      ForecastinfoID = id,
                      PatientGroupName = z.GroupName,
                      PatientPercentage = 0,
                      PatientRatio = 0,
                      GroupID = z.Id

                  }).ToList();
                return patientgroup;
            }
        }
        public int savepatientgroup(IEnumerable<PatientGroup> b)
        {
            int res = 0;
            var forecastinfo = ctx.ForecastInfo.Find(b.FirstOrDefault().ForecastinfoID);
            if (forecastinfo.Forecastlock != true)
            {
                foreach (PatientGroup pg in b)
                {

                    //pg = (PatientGroup)b.GetValue(i);
                    var patientgroup = ctx.PatientGroup.Find(pg.ID);
                    if (patientgroup != null)
                    {
                        patientgroup.PatientPercentage = pg.PatientPercentage;
                        patientgroup.PatientRatio = pg.PatientRatio;
                        ctx.SaveChanges();
                        res = patientgroup.ID;

                    }
                    else
                    {
                        PatientGroup newpg = new PatientGroup();
                        newpg.ForecastinfoID = pg.ForecastinfoID;
                        newpg.GroupID = pg.GroupID;
                        newpg.PatientGroupName = pg.PatientGroupName;
                        newpg.PatientPercentage = pg.PatientPercentage;
                        newpg.PatientRatio = pg.PatientRatio;
                        ctx.PatientGroup.Add(newpg);
                        ctx.SaveChanges();
                        res = newpg.ID;
                    }
                }

            }
            return res;



        }
        public string getforecasttype(int id)
        {
            var forecasttype = ctx.ForecastInfo.Where(b => b.ForecastID == id).Select(x => x.ForecastType).FirstOrDefault();
            return forecasttype;
        }
        public int savecategorysiteinfo(IEnumerable<ForecastCategorySiteInfolist> b,int userid)
        {
            int res = 0;
            foreach (ForecastCategorySiteInfolist FAS in b)
            {
                var forecastsitecategory = ctx.ForecastCategorySiteInfo.Where(c => c.ForecastInfoID == FAS.ForecastInfoID && c.CategoryID == FAS.CategoryID && c.SiteID == FAS.SiteID).FirstOrDefault();
                if (forecastsitecategory==null)
                {
                    ForecastCategorySiteInfo FS = new ForecastCategorySiteInfo();
                    FS.CategoryID = FAS.CategoryID;
                    FS.SiteID = FAS.SiteID;
                    FS.ForecastInfoID = FAS.ForecastInfoID;
                    FS.UserId = userid;
                    ctx.ForecastCategorySiteInfo.Add(FS);
                    ctx.SaveChanges();
                    res = FS.ID;
                }


            }
            return res;
        }
        public Array getrecentforecast(int id,int userid, string Role)
        {
            Array array;

            if (Role == "admin")
            {

            }
            else
            {
                var adminuserid = ctx.User.Where(b => b.Role == "admin").Select(x =>

                x.Id

           ).FirstOrDefault();
              

            }
            var list = ctx.ForecastedResult.Where(x =>  x.TotalValue != 0)
            .GroupBy(s => new {s.ForecastId, s.SiteId, s.ProductTypeId, s.ProductId })
            .Select(n => new
            {
                forecastid=n.Key.ForecastId,
                totalcost = Math.Round(Math.Ceiling(n.Sum(s => s.TotalValue) / n.Max(s => s.PackQty)) * n.Max(s => s.PackPrice), 2),

            }).ToList();

            if (id > 0)
            {




              



                if (Role == "admin")
                {
                    array = ctx.ForecastInfo.Select(x => new
                    {
                        ID = x.ForecastID,
                        Name = x.ForecastNo,
                        Dateofforecast = x.LastUpdated.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture),
                        Duration = x.StartDate.ToString("MMM") + "-" + x.StartDate.Year + "  " + x.ForecastDate.Value.ToString("MMM") + "-" + x.ForecastDate.Value.Year,
                        forecastvalue =x.Methodology != "MORBIDITY" ? String.Format("{0:n}", ctx.ForecastedResult.Where(b => b.ForecastId == x.ForecastID).Sum(s => s.PackPrice)) : String.Format("{0:n}", list.Where(y=>y.forecastid==x.ForecastID).Sum(d=>d.totalcost)),
                        userid = x.UserId,
                     x.Methodology
                    }).OrderByDescending(s => s.ID).Take(5).ToArray();
                }
                else
                {
                    array = ctx.ForecastInfo.Where(b=>b.UserId==userid).Select(x => new
                    {
                        ID = x.ForecastID,
                        Name = x.ForecastNo,
                        Dateofforecast = x.LastUpdated.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture),
                        Duration = x.StartDate.ToString("MMM") + "-" + x.StartDate.Year + "  " + x.ForecastDate.Value.ToString("MMM") + "-" + x.ForecastDate.Value.Year,
                        forecastvalue = x.Methodology != "MORBIDITY" ? String.Format("{0:n}", ctx.ForecastedResult.Where(b => b.ForecastId == x.ForecastID).Sum(s => s.PackPrice)) : String.Format("{0:n}", list.Where(y => y.forecastid == x.ForecastID).Sum(d => d.totalcost)),
                        userid = x.UserId,
                       x.Methodology
                    }).OrderByDescending(s => s.ID).Take(5).ToArray();

                }
              

              
            }
            else
            {


                if (Role == "admin")
                {
                    array = ctx.ForecastInfo.Select(x => new
                    {
                        ID = x.ForecastID,
                        Name = x.ForecastNo,
                        Dateofforecast = x.LastUpdated.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture),
                        Duration = x.StartDate.ToString("MMM") + "-" + x.StartDate.Year + "  " + x.ForecastDate.Value.ToString("MMM") + "-" + x.ForecastDate.Value.Year,
                        forecastvalue = x.Methodology != "MORBIDITY" ? String.Format("{0:n}", ctx.ForecastedResult.Where(b => b.ForecastId == x.ForecastID).Sum(s => s.PackPrice)) : String.Format("{0:n}", list.Where(y => y.forecastid == x.ForecastID).Sum(d => d.totalcost)),
                        userid = x.UserId,
                       x.Methodology
                    }).OrderByDescending(s => s.ID).ToArray();
                }
                else
                {
                    array = ctx.ForecastInfo.Where(b => b.UserId == userid).Select(x => new
                    {
                        ID = x.ForecastID,
                        Name = x.ForecastNo,
                        Dateofforecast = x.LastUpdated.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture),
                        Duration = x.StartDate.ToString("MMM") + "-" + x.StartDate.Year + "  " + x.ForecastDate.Value.ToString("MMM") + "-" + x.ForecastDate.Value.Year,
                        forecastvalue = x.Methodology != "MORBIDITY" ? String.Format("{0:n}", ctx.ForecastedResult.Where(b => b.ForecastId == x.ForecastID).Sum(s => s.PackPrice)) : String.Format("{0:n}", list.Where(y => y.forecastid == x.ForecastID).Sum(d => d.totalcost)),
                        userid = x.UserId,
                       x.Methodology
                    }).OrderByDescending(s => s.ID).ToArray();


                }

             
            }

          
                return array;
        }

      
        public IEnumerable<ForecastCategorySiteInfolist> Getcategorysiteinfobyforecastid(int id,int userid,int categoryid)
        {
            var ForecastCategorySiteInfolist = ctx.ForecastCategorySiteInfo.Join(ctx.Site, b => b.SiteID, c => c.SiteID, (b, c) => new { b, c }).Where(f => f.b.ForecastInfoID == id).Select(g => new ForecastCategorySiteInfolist
            {
                CategoryID = g.b.CategoryID,
                ForecastInfoID = g.b.ForecastInfoID,
                ID = g.b.ID,
                SiteID = g.b.SiteID,
                SiteName =  g.c.SiteName
            }).Where(s=>s.CategoryID==categoryid).ToList();
            int? countryid = ctx.User.Where(b => b.Id == userid).Select(s => s.CountryId).FirstOrDefault();
            var sites=ctx.Site.Where(b=>b.UserId==userid || b.CountryId== countryid).Select(g => new ForecastCategorySiteInfolist
            {
                CategoryID=0,
                ForecastInfoID=id,             
                ID=0,
                SiteID= g.SiteID,
                SiteName= g.SiteName
            }).ToList();



            var sitearry= ForecastCategorySiteInfolist.Union(sites).GroupBy(x => x.SiteID).Select(g => new ForecastCategorySiteInfolist
            {
                CategoryID = g.Max(x => x.CategoryID),
                ForecastInfoID = id,
                ID = g.Max(x => x.ID),
                SiteID = g.Key,
                SiteName = g.Max(x => x.SiteName)
              
              
            }).OrderByDescending(s => s.CategoryID).ToList();
            return sitearry;
        }
        public IEnumerable<ForecastCategoryInfo> Getcategoryinfobyforecastid(int id)
        {
            var forecastcategoryinfo = ctx.ForecastCategoryInfo.Where(f => f.ForecastinfoID == id).ToList();
            return forecastcategoryinfo;
        }
        public int saveforecastsiteinfo(ForecastSiteInfoList b)

        {
            int res = 0;
            var forecastinfo = ctx.ForecastInfo.Find(b.patientnumberusage[0].ForecastinfoID);
            if (forecastinfo.Forecastlock != true)
            {
                foreach (ForecastSiteInfo FS in b.patientnumberusage)
                {
                    var ForecastSiteInfo = ctx.ForecastSiteInfo.Where(c=>c.ForecastinfoID==FS.ForecastinfoID && c.SiteID==FS.SiteID).FirstOrDefault();
                    if (ForecastSiteInfo != null)
                    {
                        ForecastSiteInfo.CurrentPatient = FS.CurrentPatient;
                        ForecastSiteInfo.TargetPatient = FS.TargetPatient;
                        ForecastSiteInfo.PopulationNumber = FS.PopulationNumber;
                        ForecastSiteInfo.PrevalenceRate = FS.PrevalenceRate;
                        ctx.SaveChanges();
                    }
                    else
                    {
                        ForecastSiteInfo siteInfo = new ForecastSiteInfo();
                        siteInfo.ForecastinfoID = FS.ForecastinfoID;
                        siteInfo.SiteID = FS.SiteID;
                        siteInfo.CurrentPatient = FS.CurrentPatient;
                        siteInfo.TargetPatient = FS.TargetPatient;
                        siteInfo.PopulationNumber = FS.PopulationNumber;
                        siteInfo.PrevalenceRate = FS.PrevalenceRate;
                        ctx.ForecastSiteInfo.Add(siteInfo);
                        ctx.SaveChanges();
                        res = siteInfo.ID;
                    }
                }
            }
            return res;
        }
        public List<ForecastInstrumentlist> getallinstrumentbyforecasttest(int Forecstid, int userid,string Role)
        {
            List<ForecastInstrumentlist> FI = new List<ForecastInstrumentlist>();
            //var testids = ctx.ForecastTest.Where(b => b.forecastID == Forecstid).Select(c => c.TestID).ToArray();
            //var testareaids = ctx.Test.Where(b => testids.Contains(b.TestID)).Select(c => c.TestingAreaID).ToArray();
            //var Roles = ctx.User.Where(b => b.Id == userid).Select(x => x.Role).FirstOrDefault();
            var TestingArea = ctx.TestingArea.OrderBy(b => b.AreaName).ToList();

          //  var TestingArea = ctx.TestingArea.OrderByDescending(x => x.TestingAreaID).ToList();

            if (userid == 0)
            {
                TestingArea = TestingArea.Join(ctx.User, b => b.UserId, c => c.Id, (b, c) => new { b, c }).Where(x => x.c.Role == "admin").Select(x => new TestingArea
                {
                    AreaName = x.b.AreaName,
                    Category = x.b.Category,
                    TestingAreaID = x.b.TestingAreaID,
                    UseInDemography = x.b.UseInDemography,
                    UserId = x.b.UserId
                }).ToList();
            }
            else
            {
                if (Role == "admin")
                {

                }
                else
                {
                    var adminuserid = ctx.User.Where(b => b.Role == "admin").Select(x =>

                    x.Id

               ).FirstOrDefault();
                    TestingArea = TestingArea.Where(b => b.UserId == userid || b.UserId == adminuserid || b.Isapprove == true).ToList();
                }
            }



            var Instruments = ctx.Instrument.Select(x => new InstrumentList1
            {
                InstrumentID = x.InstrumentID,
                InstrumentName = x.InstrumentName,
                TestingAreaID = x.testingArea.TestingAreaID,
                type = "N"
            }).ToList();


            var ForecastIns = ctx.ForecastIns.Where(b => b.forecastID == Forecstid).Select(c=>c.InsID).ToList();
            for (int i = 0; i < ForecastIns.Count; i++)
            {
                for (int j = 0; j < Instruments.Count; j++)
                {
                    if (ForecastIns[i] == Instruments[j].InstrumentID)
                    {
                        Instruments[j].type = "A";
                        break;
                    }
                }
            }




            for (int i = 0; i < TestingArea.Count; i++)
            {
                FI.Add(new ForecastInstrumentlist
                {
                    testareaid = TestingArea[i].TestingAreaID,
                    testareaname = TestingArea[i].AreaName,
                    instruments = Instruments.Where(x => x.TestingAreaID == TestingArea[i].TestingAreaID).ToArray()

                });
            }


            return FI;
        }
        public void updateforecast(int forecastid, string type)
        {
            var forecastinfo = ctx.ForecastInfo.Find(forecastid);
            if(forecastinfo !=null)
            {
                forecastinfo.Methodology = type;
                if(type=="S")
                {
                    forecastinfo.DataUsage = "DATA_USAGE1";
                }
                else
                {
                    forecastinfo.DataUsage = "DATA_USAGE2";
                }
            }
            ctx.SaveChanges();

        }
        public void updateforecast01(int forecastid, string type)
        {
            var forecastinfo = ctx.ForecastInfo.Find(forecastid);
            if (forecastinfo != null)
            {
                forecastinfo.ForecastType = type;
            }
            ctx.SaveChanges();
        }
        public void updateforecast02(int forecastid, string data)
        {
            string[] param= new string[2] ;
            param = data.Split(",");
            var forecastinfo = ctx.ForecastInfo.Find(forecastid);
            if (forecastinfo != null)
            {
                forecastinfo.Scaleup =Convert.ToDecimal(param[1]);
                forecastinfo.Westage = Convert.ToDecimal(param[0]);
            }
            ctx.SaveChanges();
        }
        public int saveforecastcategoryinfo(ForecastCategoryInfoList b)

        {

            int res = 0;
            var forecastinfo = ctx.ForecastInfo.Find(b.patientnumberusage[0].ForecastinfoID);
            if (forecastinfo.Forecastlock != true)
            {
                foreach (ForecastCategoryInfo FS in b.patientnumberusage)
                {
                    var ForecastSiteInfo = ctx.ForecastCategoryInfo.Where(c => c.ForecastinfoID == FS.ForecastinfoID && c.SiteCategoryId == FS.SiteCategoryId).FirstOrDefault();
                    if (ForecastSiteInfo != null)
                    {
                        ForecastSiteInfo.CurrentPatient = FS.CurrentPatient;
                        ForecastSiteInfo.TargetPatient = FS.TargetPatient;
                        ForecastSiteInfo.PopulationNumber = FS.PopulationNumber;
                        ForecastSiteInfo.PrevalenceRate = FS.PrevalenceRate;
                        ctx.SaveChanges();
                    }
                    else
                    {
                        ForecastCategoryInfo catinfo = new ForecastCategoryInfo();
                        catinfo.ForecastinfoID = FS.ForecastinfoID;
                        catinfo.SiteCategoryId = FS.SiteCategoryId;
                        catinfo.SiteCategoryName = FS.SiteCategoryName;
                        catinfo.CurrentPatient = FS.CurrentPatient;
                        catinfo.TargetPatient = FS.TargetPatient;
                        catinfo.PopulationNumber = FS.PopulationNumber;
                        catinfo.PrevalenceRate = FS.PrevalenceRate;
                        catinfo.methodtype = FS.methodtype;
                        ctx.ForecastCategoryInfo.Add(catinfo);
                        ctx.SaveChanges();
                        res = catinfo.ID;
                    }
                }
                foreach (ForecastCategorySiteInfo FS in b.categorysite)
                {
                    var ForecastCategorySiteInfo = ctx.ForecastCategorySiteInfo.Where(c=>c.CategoryID==FS.CategoryID && c.ForecastInfoID==FS.ForecastInfoID && c.SiteID==FS.SiteID).FirstOrDefault();
                    if (ForecastCategorySiteInfo != null)
                    {

                    }
                    else
                    {
                        ForecastCategorySiteInfo catinfo = new ForecastCategorySiteInfo();
                        catinfo.ForecastInfoID = FS.ForecastInfoID;
                        catinfo.CategoryID = FS.CategoryID;
                        catinfo.SiteID = FS.SiteID;

                        ctx.ForecastCategorySiteInfo.Add(catinfo);
                        ctx.SaveChanges();
                        res = catinfo.ID;
                    }
                }
            }
            return res;

        }
        public int getgroupexistintestingprotocol(int id, int groupid)
        {
            int cnt = 0;
            cnt = ctx.TestingProtocol.Where(b => b.PatientGroupID == groupid && b.ForecastinfoID == id).Count();
            return cnt;
        }

        public int delpatientgroup(int id, int groupid)
        {
            var patiengroup = ctx.PatientGroup.Where(b => b.ForecastinfoID == id && b.GroupID == Convert.ToInt32(groupid)).FirstOrDefault();
            ctx.PatientGroup.Remove(patiengroup);
            int res = ctx.SaveChanges();
            return res;

        }
        public decimal Gettotaltargetpatient(int id, string programid)
        {
            decimal totaltargetpatient = 0;
            int forecastmethod = ctx.MMForecastParameter.Where(b => b.ProgramId == Convert.ToInt32(programid)).Select(x => x.ForecastMethod).FirstOrDefault();

            string forecasttype = ctx.ForecastInfo.Where(b => b.ForecastID == id).Select(x => x.ForecastType).FirstOrDefault();

            if (forecastmethod == 1)
            {
                if (forecasttype == "S")
                {
                    totaltargetpatient = ctx.ForecastSiteInfo.Where(b => b.ForecastinfoID == id).Sum(x => x.TargetPatient);
                }
                else
                {
                    totaltargetpatient = ctx.ForecastCategoryInfo.Where(b => b.ForecastinfoID == id).Sum(x => x.TargetPatient);

                }
            }
            else
            {
                if (forecasttype == "S")
                {
                    totaltargetpatient = ctx.ForecastSiteInfo.Where(b => b.ForecastinfoID == id).Sum(x => (x.PopulationNumber * x.PrevalenceRate) / 100);
                }
                else
                {
                    totaltargetpatient = ctx.ForecastCategoryInfo.Where(b => b.ForecastinfoID == id).Sum(x => (x.PopulationNumber * x.PrevalenceRate) / 100);

                }


            }
            return totaltargetpatient;
        }

        public List<ForecastProductUsageDetail> getControlProductUsage(int forecastid, int testid)
        {
            var result = (from PU in ctx.ForecastProductUsage
                          where PU.Forecastid == forecastid && PU.IsForControl == true && PU.TestId == testid
                          join Ins in ctx.Instrument on PU.InstrumentId equals Ins.InstrumentID
                          join MP in ctx.MasterProduct on PU.ProductId equals MP.ProductID
                          join ts in ctx.Test on PU.TestId equals ts.TestID
                          select new ForecastProductUsagelist
                          {
                              Id = PU.Id,
                              ProductId = MP.ProductID,
                              ProductName = MP.ProductName,
                              InstrumentId = Ins.InstrumentID,
                              InstrumentName = Ins.InstrumentName,
                              Rate = PU.Rate,
                              ProductUsedIn = PU.ProductUsedIn,
                              IsForControl = PU.IsForControl,
                              TestId = PU.TestId,
                              // test = ts.TestName
                          }

                       ).ToList();
            var result1 = (from PU in ctx.ProductUsage
                           where PU.TestId == testid && PU.IsForControl == true
                           join Ins in ctx.Instrument on PU.InstrumentId equals Ins.InstrumentID
                           join MP in ctx.MasterProduct on PU.ProductId equals MP.ProductID
                           join ts in ctx.Test on PU.TestId equals ts.TestID
                           select new ForecastProductUsagelist
                           {
                               Id = PU.Id,
                               ProductId = MP.ProductID,
                               ProductName = MP.ProductName,
                               InstrumentId = Ins.InstrumentID,
                               InstrumentName = Ins.InstrumentName,
                               Rate = PU.Rate,
                               ProductUsedIn = PU.ProductUsedIn,
                               IsForControl = PU.IsForControl,
                               TestId = PU.TestId,
                               // test = ts.TestName
                           }

                  ).ToList();
            var Finalresult = result.Union(result1).GroupBy(b => new { b.TestId, b.InstrumentId, b.ProductId }).Select(x => new ForecastProductUsagelist
            {
                ProductId = x.Key.ProductId,
                ProductName = x.Max(s => s.ProductName),
                InstrumentId = x.Key.InstrumentId,
                InstrumentName = x.Max(s => s.InstrumentName),
                Rate = x.Max(s => s.Rate),
                ProductUsedIn = x.Max(s => s.ProductUsedIn),
                IsForControl = x.Max(s => s.IsForControl),
                TestId = x.Key.TestId,

            }).ToList();



            var ins = Finalresult.GroupBy(x => x.InstrumentId).Select(lg =>
                                new {
                                    id=lg.Key,
                                    name = lg.Max(w => w.InstrumentName)

                                }).ToList();

            List<ForecastProductUsageDetail> productUsagelist = new List<ForecastProductUsageDetail>();
            foreach (var i in ins)
            {
                productUsagelist.Add(new ForecastProductUsageDetail
                {
                    ID=i.id,
                    name = i.name,
                    value = Finalresult.Where(b => b.InstrumentName == i.name).ToList()
                }

                    );

            }

            return productUsagelist;

        }
       public List<ForecastProductUsageDetail> getForecastProductUsage(int forecastid,int testid)
        {
            var result = (from PU in ctx.ForecastProductUsage
                          where PU.Forecastid==forecastid && PU.IsForControl == false && PU.TestId==testid
                          join Ins in ctx.Instrument on PU.InstrumentId equals Ins.InstrumentID
                          join MP in ctx.MasterProduct on PU.ProductId equals MP.ProductID
                          join ts in ctx.Test on PU.TestId equals ts.TestID
                          select new ForecastProductUsagelist
                          {
                              Id = PU.Id,
                              ProductId = MP.ProductID,
                              ProductName = MP.ProductName,
                              InstrumentId = Ins.InstrumentID,
                              InstrumentName = Ins.InstrumentName,
                              Rate = PU.Rate,
                              ProductUsedIn = PU.ProductUsedIn,
                              IsForControl = PU.IsForControl,
                              TestId = PU.TestId,
                             // test = ts.TestName
                          }

                      ).ToList();
            var result1 = (from PU in ctx.ProductUsage
                          where PU.TestId == testid && PU.IsForControl == false
                           join Ins in ctx.Instrument on PU.InstrumentId equals Ins.InstrumentID
                          join MP in ctx.MasterProduct on PU.ProductId equals MP.ProductID
                          join ts in ctx.Test on PU.TestId equals ts.TestID
                          select new ForecastProductUsagelist
                          {
                              Id = PU.Id,
                              ProductId = MP.ProductID,
                              ProductName = MP.ProductName,
                              InstrumentId = Ins.InstrumentID,
                              InstrumentName = Ins.InstrumentName,
                              Rate = PU.Rate,
                              ProductUsedIn = PU.ProductUsedIn,
                              IsForControl = PU.IsForControl,
                              TestId = PU.TestId,
                              // test = ts.TestName
                          }

                  ).ToList();
           var Finalresult= result.Union(result1).GroupBy(b => new { b.TestId, b.InstrumentId, b.ProductId }).Select(x => new ForecastProductUsagelist
            {
                ProductId = x.Key.ProductId,
                ProductName =x.Max(s=>s.ProductName),
                InstrumentId = x.Key.InstrumentId,
                InstrumentName = x.Max(s => s.InstrumentName),
                Rate = x.Max(s => s.Rate),
                ProductUsedIn = x.Max(s => s.ProductUsedIn),
                IsForControl = x.Max(s => s.IsForControl),
                TestId = x.Key.TestId,

            } ).ToList();



            var ins = Finalresult.GroupBy(x => x.InstrumentId).Select(lg =>
                                new {
                                    id = lg.Key,
                                    name = lg.Max(w => w.InstrumentName)

                                }).ToList();

            List<ForecastProductUsageDetail> productUsagelist = new List<ForecastProductUsageDetail>();
            foreach (var i in ins)
            {
                productUsagelist.Add(new ForecastProductUsageDetail
                {
                    ID=i.id,

                    name = i.name,
                    value = Finalresult.Where(b => b.InstrumentName == i.name).ToList()
                }

                    );

            }

            return productUsagelist;
        }
        public void saveforecastusges(forecastusagesmodel FUM)
        {
            if (FUM.ForecastProductUsage.Count > 0)
            {
                var existforecastusage = ctx.ForecastProductUsage.Where(b => b.Forecastid == FUM.ForecastProductUsage[0].Forecastid && b.TestId == FUM.ForecastProductUsage[0].TestId).ToList();
                if (existforecastusage != null)
                {
                    ctx.ForecastProductUsage.RemoveRange(existforecastusage);
                    ctx.SaveChanges();

                    var productusage = FUM.ForecastProductUsage;
                    ctx.ForecastProductUsage.AddRange(FUM.ForecastProductUsage);
                    ctx.SaveChanges();
                }
            }
            if (FUM.ForecastConsumableUsage.Count > 0)
            {
                var existforecastconumable = ctx.ForecastConsumableUsage.Where(b => b.Forecastid == FUM.ForecastConsumableUsage[0].Forecastid && b.TestId == FUM.ForecastConsumableUsage[0].TestId).ToList();
                if (existforecastconumable != null)
                {
                    ctx.ForecastConsumableUsage.RemoveRange(existforecastconumable);
                    ctx.SaveChanges();
                    var consumbleusage = FUM.ForecastConsumableUsage;
                    ctx.ForecastConsumableUsage.AddRange(consumbleusage);
                    ctx.SaveChanges();
                }
            }
        }
        public List<ForecastConsumableUsageDetail> getForecastConsumbleUsagePertest(int forecastid, int testid)
        {
            List<ForecastConsumableUsageDetail> CUL = new List<ForecastConsumableUsageDetail>();
            var result = (from PU in ctx.ForecastConsumableUsage
                          where PU.Forecastid == forecastid && PU.PerTest == true && PU.TestId == testid

                          join MP in ctx.MasterProduct on PU.ProductId equals MP.ProductID
                          join PT in ctx.ProductType on MP.ProductTypeId equals PT.TypeID
                          join ts in ctx.Test on PU.TestId equals ts.TestID
                          select new ForecastConsumableUsagelist
                          {
                              Id = PU.Id,
                              ProductId = MP.ProductID,
                              ProductName = MP.ProductName,
                              ProductTypeId = PT.TypeID,
                              ProductTypeName = PT.TypeName,
                             
                              UsageRate = PU.UsageRate,
                            NoOfTest=PU.NoOfTest,
                            PerInstrument=PU.PerInstrument,
                            Period=PU.Period,
                            PerPeriod=PU.PerPeriod,
                            PerTest=PU.PerTest,
                            test=ts.TestName,
                              TestId = PU.TestId,
                              // test = ts.TestName
                          }

                      ).ToList();


            var consumablelist = (from CU in ctx.ConsumableUsage
                                  join MC in ctx.MasterConsumable on CU.ConsumableId equals MC.MasterCID
                                  join MP in ctx.MasterProduct on CU.ProductId equals MP.ProductID
                                  join PT in ctx.ProductType on MP.ProductTypeId equals PT.TypeID
                                  join ts in ctx.Test on MC.TestId equals ts.TestID
                                  where MC.TestId == testid && CU.PerTest == true
                                  select new ForecastConsumableUsagelist
                                  {
                                      Id = CU.Id,
                                      ProductId = MP.ProductID,
                                      ProductName = MP.ProductName,
                                      ProductTypeId = PT.TypeID,
                                      ProductTypeName = PT.TypeName,
                                    
                                      UsageRate = CU.UsageRate,
                                      NoOfTest = CU.NoOfTest,
                                      PerInstrument = CU.PerInstrument,
                                      Period = CU.Period,
                                      PerPeriod = CU.PerPeriod,
                                      PerTest = CU.PerTest,
                                      test = ts.TestName,
                                      TestId = MC.TestId,


                                    


                                  }


                         ).ToList();


            var Finalresult = result.Union(consumablelist).GroupBy(b => new { b.TestId,b.ProductTypeId,b.ProductId }).Select(x => new ForecastConsumableUsagelist
            {

                ProductId = x.Key.ProductId,
                ProductName = x.Max(s => s.ProductName),
                ProductTypeId = x.Key.ProductTypeId,
                ProductTypeName = x.Max(s => s.ProductTypeName),

                UsageRate = x.Max(s => s.UsageRate),
                NoOfTest = x.Sum(s => s.NoOfTest),
                PerInstrument = x.Max(s => s.PerInstrument),
                Period = x.Max(s => s.Period),
                PerPeriod = x.Max(s => s.PerPeriod),
                PerTest = x.Max(s => s.PerTest),
                test = x.Max(s => s.test),
                TestId = x.Key.TestId



               

            }).ToList();

            var ins = Finalresult.GroupBy(x => x.ProductTypeId).Select(lg =>
                        new
                        {
                            id = lg.Key,
                            name = lg.Max(w => w.ProductTypeName)

                        }).ToList();


            foreach (var i in ins)
            {
                CUL.Add(new ForecastConsumableUsageDetail
                {
                    ID=i.id,
                    name = i.name,
                    value = Finalresult.Where(b => b.ProductTypeName == i.name).ToList()
                }

                    );

            }
            return CUL;
        }
        public List<ForecastConsumableUsageDetail> getForecastConsumbleUsagePerPeriod(int forecastid, int testid)
        {
            List<ForecastConsumableUsageDetail> CUL = new List<ForecastConsumableUsageDetail>();

            var consumablelist = (from CU in ctx.ForecastConsumableUsage
                                 
                                  join MP in ctx.MasterProduct on CU.ProductId equals MP.ProductID
                                  join PT in ctx.ProductType on MP.ProductTypeId equals PT.TypeID
                                  where CU.TestId == testid && CU.PerPeriod == true
                                  select new ForecastConsumableUsagelist
                                  {
                                      Id = CU.Id,
                                      ProductId = MP.ProductID,

                                      ProductName = MP.ProductName,
                                      ProductTypeId = PT.TypeID,
                                      ProductTypeName = PT.TypeName,
                                      PerInstrument = CU.PerInstrument,
                                      UsageRate = CU.UsageRate,
                                      PerPeriod = CU.PerPeriod,
                                      PerTest = CU.PerTest,
                                      Period = CU.Period,
                                      NoOfTest = CU.NoOfTest,
                                      TestId = CU.TestId
                                  }


                       ).ToList();


            var consumablelist1 = (from CU in ctx.ConsumableUsage
                                  join MC in ctx.MasterConsumable on CU.ConsumableId equals MC.MasterCID
                                  join MP in ctx.MasterProduct on CU.ProductId equals MP.ProductID
                                  join PT in ctx.ProductType on MP.ProductTypeId equals PT.TypeID
                                  where MC.TestId == testid && CU.PerPeriod == true
                                  select new ForecastConsumableUsagelist
                                  {
                                      Id = CU.Id,
                                      ProductId = MP.ProductID,
                                    
                                      ProductName = MP.ProductName,
                                      ProductTypeId = PT.TypeID,
                                      ProductTypeName = PT.TypeName,
                                      PerInstrument = CU.PerInstrument,
                                      UsageRate = CU.UsageRate,
                                      PerPeriod = CU.PerPeriod,
                                      PerTest = CU.PerTest,
                                      Period = CU.Period,
                                      NoOfTest = CU.NoOfTest,
                                      TestId = MC.TestId
                                  }


                         ).ToList();


            var Finalresult = consumablelist.Union(consumablelist1).GroupBy(b => new { b.TestId, b.ProductTypeId, b.ProductId }).Select(x => new ForecastConsumableUsagelist
            {

                ProductId = x.Key.ProductId,
                ProductName = x.Max(s => s.ProductName),
                ProductTypeId = x.Key.ProductTypeId,
                ProductTypeName = x.Max(s => s.ProductTypeName),

                UsageRate = x.Max(s => s.UsageRate),
                NoOfTest = x.Sum(s => s.NoOfTest),
                PerInstrument = x.Max(s => s.PerInstrument),
                Period = x.Max(s => s.Period),
                PerPeriod = x.Max(s => s.PerPeriod),
                PerTest = x.Max(s => s.PerTest),
                test = x.Max(s => s.test),
                TestId = x.Key.TestId





            }).ToList();

            var ins = Finalresult.GroupBy(x => x.ProductTypeId).Select(lg =>
                      new
                      {
                          id = lg.Key,
                          name = lg.Max(w => w.ProductTypeName)

                      }).ToList();

            foreach (var i in ins)
            {
                CUL.Add(new ForecastConsumableUsageDetail
                {
                    ID=i.id,
                    name = i.name,
                    value = Finalresult.Where(b => b.ProductTypeName == i.name).ToList()
                }

                    );

            }


            return CUL;

        }


        public List<ForecastConsumableUsageDetail> getForecastConsumbleUsagePerinstrument(int forecastid, int testid)
        {
            List<ForecastConsumableUsageDetail> CUL = new List<ForecastConsumableUsageDetail>();

            var consumablelist1 = (from CU in ctx.ForecastConsumableUsage
                                 // join MC in ctx.MasterConsumable on CU.ConsumableId equals MC.MasterCID
                                  join Ins in ctx.Instrument on CU.InstrumentId equals Ins.InstrumentID
                                  join MP in ctx.MasterProduct on CU.ProductId equals MP.ProductID
                                  where CU.TestId == testid
                                  select new ForecastConsumableUsagelist()
                                  {
                                      Id = CU.Id,
                                      ProductId = MP.ProductID,

                                      ProductName = MP.ProductName,
                                      InstrumentId = Ins.InstrumentID,
                                      InstrumentName = Ins.InstrumentName,
                                      PerInstrument = CU.PerInstrument,
                                      UsageRate = CU.UsageRate,
                                      PerPeriod = CU.PerPeriod,
                                      PerTest = CU.PerTest,
                                      Period = CU.Period,
                                      NoOfTest = CU.NoOfTest,
                                      TestId = CU.TestId
                                  }


                 ).ToList();

            var consumablelist = (from CU in ctx.ConsumableUsage
                                  join MC in ctx.MasterConsumable on CU.ConsumableId equals MC.MasterCID
                                  join Ins in ctx.Instrument on CU.InstrumentId equals Ins.InstrumentID
                                  join MP in ctx.MasterProduct on CU.ProductId equals MP.ProductID
                                  where MC.TestId == testid
                                  select new ForecastConsumableUsagelist()
                                  {
                                      Id = CU.Id,
                                      ProductId = MP.ProductID,
                                     
                                      ProductName = MP.ProductName,
                                      InstrumentId = Ins.InstrumentID,
                                      InstrumentName = Ins.InstrumentName,
                                      PerInstrument = CU.PerInstrument,
                                      UsageRate = CU.UsageRate,
                                      PerPeriod = CU.PerPeriod,
                                      PerTest = CU.PerTest,
                                      Period = CU.Period,
                                      NoOfTest = CU.NoOfTest,
                                      TestId = MC.TestId
                                  }


                  ).ToList();

            var Finalresult = consumablelist.Union(consumablelist1).GroupBy(b => new { b.TestId, b.InstrumentId, b.ProductId }).Select(x => new ForecastConsumableUsagelist
            {

                ProductId = x.Key.ProductId,

                ProductName = x.Max(s=>s.ProductName),
                InstrumentId = x.Key.InstrumentId,
                InstrumentName = x.Max(s => s.InstrumentName),
                PerInstrument = x.Max(s => s.PerInstrument),
                UsageRate = x.Max(s => s.UsageRate),
                PerPeriod = x.Max(s => s.PerPeriod),
                PerTest = x.Max(s => s.PerTest),
                Period = x.Max(s => s.Period),
                NoOfTest = x.Max(s => s.NoOfTest),
                TestId = x.Key.TestId




            }).ToList();

            var ins = Finalresult.GroupBy(x => x.InstrumentId).Select(lg =>
                       new
                       {
                           id=lg.Key,

                           name = lg.Max(w => w.InstrumentName)

                       }).ToList();

            foreach (var i in ins)
            {
                CUL.Add(new ForecastConsumableUsageDetail
                {
                    ID=i.id,
                    name = i.name,
                    value = Finalresult.Where(b => b.InstrumentName == i.name).ToList()
                }

                    );

            }

      



            return CUL;

        }
        public decimal Gettotalvalue(int id, string methodology)
        {
            decimal finaltotalcost = 0;
            if (methodology == "SERVICE_STATISTIC")
            {

                var _forecastinfo = ctx.ForecastInfo.Find(id);
                var consumptionforecastsummary = ctx.ForecastedResult
                    .Join(ctx.ForecastInfo, b => b.ForecastId, c => c.ForecastID, (b, c) => new { b, c })
                    .Join(ctx.MasterProduct, e => e.b.ProductId, f => f.ProductID, (e, f) => new { e, f })
                    .Where(x => x.e.b.ServiceConverted == true && x.e.b.ForecastId == id && x.e.b.DurationDateTime >= x.e.c.StartDate && x.e.b.DurationDateTime < GetMaxForecastDate(x.e.c.Period, x.e.c.StartDate, x.e.c.Extension))
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


                var getuniqueproduct = consumptionforecastsummary.GroupBy(x => new { x.ProductType, x.ProductName }).OrderBy(x => x.Max(s => s.DurationDateTime)).Select(s => new { ProductType = s.Key.ProductType, ProductName = s.Key.ProductName }).ToList();
                for (int i = 0; i < getuniqueproduct.Count; i++)
                {

                    decimal Totalproduct = 0;
                    decimal Totalprice = 0;

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

                    if (Totalproduct != 0)
                    {

                        finaltotalcost = finaltotalcost + Totalprice;
                    }
                }
            }
            else if (methodology == "CONSUMPTION")
            {
                var _forecastinfo = ctx.ForecastInfo.Find(id);
                var consumptionforecastsummary = ctx.ForecastedResult
                    .Join(ctx.ForecastInfo, b => b.ForecastId, c => c.ForecastID, (b, c) => new { b, c })
                    .Join(ctx.MasterProduct, e => e.b.ProductId, f => f.ProductID, (e, f) => new { e, f })
                    .Where(x => x.e.b.ForecastId == id && x.e.b.DurationDateTime >= x.e.c.StartDate && x.e.b.DurationDateTime < GetMaxForecastDate(x.e.c.Period, x.e.c.StartDate, x.e.c.Extension)).GroupBy(s => new { s.f.ProductName, s.e.b.Duration, s.e.b.DurationDateTime, s.e.b.ProductType })
                    .Select(n => new
                    {

                        ProductType = n.Key.ProductType,
                        ProductName = n.Key.ProductName,
                        NoofProduct = n.Sum(x => x.e.b.TotalValue),
                        PackQty = n.Sum(x => x.e.b.PackQty),
                        Price = n.Sum(x => x.e.b.PackPrice),
                        Duration = n.Key.Duration,
                        DurationDateTime = n.Key.DurationDateTime

                    }).OrderBy(x => x.DurationDateTime).ToList();



                var getuniqueproduct = consumptionforecastsummary.GroupBy(x => new { x.ProductType, x.ProductName }).OrderBy(x => x.Max(s => s.DurationDateTime)).Select(s => new { ProductType = s.Key.ProductType, ProductName = s.Key.ProductName }).ToList();
                for (int i = 0; i < getuniqueproduct.Count; i++)
                {
                    decimal Totalproduct = 0;
                    decimal Totalprice = 0;

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

                    if (Totalproduct != 0)
                    {

                        finaltotalcost = finaltotalcost + Totalprice;
                    }
                }
            }
            else
            {
                decimal existingpatienttest;
                decimal existingpatient = 0;
                decimal testPermonth;
                decimal totaltest;
                string[] siteids;
                decimal productused = 0;
                decimal getworkingdays = 0;
                decimal existingtestnumber = 0;
                decimal TotalTestPerYear = 0;
                decimal tttest = 0;

                Dynamicarray dynamic = new Dynamicarray();
                List<int> distinctproductids = new List<int>();
                List<Productsummary> Productsummary = new List<Productsummary>();
                var _forecastinfo = ctx.ForecastInfo.Find(id);



                var distintsitecategoryids = ctx.TestByMonth.Where(b => b.ForeCastID == id).GroupBy(x => x.sitecategoryid).Select(x => x.Key).ToList();
                var occurrenceofsamemonth = ctx.TestByMonth.Where(b => b.ForeCastID == id).GroupBy(x => x.Month).Select(s => s.Key).Count();

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
                        var getgeneralvariable = ctx.MMGeneralAssumption.Where(b => b.ProgramId == _forecastinfo.ProgramId && b.Entity_type_id == 5 && !b.VariableName.Contains("Mon")).ToList();
                        foreach (MMGeneralAssumption M in getgeneralvariable)
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





                var getuniqueproduct = ctx.ForecastedResult.Join(ctx.MasterProduct, b => b.ProductId, c => c.ProductID, (b, c) => new { b, c }).Where(g => g.b.ForecastId == id).GroupBy(x => new { x.b.SiteId, x.b.ProductId })
                    .Select(s => new { Productid = s.Key.ProductId, ProductType = s.Max(x => x.b.ProductType), ProductName = s.Max(x => x.c.ProductName), siteid = s.Key.SiteId }).ToList();
                for (int i = 0; i < getuniqueproduct.Count; i++)
                {

                    decimal Totalproduct = 0;
                    decimal Totalprice = 0;
                    decimal packquantity = 0;

                    var demographicduration = ctx.ForecastedResult.Where(x => x.ProductId == getuniqueproduct[i].Productid && x.ForecastId == id).Select(n => new
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

                    if (Totalproduct != 0)
                    {

                        finaltotalcost = finaltotalcost + Math.Round(Math.Ceiling(Totalproduct / packquantity) * Totalprice, 2);
                    }
                }
            }

            return finaltotalcost;
        }

        public decimal getproductusagequantity(decimal rate, decimal testno, decimal insquantity)
        {
            decimal quantity = 0;

            quantity = rate * testno * (insquantity / 100);
            return quantity;
        }
        public decimal Getconsumablequantity(ConsumableUsage CU, string[] siteids, decimal duration, decimal workingdays, decimal testno, double insquantity, int noofsite)
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
        public decimal Getcontrolquantity(Instrument I, decimal rate, decimal duration, decimal workingdays, int noofsite)
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

        public DateTime GetMaxForecastDate(string period, DateTime Startdate, int extension)
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

     
    }

}
