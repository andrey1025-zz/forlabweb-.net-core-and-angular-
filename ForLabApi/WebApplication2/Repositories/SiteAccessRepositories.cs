using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ForLabApi.DataInterface;
using ForLabApi.Models;
using System.Globalization;

namespace ForLabApi.Repositories
{
    public class SiteAccessRepositories : IForLabSite<Site,sitebyregion,Region, defaultsite,SiteInstrumentList>
    {
        ForLabContext ctx;
        public SiteAccessRepositories(ForLabContext c)
        {
            ctx = c;
            //return ctx;
        }
        public IEnumerable<Region> GetregionbycategoryID(int id,int id2,int userid)
        {
            List<Region> region = new List<Region>();
            var regionlist = ctx.Site.Join(ctx.Region, b => b.regionid, c => c.RegionID, (b, c) => new { b, c })                
                .Where(f=>f.b.CategoryID==id )
           .GroupBy(x => new { x.b.regionid }).Select(g => new 
           {

               RegionName = g.Max(x => x.c.RegionName),
               RegionID=g.Key.regionid,
               countryid=g.Max(x=>x.c.CountryId),
               userid=g.Max(x=>x.c.UserId)

           }).ToList();


            if (id2>0)
            {
                region = regionlist.Where(b => b.countryid == id2 || b.userid ==userid ).Select(s=>new Region {
                    RegionName =s.RegionName,
                    RegionID = s.RegionID,

                }).ToList();
            }
            else
            {
                region = regionlist.Select(s => new Region
                {
                    RegionName = s.RegionName,
                    RegionID = s.RegionID,

                }).ToList();
            }


                return region;
        }
        public defaultsite Getdefaultdata(int countryid)
        {
            int userid = 0;
            defaultsite Ds = new defaultsite();

            userid = ctx.User.Where(b => b.Role == "admin").Select(g => g.Id).FirstOrDefault();
            var regionlist = ctx.Region.Where(b => b.CountryId == countryid || b.UserId == userid).ToList();
            Ds.Region = regionlist;
            var sitelist=ctx.Site.Where(b => b.CountryId == countryid || b.UserId == userid)
                .Join(ctx.Region,s=>s.regionid,t=>t.RegionID, (s, t) => new { s, t })
                .Join(ctx.SiteCategory,e=>e.s.CategoryID,f=>f.CategoryID,(e,f)=>new { e,f})
                .Select(g=>new defaultsitelist{

                Region=g.e.t.RegionName,
                SiteName=g.e.s.SiteName,
                SiteCategory=g.f.CategoryName,
                WorkingDays=g.e.s.WorkingDays

            }).ToList();
            Ds.Site = sitelist;
            return Ds;

        }
        public Array GetregionbyCountryID(int id,int userid,string role)
        {
            Array A;
            var regionlist = ctx.Region.Select(g=>new {
                g.RegionName ,
                g.RegionID,
                g.UserId,
                g.CountryId,
                g.ShortName,
                g.Isapprove,
                countryName =""
            }).OrderByDescending(b => b.RegionID).ToList();
            if(role=="admin")
            {
                A = regionlist.ToArray();
            }
            else
            {
                var adminuserid = ctx.User.Where(b => b.Role == "admin").Select(x =>

               x.Id

          ).FirstOrDefault();



                A = regionlist.Where(s => (s.UserId == userid || s.UserId == adminuserid) || (s.CountryId == id && s.Isapprove == true)).Select(g => new
                {
                    g.RegionName,
                    g.RegionID,
                    g.UserId,
                    g.CountryId,
                    g.ShortName,
                    countryName = ctx.Country.Where(b => b.Id == g.CountryId).Select(c=>c.Name).FirstOrDefault()
                }).ToArray();
            }

         

            return A;

        }
        public int SaveData(Site b)
        {
            int res=0;
            var Site = ctx.Site.FirstOrDefault(c => c.SiteName == b.SiteName && c.regionid == b.regionid);
            if (Site != null)
            {
                return 0;
            }
            Site newsite = new Site();

            newsite.SiteName = b.SiteName;

            newsite.CategoryID = b.CategoryID;
            newsite.CD4TestingDaysPerMonth = b.CD4TestingDaysPerMonth;
            newsite.ChemistryTestingDaysPerMonth = b.ChemistryTestingDaysPerMonth;
            newsite.HematologyTestingDaysPerMonth = b.HematologyTestingDaysPerMonth;
            newsite.CountryId = b.CountryId;
            newsite.ViralLoadTestingDaysPerMonth = b.ViralLoadTestingDaysPerMonth;
            newsite.OtherTestingDaysPerMonth = b.ViralLoadTestingDaysPerMonth;
            newsite.regionid = b.regionid;

            // public SiteCategory siteCategory { get; set; }
            newsite.CD4RefSite = b.CD4RefSite;
            newsite.ChemistryRefSite = b.ChemistryRefSite;
            newsite.HematologyRefSite = b.HematologyRefSite;
            newsite.ViralLoadRefSite = b.ViralLoadRefSite;
            newsite.OtherRefSite = b.OtherRefSite;
            newsite.WorkingDays = b.WorkingDays;
            newsite.SiteLevel = b.SiteLevel;
            newsite.Latitude = b.Latitude;
            newsite.Longitude = b.Longitude;
            newsite.UserId = b.UserId;
          ctx.Site.Add(newsite);
           ctx.SaveChanges();
            int res1 = newsite.SiteID;
          
            foreach (SiteTestingdaysList SL in b._sitetestingdays)
            {
                SiteTestingdays std = new SiteTestingdays();
                std.siteid = res1;
                    std.testingareaid = SL.testingareaid;
                    std.testingdays = SL.testingdays;
                    ctx.SiteTestingdays.Add(std);
                    res = ctx.SaveChanges();
               
            }
            ///Update Referral Linkage
         
            foreach (ReferralLinkageList RL in b._ReferralLinkage)
            {
                ReferralLinkage RLD = new ReferralLinkage();
                RLD.referensiteid = res1;
                    RLD.siteid = RL.siteid;
                    RLD.testingareaid = RL.testingareaid;
                    RLD.testingdays = RL.testingdays;

                    ctx.ReferralLinkage.Add(RLD);
                    res = ctx.SaveChanges();
                
            }
            ///Update Site Status
            //foreach (SiteStatus SS in b._siteStatuses)
            //{
            //    var SiteStatus = ctx.SiteStatus.Find(SS.Id);
            //    if (SiteStatus != null)
            //    {
            //        SiteStatus.OpenedFrom = SS.OpenedFrom;
            //        SiteStatus.ClosedOn = SS.ClosedOn;
            //        SiteStatus.SiteID = SS.SiteID;

            //        ctx.SaveChanges();
            //    }
            //    else
            //    {

            //        ctx.SiteStatus.Add(SS);
            //        res = ctx.SaveChanges();
            //    }
            //}
            //res = ctx.SaveChanges();

            ///Update Site instrument
       //     SiteInstrument SI = new SiteInstrument();
            foreach (SiteInstrumentList SIL in b._siteInstruments)
            {
                SiteInstrument SI = new SiteInstrument();
                SI.SiteID = res1;
                    SI.InstrumentID = SIL.InstrumentID;
                    SI.Quantity = SIL.Quantity;
                    SI.TestRunPercentage = SIL.TestRunPercentage;
                SI.UserId = b.UserId;
                    ctx.siteinstrument.Add(SI);
                    res = ctx.SaveChanges();
                
            }

            return res1;
        }
        public Array Getcountrylist()
        {
            var country = ctx.Country.ToArray();
            return country;
        }
        public int DeleteData(int id)
        {
            int res = 0;
            var Site = ctx.Site.FirstOrDefault(b => b.SiteID == id);
            if (Site != null)
            {
                var sitestatus = ctx.SiteStatus.Where(b => b.SiteID == id).ToList();
                ctx.SiteStatus.RemoveRange(sitestatus);
               // ctx.SaveChanges();
                
                var siteinstrument = ctx.siteinstrument.Where(b => b.SiteID == id).ToList();              
                ctx.siteinstrument.RemoveRange(siteinstrument);

                var SiteTestingdays = ctx.SiteTestingdays.Where(b => b.siteid == id).ToList();
                ctx.SiteTestingdays.RemoveRange(SiteTestingdays);

                var ReferralLinkage = ctx.ReferralLinkage.Where(b => b.referensiteid == id).ToList();
                ctx.ReferralLinkage.RemoveRange(ReferralLinkage);
                ctx.SaveChanges();
                ctx.Site.Remove(Site);
                res = ctx.SaveChanges();
            }
            return res;
        }
        public IEnumerable<SiteInstrumentList> Getdefaultsiteinstrument(int countryid)
        {
            int userid = 0;
            userid = ctx.User.Where(b => b.Role == "admin").Select(g => g.Id).FirstOrDefault();
            var sitelist = ctx.Site.Where(b => b.CountryId == countryid || b.UserId == userid).Select(c => c.SiteID).ToArray();
            var ins1= (from si in ctx.siteinstrument
                      join ins in ctx.Instrument on si.InstrumentID equals ins.InstrumentID
                      join ar in ctx.TestingArea on ins.testingArea.TestingAreaID equals ar.TestingAreaID
                      join st in ctx.Site on si.SiteID equals st.SiteID
                       join rg in ctx.Region on st.regionid equals rg.RegionID
                       where sitelist.Contains(si.SiteID)
                      select new SiteInstrumentList()
                      {

                          ID = si.ID,
                          InstrumentID = si.InstrumentID,
                          InstrumentName = ins.InstrumentName,
                          testingareaId = ins.testingArea.TestingAreaID,
                          testingareaName = ins.testingArea.AreaName,
                          Quantity = si.Quantity,
                          TestRunPercentage = si.TestRunPercentage,
                          SiteID = si.SiteID,
                          Site=st.SiteName,
                          Region=rg.RegionName
                      }).ToList();
            return ins1;
        }
        public Site Getbyid(int id)
        {
      
            var Site = ctx.Site.Where(b => b.SiteID == id).Select(item=> new Site
            {
                SiteID=item.SiteID,
                SiteName=item.SiteName,
                   CategoryID =item.CategoryID,
         CD4TestingDaysPerMonth =item.CD4TestingDaysPerMonth,
         ChemistryTestingDaysPerMonth = item.ChemistryTestingDaysPerMonth,
                HematologyTestingDaysPerMonth = item.HematologyTestingDaysPerMonth,
                ViralLoadTestingDaysPerMonth = item.ViralLoadTestingDaysPerMonth,
                OtherTestingDaysPerMonth = item.OtherTestingDaysPerMonth,
                regionid = item.regionid,
                _siteStatuses = ctx.SiteStatus.Where(b=>b.SiteID==id).ToList(),
                //_siteInstruments = ctx.siteinstrument.Where(b=>b.SiteID==id).ToList(),
                // public SiteCategory siteCategory 
                CD4RefSite = item.CD4RefSite,
                ChemistryRefSite = item.ChemistryRefSite,
                HematologyRefSite = item.HematologyRefSite,
                ViralLoadRefSite = item.ViralLoadRefSite,
                OtherRefSite = item.OtherRefSite,
                WorkingDays = item.WorkingDays,
                SiteLevel = item.SiteLevel,
                Latitude = item.Latitude,
                Longitude = item.Longitude,
                CountryId=item.CountryId
            }
                ).FirstOrDefault();
            
            Site._siteInstruments = (from si in ctx.siteinstrument
                                  join ins in ctx.Instrument on si.InstrumentID equals ins.InstrumentID
                                  join ar in ctx.TestingArea on ins.testingArea.TestingAreaID equals ar.TestingAreaID
                                  where si.SiteID ==id
                                  select new SiteInstrumentList()
                                  {

                                      ID=si.ID,
                                      InstrumentID=si.InstrumentID,
                                      InstrumentName=ins.InstrumentName,
                                      testingareaId=ins.testingArea.TestingAreaID,
                                      testingareaName=ins.testingArea.AreaName,
                                      Quantity=si.Quantity,
                                      TestRunPercentage=si.TestRunPercentage,
                                      SiteID=si.SiteID
                                  }).ToList();

            Site._ReferralLinkage = (from rl in ctx.ReferralLinkage
                                     join st in ctx.Site on rl.siteid equals st.SiteID
                                     join ar in ctx.TestingArea on rl.testingareaid equals ar.TestingAreaID
                                     where rl.referensiteid == id
                                     select new ReferralLinkageList()
                                     {
                                         Id=rl.Id,
                                         referensiteid=rl.referensiteid,
                                         siteid=rl.siteid,
                                         siteName=st.SiteName,
                                         testingareaid=rl.testingareaid,
                                         testingareaName=ar.AreaName,
                                         testingdays=rl.testingdays
                                     }
                                   ).ToList();
            Site._sitetestingdays = (from rl in ctx.SiteTestingdays                                   
                                     join ar in ctx.TestingArea on rl.testingareaid equals ar.TestingAreaID
                                     where rl.siteid == id
                                     select new SiteTestingdaysList()
                                     {
                                         Id = rl.Id,                                        
                                         siteid = rl.siteid,                                        
                                         testingareaid = rl.testingareaid,
                                         testingareaName = ar.AreaName,
                                         testingdays = rl.testingdays
                                     }
                                   ).ToList();
            return Site;
        }
        public IEnumerable<Site> GetSitebyregions(string regionsid)
        {
            string[] arrids;
            int res = 0;
            arrids = regionsid.Trim(',').Split(',');

            var sites = ctx.Site.Where(b => arrids.Contains(Convert.ToString(b.regionid))).ToList();
            return sites;
        }
        public IEnumerable<sitebyregion> GetAll(int id,int userid,string role)


