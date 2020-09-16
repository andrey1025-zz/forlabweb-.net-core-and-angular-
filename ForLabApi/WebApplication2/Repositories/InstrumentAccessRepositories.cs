using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForLabApi.DataInterface;
using ForLabApi.Models;
namespace ForLabApi.Repositories
{
    public class InstrumentAccessRepositories : IInstrument<Instrument, InstrumentList, getinstrument, ForecastIns, ForecastInsmodel, forecastinslist>
    {

        ForLabContext ctx;
        public InstrumentAccessRepositories(ForLabContext c)
        {
            ctx = c;
            //return ctx;
        }



        public List<forecastinslist> getAllforecastinstrumentlist(int forecastid)
        {
            List<forecastinslist> FI = new List<forecastinslist>();
            var ForecastIns = ctx.ForecastIns.Join(ctx.Instrument, b => b.InsID, c => c.InstrumentID, (b, c) => new { b, c })
                .Join(ctx.TestingArea, e => e.c.testingArea.TestingAreaID, f => f.TestingAreaID, (e, f) => new { e, f })
                .Where(x => x.e.b.forecastID == forecastid).Select(s => new forecastinslist
                {
                    forecastID = s.e.b.forecastID,
                    InsID = s.e.b.InsID,
                    Quantity = s.e.b.Quantity,
                    TestRunPercentage = s.e.b.TestRunPercentage,
                    AreaName = s.f.AreaName,
                    InstrumentName = s.e.c.InstrumentName,
                    TestingAreaID = s.f.TestingAreaID

                }).ToList();

            for (int i = 0; i < ForecastIns.Count(); i++)
            {
                if (ForecastIns[i].TestRunPercentage == 0)
                {
                    if (ForecastIns.Where(b => b.TestingAreaID == ForecastIns[i].TestingAreaID).Count() == 1)
                    {
                        ForecastIns[i].TestRunPercentage = 100;
                    }
                }
            }



            return ForecastIns;

        }
        public void saveforecastIns(int id,IEnumerable<ForecastIns> b)
        {
            //List<ForecastIns> FI = new List<ForecastIns>();
            //foreach (var item in b)
            //{
            //    var isexist = ctx.ForecastIns.Where(c => c.forecastID == item.forecastID && c.InsID == item.InsID).FirstOrDefault();
            //    if (isexist == null)
            //    {
            //        FI.Add(new ForecastIns
            //        {
            //            forecastID = item.forecastID,
            //            ID = item.ID,
            //            InsID = item.InsID,
            //            Quantity = item.Quantity,
            //            UserId = item.UserId,dee
            //            TestRunPercentage = item.TestRunPercentage
            //        });

            //    }
            //}
            //if (FI.Count>0)
            //  {
            if (b.Count() > 0)
            {
                var islock = ctx.ForecastInfo.Find(b.FirstOrDefault().forecastID);
                if (islock.Forecastlock == false)
                {
                    var forecasttests = ctx.ForecastIns.Where(c => c.forecastID == b.FirstOrDefault().forecastID).ToList();

                    for (int i = 0; i < forecasttests.Count; i++)
                    {
                        ctx.ForecastIns.Remove(forecasttests[i]);
                        ctx.SaveChanges();
                    }


                    ctx.ForecastIns.AddRange(b);
                    ctx.SaveChanges();
                }
            }
            else
            {
                var islock = ctx.ForecastInfo.Find(id);
                if (islock.Forecastlock == false)
                {
                    var forecasttests = ctx.ForecastIns.Where(c => c.forecastID == id).ToList();

                    for (int i = 0; i < forecasttests.Count; i++)
                    {
                        ctx.ForecastIns.Remove(forecasttests[i]);
                        ctx.SaveChanges();
                    }
                }
            }


        }
        public void Updateforecastinstrument(ForecastInsmodel b)
        {
            var inslist = b.ForecastIns;
            var ForecastIns = ctx.ForecastIns.Where(c => c.forecastID == inslist.FirstOrDefault().forecastID).ToList();
            ctx.ForecastIns.RemoveRange(ForecastIns);
            ctx.SaveChanges();
            ctx.ForecastIns.AddRange(inslist);
            ctx.SaveChanges();
        }
        public int SaveData(getinstrument b)
        {

            Instrument I = new Instrument();
            var Instrument = ctx.Instrument.FirstOrDefault(c => c.InstrumentName == b.InstrumentName && c.UserId == b.UserId);
            if (Instrument != null)
            {
                return 0;
            }


            if (b.Frequency == "1")
            {
                I.DailyCtrlTest = b.noofrun;
            }
            else if (b.Frequency == "2")
            {
                I.WeeklyCtrlTest = b.noofrun;
            }
            else if (b.Frequency == "3")
            {
                I.MonthlyCtrlTest = b.noofrun;
            }
            else if (b.Frequency == "4")
            {
                I.QuarterlyCtrlTest = b.noofrun;
            }
            else
            {
                I.MaxTestBeforeCtrlTest = b.noofrun;
            }
            I.InstrumentName = b.InstrumentName;
            I.testingArea = ctx.TestingArea.FirstOrDefault(p => p.TestingAreaID == b.TestingArea);
            I.MaxThroughPut = b.MaxThroughPut;
            I.UserId = b.UserId;
            ctx.Instrument.Add(I);
            ctx.SaveChanges();
            int res = I.InstrumentID;
            return res;
        }

