using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForLabApi.DataInterface;
using ForLabApi.Models;
namespace ForLabApi.Repositories
{
    public class TestAccessRepositories : ITest<TestList_area,Gettotalcount, Test, TestingArea, testList, ProductUsagelist, ConsumableUsagelist, Masterconsumablelist, ProductUsageDetail, ConsumableUsageDetail, forecasttest, ForecastTest>
    {
        ForLabContext ctx;
        public TestAccessRepositories(ForLabContext c)
        {
            ctx = c;
            //return ctx;
        }
        public List<TestList_area> Getallarea(int userid, string Role)
        {
            List<TestList_area> LT = new List<TestList_area>();



            var TestingArea = ctx.TestingArea.OrderByDescending(x => x.TestingAreaID).ToList();

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
            for (int i = 0; i < TestingArea.Count; i++)
            {
                LT.Add(new TestList_area
                {
                    Name = TestingArea[i].AreaName,
                    Id = TestingArea[i].TestingAreaID,
                    Type = "A"

                });
                var test = ctx.Test.Where(b => b.TestingAreaID == TestingArea[i].TestingAreaID).ToList();
                for (int j = 0; j < test.Count; j++)
                {
                    LT.Add(new TestList_area
                    {
                        Name = test[j].TestName,
                        Id = test[j].TestID,
                        Type = "T"

                    });

                }
            }
            return LT;

        }
        public void saveforecasttest(int id,IEnumerable<ForecastTest> b)
        {
            //  int i = 0;
            try
            {
                if (b.Count() > 0)
                {
                    var islock = ctx.ForecastInfo.Find(b.FirstOrDefault().forecastID);
                    if (islock.Forecastlock == false)
                    {

                        var forecasttests = ctx.ForecastTest.Where(c => c.forecastID == b.FirstOrDefault().forecastID).ToList();
                        for (int i = 0; i < forecasttests.Count; i++)
                        {
                            ctx.ForecastTest.Remove(forecasttests[i]);
                            ctx.SaveChanges();
                        }
                        ctx.ForecastTest.AddRange(b);
                        ctx.SaveChanges();
                        //}
                        //ctx.ForecastTest.AddRange(b);
                        //ctx.SaveChanges();

                        var forecastproductusage = ctx.ForecastProductUsage.Where(c => c.Forecastid == b.FirstOrDefault().forecastID).ToList();
                        //for (int i = 0; i < forecastproductusage.Count; i++)
                        //{
                        //    ctx.ForecastProductUsage.Remove(forecastproductusage[i]);
                        //    ctx.SaveChanges();
                        //}


                      
                        var forecastconsumble = ctx.ForecastConsumableUsage.Where(c => c.Forecastid == b.FirstOrDefault().forecastID).ToList();
                        //for (int i = 0; i < forecastconsumble.Count; i++)
                        //{
                        //    ctx.ForecastConsumableUsage.Remove(forecastconsumble[i]);
                        //    ctx.SaveChanges();
                        //}
                        List<ForecastProductUsage> FP = new List<ForecastProductUsage>();
                        List<ForecastConsumableUsage> FC = new List<ForecastConsumableUsage>();
                        foreach (ForecastTest item in b)
                        {
                            var productuages = ctx.ProductUsage.Where(c => c.TestId == item.TestID).ToList();
                            FP = new List<ForecastProductUsage>();
                            FC = new List<ForecastConsumableUsage>();
                            for (int i = 0; i < productuages.Count; i++)
                            {

                                FP.Add(new ForecastProductUsage
                                {
                                    Forecastid = item.forecastID,
                                    TestId = item.TestID,
                                    UserId = item.UserId,
                                    InstrumentId = productuages[i].InstrumentId,
                                    ProductId = productuages[i].ProductId,
                                    IsForControl = productuages[i].IsForControl,
                                    ProductUsedIn = productuages[i].ProductUsedIn,
                                    Rate = productuages[i].Rate,

                                });
                            }

                            //var existforecastusage = ctx.ForecastProductUsage.Where(c => c.Forecastid == item.forecastID && c.TestId == item.TestID).ToList();
                            //if (existforecastusage.Count == 0)
                            //{


                            // var productusage = FUM.ForecastProductUsage;
                            ctx.ForecastProductUsage.AddRange(FP);
                            ctx.SaveChanges();
                            // }

                            var ConsumableUsage = ctx.ConsumableUsage.Join(ctx.MasterConsumable, c => c.ConsumableId, e => e.MasterCID, (c, e) => new { c, e })
                                .Where(x => x.e.TestId == item.TestID).ToList();

                            for (int i = 0; i < ConsumableUsage.Count; i++)
                            {
                                FC.Add(new ForecastConsumableUsage
                                {
                                    Forecastid = item.forecastID,
                                    TestId = item.TestID,
                                    UserId = item.UserId,
                                    InstrumentId = ConsumableUsage[i].c.InstrumentId,
                                    ProductId = ConsumableUsage[i].c.ProductId,
                                    UsageRate = ConsumableUsage[i].c.UsageRate,
                                    NoOfTest = ConsumableUsage[i].c.NoOfTest,
                                    PerInstrument = ConsumableUsage[i].c.PerInstrument,
                                    Period = ConsumableUsage[i].c.Period,
                                    PerPeriod = ConsumableUsage[i].c.PerPeriod,
                                    PerTest = ConsumableUsage[i].c.PerTest,

                                });
                            }
                            //var existforecastconsumptionusage = ctx.ForecastConsumableUsage.Where(c => c.Forecastid == item.forecastID && c.TestId == item.TestID).ToList();
                            //if (existforecastconsumptionusage.Count == 0)
                            //{


                            // var productusage = FUM.ForecastProductUsage;
                            ctx.ForecastConsumableUsage.AddRange(FC);
                            ctx.SaveChanges();
                            //  }

                        }

                    }
                }
                else
                {
                    var islock = ctx.ForecastInfo.Find(id);
                    if (islock.Forecastlock == false)
                    {
                        var forecasttests = ctx.ForecastTest.Where(c => c.forecastID == id).ToList();
                        for (int i = 0; i < forecasttests.Count; i++)
                        {
                            ctx.ForecastTest.Remove(forecasttests[i]);
                            ctx.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

              //  throw;
            }
         
         //   return i;

        }
        public IEnumerable<TestingArea> GetAllbyadmin()
        {
            var adminuserid = ctx.User.Where(b => b.Role == "admin").FirstOrDefault();
            var TestingArea = ctx.TestingArea.Where(b => b.UserId == adminuserid.Id).ToList();
            return TestingArea;


        }
        public List<forecasttest> getAlltestbytestingarea(int Forecstid,int userid,string Role)
        {

            List<forecasttest> FT = new List<forecasttest>();


            //var Roles = ctx.User.Where(b => b.Id == userid).Select(x => x.Role).FirstOrDefault();
            //var TestingArea = ctx.TestingArea.OrderBy(b => b.AreaName).ToList();

            //if (userid == 0)
            //{
            //    TestingArea = TestingArea.Join(ctx.User, b => b.UserId, c => c.Id, (b, c) => new { b, c }).Where(x => x.c.Role == "admin").Select(x => new TestingArea
            //    {
            //        AreaName = x.b.AreaName,
                 
            //        TestingAreaID = x.b.TestingAreaID,
                 
            //    }).ToList();
            //}
            //else
            //{
            //    if (Roles == "admin")
            //    {

            //    }
            //    else
            //    {
            //        TestingArea = TestingArea.Where(b => b.UserId == userid).ToList();
            //    }
            //}




            var TestingArea = ctx.TestingArea.OrderByDescending(x => x.TestingAreaID).ToList();

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


            var tests = ctx.Test.Select(x => new Testlist1
            {
               TestID= x.TestID,
               TestName= x.TestName,
               TestingAreaID= x.TestingAreaID,
                type = "N"
            }).ToList();




            var forecasttest = ctx.ForecastTest.Where(b => b.forecastID == Forecstid).ToList();
            for (int i = 0; i < forecasttest.Count; i++)
            {
                for (int j = 0; j < tests.Count; j++)
                {
                    if(forecasttest[i].TestID==tests[j].TestID)
                    {
                        tests[j].type = "A";
                        break;
                    }
                }
            }


            for (int i = 0; i < TestingArea.Count; i++)
            {
                FT.Add(new forecasttest
                {
                    testareaid = TestingArea[i].TestingAreaID,
                    testareaname = TestingArea[i].AreaName,
                    tests = tests.Where(x => x.TestingAreaID == TestingArea[i].TestingAreaID).ToArray()

                });
            }
            return FT;
        }
        public int SaveData(Test b)
        {
            var Test = ctx.Test.FirstOrDefault(c => c.TestName == b.TestName && c.UserId==b.UserId);
            if (Test != null)
            {
                return 0;
            }

            ctx.Test.Add(b);
          ctx.SaveChanges();
            int res = b.TestID;
            return res;
        }
        public int saveconsumabledata(Masterconsumablelist b)
        {
            MasterConsumable mc = new MasterConsumable();


            mc.MasterCID = b.MasterCID;
            mc.TestId = b.TestId;
            mc.TestingAreaId = b.TestingAreaId;
            mc.UserId = b.UserId;
            ctx.MasterConsumable.Add(mc);
            ctx.SaveChanges();
            int res1= mc.MasterCID;
            int res = 0;
            
            foreach(ConsumableUsage c in b._consumablesUsages)
            {
                c.ConsumableId = res1;
                c.UserId = b.UserId;
                ctx.ConsumableUsage.Add(c);
                res = ctx.SaveChanges();

            }
            return res;

        }
        public int Deleteproductusage(string ids)
        {
            string []arrids;
            int res=0;
            arrids = ids.Trim(',').Split(',');
            for(int i=0;i<arrids.Length;i++)
            {
                var productusage = ctx.ProductUsage.FirstOrDefault(b => b.Id == Convert.ToInt32(arrids[i]));
                if (productusage != null)
                {
                    ctx.ProductUsage.Remove(productusage);
                    res = ctx.SaveChanges();
                }
              
            }
            return res;

        }
        public int Deletconsumableusage(string ids)
        {
            string[] arrids;
            int res = 0;
            arrids = ids.Trim(',').Split(',');
            for (int i = 0; i < arrids.Length; i++)
            {
                var consumableusage = ctx.ConsumableUsage.FirstOrDefault(b => b.Id == Convert.ToInt32(arrids[i]));
                if (consumableusage != null)
                {
                    ctx.ConsumableUsage.Remove(consumableusage);
                    res = ctx.SaveChanges();
                }

            }
            return res;
        }
        public int DeleteData(int id)
        {
            int res = 0;
            var Test = ctx.Test.FirstOrDefault(b => b.TestID == id);
            if (Test != null)
            {
                try
                {
                    var forecastcategorytest = ctx.ForecastCategoryTest.Where(b => b.TestID == id).ToList();
                    var forecastsitetest = ctx.ForecastSiteTest.Where(b => b.TestID == id).ToList();
                    var testingprotocol = ctx.TestingProtocol.Where(b => b.TestID == id).ToList();
                    if (forecastcategorytest.Count !=0 || forecastsitetest.Count != 0 || testingprotocol.Count != 0)
                    {
                        return res;
                    }
                    var productusage = ctx.ProductUsage.Where(b => b.TestId == id).ToList();
                    ctx.ProductUsage.RemoveRange(productusage);
                    ctx.SaveChanges();
                    var masterconsum = ctx.MasterConsumable.FirstOrDefault(b => b.TestId == id);
                    var consumable = ctx.ConsumableUsage.Where(b => b.ConsumableId == masterconsum.MasterCID).ToList();
                    ctx.MasterConsumable.Remove(masterconsum);
                    ctx.ConsumableUsage.RemoveRange(consumable);
                    ctx.SaveChanges();

                    ctx.Test.Remove(Test);
                    ctx.SaveChanges();
                 
                    res = id;
                }
                catch(Exception ex)
                {

                }
             
            }
            return res;
        }

        public testList Getbyid(int id)
        {
            var productusage1 = GetProductUsagelist(id);
            var controlusage2 = GetControltUsagelist(id);
            var consumablepertest1 = GetConsumableUsagelist(id, "PerTest");
            var consumableperperiod1 = GetConsumableUsagelist(id, "PerPeriod");
            var consumableperins1 = GetConsumableUsagelist(id, "PerIns");
            var Test = (from t in ctx.Test
                        where t.TestID == id
                        join ta in ctx.TestingArea on t.TestingAreaID equals ta.TestingAreaID
                        select new testList
                        {
                             TestID = t.TestID,
                             TestName = t.TestName,
                             testingArea = ta.AreaName,
                             testingAreaID=ta.TestingAreaID,
                             Productusage= productusage1,
                             controlusage= controlusage2,
                             consumablepertest= consumablepertest1,
                             consumableperperiod = consumableperperiod1,
                             consumableperins = consumableperins1
                        }

                        ).Single();


              
            return Test;
        }
        public List<testList> Getdefaulttest(string areaids)
        {
            string[] arrids;
            int res = 0;
            arrids = areaids.Trim(',').Split(',');
            List<testList> testlists = new List<testList>();

            testlists = (from t in ctx.Test.Where(b => arrids.Contains(Convert.ToString(b.TestingAreaID)))
                         join ta in ctx.TestingArea on t.TestingAreaID equals ta.TestingAreaID

                         select new testList
                         {
                             TestID = t.TestID,
                             TestName = t.TestName,
                             testingArea = ta.AreaName,
                             UserId = t.UserId
                         }

                        ).OrderByDescending(x => x.TestID).ToList();

            return testlists;
        }
        public Gettotalcount GettotalcountNo(int userid, string role)
        {
            Gettotalcount GT = new Gettotalcount();
            if (role == "admin")
            {

                GT.Totalins = ctx.Instrument.Count();
                GT.Totalproduct = ctx.MasterProduct.Count();
                GT.Totalproducttype = ctx.ProductType.Count();
                GT.Totalregion = ctx.Region.Count();
                GT.Totalsite = ctx.Site.Count();
                GT.Totalsitecategory = ctx.SiteCategory.Count();
                GT.Totaltest = ctx.Test.Count();
                GT.Totaltestarea = ctx.TestingArea.Count();
            }
            else
            {
                var adminuserid = ctx.User.Where(b => b.Role == "admin").Select(x =>

                   x.Id

              ).FirstOrDefault();
                GT.Totalins = ctx.Instrument.Where(b=>b.UserId==userid || b.UserId== adminuserid ||b.Isapprove==true).Count();
                GT.Totalproduct = ctx.MasterProduct.Where(b => b.UserId == userid || b.UserId == adminuserid || b.Isapprove == true).Count();
                GT.Totalproducttype = ctx.ProductType.Where(b => b.UserId == userid || b.UserId == adminuserid || b.Isapprove == true).Count();
                GT.Totalregion = ctx.Region.Where(b => b.UserId == userid || b.UserId == adminuserid || b.Isapprove == true).Count();
                GT.Totalsite = ctx.Site.Where(b => b.UserId == userid || b.UserId == adminuserid || b.Isapprove == true).Count();
                GT.Totalsitecategory = ctx.SiteCategory.Where(b => b.UserId == userid || b.UserId == adminuserid || b.Isapprove == true).Count();
                GT.Totaltest = ctx.Test.Where(b => b.UserId == userid || b.UserId == adminuserid || b.Isapprove == true).Count();
                GT.Totaltestarea = ctx.TestingArea.Where(b => b.UserId == userid || b.UserId == adminuserid || b.Isapprove == true).Count();

            }
            return GT;
        }
        public IEnumerable<testList> GetAll(int userid, string role)
        {
          
            List<testList> testlists = new List<testList>();

          
            if (role=="admin")
            {
                testlists = (from t in ctx.Test
                             join ta in ctx.TestingArea on t.TestingAreaID equals ta.TestingAreaID
                             into leftJ
                             from lj in leftJ.DefaultIfEmpty()
                             select new testList
                             {
                                 TestID = t.TestID,
                                 TestName = t.TestName,
                                 testingArea = lj.AreaName,
                                 UserId = t.UserId,
                                // username=ctx.User.Find(t.UserId).FirstName +" "+ctx.User.Find(t.UserId).LastName
                             }

                       ).OrderByDescending(x => x.TestID).ToList();
            }
            else
            {
                var adminuserid = ctx.User.Where(b => b.Role == "admin").Select(x => 

                    x.Id

               ).FirstOrDefault();

                testlists = (from t in ctx.Test
                             join ta in ctx.TestingArea on t.TestingAreaID equals ta.TestingAreaID
                             into leftJ
                             from lj in leftJ.DefaultIfEmpty()
                             where t.UserId==userid || t.UserId==adminuserid || t.Isapprove==true
                             select new testList
                             {
                                 TestID = t.TestID,
                                 TestName = t.TestName,
                                 testingArea = lj.AreaName,
                                 UserId = t.UserId,
                                // username = ctx.User.Find(t.UserId).FirstName + " " + ctx.User.Find(t.UserId).LastName
                             }

                     ).OrderByDescending(x => x.TestID).ToList();
             
            }


          //  ctx.ConsumableUsage.Find(24);
            return testlists;
        }

        public int UpdateData(int id, Test b)
        {
            int res = 0;
            //var Test = ctx.Test.Find(id);
            //if (Test != null)
            //{
                var Test = ctx.Test.Where(c => c.TestName == b.TestName && c.UserId==b.UserId).ToList();
                if (Test != null)
                {
                if (Test.Count == 1 && b.TestID.Equals(Test[0].TestID))
                {
                    Test[0].TestName = b.TestName;

                    foreach (ProductUsage P in b.ProductUsageArray)
                    {
                        var productusage = ctx.ProductUsage.Find(P.Id);
                        if (productusage != null)
                        {
                            productusage.InstrumentId = P.InstrumentId;
                            productusage.ProductId = P.ProductId;
                            productusage.Rate = P.Rate;
                            productusage.IsForControl = P.IsForControl;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            P.TestId = b.TestID;
                            P.UserId = b.UserId;
                            ctx.ProductUsage.Add(P);
                            res = ctx.SaveChanges();
                        }
                    }
                    ctx.SaveChanges();
                    res = b.TestID;
                }
              //  res = 
            }
            return res;
        }
        public int UpdatConsumableeData(int id, Masterconsumablelist b)
        {
          
            int res = 0;

            var MasterConsumable = ctx.MasterConsumable.Find(id);
            if (MasterConsumable != null)
            {

                foreach (ConsumableUsage P in b._consumablesUsages)
                {

                    var ConsumableUsage = ctx.ConsumableUsage.Find(P.Id);
                    if (ConsumableUsage != null)
                    {
                        if (P.InstrumentId > 0)
                        {
                            ConsumableUsage.InstrumentId = P.InstrumentId;
                        }
                        ConsumableUsage.ProductId = P.ProductId;
                        ConsumableUsage.UsageRate = P.UsageRate;
                        ConsumableUsage.ConsumableId = P.ConsumableId;
                        ConsumableUsage.NoOfTest = P.NoOfTest;
                        ConsumableUsage.PerInstrument = P.PerInstrument;
                        ConsumableUsage.Period = P.Period;
                        ConsumableUsage.PerPeriod = P.PerPeriod;
                        ConsumableUsage.PerTest = P.PerTest;

                        ctx.SaveChanges();
                    }
                    else
                    {
                        P.ConsumableId = b.MasterCID;
                        ctx.ConsumableUsage.Add(P);
                        res = ctx.SaveChanges();
                    }
                }
                res = ctx.SaveChanges();
            }
            else
            {
                MasterConsumable m = new MasterConsumable();
                m.TestId = b.TestId;
                m.TestingAreaId = b.TestingAreaId;
                ctx.MasterConsumable.Add(m);
                ctx.SaveChanges();
                res = m.MasterCID;
                foreach (ConsumableUsage P in b._consumablesUsages)
                {
                    P.ConsumableId = m.MasterCID;
                    ctx.ConsumableUsage.Add(P);
                    res = ctx.SaveChanges();
                }
            }
            return res;
        }
        private int updateconsumable(IList<ConsumableUsage> CU)
        {
            int res = 0;
           // var ConsumableUsage = ctx.ConsumableUsage.Find(CU.Id);
          foreach (ConsumableUsage P in CU)
            {
                //ctx.ConsumableUsage.Find(25);
                ctx.ConsumableUsage.Where(tr => tr.Id == P.Id);
                var ConsumableUsage = ctx.ConsumableUsage.Where(tr => tr.Id == P.Id).FirstOrDefault();
                //var ConsumableUsage = ctx.ConsumableUsage.Find(P.Id);
                if (ConsumableUsage != null)
                {
                    ConsumableUsage.InstrumentId = P.InstrumentId;
                    ConsumableUsage.ProductId = P.ProductId;
                    ConsumableUsage.UsageRate = P.UsageRate;
                    ConsumableUsage.ConsumableId = P.ConsumableId;
                    ConsumableUsage.NoOfTest = P.NoOfTest;
                    ConsumableUsage.PerInstrument = P.PerInstrument;
                    ConsumableUsage.Period = P.Period;
                    ConsumableUsage.PerPeriod = P.PerPeriod;
                    ConsumableUsage.PerTest = P.PerTest;

                    ctx.SaveChanges();
                }
                else
                {
                  //  P.ConsumableId = CU.;
                    ctx.ConsumableUsage.Add(P);
                    res = ctx.SaveChanges();
                }
            }

            return res;
        }


        public IList<Test> GetAllTestsByforecastid(int id)
        {
            var test = ctx.ForecastTest.Join(ctx.Test,b=>b.TestID,c=>c.TestID, (b, c) => new { b, c }).Where(s => s.b.forecastID == id).Select(s=>new Test {
                TestID=s.b.TestID,
                TestName=s.c.TestName
            }).ToList();
            return test;
        }
        public IList<Test> GetAllTestsByAreaId(int areaid,int userid)
        {
            var Test = ctx.Test.Where(b => b.TestingAreaID == areaid && b.UserId==userid).ToList();
            return Test;
        }
        public Test GetTestByName(string name)
        {
            var Test = ctx.Test.FirstOrDefault(b => b.TestName == name);
            return Test;
        }
        public Test GetTestByNameAndTestArea(string name, int areaid)
        {
            var Test = ctx.Test.FirstOrDefault(b => b.TestName == name && b.TestingAreaID == areaid);
            return Test;
        }
  

        public IEnumerable<ConsumableUsagelist> Getdefaulttestconsumble(string areaids)
        {
            int userid;
            int[] testids;
            userid = ctx.User.Where(b => b.Role == "admin").Select(g => g.Id).FirstOrDefault();
            string[] arrids;
            int res = 0;
            arrids = areaids.Trim(',').Split(',');
            if (arrids.Length > 0)
            {
                testids = ctx.Test.Where(b => arrids.Contains(Convert.ToString(b.TestingAreaID))).Select(c => c.TestID).ToArray();
            }
            else
            {
                testids = ctx.Test.Where(b => b.UserId == userid).Select(b => b.TestID).ToArray();
            }
           var consumablelist = (from CU in ctx.ConsumableUsage
                                               join MC in ctx.MasterConsumable on CU.ConsumableId equals MC.MasterCID
                                               join Ins in ctx.Instrument on CU.InstrumentId equals Ins.InstrumentID
                                               join MP in ctx.MasterProduct on CU.ProductId equals MP.ProductID
                                 join ts in ctx.Test on MC.TestId equals ts.TestID
                                 where testids.Contains(MC.TestId)
                                 select new ConsumableUsagelist()
                                 {
                                    Id = CU.Id,
                                    ProductId = MP.ProductID,
                                    ConsumableId = CU.ConsumableId,
                                    ProductName = MP.ProductName,
                                    InstrumentId = Ins.InstrumentID,
                                    InstrumentName = Ins.InstrumentName,
                                    PerInstrument = CU.PerInstrument,
                                    UsageRate = CU.UsageRate,
                                    PerPeriod = CU.PerPeriod,
                                    PerTest = CU.PerTest,
                                    Period = CU.Period,
                                    NoOfTest = CU.NoOfTest,
                                    testId = MC.TestId,
                                     test=ts.TestName
                                 }


                    ).ToList();



        
            return consumablelist;
        }
        public IEnumerable<ProductUsagelist> Getdefaulttestproduct(string areaids)
        {
            int userid;
            int[] testids;
            userid = ctx.User.Where(b => b.Role == "admin").Select(g => g.Id).FirstOrDefault();
            string[] arrids;
            int res = 0;
            arrids = areaids.Trim(',').Split(',');
            if (arrids.Length > 0)
            {
                testids = ctx.Test.Where(b => arrids.Contains(Convert.ToString(b.TestingAreaID))).Select(c => c.TestID).ToArray();
            }
            else
            {
                testids = ctx.Test.Where(b => b.UserId == userid).Select(b => b.TestID).ToArray();
            }
            var result = (from PU in ctx.ProductUsage
                          where testids.Contains(PU.TestId) && PU.IsForControl == false
                          join Ins in ctx.Instrument on PU.InstrumentId equals Ins.InstrumentID
                          join MP in ctx.MasterProduct on PU.ProductId equals MP.ProductID
                          join ts in ctx.Test on PU.TestId equals ts.TestID
                          select new ProductUsagelist
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
                              test=ts.TestName
                          }

                        ).ToList();
            return result;
        }
        public IEnumerable<ProductUsageDetail> GetProductUsagelist(int testid)
        {
            var result = (from PU in ctx.ProductUsage
                          where PU.TestId == testid && PU.IsForControl==false
                          join Ins in ctx.Instrument on PU.InstrumentId equals Ins.InstrumentID
                          join MP in ctx.MasterProduct on PU.ProductId equals MP.ProductID
                          select new ProductUsagelist
                          {
                              Id=PU.Id,
                              ProductId=MP.ProductID,
                              ProductName=MP.ProductName,
                              InstrumentId=Ins.InstrumentID,
                              InstrumentName=Ins.InstrumentName,
                              Rate=PU.Rate,
                              ProductUsedIn=PU.ProductUsedIn,
                              IsForControl=PU.IsForControl,
                              TestId=PU.TestId
                          }

                         ).ToList();

            var ins= result.GroupBy(x => x.InstrumentId).Select(lg =>
                                 new {

                                     name= lg.Max(w => w.InstrumentName)
                                 
                                 }).ToList();

           List<ProductUsageDetail> productUsagelist= new List<ProductUsageDetail>();
          foreach(var i in ins)
            {
                productUsagelist.Add(new ProductUsageDetail{

                   name=  i.name,
                    value = result.Where(b => b.InstrumentName == i.name).ToList()
                }

                    );
             
            }
            return productUsagelist;
        }

        public IEnumerable<ProductUsageDetail> GetControltUsagelist(int testid)
        {
            var result = (from PU in ctx.ProductUsage
                          where PU.TestId == testid && PU.IsForControl == true
                          join Ins in ctx.Instrument on PU.InstrumentId equals Ins.InstrumentID
                          join MP in ctx.MasterProduct on PU.ProductId equals MP.ProductID
                          select new ProductUsagelist
                          {
                              Id = PU.Id,
                              ProductId = MP.ProductID,
                              ProductName = MP.ProductName,
                              InstrumentId = Ins.InstrumentID,
                              InstrumentName = Ins.InstrumentName,
                              Rate = PU.Rate,
                              ProductUsedIn = PU.ProductUsedIn,
                              IsForControl = PU.IsForControl,
                              TestId = PU.TestId
                          }

                         ).ToList();

            var ins = result.GroupBy(x => x.InstrumentId).Select(lg =>
                                 new {

                                     name = lg.Max(w => w.InstrumentName)

                                 }).ToList();

            List<ProductUsageDetail> productUsagelist = new List<ProductUsageDetail>();
            foreach (var i in ins)
            {
                productUsagelist.Add(new ProductUsageDetail
                {

                    name = i.name,
                    value = result.Where(b => b.InstrumentName == i.name).ToList()
                }

                    );

            }
            return productUsagelist;
        }


        public IEnumerable<ConsumableUsageDetail> GetConsumableUsagelist(int testid,string type)
        {
            List<ConsumableUsageDetail> CUL = new List<ConsumableUsageDetail>();
            var list = "";
            if (type == "PerTest")
            {
                var consumablelist = (from CU in ctx.ConsumableUsage
                                      join MC in ctx.MasterConsumable on CU.ConsumableId equals MC.MasterCID
                                      join MP in ctx.MasterProduct on CU.ProductId equals MP.ProductID
                                      join PT in ctx.ProductType on MP.ProductTypeId equals PT.TypeID
                                      where MC.TestId == testid && CU.PerTest == true
                                      select new ConsumableUsagelist()
                                      {
                                          Id = CU.Id,
                                          ConsumableId = CU.ConsumableId,
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
                                          testId = MC.TestId,
                                      

                                      }


                          ).ToList();
                var ins = consumablelist.GroupBy(x => x.ProductTypeId).Select(lg =>
                            new
                            {

                                name = lg.Max(w => w.ProductTypeName)

                            }).ToList();


                foreach (var i in ins)
                {
                    CUL.Add(new ConsumableUsageDetail
                    {

                        name = i.name,
                        value = consumablelist.Where(b => b.ProductTypeName == i.name).ToList()
                    }

                        );

                }



            }
            else if (type == "PerPeriod")
            {
                var consumablelist = (from CU in ctx.ConsumableUsage
                                      join MC in ctx.MasterConsumable on CU.ConsumableId equals MC.MasterCID
                                      join MP in ctx.MasterProduct on CU.ProductId equals MP.ProductID
                                      join PT in ctx.ProductType on MP.ProductTypeId equals PT.TypeID
                                      where MC.TestId == testid && CU.PerPeriod == true
                                      select new ConsumableUsagelist()
                                      {
                                          Id = CU.Id,
                                          ProductId = MP.ProductID,
                                          ConsumableId = CU.ConsumableId,
                                          ProductName = MP.ProductName,
                                          ProductTypeId = PT.TypeID,
                                          ProductTypeName = PT.TypeName,
                                          PerInstrument = CU.PerInstrument,
                                          UsageRate = CU.UsageRate,
                                          PerPeriod = CU.PerPeriod,
                                          PerTest = CU.PerTest,
                                          Period = CU.Period,
                                          NoOfTest = CU.NoOfTest,
                                          testId = MC.TestId
                                      }


                          ).ToList();
                var ins = consumablelist.GroupBy(x => x.ProductTypeId).Select(lg =>
                          new
                          {

                              name = lg.Max(w => w.ProductTypeName)

                          }).ToList();

                foreach (var i in ins)
                {
                    CUL.Add(new ConsumableUsageDetail
                    {

                        name = i.name,
                        value = consumablelist.Where(b => b.ProductTypeName == i.name).ToList()
                    }

                        );

                }
            }
            else
            {
                var consumablelist = (from CU in ctx.ConsumableUsage
                                      join MC in ctx.MasterConsumable on CU.ConsumableId equals MC.MasterCID
                                      join Ins in ctx.Instrument on CU.InstrumentId equals Ins.InstrumentID
                                      join MP in ctx.MasterProduct on CU.ProductId equals MP.ProductID
                                      where MC.TestId == testid
                                      select new ConsumableUsagelist()
                                      {
                                          Id = CU.Id,
                                          ProductId = MP.ProductID,
                                          ConsumableId = CU.ConsumableId,
                                          ProductName = MP.ProductName,
                                          InstrumentId = Ins.InstrumentID,
                                          InstrumentName = Ins.InstrumentName,
                                          PerInstrument = CU.PerInstrument,
                                          UsageRate = CU.UsageRate,
                                          PerPeriod = CU.PerPeriod,
                                          PerTest = CU.PerTest,
                                          Period = CU.Period,
                                          NoOfTest = CU.NoOfTest,
                                          testId = MC.TestId
                                      }


                     ).ToList();
                var ins = consumablelist.GroupBy(x => x.InstrumentId).Select(lg =>
                           new
                           {

                               name = lg.Max(w => w.InstrumentName)

                           }).ToList();

                foreach (var i in ins)
                {
                    CUL.Add(new ConsumableUsageDetail
                    {

                        name = i.name,
                        value = consumablelist.Where(b => b.InstrumentName == i.name).ToList()
                    }

                        );

                }

            }



            return CUL;

        }
    }
}