        {
            List<sitebyregion> SR = new List<sitebyregion>();
           
                SR = (from prd in ctx.Site
                            join type in ctx.Region on prd.regionid equals type.RegionID
                            join cat in ctx.SiteCategory on prd.CategoryID equals cat.CategoryID
                          
                            select new sitebyregion
                            {
                                SiteID = prd.SiteID,
                                SiteName = prd.SiteName,
                                RegionName = type.RegionName,
                                CategoryName = cat.CategoryName,
                                WorkingDays = prd.WorkingDays,
                                CountryName="",
                                Countryid=prd.CountryId.Value,
                                UserId=prd.UserId,
                                Isapprove=prd.Isapprove
                                
                            }
                                                     ).OrderByDescending(x=>x.SiteID).ToList();
            if (role != "admin")
            {
                var adminuserid = ctx.User.Where(b => b.Role == "admin").Select(x =>

               x.Id

          ).FirstOrDefault();

                SR = SR
                    .Where(bs => (bs.UserId == userid || bs.UserId == adminuserid) || (bs.Countryid==id && bs.Isapprove ==true))
                    .Select(x=> new sitebyregion
                       {
                           SiteID = x.SiteID,
                           SiteName = x.SiteName,
                           RegionName = x.RegionName,
                           CategoryName = x.CategoryName,
                           WorkingDays = x.WorkingDays,
                           CountryName =ctx.Country.Where(b=>b.Id==x.Countryid).Select(s=>s.Name).FirstOrDefault(),
                           Countryid = x.Countryid,
                           UserId = x.UserId,
                           Isapprove = x.Isapprove

                       }).OrderByDescending(x => x.SiteID)
                    .ToList();
            }
        
             for(int i=0;i< SR.Count;i++)
            {
                var ss = ctx.SiteStatus.Where(x => x.SiteID == SR[i].SiteID).OrderByDescending(Y => Y.Id).FirstOrDefault();
                if (ss !=null)
                {

                    if (ss.ClosedOn != null)
                    {
                        var DATE1 = ss.ClosedOn.Value.Day + "/" + ss.ClosedOn.Value.Month.ToString("mmm") + "/" + ss.ClosedOn.Value.Year;
                        SR[i].GetLastClosedDate = ss.ClosedOn.Value.ToString("dd/MMM/yyyy",CultureInfo.InvariantCulture);
                    }
                    SR[i].GetLastOpenDate = ss.OpenedFrom.Value.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture);
                }
            }
            return SR;
        }