        public int DeleteData(int id)
        {
            int res = 0;
            var Instrument = ctx.Instrument.FirstOrDefault(b => b.InstrumentID == id);
            if (Instrument != null)
            {
                try
                {
                    ctx.Instrument.Remove(Instrument);
                    ctx.SaveChanges();
                    res = id;
                }
                catch (Exception ex)
                {

                }
            }
            return res;
        }
        public List<InstrumentList> GetdefaultdataIns()
        {
            int userid = 0;


            userid = ctx.User.Where(b => b.Role == "admin").Select(g => g.Id).FirstOrDefault();



            var result = (from r in ctx.Instrument.Where(b => b.UserId == userid)
                              //where r.UserId==userid
                          join ss in ctx.TestingArea on r.testingArea.TestingAreaID equals ss.TestingAreaID
                          select new InstrumentList()
                          {
                              InstrumentID = r.InstrumentID,
                              InstrumentName = r.InstrumentName,
                              testingArea = ss.AreaName,
                              testingAreaid = ss.TestingAreaID,
                              MaxTestBeforeCtrlTest = r.MaxTestBeforeCtrlTest,
                              MaxThroughPut = r.MaxThroughPut,
                              MonthlyCtrlTest = r.MonthlyCtrlTest,
                              QuarterlyCtrlTest = r.QuarterlyCtrlTest,
                              WeeklyCtrlTest = r.WeeklyCtrlTest,
                              DailyCtrlTest = r.DailyCtrlTest,
                              CtrlTestDuration = r.CtrlTestDuration,
                              CtrlTestNoOfRun = r.CtrlTestNoOfRun,
                              MonthMaxTPut = r.MonthMaxTPut,
                              UserId = r.UserId
                          }).OrderByDescending(x => x.InstrumentID).ToList();



            return result;
        }
        public Instrument Getbyid(int id)
        {
            // var Instrument = ctx.Instrument.FirstOrDefault(b => b.InstrumentID == id);
            var Instrument1 = (from r in ctx.Instrument
                               where r.InstrumentID == id
                               select new Instrument
                               {
                                   InstrumentID = r.InstrumentID,
                                   InstrumentName = r.InstrumentName,
                                   testingArea = ctx.TestingArea.FirstOrDefault(b => b.TestingAreaID == r.testingArea.TestingAreaID),
                                   MaxTestBeforeCtrlTest = r.MaxTestBeforeCtrlTest,
                                   MaxThroughPut = r.MaxThroughPut,
                                   MonthlyCtrlTest = r.MonthlyCtrlTest,
                                   QuarterlyCtrlTest = r.QuarterlyCtrlTest,
                                   WeeklyCtrlTest = r.WeeklyCtrlTest,
                                   DailyCtrlTest = r.DailyCtrlTest,
                                   CtrlTestDuration = r.CtrlTestDuration,
                                   CtrlTestNoOfRun = r.CtrlTestNoOfRun,
                                   MonthMaxTPut = r.MonthMaxTPut
                               }).Single();
            return Instrument1;
        }