        public int UpdateData(int id, Site b)
        {
            int res = 0;
            var Site = ctx.Site.Find(id);
         
            if (Site != null)
            {
                Site.SiteName = b.SiteName;
                Site.Longitude = b.Longitude;
                Site.Latitude = b.Latitude;
                Site.WorkingDays = b.WorkingDays;
                Site.CountryId = b.CountryId;
                Site.regionid = b.regionid;
                ///Update SiteTesting Days
                foreach (SiteTestingdaysList SL in b._sitetestingdays)
                {
                    var SiteTestingday = ctx.SiteTestingdays.Find(SL.Id);
                    if (SiteTestingday != null)
                    {
                        SiteTestingday.testingareaid = SL.testingareaid;
                        SiteTestingday.testingdays = SL.testingdays;                       
                        ctx.SaveChanges();
                    }
                    else
                    {
                        SiteTestingdays std = new SiteTestingdays();
                        std.siteid = SL.siteid;
                        std.testingareaid = SL.testingareaid;
                        std.testingdays = SL.testingdays;
                        ctx.SiteTestingdays.Add(std);
                        res = ctx.SaveChanges();
                    }
                }
                ///Update Referral Linkage
             
                foreach (ReferralLinkageList RL in b._ReferralLinkage)
                {
                    var ReferralLinkage = ctx.ReferralLinkage.Find(RL.Id);
                    if (ReferralLinkage != null)
                    {
                        ReferralLinkage.testingareaid = RL.testingareaid;
                        ReferralLinkage.testingdays = RL.testingdays;
                        ReferralLinkage.siteid = RL.siteid;
                        ReferralLinkage.testingdays = RL.testingdays;
                        ctx.SaveChanges();
                    }
                    else
                    {
                        ReferralLinkage RLD = new ReferralLinkage();
                        RLD.referensiteid = RL.referensiteid;
                        RLD.siteid = RL.siteid;
                        RLD.testingareaid = RL.testingareaid;
                        RLD.testingdays = RL.testingdays;
                      
                        ctx.ReferralLinkage.Add(RLD);
                        res = ctx.SaveChanges();
                    }
                }
                ///Update Site Status
                foreach (SiteStatus SS in b._siteStatuses)
                {
                    var SiteStatus = ctx.SiteStatus.Find(SS.Id);
                    if (SiteStatus != null)
                    {
                        SiteStatus.OpenedFrom = SS.OpenedFrom;
                        SiteStatus.ClosedOn = SS.ClosedOn;
                        SiteStatus.SiteID = SS.SiteID;                    

                        ctx.SaveChanges();
                    }
                    else
                    {
                      
                        ctx.SiteStatus.Add(SS);
                        res = ctx.SaveChanges();
                    }
                }
                res = ctx.SaveChanges();

                ///Update Site instrument
             
                foreach (SiteInstrumentList SIL in b._siteInstruments)
                {
                    var siteinstrument = ctx.siteinstrument.Find(SIL.ID);
                    if (siteinstrument != null)
                    {
                        siteinstrument.InstrumentID = SIL.InstrumentID;
                        siteinstrument.Quantity = SIL.Quantity;
                        siteinstrument.TestRunPercentage = SIL.TestRunPercentage;

                        ctx.SaveChanges();
                    }
                    else
                    {
                        SiteInstrument SI = new SiteInstrument();
                        SI.SiteID = SIL.SiteID;
                        SI.InstrumentID = SIL.InstrumentID;
                        SI.Quantity = SIL.Quantity;
                        SI.TestRunPercentage = SIL.TestRunPercentage;
                        ctx.siteinstrument.Add(SI);
                        res = ctx.SaveChanges();
                    }
                }
                res = b.SiteID;

            }


            ///Update Site instrument
            
            return res;
        }