        public IEnumerable<InstrumentList> GetAll(int userid, string role)


        {

            IEnumerable<InstrumentList> IL;
            var Roles = ctx.User.Where(b => b.Id == userid).Select(x => x.Role).FirstOrDefault();

            if (role == "admin")
            {
                IL = (from r in ctx.Instrument
                      join ss in ctx.TestingArea on r.testingArea.TestingAreaID equals ss.TestingAreaID
                     into TA
                      from TA1 in TA.DefaultIfEmpty()
                      select new InstrumentList()
                      {
                          InstrumentID = r.InstrumentID,
                          InstrumentName = r.InstrumentName,
                          testingArea = TA1.AreaName,
                          testingAreaid = TA1.TestingAreaID,
                          MaxTestBeforeCtrlTest = r.MaxTestBeforeCtrlTest,
                          MaxThroughPut = r.MaxThroughPut,
                          MonthlyCtrlTest = r.MonthlyCtrlTest,
                          QuarterlyCtrlTest = r.QuarterlyCtrlTest,
                          WeeklyCtrlTest = r.WeeklyCtrlTest,
                          DailyCtrlTest = r.DailyCtrlTest,
                          CtrlTestDuration = r.CtrlTestDuration,
                          CtrlTestNoOfRun = r.CtrlTestNoOfRun,
                          MonthMaxTPut = r.MonthMaxTPut,
                          UserId = r.UserId
                      }).OrderByDescending(x => x.InstrumentID).ToList();
            }
            else
            {

                var adminuserid = ctx.User.Where(b => b.Role == "admin").Select(x =>

                  x.Id

             ).FirstOrDefault();


                IL = (from r in ctx.Instrument
                      join ss in ctx.TestingArea on r.testingArea.TestingAreaID equals ss.TestingAreaID
                     into TA
                      from TA1 in TA.DefaultIfEmpty()
                      where r.UserId == userid || r.UserId == adminuserid || r.Isapprove == true
                      select new InstrumentList()
                      {
                          InstrumentID = r.InstrumentID,
                          InstrumentName = r.InstrumentName,
                          testingArea = TA1.AreaName,
                          testingAreaid = TA1.TestingAreaID,
                          MaxTestBeforeCtrlTest = r.MaxTestBeforeCtrlTest,
                          MaxThroughPut = r.MaxThroughPut,
                          MonthlyCtrlTest = r.MonthlyCtrlTest,
                          QuarterlyCtrlTest = r.QuarterlyCtrlTest,
                          WeeklyCtrlTest = r.WeeklyCtrlTest,
                          DailyCtrlTest = r.DailyCtrlTest,
                          CtrlTestDuration = r.CtrlTestDuration,
                          CtrlTestNoOfRun = r.CtrlTestNoOfRun,
                          MonthMaxTPut = r.MonthMaxTPut,
                          UserId = r.UserId
                      }).OrderByDescending(x => x.InstrumentID).ToList();

                //  result = result.Where(b => b.UserId == userid).ToList();
            }
            //  var Instruments = ctx.Instrument.ToList();
            return IL;
        }

        public int UpdateData(int id, getinstrument b)
        {
            int res = 0;

            var Instrument = ctx.Instrument.Where(p => p.InstrumentName == b.InstrumentName && p.UserId == b.UserId).ToList();
            if (Instrument != null && Instrument.Count == 1 && Instrument[0].InstrumentID == b.InstrumentID)
            {
                if (b.Frequency == "1")
                {
                    Instrument[0].DailyCtrlTest = b.noofrun;
                    Instrument[0].WeeklyCtrlTest = 0;
                    Instrument[0].MonthlyCtrlTest = 0;
                    Instrument[0].QuarterlyCtrlTest = 0;
                    Instrument[0].MaxTestBeforeCtrlTest = 0;
                }
                else if (b.Frequency == "2")
                {
                    Instrument[0].DailyCtrlTest = 0;

                    Instrument[0].MonthlyCtrlTest = 0;
                    Instrument[0].QuarterlyCtrlTest = 0;
                    Instrument[0].MaxTestBeforeCtrlTest = 0;
                    Instrument[0].WeeklyCtrlTest = b.noofrun;
                }
                else if (b.Frequency == "3")
                {
                    Instrument[0].DailyCtrlTest = 0;
                    Instrument[0].WeeklyCtrlTest = 0;

                    Instrument[0].QuarterlyCtrlTest = 0;
                    Instrument[0].MaxTestBeforeCtrlTest = 0;
                    Instrument[0].MonthlyCtrlTest = b.noofrun;
                }
                else if (b.Frequency == "4")
                {
                    Instrument[0].DailyCtrlTest = 0;
                    Instrument[0].WeeklyCtrlTest = 0;
                    Instrument[0].MonthlyCtrlTest = 0;
                    Instrument[0].MaxTestBeforeCtrlTest = 0;
                    Instrument[0].QuarterlyCtrlTest = b.noofrun;
                }
                else
                {
                    Instrument[0].DailyCtrlTest = 0;
                    Instrument[0].WeeklyCtrlTest = 0;
                    Instrument[0].MonthlyCtrlTest = 0;
                    Instrument[0].QuarterlyCtrlTest = 0;

                    Instrument[0].MaxTestBeforeCtrlTest = b.noofrun;
                }
                Instrument[0].InstrumentName = b.InstrumentName;
                Instrument[0].testingArea = ctx.TestingArea.FirstOrDefault(p => p.TestingAreaID == b.TestingArea);
                Instrument[0].InstrumentName = b.InstrumentName;

                Instrument[0].MaxThroughPut = b.MaxThroughPut;
                ctx.SaveChanges();
                res = Instrument[0].InstrumentID;
            }
            return res;
        }

        public Instrument GetInstrumentByName(string name)
        {
            var Instrument = ctx.Instrument.FirstOrDefault(b => b.InstrumentName == name);
            return Instrument;
        }
        public Instrument GetInstrumentByNameAndTestingArea(string name, int testingAreaId)
        {
            var Instrument = ctx.Instrument.FirstOrDefault(b => b.InstrumentName == name && b.testingArea.TestingAreaID == testingAreaId);
            return Instrument;
        }
        public IEnumerable<InstrumentList> GetListOfInstrumentByTestingArea(int testingAreaId)
        {
            var result = (from r in ctx.Instrument
                          join ss in ctx.TestingArea on r.testingArea.TestingAreaID equals ss.TestingAreaID
                          where ss.TestingAreaID == testingAreaId
                          select new InstrumentList()
                          {
                              InstrumentID = r.InstrumentID,
                              InstrumentName = r.InstrumentName,
                              testingArea = ss.AreaName,
                              MaxTestBeforeCtrlTest = r.MaxTestBeforeCtrlTest,
                              MaxThroughPut = r.MaxThroughPut,
                              MonthlyCtrlTest = r.MonthlyCtrlTest,
                              QuarterlyCtrlTest = r.QuarterlyCtrlTest,
                              WeeklyCtrlTest = r.WeeklyCtrlTest,
                              DailyCtrlTest = r.DailyCtrlTest,
                              CtrlTestDuration = r.CtrlTestDuration,
                              CtrlTestNoOfRun = r.CtrlTestNoOfRun,
                              MonthMaxTPut = r.MonthMaxTPut
                          }).ToList();
            //  var Instruments = ctx.Instrument.ToList();
            return result;
            //  var Instrument = ctx.Instrument.Where(b => b.testingArea.TestingAreaID == testingAreaId).ToList();
            //  return Instrument;
        }
        public IEnumerable<Instrument> GetListOfInstrumentByTestingArea(string classofTest)
        {
            var Instrument = ctx.Instrument.Where(b => b.testingArea.Category == classofTest).ToList();
            return Instrument;
        }
    }
}