        public Site GetSiteByName(string name, int regionid)
        {
            var Site = ctx.Site.FirstOrDefault(b => b.SiteName == name && b.regionid == regionid);
            return Site;
        }
        public IEnumerable<sitebyregion> GetAllSiteByRegionId(int regionid)
        {

           
            var result = (from r in ctx.Site
                          where r.regionid == regionid
                          join ss in ctx.SiteStatus on r.SiteID equals ss.SiteID
                          select new sitebyregion()
                          {
                              SiteID = r.SiteID,
                              SiteName = r.SiteName,
                            //  GetLastClosedDate = ss.ClosedOn.Value.ToShortDateString(),
                            //  GetLastOpenDate = ss.OpenedFrom.Value.ToShortDateString()
                          }).ToList();
            if (result.Count > 0)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    var ss = ctx.SiteStatus.Where(x => x.SiteID == result[i].SiteID).OrderByDescending(Y => Y.Id).FirstOrDefault();
                    if (ss != null)
                    {

                        if (ss.ClosedOn != null)
                        {

                            result[i].GetLastClosedDate = ss.ClosedOn.Value.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture);
                            result[i].CurrentlyOpen = "No";
                        }
                        else
                        {
                            result[i].CurrentlyOpen = "Yes";
                        }
                        result[i].GetLastOpenDate = ss.OpenedFrom.Value.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture);
                    }
                }
            }
           // var Site = ctx.Site.Where(b => b.region.RegionID == regionid).ToList();
            return result;

        }



        public IEnumerable<sitebyregion> GetSitebykeyword(string keyword)
        {
            var sitelist = (from prd in ctx.Site
                               join type in ctx.Region on prd.regionid equals type.RegionID
                               join cat in ctx.SiteCategory on prd.CategoryID equals cat.CategoryID
                               join ss in ctx.SiteStatus on prd.SiteID equals ss.SiteID
                               where prd.SiteName.Contains(keyword)
                               //join pp in ctx.ProductPrice on prd.ProductID equals pp.Product.ProductID                       
                               select new sitebyregion
                               {
                                   SiteID = prd.SiteID,
                                   SiteName = prd.SiteName,
                                   RegionName = type.RegionName,
                                   // ProductType = ctx.ProductType.Where(b => b.TypeID == prd.ProductTypeId).Select(x => x.TypeName).FirstOrDefault(),
                                   CategoryName = cat.CategoryName,
                                  // GetLastClosedDate = ss.ClosedOn.Value.ToShortDateString(),
                                 //  GetLastOpenDate = ss.OpenedFrom.Value.ToShortDateString()
                               }).ToList();
            if (sitelist.Count > 0)
            {

                for (int i = 0; i < sitelist.Count; i++)
                {
                    var ss = ctx.SiteStatus.Where(x => x.SiteID == sitelist[i].SiteID).OrderByDescending(Y => Y.Id).FirstOrDefault();
                    if (ss != null)
                    {

                        if (ss.ClosedOn != null)
                        {
                         
                            sitelist[i].GetLastClosedDate = ss.ClosedOn.Value.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture);
                            sitelist[i].CurrentlyOpen = "No";
                        }
                        else
                        {
                            sitelist[i].CurrentlyOpen = "Yes";
                        }
                        sitelist[i].GetLastOpenDate = ss.OpenedFrom.Value.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture);
                    }
                }

            }
            // var Site = ctx.Site.Where(b => b.region.RegionID == regionid).ToList();
            return sitelist;
        }


        public IEnumerable<sitebyregion> GetSitebykeywordtypes(string keyword, string type)
        {
            var comparestring = type.TrimStart(',');
            var sitelist = (from prd in ctx.Site
                               join reg in ctx.Region on prd.regionid equals reg.RegionID
                               join cat in ctx.SiteCategory on prd.CategoryID equals cat.CategoryID
                               join ss in ctx.SiteStatus on prd.SiteID equals ss.SiteID
                               where (prd.SiteName.Contains(keyword) && (Convert.ToString(reg.RegionID).Contains(comparestring)))
                               //join pp in ctx.ProductPrice on prd.ProductID equals pp.Product.ProductID                       
                               select new sitebyregion
                               {
                                   SiteID = prd.SiteID,
                                   SiteName = prd.SiteName,
                                   RegionName = reg.RegionName,
                                   // ProductType = ctx.ProductType.Where(b => b.TypeID == prd.ProductTypeId).Select(x => x.TypeName).FirstOrDefault(),
                                   CategoryName = cat.CategoryName,
                                   GetLastClosedDate = ss.ClosedOn.Value.ToShortDateString(),
                                   GetLastOpenDate = ss.OpenedFrom.Value.ToShortDateString()
                               }).ToList();
            if (sitelist.Count > 0)
            {
                for (int i = 0; i < sitelist.Count; i++)
                {
                    var ss = ctx.SiteStatus.Where(x => x.SiteID == sitelist[i].SiteID).OrderByDescending(Y => Y.Id).FirstOrDefault();
                    if (ss != null)
                    {

                        if (ss.ClosedOn != null)
                        {
                           
                            sitelist[i].GetLastClosedDate = ss.ClosedOn.Value.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture);
                            sitelist[i].CurrentlyOpen = "No";
                        }
                        else
                        {
                            sitelist[i].CurrentlyOpen = "Yes";
                        }
                        sitelist[i].GetLastOpenDate = ss.OpenedFrom.Value.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture);
                    }
                }
            }
            // var Site = ctx.Site.Where(b => b.region.RegionID == regionid).ToList();
            return sitelist;
        }
        public int Deletesiteinstrument(string ids)
        {
            string[] arrids;
            int res = 0;
            arrids = ids.Trim(',').Split(',');
            //for (int i = 0; i < arrids.Length; i++)
            //{
            //    var productusage = ctx.ProductUsage.FirstOrDefault(b => b.Id == Convert.ToInt32(arrids[i]));
            //    if (productusage != null)
            //    {
            //        ctx.ProductUsage.Remove(productusage);
            //        res = ctx.SaveChanges();
            //    }

            //}
          
            var siteinstrument = ctx.siteinstrument.Where(b => arrids.Contains(Convert.ToString(b.ID))).ToList();

            ctx.siteinstrument.RemoveRange(siteinstrument);
            res=  ctx.SaveChanges();
            return res;

        }
        public int Deletesitetestingdays(string ids)
        {
            string[] arrids;
            int res = 0;
            arrids = ids.Trim(',').Split(',');
            //for (int i = 0; i < arrids.Length; i++)
            //{
            //    var productusage = ctx.ProductUsage.FirstOrDefault(b => b.Id == Convert.ToInt32(arrids[i]));
            //    if (productusage != null)
            //    {
            //        ctx.ProductUsage.Remove(productusage);
            //        res = ctx.SaveChanges();
            //    }

            //}
            var SiteTestingdays = ctx.SiteTestingdays.Where(b => arrids.Contains(Convert.ToString(b.Id))).ToList();
            ctx.SiteTestingdays.RemoveRange(SiteTestingdays);
            res = ctx.SaveChanges();
            return res;

        }
        public int Deletereferrallink(string ids)
        {
            string[] arrids;
            int res = 0;
            arrids = ids.Trim(',').Split(',');
            //for (int i = 0; i < arrids.Length; i++)
            //{
            //    var productusage = ctx.ProductUsage.FirstOrDefault(b => b.Id == Convert.ToInt32(arrids[i]));
            //    if (productusage != null)
            //    {
            //        ctx.ProductUsage.Remove(productusage);
            //        res = ctx.SaveChanges();
            //    }

            //}
            var ReferralLinkage = ctx.ReferralLinkage.Where(b => arrids.Contains(Convert.ToString(b.Id))).ToList();
            ctx.ReferralLinkage.RemoveRange(ReferralLinkage);
            res = ctx.SaveChanges();
            return res;

        }
        //public IEnumerable<Site> GetReferingSiteByPlatform(string platform)

        //{

        //    string forcd4 = System.Text.RegularExpressions.Regex.Replace("Flow Cytometry", @"\s+", "");

        //    if (System.Text.RegularExpressions.Regex.Replace(platform, @"\s+", "").ToLower() == forcd4.ToLower())
        //        platform = "CD4";
        //    var eParam = Expression.Parameter(typeof(Site), "e");

        //    var comparison = Expression.Lambda(
        //        Expression.NotEqual(
        //            Expression.Property(eParam, platform + "RefSite"),
        //            Expression.Constant(0)),
        //        eParam);



        //    var Site = ctx.Site.Where(comparison).ToList();
        //    return Site;
        //}
        //public Site  GetSiteByName(string sname)
        //{
        //    var Site = ctx.Site.FirstOrDefault(b => b.SiteName == sname );
        //    return Site;
        //}
    }
}
