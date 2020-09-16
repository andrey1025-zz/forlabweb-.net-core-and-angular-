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
    public class AssumptionAccessRepositories : IAssumption<Dynamiccontrol, PatientAssumption, MMGeneralAssumptionValue, TestingAssumption, patientnumberlist, TestingProtocol>
    {

        ForLabContext ctx;
        List<string> Lstmonthname = new List<string>();
        string varids = "";
        DataTable dtGetValue = new DataTable();
        DataTable dtValue = new DataTable();
        public AssumptionAccessRepositories(ForLabContext c)
        {
            ctx = c;
            //return ctx;
        }
        public Array Gettestfromtestingprotocol(int id)
        {
            var test = ctx.TestingProtocol.Join(ctx.Test, b => b.TestID, c => c.TestID, (b, c) => new { b, c })
                .Where(g => g.b.ForecastinfoID == id).GroupBy(x => new { x.b.TestID }).Select(g => new
                {
                    testID = g.Key.TestID,
                    testName = g.Max(x => x.c.TestName)

                }).ToArray();
            return test;
        }
        public int Savelineargrowth(patientnumberlist b)
        {
            int res = 0;
            var forecastinfo = ctx.ForecastInfo.Find(b.ForecastinfoID);
            if (forecastinfo.Forecastlock != true)
            {
                var PatientNumberHeader = ctx.PatientNumberHeader.Where(c => c.ForecastinfoID == b.ForecastinfoID).FirstOrDefault();
                if (PatientNumberHeader != null)
                {
                    var Patientnumberdetail = ctx.PatientNumberDetail.Where(c => c.ForeCastId == b.ForecastinfoID).ToList();
                    ctx.PatientNumberDetail.RemoveRange(Patientnumberdetail);
                    ctx.SaveChanges();

                    var PA = ctx.PatientNumberDetail.Where(c => c.ForeCastId == b.ForecastinfoID).ToList();
                    if (PA == null)
                    {
                        ctx.PatientNumberHeader.Remove(PatientNumberHeader);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        ctx.PatientNumberDetail.RemoveRange(PA);
                        ctx.SaveChanges();
                        ctx.PatientNumberHeader.Remove(PatientNumberHeader);
                        ctx.SaveChanges();

                    }

                }
                PatientNumberHeader PH = new PatientNumberHeader();
                PH.ForecastinfoID = b.ForecastinfoID;
                PH.CurrentPatient = b.CurrentPatient;
                PH.TargetPatient = b.TargetPatient;
                PH.UserId = b.UserId;
                ctx.PatientNumberHeader.Add(PH);
                ctx.SaveChanges();

                foreach (PatientNumberDetail PD in b.patientdetaillist)
                {
                    PatientNumberDetail PDNew = new PatientNumberDetail();
                    PDNew.HeaderID = PH.ID;
                    PDNew.ForeCastId = PD.ForeCastId;
                    PDNew.SiteCategoryId = PD.SiteCategoryId;
                    PDNew.Serial = PD.Serial;
                    PDNew.Columnname = PD.Columnname;
                    PDNew.UserId = b.UserId;
                    ctx.PatientNumberDetail.Add(PDNew);
                    ctx.SaveChanges();
                    res = PDNew.ID;
                }
            }
            return res;
        }

        public int savetestinggeneralassumptionvalue(IEnumerable<MMGeneralAssumptionValue> b)
        {
            int res = 0;
            var forecastinfo = ctx.ForecastInfo.Find(b.FirstOrDefault().Forecastid);
            if (forecastinfo.Forecastlock != true)
            {
                foreach (MMGeneralAssumptionValue MMG in b)
                {

                    //pg = (PatientGroup)b.GetValue(i);
                    var MMGeneralAssumptionValue = ctx.MMGeneralAssumptionValue.Where(c => c.TestID == MMG.TestID && c.Forecastid == MMG.Forecastid && c.Parameterid == MMG.Parameterid && c.PatientGroupID == MMG.PatientGroupID).FirstOrDefault();
                    if (MMGeneralAssumptionValue != null)
                    {

                        MMGeneralAssumptionValue.Parametervalue = MMG.Parametervalue;
                        ctx.SaveChanges();
                        res = MMGeneralAssumptionValue.ID;

                    }
                    else
                    {
                        MMGeneralAssumptionValue MMG1 = new MMGeneralAssumptionValue();
                        MMG1.Forecastid = MMG.Forecastid;
                        MMG1.TestID = MMG.TestID;
                        MMG1.Parameterid = MMG.Parameterid;
                        MMG1.Parametervalue = MMG.Parametervalue;
                        MMG1.PatientGroupID = MMG.PatientGroupID;
                        MMG1.Userid = MMG.Userid;
                        ctx.MMGeneralAssumptionValue.Add(MMG1);
                        ctx.SaveChanges();
                        res = MMG.ID;
                    }
                }

            }
            return res;
        }
        public int saveproductgeneralassumptionvalue(IEnumerable<MMGeneralAssumptionValue> b)
        {
            int res = 0;
            var forecastinfo = ctx.ForecastInfo.Find(b.FirstOrDefault().Forecastid);
            if (forecastinfo.Forecastlock != true)
            {
                foreach (MMGeneralAssumptionValue MMG in b)
                {

                    //pg = (PatientGroup)b.GetValue(i);
                    var MMGeneralAssumptionValue = ctx.MMGeneralAssumptionValue.Where(c => c.ProductTypeID == MMG.ProductTypeID && c.Forecastid == MMG.Forecastid && c.Parameterid == MMG.Parameterid && c.Userid == MMG.Userid).FirstOrDefault();
                    if (MMGeneralAssumptionValue != null)
                    {

                        MMGeneralAssumptionValue.Parametervalue = MMG.Parametervalue;
                        ctx.SaveChanges();
                        res = MMGeneralAssumptionValue.ID;

                    }
                    else
                    {
                        MMGeneralAssumptionValue MMG1 = new MMGeneralAssumptionValue();
                        MMG1.Forecastid = MMG.Forecastid;
                        MMG1.ProductTypeID = MMG.ProductTypeID;
                        MMG1.Parameterid = MMG.Parameterid;
                        MMG1.Parametervalue = MMG.Parametervalue;
                        MMG1.Userid = MMG.Userid;

                        ctx.MMGeneralAssumptionValue.Add(MMG1);
                        ctx.SaveChanges();
                        res = MMG.ID;
                    }
                }

            }
            return res;
        }
        public int SaveproductAssumption(IEnumerable<TestingAssumption> b)
        {
            int res = 0;
            var forecastinfo = ctx.ForecastInfo.Find(b.FirstOrDefault().ForecastinfoID);
            if (forecastinfo.Forecastlock != true)
            {
                foreach (TestingAssumption TA in b)
                {

                    //TA = (PatientGroup)b.GetValue(i);
                    var TestingAssumption = ctx.TestingAssumption.Find(TA.ID);
                    if (TestingAssumption != null)
                    {
                        TestingAssumption.ForecastinfoID = TA.ForecastinfoID;
                        TestingAssumption.ProductTypeID = TA.ProductTypeID;
                        TestingAssumption.UserId = TA.UserId;

                        ctx.SaveChanges();
                        res = TestingAssumption.ID;

                    }
                    else
                    {
                        TestingAssumption newTA = new TestingAssumption();
                        newTA.ForecastinfoID = TA.ForecastinfoID;
                        newTA.ProductTypeID = TA.ProductTypeID;
                        newTA.UserId = TA.UserId;
                        ctx.TestingAssumption.Add(newTA);
                        ctx.SaveChanges();
                        res = newTA.ID;
                    }
                }
            }

            return res;

        }
        public int Savetestingprotocol(IEnumerable<TestingProtocol> b)
        {
            int res = 0;
            var forecastinfo = ctx.ForecastInfo.Find(b.FirstOrDefault().ForecastinfoID);
            if (forecastinfo.Forecastlock != true)
            {
                foreach (TestingProtocol TA in b)
                {

                    //TA = (PatientGroup)b.GetValue(i);
                    //var TestingProtocol = ctx.TestingProtocol.Find(TA.ID);
                    var TestingProtocol = ctx.TestingProtocol.Where(c => c.ForecastinfoID == TA.ForecastinfoID && c.PatientGroupID == TA.PatientGroupID && c.TestID == TA.TestID).FirstOrDefault();
                    if (TestingProtocol != null)
                    {
                        TestingProtocol.ForecastinfoID = TA.ForecastinfoID;
                        TestingProtocol.PercentagePanel = TA.PercentagePanel;
                        TestingProtocol.Baseline = TA.Baseline;
                        TestingProtocol.TotalTestPerYear = TA.TotalTestPerYear;
                        TestingProtocol.UserId = TA.UserId;
                        ctx.SaveChanges();
                        res = TestingProtocol.ID;

                    }
                    else
                    {
                        //   var testingpro=
                        TestingProtocol newTP = new TestingProtocol();
                        newTP.ForecastinfoID = TA.ForecastinfoID;
                        newTP.PatientGroupID = TA.PatientGroupID;
                        newTP.TestID = TA.TestID;
                        newTP.PercentagePanel = TA.PercentagePanel;
                        newTP.Baseline = TA.Baseline;
                        newTP.UserId = TA.UserId;
                        newTP.TotalTestPerYear = TA.TotalTestPerYear;
                        ctx.TestingProtocol.Add(newTP);
                        ctx.SaveChanges();
                        res = newTP.ID;
                    }
                }
            }

            return res;
        }
        public int savemmgeneralassumptionvalue(IEnumerable<MMGeneralAssumptionValue> b)
        {
            int res = 0;
            var forecastinfo = ctx.ForecastInfo.Find(b.FirstOrDefault().Forecastid);
            if (forecastinfo.Forecastlock != true)
            {
                foreach (MMGeneralAssumptionValue MMG in b)
                {
                    MMGeneralAssumptionValue MMG2 = new MMGeneralAssumptionValue();
                    var forcastinfo = ctx.ForecastInfo.Where(c => c.ForecastID == MMG.Forecastid).FirstOrDefault();
                    if (forcastinfo.ForecastType == "S")
                        MMG2 = ctx.MMGeneralAssumptionValue.Where(c => c.CategoryID == MMG.CategoryID && c.Forecastid == MMG.Forecastid && c.Parameterid == MMG.Parameterid).FirstOrDefault();
                    else
                        MMG2 = ctx.MMGeneralAssumptionValue.Where(c => c.CategoryID == MMG.CategoryID && c.Forecastid == MMG.Forecastid && c.Parameterid == MMG.Parameterid).FirstOrDefault();
                    if (MMG2 != null)
                    {

                        MMG2.Parametervalue = MMG.Parametervalue;
                        ctx.SaveChanges();
                        res = MMG2.ID;

                    }
                    else
                    {
                        MMGeneralAssumptionValue MMG1 = new MMGeneralAssumptionValue();
                        MMG1.Forecastid = MMG.Forecastid;
                        MMG1.SiteID = MMG.SiteID;
                        MMG1.Parameterid = MMG.Parameterid;
                        MMG1.Parametervalue = MMG.Parametervalue;
                        MMG1.CategoryID = MMG.CategoryID;
                        MMG1.Userid = MMG.Userid;
                        ctx.MMGeneralAssumptionValue.Add(MMG1);
                        ctx.SaveChanges();
                        res = MMG.ID;
                    }
                }
            }

            return res;
        }
        public int SavepatientAssumption(IEnumerable<PatientAssumption> b)
        {
            int res = 0;
            PatientAssumption PA = new PatientAssumption();
            var forecastinfo = ctx.ForecastInfo.Find(b.FirstOrDefault().ForecastinfoID);
            if (forecastinfo.Forecastlock != true)
            {
                foreach (PatientAssumption pg in b)
                {

                    //if (forecastinfo.ForecastType == "S")
                    //{
                    //    PA = ctx.PatientAssumption.Where(c => c.CategoryID == pg.CategoryID && c.ForecastinfoID == pg.ForecastinfoID).FirstOrDefault();
                    //}
                    //else
                    //{
                        PA = ctx.PatientAssumption.Where(c => c.CategoryID == pg.CategoryID && c.ForecastinfoID == pg.ForecastinfoID).FirstOrDefault();
                    //}
                    //////pg = (PatientGroup)b.GetValue(i);
                    //////    var PatientAssumption = 
                    if (PA != null)
                    {
                        PA.ForecastinfoID = pg.ForecastinfoID;
                        PA.SiteID = pg.SiteID;
                        PA.CategoryID = pg.CategoryID;
                        PA.Userid = pg.Userid;
                        ctx.SaveChanges();
                        res = PA.ID;

                    }
                    else
                    {
                        PatientAssumption newpg = new PatientAssumption();
                        newpg.ForecastinfoID = pg.ForecastinfoID;
                        newpg.SiteID = pg.SiteID;
                        newpg.CategoryID = pg.CategoryID;
                        newpg.Userid = pg.Userid;
                        ctx.PatientAssumption.Add(newpg);
                        ctx.SaveChanges();
                        res = newpg.ID;
                    }
                }

            }
            return res;

        }
        public List<string> getvariablrdynamicheader(int id, int entitytype)
        {
            var forecastinfo = ctx.ForecastInfo.Where(b => b.ForecastID == id).FirstOrDefault();

            List<string> header = new List<string>();
            //   var mmprogrem = ctx.MMProgram.Where(b => b.Id == forecastinfo.ProgramId).FirstOrDefault();
            var MMGeneralAssumption = ctx.demographicMMGeneralAssumption.Where(b => b.Forecastid == id && b.Entity_type_id == entitytype).ToList();

            if (entitytype == 3)
            {

            }
            else if (entitytype == 4)
            {

            }
            else
            {



                var MMGeneralAssumption1 = ctx.demographicMMGeneralAssumption.Where(b => b.Entity_type_id == entitytype).ToList();
                var testingprotocol = MMGeneralAssumption1.Where(c => c.VariableName.Contains("Mon")).ToList();


                switch (forecastinfo.months)
                {
                    case 13:
                        for (int i = 1; i < 13; i++)
                        {
                            header.Add("M" + i);
                        }
                        header.Add("Total Test Per Year");
                        break;
                    case 12:
                        for (int i = 1; i < 13; i++)
                        {
                            header.Add("M" + i);
                        }
                        break;
                    case 18:
                        for (int i = 1; i < 19; i++)
                        {
                            header.Add("M" + i);
                        }
                        break;
                    case 24:
                        for (int i = 1; i < 25; i++)
                        {
                            header.Add("M" + i);
                        }
                        break;
                }


                ////if (mmprogrem.NoofYear == 2)
                ////{
                ////    for (int i = 1; i < 25; i++)
                ////    {
                ////        //header.Add(char.ToUpperInvariant(testingprotocol[i].VariableName.ElementAt(0)) + testingprotocol[i].VariableName.Substring(1));
                ////        // DT.Rows.Add(Dr);
                ////        header.Add("M" + i);

                ////    }
                ////}
                ////else
                ////{
                ////    for (int i = 1; i < 13; i++)
                ////    {
                ////        //  header.Add(char.ToUpperInvariant(testingprotocol[i].VariableName.ElementAt(0)) + testingprotocol[i].VariableName.Substring(1));
                ////        header.Add("M" + i);
                ////        // DT.Columns.Add(testingprotocol[i].VariableName, typeof(Int32), Convert.ToString(testingprotocol[i].Id));
                ////    }
                ////}


                // DT.Columns.Add("TotalTestPerYear", typeof(Int32), "0");
                var testingprotocol1 = MMGeneralAssumption1.Where(c => !c.VariableName.Contains("Mon") && c.Forecastid == id).ToList();
                for (int i = 0; i < testingprotocol1.Count; i++)
                {
                    header.Add(char.ToUpperInvariant(testingprotocol1[i].VariableName.ElementAt(0)) + testingprotocol1[i].VariableName.Substring(1));
                    //  DT.Rows.Add(Dr);
                    // DT.Columns.Add(testingprotocol1[i].VariableName, typeof(Int32), Convert.ToString(testingprotocol1[i].Id));
                }
            }

            if (entitytype != 5)
            {
                for (int i = 0; i < MMGeneralAssumption.Count; i++)
                {
                    header.Add(char.ToUpperInvariant(MMGeneralAssumption[i].VariableName.ElementAt(0)) + MMGeneralAssumption[i].VariableName.Substring(1));


                    // DT.Columns.Add(MMGeneralAssumption[i].VariableName, typeof(Int32), Convert.ToString(MMGeneralAssumption[i].Id));
                }
            }
            return header;
        }
        public List<string> getdynamicheader(int id, int entitytype)
        {
            var forecastinfo = ctx.ForecastInfo.Where(b => b.ForecastID == id).FirstOrDefault();
            int vardatatye = 0;
            List<string> header = new List<string>();
            //   var mmprogrem = ctx.MMProgram.Where(b => b.Id == forecastinfo.ProgramId).FirstOrDefault();
            var MMGeneralAssumption = ctx.demographicMMGeneralAssumption.Where(b => b.Forecastid == id && b.Entity_type_id == entitytype).ToList();
            List<Dynamiccontrol> Db = new List<Dynamiccontrol>();
            DataTable DT = new DataTable();
            DT.Columns.Add("Name");
            DataRow Dr = DT.NewRow();
            if (entitytype == 3)
            {
                header.Add("SiteCategory Name");
                // DT.Columns.Add("sitecategoryname", typeof(string), "0");
            }
            else if (entitytype == 4)
            {
                header.Add("ProductType Name");

                //  DT.Columns.Add("ProductTypeName", typeof(string), "0");
            }
            else
            {
                header.Add("Test");
                header.Add("PatientGroup Name");
                header.Add("Percentage Panel");


                header.Add("Baseline");


                //var MMGeneralAssumption1 = ctx.demographicMMGeneralAssumption.Where(b => b.Entity_type_id == entitytype).ToList();
                //var testingprotocol = MMGeneralAssumption1.Where(c => c.VariableName.Contains("Mon")).ToList();


                switch (forecastinfo.months)
                {
                    case 13:

                        header.Add("Total Test Per Year");
                        break;

                }


                ////if (mmprogrem.NoofYear == 2)
                ////{
                ////    for (int i = 1; i < 25; i++)
                ////    {
                ////        //header.Add(char.ToUpperInvariant(testingprotocol[i].VariableName.ElementAt(0)) + testingprotocol[i].VariableName.Substring(1));
                ////        // DT.Rows.Add(Dr);
                ////        header.Add("M" + i);

                ////    }
                ////}
                ////else
                ////{
                ////    for (int i = 1; i < 13; i++)
                ////    {
                ////        //  header.Add(char.ToUpperInvariant(testingprotocol[i].VariableName.ElementAt(0)) + testingprotocol[i].VariableName.Substring(1));
                ////        header.Add("M" + i);
                ////        // DT.Columns.Add(testingprotocol[i].VariableName, typeof(Int32), Convert.ToString(testingprotocol[i].Id));
                ////    }
                ////}


                //// DT.Columns.Add("TotalTestPerYear", typeof(Int32), "0");
                //var testingprotocol1 = MMGeneralAssumption1.Where(c => !c.VariableName.Contains("Mon") && c.Forecastid  == id).ToList();
                //for (int i = 0; i < testingprotocol1.Count; i++)
                //{
                //    header.Add(char.ToUpperInvariant(testingprotocol1[i].VariableName.ElementAt(0)) + testingprotocol1[i].VariableName.Substring(1));
                //    //  DT.Rows.Add(Dr);
                //    // DT.Columns.Add(testingprotocol1[i].VariableName, typeof(Int32), Convert.ToString(testingprotocol1[i].Id));
                //}
            }

            if (entitytype != 5)
            {
                for (int i = 0; i < MMGeneralAssumption.Count; i++)
                {
                    header.Add(char.ToUpperInvariant(MMGeneralAssumption[i].VariableName.ElementAt(0)) + MMGeneralAssumption[i].VariableName.Substring(1));


                    // DT.Columns.Add(MMGeneralAssumption[i].VariableName, typeof(Int32), Convert.ToString(MMGeneralAssumption[i].Id));
                }
            }
            return header;
        }


        public List<string> getforecastdynamicheader(int id, int entitytype)
        {
            var forecastinfo = ctx.ForecastInfo.Where(b => b.ForecastID == id).FirstOrDefault();
            int vardatatye = 0;
            List<string> header = new List<string>();
            var mmprogrem = ctx.MMProgram.Where(b => b.Id == forecastinfo.ProgramId).FirstOrDefault();
            var MMGeneralAssumption = ctx.demographicMMGeneralAssumption.Where(b => b.ProgramId == forecastinfo.ProgramId && b.Forecastid == forecastinfo.ForecastID && b.Entity_type_id == entitytype).ToList();
            List<Dynamiccontrol> Db = new List<Dynamiccontrol>();
            DataTable DT = new DataTable();
            DT.Columns.Add("Name");
            DataRow Dr = DT.NewRow();
            if (entitytype == 3)
            {
                header.Add("SiteCategory Name");
                // DT.Columns.Add("sitecategoryname", typeof(string), "0");
            }
            else if (entitytype == 4)
            {
                header.Add("ProductType Name");

                //  DT.Columns.Add("ProductTypeName", typeof(string), "0");
            }
            else
            {
                header.Add("Test");
                header.Add("PatientGroup Name");
                header.Add("Percentage Panel");


                header.Add("Baseline");


                var MMGeneralAssumption1 = ctx.demographicMMGeneralAssumption.Where(b => b.Entity_type_id == entitytype).ToList();
                var testingprotocol = MMGeneralAssumption1.Where(c => c.VariableName.Contains("Mon") && c.Forecastid == forecastinfo.ForecastID).ToList();

                switch (forecastinfo.months)
                {
                    case 13:
                        for (int i = 0; i < 12; i++)
                        {
                            header.Add("M" + i);
                        }
                        header.Add("Total Test Per Year");
                        break;
                    case 12:
                        for (int i = 0; i < 12; i++)
                        {
                            header.Add("M" + i);
                        }
                        break;
                    case 18:
                        for (int i = 0; i < 18; i++)
                        {
                            header.Add("M" + i);
                        }
                        break;
                    case 24:
                        for (int i = 0; i < 24; i++)
                        {
                            header.Add("M" + i);
                        }
                        break;
                }



                //if (mmprogrem.NoofYear == 2)
                //{
                //    for (int i = 1; i < 25; i++)
                //    {
                //        //header.Add(char.ToUpperInvariant(testingprotocol[i].VariableName.ElementAt(0)) + testingprotocol[i].VariableName.Substring(1));
                //        // DT.Rows.Add(Dr);
                //        header.Add("M" + i);

                //    }
                //}
                //else
                //{
                //    for (int i = 1; i < 13; i++)
                //    {
                //        //  header.Add(char.ToUpperInvariant(testingprotocol[i].VariableName.ElementAt(0)) + testingprotocol[i].VariableName.Substring(1));
                //        header.Add("M" + i);
                //        // DT.Columns.Add(testingprotocol[i].VariableName, typeof(Int32), Convert.ToString(testingprotocol[i].Id));
                //    }
                //}


                // DT.Columns.Add("TotalTestPerYear", typeof(Int32), "0");
                var testingprotocol1 = MMGeneralAssumption1.Where(c => !c.VariableName.Contains("Mon") && c.ProgramId == forecastinfo.ProgramId && c.Forecastid == forecastinfo.ForecastID).ToList();
                for (int i = 0; i < testingprotocol1.Count; i++)
                {
                    header.Add(char.ToUpperInvariant(testingprotocol1[i].VariableName.ElementAt(0)) + testingprotocol1[i].VariableName.Substring(1));
                    //  DT.Rows.Add(Dr);
                    // DT.Columns.Add(testingprotocol1[i].VariableName, typeof(Int32), Convert.ToString(testingprotocol1[i].Id));
                }
            }

            if (entitytype != 5)
            {
                for (int i = 0; i < MMGeneralAssumption.Count; i++)
                {
                    header.Add(char.ToUpperInvariant(MMGeneralAssumption[i].VariableName.ElementAt(0)) + MMGeneralAssumption[i].VariableName.Substring(1));


                    // DT.Columns.Add(MMGeneralAssumption[i].VariableName, typeof(Int32), Convert.ToString(MMGeneralAssumption[i].Id));
                }
            }
            return header;
        }
        public IList<Dynamiccontrol> GetDynamiccontrol(int id, int entitytype)
        {
            var forecastinfo = ctx.ForecastInfo.Where(b => b.ForecastID == id).FirstOrDefault();
            int vardatatye = 0;
            var mmprogrem = ctx.MMProgram.Where(b => b.Id == forecastinfo.ProgramId).FirstOrDefault();
            var MMGeneralAssumption = ctx.MMGeneralAssumption.Where(b => b.ProgramId == forecastinfo.ProgramId && b.Entity_type_id == entitytype).ToList();
            List<Dynamiccontrol> Db = new List<Dynamiccontrol>();
            DataTable DT = new DataTable();

            if (entitytype == 3)
            {
                DT.Columns.Add("sitecategoryname", typeof(string), "0");
            }
            else if (entitytype == 4)
            {
                DT.Columns.Add("ProductTypeName", typeof(string), "0");
            }
            else
            {
                DT.Columns.Add("TestName", typeof(string), "0");
                DT.Columns.Add("PatientGroupName", typeof(string), "0");
                DT.Columns.Add("PercentagePanel", typeof(Int32), "0");
                DT.Columns.Add("Baseline", typeof(Int32), "0");
                var MMGeneralAssumption1 = ctx.MMGeneralAssumption.Where(b => b.Entity_type_id == entitytype).ToList();
                var testingprotocol = MMGeneralAssumption1.Where(c => c.VariableName.Contains("Mon")).ToList();
                if (mmprogrem.NoofYear == 2)
                {
                    for (int i = 0; i < 24; i++)
                    {
                        DT.Columns.Add(testingprotocol[i].VariableName, typeof(Int32), Convert.ToString(testingprotocol[i].Id));
                    }
                }
                else
                {
                    for (int i = 0; i < 12; i++)
                    {
                        DT.Columns.Add(testingprotocol[i].VariableName, typeof(Int32), Convert.ToString(testingprotocol[i].Id));
                    }
                }
                DT.Columns.Add("TotalTestPerYear", typeof(Int32), "0");
                var testingprotocol1 = MMGeneralAssumption1.Where(c => !c.VariableName.Contains("Mon") && c.ProgramId == forecastinfo.ProgramId).ToList();
                for (int i = 0; i < testingprotocol1.Count; i++)
                {
                    DT.Columns.Add(testingprotocol1[i].VariableName, typeof(Int32), Convert.ToString(testingprotocol1[i].Id));
                }
            }

            if (entitytype != 5)
            {
                for (int i = 0; i < MMGeneralAssumption.Count; i++)
                {
                    DT.Columns.Add(MMGeneralAssumption[i].VariableName, typeof(Int32), Convert.ToString(MMGeneralAssumption[i].Id));
                }
            }
            for (int i = 0; i < DT.Columns.Count; i++)
            {
                if (DT.Columns[i].Expression != "0")
                {

                    vardatatye = ctx.MMGeneralAssumption.Where(b => b.Id == Convert.ToInt32(DT.Columns[i].Expression)).Select(x => x.VariableDataType).FirstOrDefault();



                }


                if (entitytype == 3)
                {
                    Db.Add(new Dynamiccontrol
                    {
                        name = DT.Columns[i].Caption.ToLower(),
                        ID = Convert.ToInt32(DT.Columns[i].Expression),
                        type = DT.Columns[i].DataType == typeof(Int32) ? "number" : "text",
                        datatype = vardatatye
                    });
                }
                else
                {
                    Db.Add(new Dynamiccontrol
                    {
                        name = Char.ToLowerInvariant(DT.Columns[i].Caption[0]) + DT.Columns[i].Caption.Substring(1),
                        ID = Convert.ToInt32(DT.Columns[i].Expression),
                        type = DT.Columns[i].DataType == typeof(Int32) ? "number" : "text",
                        datatype = vardatatye
                    });
                }

            }

            return Db;
        }


        public IList<Dynamiccontrol> GetforecastDynamiccontrol(int id, int entitytype)
        {
            var forecastinfo = ctx.ForecastInfo.Where(b => b.ForecastID == id).FirstOrDefault();
            int vardatatye = 0;
            var mmprogrem = ctx.MMProgram.Where(b => b.Id == forecastinfo.ProgramId).FirstOrDefault();
            var MMGeneralAssumption = ctx.demographicMMGeneralAssumption.Where(b => b.ProgramId == forecastinfo.ProgramId && b.Forecastid == forecastinfo.ForecastID && b.Entity_type_id == entitytype).ToList();
            List<Dynamiccontrol> Db = new List<Dynamiccontrol>();
            DataTable DT = new DataTable();

            if (entitytype == 3)
            {
                DT.Columns.Add("sitecategoryname", typeof(string), "0");
            }
            else if (entitytype == 4)
            {
                DT.Columns.Add("ProductTypeName", typeof(string), "0");
            }
            else
            {
                DT.Columns.Add("TestName", typeof(string), "0");
                DT.Columns.Add("PatientGroupName", typeof(string), "0");
                DT.Columns.Add("PercentagePanel", typeof(Int32), "0");
                DT.Columns.Add("Baseline", typeof(Int32), "0");
                //var MMGeneralAssumption1 = ctx.demographicMMGeneralAssumption.Where(b => b.Entity_type_id == entitytype).ToList();
                //  var testingprotocol = MMGeneralAssumption1.Where(c => c.VariableName.Contains("Mon") && c.Forecastid==forecastinfo.ForecastID).ToList();

                switch (forecastinfo.months)
                {
                    case 13:

                        DT.Columns.Add("TotalTestPerYear", typeof(Int32), "0");
                        break;

                }



                //if (mmprogrem.NoofYear == 2)
                //{
                //    for (int i = 0; i < 24; i++)
                //    {
                //        DT.Columns.Add(testingprotocol[i].VariableName, typeof(Int32), Convert.ToString(testingprotocol[i].Id));
                //    }
                //}
                //else
                //{
                //    for (int i = 0; i < 12; i++)
                //    {
                //        DT.Columns.Add(testingprotocol[i].VariableName, typeof(Int32), Convert.ToString(testingprotocol[i].Id));
                //    }
                //}
                //DT.Columns.Add("TotalTestPerYear", typeof(Int32), "0");
                //var testingprotocol1 = MMGeneralAssumption1.Where(c => !c.VariableName.Contains("Mon") && c.ProgramId == forecastinfo.ProgramId && c.Forecastid==forecastinfo.ForecastID).ToList();
                //for (int i = 0; i < testingprotocol1.Count; i++)
                //{
                //    DT.Columns.Add(testingprotocol1[i].VariableName, typeof(Int32), Convert.ToString(testingprotocol1[i].Id));
                //}
            }

            if (entitytype != 5)
            {
                for (int i = 0; i < MMGeneralAssumption.Count; i++)
                {
                    DT.Columns.Add(MMGeneralAssumption[i].VariableName, typeof(Int32), Convert.ToString(MMGeneralAssumption[i].Id));
                }
            }
            for (int i = 0; i < DT.Columns.Count; i++)
            {
                if (DT.Columns[i].Expression != "0")
                {

                    vardatatye = MMGeneralAssumption.Where(b => b.Id == Convert.ToInt32(DT.Columns[i].Expression)).Select(x => x.VariableDataType).FirstOrDefault();



                }


                if (entitytype == 3)
                {
                    Db.Add(new Dynamiccontrol
                    {
                        name = DT.Columns[i].Caption.ToLower(),
                        ID = Convert.ToInt32(DT.Columns[i].Expression),
                        type = DT.Columns[i].DataType == typeof(Int32) ? "number" : "text",
                        datatype = vardatatye
                    });
                }
                else
                {
                    Db.Add(new Dynamiccontrol
                    {
                        name = Char.ToLowerInvariant(DT.Columns[i].Caption[0]) + DT.Columns[i].Caption.Substring(1),
                        ID = Convert.ToInt32(DT.Columns[i].Expression),
                        type = DT.Columns[i].DataType == typeof(Int32) ? "number" : "text",
                        datatype = vardatatye
                    });
                }

            }

            return Db;
        }
        public DataTable GettestAssumption(int id, int testid)
        {
            Array A;
            DataTable DT = new DataTable();
            int k;
            DT.Columns.Add("ID");
            DT.Columns.Add("TestID");
            DT.Columns.Add("TestName");
            DT.Columns.Add("PatientGroupID");
            DT.Columns.Add("PatientGroupName");

            DT.Columns.Add("ForecastinfoID");
            DT.Columns.Add("PercentagePanel");
            DT.Columns.Add("Baseline");
            var forecastinfo = ctx.ForecastInfo.Where(b => b.ForecastID == id).FirstOrDefault();
            var mmprogram = ctx.MMProgram.Where(b => b.Id == forecastinfo.ProgramId).FirstOrDefault();
            var MMGeneralAssumption1 = ctx.MMGeneralAssumption.Where(b => b.Entity_type_id == 5).ToList();

            var testingprotocol = MMGeneralAssumption1.Where(c => c.VariableName.Contains("Mon")).ToList();
            if (mmprogram.NoofYear == 2)
            {
                for (int i = 0; i < 24; i++)
                {
                    DT.Columns.Add(testingprotocol[i].VariableName, typeof(Int32));
                }
            }
            else
            {
                for (int i = 0; i < 12; i++)
                {
                    DT.Columns.Add(testingprotocol[i].VariableName, typeof(Int32));
                }
            }
            DT.Columns.Add("TotalTestPerYear", typeof(Int32));
            var testingprotocol1 = MMGeneralAssumption1.Where(c => !c.VariableName.Contains("Mon") && c.ProgramId == forecastinfo.ProgramId).ToList();
            for (int i = 0; i < testingprotocol1.Count; i++)
            {
                DT.Columns.Add(testingprotocol1[i].VariableName, typeof(Int32));
            }
            var mmgroup = ctx.MMGroup.Where(c => c.ProgramId == mmprogram.Id && c.IsActive == true).ToList();
            //if (testid == 0)
            //{
            //    for (int i = 0; i < mmgroup.Count; i++)
            //    {
            //        k = 7;
            //        DataRow Dr = DT.NewRow();
            //        Dr["ID"] = 0;
            //        Dr["TestID"] = 0;
            //        Dr["Test"] = "";
            //        Dr["PatientGroupID"] = mmgroup[i].Id;
            //        Dr["PatientGroupName"] = mmgroup[i].GroupName;
            //        Dr["ForecastinfoID"] = id;
            //        Dr["PercentagePanel"] = 0;
            //        Dr["Baseline"] = 0;

            //        if (mmprogram.NoofYear == 2)
            //        {
            //            for (int j = 0; j < 24; j++)
            //            {
            //                Dr[k] = 0;
            //                k++;
            //            }
            //        }
            //        else
            //        {
            //            for (int j = 0; j < 12; j++)
            //            {
            //                Dr[k] = 0;
            //                k++;
            //            }
            //        }
            //        Dr["TotalTestPerYear"] = 0;
            //        k++;
            //        for (int j = 0; j < testingprotocol1.Count; j++)
            //        {
            //            Dr[k] = 0;
            //            k++;
            //        }
            //        DT.Rows.Add(Dr);
            //    }
            //}
            //else
            //{

            if (testid == 0)
            {
                var TestingprotocolEnt = ctx.TestingProtocol.Where(b => b.ForecastinfoID == id).ToList();
                if (TestingprotocolEnt != null)
                {
                    for (int i = 0; i < TestingprotocolEnt.Count; i++)
                    {



                        DataRow Dr = DT.NewRow();
                        Dr["ID"] = TestingprotocolEnt[i].ID;
                        Dr["TestID"] = TestingprotocolEnt[i].TestID;
                        Dr["TestName"] = ctx.Test.Find(TestingprotocolEnt[i].TestID).TestName;
                        Dr["PatientGroupID"] = TestingprotocolEnt[i].PatientGroupID;
                        Dr["PatientGroupName"] = mmgroup.Where(d => d.Id == TestingprotocolEnt[i].PatientGroupID).FirstOrDefault().GroupName;
                        Dr["ForecastinfoID"] = TestingprotocolEnt[i].ForecastinfoID;
                        Dr["PercentagePanel"] = TestingprotocolEnt[i].PercentagePanel;
                        Dr["Baseline"] = TestingprotocolEnt[i].Baseline;
                        if (mmprogram.NoofYear == 2)
                        {
                            for (int j = 0; j < 24; j++)
                            {
                                var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == TestingprotocolEnt[i].TestID && b.Forecastid == id && b.Parameterid == testingprotocol[j].Id && b.PatientGroupID == TestingprotocolEnt[i].PatientGroupID).FirstOrDefault();

                                if (MMGeneralAssumptionvalue != null)
                                    Dr[testingprotocol[j].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                                else
                                    Dr[testingprotocol[j].VariableName] = 0;


                            }
                        }
                        else
                        {
                            for (int j = 0; j < 12; j++)
                            {
                                var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == TestingprotocolEnt[i].TestID && b.Forecastid == id && b.Parameterid == testingprotocol[j].Id && b.PatientGroupID == TestingprotocolEnt[i].PatientGroupID).FirstOrDefault();

                                if (MMGeneralAssumptionvalue != null)
                                    Dr[testingprotocol[j].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                                else
                                    Dr[testingprotocol[j].VariableName] = 0;
                            }
                        }
                        Dr["TotalTestPerYear"] = TestingprotocolEnt[i].TotalTestPerYear;


                        for (int ki = 0; ki < testingprotocol1.Count; ki++)
                        {

                            var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == TestingprotocolEnt[i].TestID && b.Forecastid == id && b.Parameterid == testingprotocol1[ki].Id && b.PatientGroupID == TestingprotocolEnt[i].PatientGroupID).FirstOrDefault();

                            if (MMGeneralAssumptionvalue != null)
                                Dr[testingprotocol1[ki].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                            else
                                Dr[testingprotocol1[ki].VariableName] = 0;

                        }
                        DT.Rows.Add(Dr);
                    }


                }
            }
            else
            {
                for (int i = 0; i < mmgroup.Count; i++)
                {
                    k = 8;
                    DataRow Dr = DT.NewRow();
                    Dr["ID"] = 0;
                    Dr["TestID"] = testid;
                    Dr["TestName"] = ctx.Test.Find(testid).TestName;
                    Dr["PatientGroupID"] = mmgroup[i].Id;
                    Dr["PatientGroupName"] = mmgroup[i].GroupName;
                    Dr["ForecastinfoID"] = id;
                    Dr["PercentagePanel"] = 0;
                    Dr["Baseline"] = 0;

                    if (mmprogram.NoofYear == 2)
                    {
                        for (int j = 0; j < 24; j++)
                        {
                            Dr[k] = 0;
                            k++;
                        }
                    }
                    else
                    {
                        for (int j = 0; j < 12; j++)
                        {
                            Dr[k] = 0;
                            k++;
                        }
                    }
                    Dr["TotalTestPerYear"] = 0;
                    k++;
                    for (int j = 0; j < testingprotocol1.Count; j++)
                    {
                        Dr[k] = 0;
                        k++;
                    }
                    DT.Rows.Add(Dr);
                }
            }

            //}
            A = DT.Rows.Cast<DataRow>().ToArray();
            return DT;
        }


        public DataTable GettestforecastAssumption(int id, int testid)
        {
            Array A;
            DataTable DT = new DataTable();
            int k;
            DT.Columns.Add("ID");
            DT.Columns.Add("TestID");
            DT.Columns.Add("TestName");
            DT.Columns.Add("PatientGroupID");
            DT.Columns.Add("PatientGroupName");

            DT.Columns.Add("ForecastinfoID");
            DT.Columns.Add("PercentagePanel");
            DT.Columns.Add("Baseline");
            var forecastinfo = ctx.ForecastInfo.Where(b => b.ForecastID == id).FirstOrDefault();
            var mmprogram = ctx.MMProgram.Where(b => b.Id == forecastinfo.ProgramId).FirstOrDefault();
            var MMGeneralAssumption1 = ctx.demographicMMGeneralAssumption.Where(b => b.Entity_type_id == 5).ToList();

            var testingprotocol = MMGeneralAssumption1.Where(c => c.VariableName.Contains("Mon") && c.Forecastid == forecastinfo.ForecastID).ToList();


            switch (forecastinfo.months)
            {
                case 13:
                    for (int i = 0; i < 12; i++)
                    {
                        DT.Columns.Add(testingprotocol[i].VariableName, typeof(Int32));
                    }
                    DT.Columns.Add("TotalTestPerYear", typeof(Int32));
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



            var testingprotocol1 = MMGeneralAssumption1.Where(c => !c.VariableName.Contains("Mon") && c.ProgramId == forecastinfo.ProgramId && c.Forecastid == forecastinfo.ForecastID).ToList();
            for (int i = 0; i < testingprotocol1.Count; i++)
            {
                DT.Columns.Add(testingprotocol1[i].VariableName, typeof(Int32));
            }
            var mmgroup = ctx.DemographicMMGroup.Where(c => c.ProgramId == mmprogram.Id && c.Forecastid == forecastinfo.ForecastID && c.IsActive == true).ToList();
            //if (testid == 0)
            //{
            //    for (int i = 0; i < mmgroup.Count; i++)
            //    {
            //        k = 7;
            //        DataRow Dr = DT.NewRow();
            //        Dr["ID"] = 0;
            //        Dr["TestID"] = 0;
            //        Dr["Test"] = "";
            //        Dr["PatientGroupID"] = mmgroup[i].Id;
            //        Dr["PatientGroupName"] = mmgroup[i].GroupName;
            //        Dr["ForecastinfoID"] = id;
            //        Dr["PercentagePanel"] = 0;
            //        Dr["Baseline"] = 0;

            //        if (mmprogram.NoofYear == 2)
            //        {
            //            for (int j = 0; j < 24; j++)
            //            {
            //                Dr[k] = 0;
            //                k++;
            //            }
            //        }
            //        else
            //        {
            //            for (int j = 0; j < 12; j++)
            //            {
            //                Dr[k] = 0;
            //                k++;
            //            }
            //        }
            //        Dr["TotalTestPerYear"] = 0;
            //        k++;
            //        for (int j = 0; j < testingprotocol1.Count; j++)
            //        {
            //            Dr[k] = 0;
            //            k++;
            //        }
            //        DT.Rows.Add(Dr);
            //    }
            //}
            //else
            //{

            if (testid != 0)
            {
                var TestingprotocolEnt = ctx.TestingProtocol.Where(b => b.ForecastinfoID == id && b.TestID == testid).ToList();
                if (TestingprotocolEnt.Count != 0)
                {
                    for (int i = 0; i < TestingprotocolEnt.Count; i++)
                    {



                        DataRow Dr = DT.NewRow();
                        Dr["ID"] = TestingprotocolEnt[i].ID;
                        Dr["TestID"] = TestingprotocolEnt[i].TestID;
                        Dr["TestName"] = ctx.Test.Find(TestingprotocolEnt[i].TestID).TestName;
                        Dr["PatientGroupID"] = TestingprotocolEnt[i].PatientGroupID;
                        Dr["PatientGroupName"] = mmgroup.Where(d => d.Id == TestingprotocolEnt[i].PatientGroupID).FirstOrDefault().GroupName;
                        Dr["ForecastinfoID"] = TestingprotocolEnt[i].ForecastinfoID;
                        Dr["PercentagePanel"] = TestingprotocolEnt[i].PercentagePanel;
                        Dr["Baseline"] = TestingprotocolEnt[i].Baseline;

                        switch (forecastinfo.months)
                        {
                            case 13:
                                for (int j = 0; j < 12; j++)
                                {
                                    var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == TestingprotocolEnt[i].TestID && b.Forecastid == id && b.Parameterid == testingprotocol[j].Id && b.PatientGroupID == TestingprotocolEnt[i].PatientGroupID).FirstOrDefault();

                                    if (MMGeneralAssumptionvalue != null)
                                        Dr[testingprotocol[j].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                                    else
                                        Dr[testingprotocol[j].VariableName] = 0;
                                }
                                Dr["TotalTestPerYear"] = TestingprotocolEnt[i].TotalTestPerYear;
                                break;
                            case 12:
                                for (int j = 0; j < 12; j++)
                                {
                                    var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == TestingprotocolEnt[i].TestID && b.Forecastid == id && b.Parameterid == testingprotocol[j].Id && b.PatientGroupID == TestingprotocolEnt[i].PatientGroupID).FirstOrDefault();

                                    if (MMGeneralAssumptionvalue != null)
                                        Dr[testingprotocol[j].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                                    else
                                        Dr[testingprotocol[j].VariableName] = 0;
                                }
                                break;
                            case 18:
                                for (int j = 0; j < 18; j++)
                                {
                                    var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == TestingprotocolEnt[i].TestID && b.Forecastid == id && b.Parameterid == testingprotocol[j].Id && b.PatientGroupID == TestingprotocolEnt[i].PatientGroupID).FirstOrDefault();

                                    if (MMGeneralAssumptionvalue != null)
                                        Dr[testingprotocol[j].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                                    else
                                        Dr[testingprotocol[j].VariableName] = 0;
                                }
                                break;
                            case 24:
                                for (int j = 0; j < 24; j++)
                                {
                                    var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == TestingprotocolEnt[i].TestID && b.Forecastid == id && b.Parameterid == testingprotocol[j].Id && b.PatientGroupID == TestingprotocolEnt[i].PatientGroupID).FirstOrDefault();

                                    if (MMGeneralAssumptionvalue != null)
                                        Dr[testingprotocol[j].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                                    else
                                        Dr[testingprotocol[j].VariableName] = 0;
                                }
                                break;
                        }
                        //if (mmprogram.NoofYear == 2)
                        //{
                        //    for (int j = 0; j < 24; j++)
                        //    {
                        //        var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == TestingprotocolEnt[i].TestID && b.Forecastid == id && b.Parameterid == testingprotocol[j].Id && b.PatientGroupID == TestingprotocolEnt[i].PatientGroupID).FirstOrDefault();

                        //        if (MMGeneralAssumptionvalue != null)
                        //            Dr[testingprotocol[j].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                        //        else
                        //            Dr[testingprotocol[j].VariableName] = 0;


                        //    }
                        //}
                        //else
                        //{
                        //    for (int j = 0; j < 12; j++)
                        //    {
                        //        var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == TestingprotocolEnt[i].TestID && b.Forecastid == id && b.Parameterid == testingprotocol[j].Id && b.PatientGroupID == TestingprotocolEnt[i].PatientGroupID).FirstOrDefault();

                        //        if (MMGeneralAssumptionvalue != null)
                        //            Dr[testingprotocol[j].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                        //        else
                        //            Dr[testingprotocol[j].VariableName] = 0;
                        //    }
                        //}



                        for (int ki = 0; ki < testingprotocol1.Count; ki++)
                        {

                            var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == TestingprotocolEnt[i].TestID && b.Forecastid == id && b.Parameterid == testingprotocol1[ki].Id && b.PatientGroupID == TestingprotocolEnt[i].PatientGroupID).FirstOrDefault();

                            if (MMGeneralAssumptionvalue != null)
                                Dr[testingprotocol1[ki].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                            else
                                Dr[testingprotocol1[ki].VariableName] = 0;

                        }
                        DT.Rows.Add(Dr);
                    }


                }
                else
                {
                    for (int i = 0; i < mmgroup.Count; i++)
                    {
                        k = 8;
                        DataRow Dr = DT.NewRow();
                        Dr["ID"] = 0;
                        Dr["TestID"] = testid;
                        Dr["TestName"] = ctx.Test.Find(testid).TestName;
                        Dr["PatientGroupID"] = mmgroup[i].Id;
                        Dr["PatientGroupName"] = mmgroup[i].GroupName;
                        Dr["ForecastinfoID"] = id;
                        Dr["PercentagePanel"] = 0;
                        Dr["Baseline"] = 0;


                        switch (forecastinfo.months)
                        {
                            case 13:
                                for (int j = 0; j < 12; j++)
                                {
                                    Dr[k] = 0;
                                    k++;
                                }
                                Dr["TotalTestPerYear"] = 0;
                                k++;
                                break;
                            case 12:
                                for (int j = 0; j < 12; j++)
                                {
                                    Dr[k] = 0;
                                    k++;
                                }
                                break;
                            case 18:
                                for (int j = 0; j < 18; j++)
                                {
                                    Dr[k] = 0;
                                    k++;
                                }
                                break;
                            case 24:
                                for (int j = 0; j < 24; j++)
                                {
                                    Dr[k] = 0;
                                    k++;
                                }
                                break;
                        }


                        //if (mmprogram.NoofYear == 2)
                        //{
                        //    for (int j = 0; j < 24; j++)
                        //    {
                        //        Dr[k] = 0;
                        //        k++;
                        //    }
                        //}
                        //else
                        //{
                        //    for (int j = 0; j < 12; j++)
                        //    {
                        //        Dr[k] = 0;
                        //        k++;
                        //    }
                        //}
                        //Dr["TotalTestPerYear"] = 0;
                        //k++;
                        for (int j = 0; j < testingprotocol1.Count; j++)
                        {
                            Dr[k] = 0;
                            k++;
                        }
                        DT.Rows.Add(Dr);
                    }
                }
            }
            else
            {
                for (int i = 0; i < mmgroup.Count; i++)
                {
                    k = 8;
                    DataRow Dr = DT.NewRow();
                    Dr["ID"] = 0;
                    Dr["TestID"] = testid;
                    Dr["TestName"] = "";
                    Dr["PatientGroupID"] = mmgroup[i].Id;
                    Dr["PatientGroupName"] = mmgroup[i].GroupName;
                    Dr["ForecastinfoID"] = id;
                    Dr["PercentagePanel"] = 0;
                    Dr["Baseline"] = 0;


                    switch (forecastinfo.months)
                    {
                        case 13:
                            for (int j = 0; j < 12; j++)
                            {
                                Dr[k] = 0;
                                k++;
                            }
                            Dr["TotalTestPerYear"] = 0;
                            k++;
                            break;
                        case 12:
                            for (int j = 0; j < 12; j++)
                            {
                                Dr[k] = 0;
                                k++;
                            }
                            break;
                        case 18:
                            for (int j = 0; j < 18; j++)
                            {
                                Dr[k] = 0;
                                k++;
                            }
                            break;
                        case 24:
                            for (int j = 0; j < 24; j++)
                            {
                                Dr[k] = 0;
                                k++;
                            }
                            break;
                    }


                    //if (mmprogram.NoofYear == 2)
                    //{
                    //    for (int j = 0; j < 24; j++)
                    //    {
                    //        Dr[k] = 0;
                    //        k++;
                    //    }
                    //}
                    //else
                    //{
                    //    for (int j = 0; j < 12; j++)
                    //    {
                    //        Dr[k] = 0;
                    //        k++;
                    //    }
                    //}
                    //Dr["TotalTestPerYear"] = 0;
                    //k++;
                    for (int j = 0; j < testingprotocol1.Count; j++)
                    {
                        Dr[k] = 0;
                        k++;
                    }
                    DT.Rows.Add(Dr);
                }
            }

            //}
            A = DT.Rows.Cast<DataRow>().ToArray();
            return DT;
        }
        public Array GetproductAssumption(int id)
        {
            Array A;
            var forecastinfo = ctx.ForecastInfo.Where(b => b.ForecastID == id).FirstOrDefault();
            var MMGeneralAssumption = ctx.MMGeneralAssumption.Where(b => b.ProgramId == forecastinfo.ProgramId && b.Entity_type_id == 4).ToList();

            DataTable DT = new DataTable();
            var TestingAssumption = ctx.TestingAssumption.Where(b => b.ForecastinfoID == id).ToList();

            DT.Columns.Add("ID");
            DT.Columns.Add("ForecastinfoID");
            DT.Columns.Add("ProductTypeID");
            DT.Columns.Add("ProductTypeName");
            for (int i = 0; i < MMGeneralAssumption.Count; i++)
            {

                DT.Columns.Add(MMGeneralAssumption[i].VariableName);

            }
            for (int j = 0; j < TestingAssumption.Count; j++)
            {
                DataRow Dr = DT.NewRow();
                Dr[0] = TestingAssumption[j].ID;
                Dr[1] = TestingAssumption[j].ForecastinfoID;


                Dr[2] = TestingAssumption[j].ProductTypeID;
                Dr[3] = ctx.ProductType.Where(b => b.TypeID == TestingAssumption[j].ProductTypeID).Select(g => g.TypeName).FirstOrDefault();
                for (int i = 0; i < MMGeneralAssumption.Count; i++)
                {
                    var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.ProductTypeID == TestingAssumption[j].ProductTypeID && b.Forecastid == id && b.Parameterid == MMGeneralAssumption[i].Id).FirstOrDefault();

                    if (MMGeneralAssumptionvalue != null)
                        Dr[MMGeneralAssumption[i].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                    else
                        Dr[MMGeneralAssumption[i].VariableName] = 0;


                }
                DT.Rows.Add(Dr);

            }
            A = DT.Rows.Cast<DataRow>().ToArray();
            return A;

        }


        public DataTable GettestforecastAssumptionnewbytestId(int id, string param)
        {
            string[] param1 = param.TrimEnd(',').Split(",");

            DataTable dt1 = new DataTable();
            dt1.Columns.Add("Id");
            // DataTable DT = new DataTable();
            dt1.Columns.Add("Variable");
            dt1.Columns.Add("Value");
            var forecastinfo = ctx.ForecastInfo.Where(b => b.ForecastID == id).FirstOrDefault();
            var mmprogram = ctx.MMProgram.Where(b => b.Id == forecastinfo.ProgramId).FirstOrDefault();
            var MMGeneralAssumption1 = ctx.demographicMMGeneralAssumption.Where(b => b.Entity_type_id == 5).ToList();
            //   var forecasttestlist = ctx.ForecastTest.Where(b => b.forecastID == id && b.TestID==testID).FirstOrDefault();
            var testingprotocol = MMGeneralAssumption1.Where(c => c.VariableName.Contains("Mon") && c.Forecastid == forecastinfo.ForecastID).ToList();
            var mmgroup = ctx.DemographicMMGroup.Where(c => c.ProgramId == mmprogram.Id && c.Forecastid == forecastinfo.ForecastID && c.IsActive == true && c.Id == Convert.ToInt32(param1[1])).ToList();
            var testingprotocol1 = MMGeneralAssumption1.Where(c => !c.VariableName.Contains("Mon") && c.ProgramId == forecastinfo.ProgramId && c.Forecastid == forecastinfo.ForecastID).ToList();
            //   var TestingprotocolEnt = ctx.TestingProtocol.Where(b => b.ForecastinfoID == id && b.TestID == Convert.ToInt32(param1[0]) && b.PatientGroupID== Convert.ToInt32(param1[1])).ToList();

            var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.Forecastid == id).ToList();

            //if (TestingprotocolEnt.Count != 0)
            //{
            //for (int i = 0; i < TestingprotocolEnt.Count; i++)
            //{



            DataRow Dr = dt1.NewRow();

            int k = 0;
            switch (forecastinfo.months)
            {
                case 13:
                    for (int j = 0; j < 12; j++)
                    {
                        Dr = dt1.NewRow();
                        k = j + 1;
                        Dr["Id"] = testingprotocol[j].Id;
                        Dr["Variable"] = "M" + k;

                        // var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == TestingprotocolEnt[i].TestID && b.Forecastid == id && b.Parameterid == testingprotocol[j].Id && b.PatientGroupID == TestingprotocolEnt[i].PatientGroupID).FirstOrDefault();

                        if (MMGeneralAssumptionvalue != null)
                        {
                            if (MMGeneralAssumptionvalue.Where(b => b.TestID == Convert.ToInt32(param1[0]) && b.Parameterid == testingprotocol[j].Id && b.PatientGroupID == Convert.ToInt32(param1[1])).FirstOrDefault() != null)
                            {
                                Dr["Value"] = MMGeneralAssumptionvalue.Where(b => b.TestID == Convert.ToInt32(param1[0]) && b.Parameterid == testingprotocol[j].Id && b.PatientGroupID == Convert.ToInt32(param1[1])).FirstOrDefault().Parametervalue;
                            }

                            else
                                Dr["Value"] = 0;
                        }


                        else
                            Dr["Value"] = 0;


                        dt1.Rows.Add(Dr);
                    }
                    //Dr = dt1.NewRow();
                    //Dr["Id"] = 0;
                    //Dr["Variable"] = "TotalTestPerYear";
                    //Dr["Value"] = TestingprotocolEnt[i].TotalTestPerYear;
                    //dt1.Rows.Add(Dr);
                    break;
                case 12:
                    for (int j = 0; j < 12; j++)
                    {
                        Dr = dt1.NewRow();
                        k = j + 1;
                        Dr["Id"] = testingprotocol[j].Id;
                        Dr["Variable"] = "M" + k;

                        //    var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == TestingprotocolEnt[i].TestID && b.Forecastid == id && b.Parameterid == testingprotocol[j].Id && b.PatientGroupID == TestingprotocolEnt[i].PatientGroupID).FirstOrDefault();

                        if (MMGeneralAssumptionvalue != null)


                        {
                            if (MMGeneralAssumptionvalue.Where(b => b.TestID == Convert.ToInt32(param1[0]) && b.Parameterid == testingprotocol[j].Id && b.PatientGroupID == Convert.ToInt32(param1[1])).FirstOrDefault() != null)
                            {
                                Dr["Value"] = MMGeneralAssumptionvalue.Where(b => b.TestID == Convert.ToInt32(param1[0]) && b.Parameterid == testingprotocol[j].Id && b.PatientGroupID == Convert.ToInt32(param1[1])).FirstOrDefault().Parametervalue;
                            }
                            else
                                Dr["Value"] = 0;
                        }

                        else
                            Dr["Value"] = 0;
                        dt1.Rows.Add(Dr);
                    }
                    break;
                case 18:
                    for (int j = 0; j < 18; j++)
                    {
                        Dr = dt1.NewRow();
                        k = j + 1;
                        Dr["Id"] = testingprotocol[j].Id;
                        Dr["Variable"] = "M" + k;

                        // var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == TestingprotocolEnt[i].TestID && b.Forecastid == id && b.Parameterid == testingprotocol[j].Id && b.PatientGroupID == TestingprotocolEnt[i].PatientGroupID).FirstOrDefault();


                        if (MMGeneralAssumptionvalue != null)


                        {
                            if (MMGeneralAssumptionvalue.Where(b => b.TestID == Convert.ToInt32(param1[0]) && b.Parameterid == testingprotocol[j].Id && b.PatientGroupID == Convert.ToInt32(param1[1])).FirstOrDefault() != null)
                            {
                                Dr["Value"] = MMGeneralAssumptionvalue.Where(b => b.TestID == Convert.ToInt32(param1[0]) && b.Parameterid == testingprotocol[j].Id && b.PatientGroupID == Convert.ToInt32(param1[1])).FirstOrDefault().Parametervalue;
                            }
                            else
                                Dr["Value"] = 0;
                        }

                        else
                            Dr["Value"] = 0;
                        dt1.Rows.Add(Dr);
                    }
                    break;
                case 24:
                    for (int j = 0; j < 24; j++)
                    {
                        Dr["Id"] = testingprotocol[j].Id;
                        Dr = dt1.NewRow();
                        k = j + 1;
                        Dr["Variable"] = "M" + k;
                        //  var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == TestingprotocolEnt[i].TestID && b.Forecastid == id && b.Parameterid == testingprotocol[j].Id && b.PatientGroupID == TestingprotocolEnt[i].PatientGroupID).FirstOrDefault();


                        if (MMGeneralAssumptionvalue != null)


                        {
                            if (MMGeneralAssumptionvalue.Where(b => b.TestID == Convert.ToInt32(param1[0]) && b.Parameterid == testingprotocol[j].Id && b.PatientGroupID == Convert.ToInt32(param1[1])).FirstOrDefault() != null)
                            {
                                Dr["Value"] = MMGeneralAssumptionvalue.Where(b => b.TestID == Convert.ToInt32(param1[0]) && b.Parameterid == testingprotocol[j].Id && b.PatientGroupID == Convert.ToInt32(param1[1])).FirstOrDefault().Parametervalue;
                            }
                            else
                                Dr["Value"] = 0;
                        }

                        else
                            Dr["Value"] = 0;
                        dt1.Rows.Add(Dr);
                    }
                    break;
            }


            for (int ki = 0; ki < testingprotocol1.Count; ki++)
            {

                Dr = dt1.NewRow();
                Dr["Id"] = testingprotocol1[ki].Id;
                Dr["Variable"] = testingprotocol1[ki].VariableName;
                // var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == TestingprotocolEnt[i].TestID && b.Forecastid == id && b.Parameterid == testingprotocol1[ki].Id && b.PatientGroupID == TestingprotocolEnt[i].PatientGroupID).FirstOrDefault();

                if (MMGeneralAssumptionvalue != null)
                {
                    if(MMGeneralAssumptionvalue.Where(b => b.TestID == Convert.ToInt32(param1[0]) && b.Parameterid == testingprotocol1[ki].Id && b.PatientGroupID == Convert.ToInt32(param1[1])).FirstOrDefault() != null)
                    {
                        Dr["Value"] = MMGeneralAssumptionvalue.Where(b => b.TestID == Convert.ToInt32(param1[0]) && b.Parameterid == testingprotocol1[ki].Id && b.PatientGroupID == Convert.ToInt32(param1[1])).FirstOrDefault().Parametervalue;
                    }

                    else
                        Dr["Value"] = 0;
                }
               
                dt1.Rows.Add(Dr);
            }



            //}
            //else
            //{
            //    for (int i = 0; i < mmgroup.Count; i++)
            //    {
            //     //  k = 8;
            //        DataRow Dr = dt1.NewRow();
            //        //Dr["ID"] = 0;
            //        //Dr["TestID"] = t;
            //        //Dr["TestName"] = ctx.Test.Find(forecasttestlist[z].TestID).TestName;
            //        //Dr["PatientGroupID"] = mmgroup[i].Id;
            //        //Dr["PatientGroupName"] = mmgroup[i].GroupName;
            //        //Dr["ForecastinfoID"] = id;
            //        //Dr["PercentagePanel"] = 0;
            //        //Dr["Baseline"] = 0;

            //        int k = 0;
            //        switch (forecastinfo.months)
            //        {
            //            case 13:
            //                for (int j = 0; j < 12; j++)
            //                {

            //                    Dr = dt1.NewRow();
            //                    k = j + 1;
            //                    Dr["Id"] = testingprotocol[j].Id;
            //                    Dr["Variable"] = "M" + k;

            //                    Dr["Value"] = 0;
            //                    dt1.Rows.Add(Dr);
            //                }
            //                //Dr["Variable"] = "TotalTestPerYear" ;

            //                //Dr["Value"] = 0;

            //                break;
            //            case 12:
            //                for (int j = 0; j < 12; j++)
            //                {
            //                    Dr = dt1.NewRow();
            //                    k = j + 1;
            //                    Dr["Id"] = testingprotocol[j].Id;
            //                    Dr["Variable"] = "M" + k;

            //                    Dr["Value"] = 0;
            //                    dt1.Rows.Add(Dr);
            //                }
            //                break;
            //            case 18:
            //                for (int j = 0; j < 18; j++)
            //                {
            //                    Dr = dt1.NewRow();
            //                    k = j + 1;
            //                    Dr["Id"] = testingprotocol[j].Id;
            //                    Dr["Variable"] = "M" + k;

            //                    Dr["Value"] = 0;
            //                    dt1.Rows.Add(Dr);
            //                }
            //                break;
            //            case 24:
            //                for (int j = 0; j < 24; j++)
            //                {
            //                    Dr = dt1.NewRow();
            //                    k = j + 1;
            //                    Dr["Id"] = testingprotocol[j].Id;
            //                    Dr["Variable"] = "M" + k;

            //                    Dr["Value"] = 0;
            //                    dt1.Rows.Add(Dr);
            //                }
            //                break;
            //        }

            //        for (int ki = 0; ki < testingprotocol1.Count; ki++)
            //        {
            //            Dr = dt1.NewRow();
            //            Dr["Id"] = testingprotocol1[ki].Id;
            //            Dr["Variable"] = testingprotocol1[ki].VariableName;
            //            //var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == TestingprotocolEnt[i].TestID && b.Forecastid == id && b.Parameterid == testingprotocol1[ki].Id && b.PatientGroupID == TestingprotocolEnt[i].PatientGroupID).FirstOrDefault();

            //            //if (MMGeneralAssumptionvalue != null)
            //            //    Dr["Value"] = MMGeneralAssumptionvalue.Parametervalue;
            //            //else
            //                Dr["Value"] = 0;
            //            dt1.Rows.Add(Dr);
            //        }
            //        //if (mmprogram.NoofYear == 2)
            //        //{
            //        //    for (int j = 0; j < 24; j++)
            //        //    {
            //        //        Dr[k] = 0;
            //        //        k++;
            //        //    }
            //        //}
            //        //else
            //        //{
            //        //    for (int j = 0; j < 12; j++)
            //        //    {
            //        //        Dr[k] = 0;
            //        //        k++;
            //        //    }
            //        //}
            //        //Dr["TotalTestPerYear"] = 0;
            //        //k++;
            //        //for (int j = 0; j < testingprotocol1.Count; j++)
            //        //{
            //        //    Dr[k] = 0;
            //        //    k++;
            //        //}

            //    }
            //}

            return dt1;
        }

        public DataTable GettestforecastAssumptionnew(int id)
        {
            Array A;
            DataTable DT = new DataTable();
            int k;
            DT.Columns.Add("ID");
            DT.Columns.Add("TestID");
            DT.Columns.Add("TestName");
            DT.Columns.Add("PatientGroupID");
            DT.Columns.Add("PatientGroupName");

            DT.Columns.Add("ForecastinfoID");
            DT.Columns.Add("PercentagePanel");
            DT.Columns.Add("Baseline");
            DT.Columns.Add("TotalTestPerYear");
            var forecastinfo = ctx.ForecastInfo.Where(b => b.ForecastID == id).FirstOrDefault();
            var mmprogram = ctx.MMProgram.Where(b => b.Id == forecastinfo.ProgramId).FirstOrDefault();
            //   var MMGeneralAssumption1 = ctx.demographicMMGeneralAssumption.Where(b => b.Entity_type_id == 5).ToList();
            var forecasttestlist = ctx.ForecastTest.Where(b => b.forecastID == id).ToList();
            // var testingprotocol = MMGeneralAssumption1.Where(c => c.VariableName.Contains("Mon") && c.Forecastid == forecastinfo.ForecastID).ToList();
            var testlist = ctx.Test.ToList();
            var TestingprotocolEnt = ctx.TestingProtocol.Where(b => b.ForecastinfoID == id).ToList();
            //switch (forecastinfo.months)
            //{
            //    case 13:
            //        for (int i = 0; i < 12; i++)
            //        {
            //            DT.Columns.Add(testingprotocol[i].VariableName, typeof(Int32));
            //        }
            //        DT.Columns.Add("TotalTestPerYear", typeof(Int32));
            //        break;
            //    case 12:
            //        for (int i = 0; i < 12; i++)
            //        {
            //            DT.Columns.Add(testingprotocol[i].VariableName, typeof(Int32));
            //        }
            //        break;
            //    case 18:
            //        for (int i = 0; i < 18; i++)
            //        {
            //            DT.Columns.Add(testingprotocol[i].VariableName, typeof(Int32));
            //        }
            //        break;
            //    case 24:
            //        for (int i = 0; i < 24; i++)
            //        {
            //            DT.Columns.Add(testingprotocol[i].VariableName, typeof(Int32));
            //        }
            //        break;
            //}



            //var testingprotocol1 = MMGeneralAssumption1.Where(c => !c.VariableName.Contains("Mon") && c.ProgramId == forecastinfo.ProgramId && c.Forecastid == forecastinfo.ForecastID).ToList();
            //for (int i = 0; i < testingprotocol1.Count; i++)
            //{
            //    DT.Columns.Add(testingprotocol1[i].VariableName, typeof(Int32));
            //}
            var mmgroup = ctx.DemographicMMGroup.Where(c => c.ProgramId == mmprogram.Id && c.Forecastid == forecastinfo.ForecastID && c.IsActive == true).ToList();


            for (int z = 0; z < forecasttestlist.Count; z++)
            {
                var testname = testlist.Where(b => b.TestID == forecasttestlist[z].TestID).FirstOrDefault().TestName;
                var TestingprotocolEnt1 = TestingprotocolEnt.Where(b => b.TestID == forecasttestlist[z].TestID).ToList();
                if (TestingprotocolEnt1.Count != 0)
                {
                    for (int i = 0; i < TestingprotocolEnt1.Count; i++)
                    {



                        DataRow Dr = DT.NewRow();
                        Dr["ID"] = TestingprotocolEnt1[i].ID;
                        Dr["TestID"] = TestingprotocolEnt1[i].TestID;
                        Dr["TestName"] = testname;
                        Dr["PatientGroupID"] = TestingprotocolEnt1[i].PatientGroupID;
                        Dr["PatientGroupName"] = mmgroup.Where(d => d.Id == TestingprotocolEnt1[i].PatientGroupID).FirstOrDefault().GroupName;
                        Dr["ForecastinfoID"] = TestingprotocolEnt1[i].ForecastinfoID;
                        Dr["PercentagePanel"] = TestingprotocolEnt1[i].PercentagePanel;
                        Dr["Baseline"] = TestingprotocolEnt1[i].Baseline;
                        Dr["TotalTestPerYear"] = TestingprotocolEnt1[i].TotalTestPerYear;
                        //switch (forecastinfo.months)
                        //{
                        //    case 13:
                        //        for (int j = 0; j < 12; j++)
                        //        {
                        //            var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == TestingprotocolEnt[i].TestID && b.Forecastid == id && b.Parameterid == testingprotocol[j].Id && b.PatientGroupID == TestingprotocolEnt[i].PatientGroupID).FirstOrDefault();

                        //            if (MMGeneralAssumptionvalue != null)
                        //                Dr[testingprotocol[j].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                        //            else
                        //                Dr[testingprotocol[j].VariableName] = 0;
                        //        }
                        //        Dr["TotalTestPerYear"] = TestingprotocolEnt[i].TotalTestPerYear;
                        //        break;
                        //    case 12:
                        //        for (int j = 0; j < 12; j++)
                        //        {
                        //            var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == TestingprotocolEnt[i].TestID && b.Forecastid == id && b.Parameterid == testingprotocol[j].Id && b.PatientGroupID == TestingprotocolEnt[i].PatientGroupID).FirstOrDefault();

                        //            if (MMGeneralAssumptionvalue != null)
                        //                Dr[testingprotocol[j].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                        //            else
                        //                Dr[testingprotocol[j].VariableName] = 0;
                        //        }
                        //        break;
                        //    case 18:
                        //        for (int j = 0; j < 18; j++)
                        //        {
                        //            var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == TestingprotocolEnt[i].TestID && b.Forecastid == id && b.Parameterid == testingprotocol[j].Id && b.PatientGroupID == TestingprotocolEnt[i].PatientGroupID).FirstOrDefault();

                        //            if (MMGeneralAssumptionvalue != null)
                        //                Dr[testingprotocol[j].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                        //            else
                        //                Dr[testingprotocol[j].VariableName] = 0;
                        //        }
                        //        break;
                        //    case 24:
                        //        for (int j = 0; j < 24; j++)
                        //        {
                        //            var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == TestingprotocolEnt[i].TestID && b.Forecastid == id && b.Parameterid == testingprotocol[j].Id && b.PatientGroupID == TestingprotocolEnt[i].PatientGroupID).FirstOrDefault();

                        //            if (MMGeneralAssumptionvalue != null)
                        //                Dr[testingprotocol[j].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                        //            else
                        //                Dr[testingprotocol[j].VariableName] = 0;
                        //        }
                        //        break;
                        //}


                        //for (int ki = 0; ki < testingprotocol1.Count; ki++)
                        //{

                        //    var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.TestID == TestingprotocolEnt[i].TestID && b.Forecastid == id && b.Parameterid == testingprotocol1[ki].Id && b.PatientGroupID == TestingprotocolEnt[i].PatientGroupID).FirstOrDefault();

                        //    if (MMGeneralAssumptionvalue != null)
                        //        Dr[testingprotocol1[ki].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                        //    else
                        //        Dr[testingprotocol1[ki].VariableName] = 0;

                        //}
                        DT.Rows.Add(Dr);
                    }


                }
                else
                {
                    for (int i = 0; i < mmgroup.Count; i++)
                    {
                        k = 8;
                        DataRow Dr = DT.NewRow();
                        Dr["ID"] = 0;
                        Dr["TestID"] = forecasttestlist[z].TestID;
                        Dr["TestName"] = testname;
                        Dr["PatientGroupID"] = mmgroup[i].Id;
                        Dr["PatientGroupName"] = mmgroup[i].GroupName;
                        Dr["ForecastinfoID"] = id;
                        Dr["PercentagePanel"] = 0;
                        Dr["Baseline"] = 0;
                        Dr["TotalTestPerYear"] = 0;

                        //switch (forecastinfo.months)
                        //{
                        //    case 13:
                        //        for (int j = 0; j < 12; j++)
                        //        {
                        //            Dr[k] = 0;
                        //            k++;
                        //        }
                        //        Dr["TotalTestPerYear"] = 0;
                        //        k++;
                        //        break;
                        //    case 12:
                        //        for (int j = 0; j < 12; j++)
                        //        {
                        //            Dr[k] = 0;
                        //            k++;
                        //        }
                        //        break;
                        //    case 18:
                        //        for (int j = 0; j < 18; j++)
                        //        {
                        //            Dr[k] = 0;
                        //            k++;
                        //        }
                        //        break;
                        //    case 24:
                        //        for (int j = 0; j < 24; j++)
                        //        {
                        //            Dr[k] = 0;
                        //            k++;
                        //        }
                        //        break;
                        //}


                        //if (mmprogram.NoofYear == 2)
                        //{
                        //    for (int j = 0; j < 24; j++)
                        //    {
                        //        Dr[k] = 0;
                        //        k++;
                        //    }
                        //}
                        //else
                        //{
                        //    for (int j = 0; j < 12; j++)
                        //    {
                        //        Dr[k] = 0;
                        //        k++;
                        //    }
                        //}
                        //Dr["TotalTestPerYear"] = 0;
                        //k++;
                        //for (int j = 0; j < testingprotocol1.Count; j++)
                        //{
                        //    Dr[k] = 0;
                        //    k++;
                        //}
                        DT.Rows.Add(Dr);
                    }
                }


            }
            //}
            A = DT.Rows.Cast<DataRow>().ToArray();
            return DT;
        }

        public Array GetforecastproductAssumption(int id)
        {
            Array A;
            var forecastinfo = ctx.ForecastInfo.Where(b => b.ForecastID == id).FirstOrDefault();
            var MMGeneralAssumption = ctx.demographicMMGeneralAssumption.Where(b => b.Forecastid == id && b.Entity_type_id == 4).ToList();

            DataTable DT = new DataTable();
            var TestingAssumption = ctx.TestingAssumption.Where(b => b.ForecastinfoID == id).ToList();

            DT.Columns.Add("ID");
            DT.Columns.Add("ForecastinfoID");
            DT.Columns.Add("ProductTypeID");
            DT.Columns.Add("ProductTypeName");
            for (int i = 0; i < MMGeneralAssumption.Count; i++)
            {

                DT.Columns.Add(MMGeneralAssumption[i].VariableName);

            }
            for (int j = 0; j < TestingAssumption.Count; j++)
            {
                DataRow Dr = DT.NewRow();
                Dr[0] = TestingAssumption[j].ID;
                Dr[1] = TestingAssumption[j].ForecastinfoID;


                Dr[2] = TestingAssumption[j].ProductTypeID;
                Dr[3] = ctx.ProductType.Where(b => b.TypeID == TestingAssumption[j].ProductTypeID).Select(g => g.TypeName).FirstOrDefault();
                for (int i = 0; i < MMGeneralAssumption.Count; i++)
                {
                    var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.ProductTypeID == TestingAssumption[j].ProductTypeID && b.Forecastid == id && b.Parameterid == MMGeneralAssumption[i].Id).FirstOrDefault();

                    if (MMGeneralAssumptionvalue != null)
                        Dr[MMGeneralAssumption[i].VariableName] = MMGeneralAssumptionvalue.Parametervalue;
                    else
                        Dr[MMGeneralAssumption[i].VariableName] = 0;


                }
                DT.Rows.Add(Dr);

            }
            A = DT.Rows.Cast<DataRow>().ToArray();
            return A;

        }

        public DataTable GetpatientAssumption(int id)
        {
            Array A;
            var forecastinfo = ctx.ForecastInfo.Where(b => b.ForecastID == id).FirstOrDefault();
            var MMGeneralAssumption = ctx.MMGeneralAssumption.Where(b => b.ProgramId == forecastinfo.ProgramId && b.Entity_type_id == 3).ToList();

            DataTable DT = new DataTable();
            var patientass = ctx.PatientAssumption.Where(b => b.ForecastinfoID == id).ToList();

            DT.Columns.Add("ID");
            DT.Columns.Add("ForecastinfoID");
            DT.Columns.Add("SiteCategoryID");
            DT.Columns.Add("sitecategoryname");
            for (int i = 0; i < MMGeneralAssumption.Count; i++)
            {

                DT.Columns.Add(MMGeneralAssumption[i].VariableName.ToLower());

            }


            for (int j = 0; j < patientass.Count; j++)
            {
                DataRow Dr = DT.NewRow();
                Dr[0] = patientass[j].ID;
                Dr[1] = patientass[j].ForecastinfoID;

                if (forecastinfo.ForecastType == "S")
                {
                    Dr[2] = patientass[j].SiteID;
                    Dr[3] = ctx.Site.Where(b => b.SiteID == patientass[j].SiteID).Select(g => g.SiteName).FirstOrDefault();
                    for (int i = 0; i < MMGeneralAssumption.Count; i++)
                    {
                        var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.SiteID == patientass[j].SiteID && b.Forecastid == id && b.Parameterid == MMGeneralAssumption[i].Id).FirstOrDefault();
                        if (MMGeneralAssumptionvalue != null)
                            Dr[MMGeneralAssumption[i].VariableName.ToLower()] = MMGeneralAssumptionvalue.Parametervalue;
                        else
                            Dr[MMGeneralAssumption[i].VariableName.ToLower()] = 0;
                    }
                    DT.Rows.Add(Dr);
                }
                else
                {
                    Dr[2] = patientass[j].CategoryID;
                    Dr[3] = ctx.SiteCategory.Where(b => b.CategoryID == patientass[j].CategoryID).Select(g => g.CategoryName).FirstOrDefault();
                    for (int i = 0; i < MMGeneralAssumption.Count; i++)
                    {
                        var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.CategoryID == patientass[j].CategoryID && b.Forecastid == id && b.Parameterid == MMGeneralAssumption[i].Id).FirstOrDefault();
                        if (MMGeneralAssumptionvalue != null)
                            Dr[MMGeneralAssumption[i].VariableName.ToLower()] = MMGeneralAssumptionvalue.Parametervalue;
                        else
                            Dr[MMGeneralAssumption[i].VariableName.ToLower()] = 0;

                    }
                    DT.Rows.Add(Dr);
                }
            }
            //  A = DT.Rows.Cast<DataRow>().ToArray();





            if (forecastinfo.ForecastType == "S")
            {
                var forecastsiteinfo = ctx.ForecastSiteInfo.Where(b => b.ForecastinfoID == id).ToList();
                for (int j = 0; j < forecastsiteinfo.Count; j++)
                {
                    if (DT.Rows.Count > 0)
                    {
                        DataRow[] filteredRows = DT.Select("SiteCategoryID = '" + forecastsiteinfo[j].SiteID + "'");
                        if (filteredRows.Length != 0)
                        {

                        }
                        else
                        {
                            DataRow Dr = DT.NewRow();
                            Dr[0] = 0;
                            Dr[1] = id;
                            Dr[2] = forecastsiteinfo[j].SiteID;
                            Dr[3] = ctx.Site.Where(b => b.SiteID == forecastsiteinfo[j].SiteID).Select(g => g.SiteName).FirstOrDefault();
                            for (int i = 0; i < MMGeneralAssumption.Count; i++)
                            {

                                Dr[MMGeneralAssumption[i].VariableName.ToLower()] = 0;

                            }
                            DT.Rows.Add(Dr);
                        }
                    }
                    else
                    {
                        DataRow Dr = DT.NewRow();
                        Dr[0] = 0;
                        Dr[1] = id;
                        Dr[2] = forecastsiteinfo[j].SiteID;
                        Dr[3] = ctx.Site.Where(b => b.SiteID == forecastsiteinfo[j].SiteID).Select(g => g.SiteName).FirstOrDefault();
                        for (int i = 0; i < MMGeneralAssumption.Count; i++)
                        {

                            Dr[MMGeneralAssumption[i].VariableName.ToLower()] = 0;

                        }
                        DT.Rows.Add(Dr);
                    }
                }
                // A = DT.Rows.Cast<DataRow>().ToArray();
            }
            else
            {
                var forecastcategoryinfo = ctx.ForecastCategoryInfo.Where(b => b.ForecastinfoID == id).ToList();
                for (int j = 0; j < forecastcategoryinfo.Count; j++)
                {
                    if (DT.Rows.Count > 0)
                    {
                        DataRow[] filteredRows = DT.Select("SiteCategoryID = '" + forecastcategoryinfo[j].SiteCategoryId + "'");
                        if (filteredRows.Length != 0)
                        {

                        }
                        else
                        {
                            DataRow Dr = DT.NewRow();
                            Dr[0] = 0;
                            Dr[1] = id;
                            Dr[2] = forecastcategoryinfo[j].SiteCategoryId;
                            Dr[3] = ctx.SiteCategory.Where(b => b.CategoryID == forecastcategoryinfo[j].SiteCategoryId).Select(g => g.CategoryName).FirstOrDefault();
                            for (int i = 0; i < MMGeneralAssumption.Count; i++)
                            {
                                Dr[MMGeneralAssumption[i].VariableName.ToLower()] = 0;


                            }
                            DT.Rows.Add(Dr);
                        }
                    }
                    else
                    {
                        DataRow Dr = DT.NewRow();
                        Dr[0] = 0;
                        Dr[1] = id;
                        Dr[2] = forecastcategoryinfo[j].SiteCategoryId;
                        Dr[3] = ctx.SiteCategory.Where(b => b.CategoryID == forecastcategoryinfo[j].SiteCategoryId).Select(g => g.CategoryName).FirstOrDefault();
                        for (int i = 0; i < MMGeneralAssumption.Count; i++)
                        {
                            Dr[MMGeneralAssumption[i].VariableName.ToLower()] = 0;


                        }
                        DT.Rows.Add(Dr);
                    }
                }
                //  A = DT.Rows.Cast<DataRow>().ToArray();
            }

            return DT;
        }




        public DataTable GetforecastpatientAssumption(int id)
        {
            Array A;
            var forecastinfo = ctx.ForecastInfo.Where(b => b.ForecastID == id).FirstOrDefault();
            var MMGeneralAssumption = ctx.demographicMMGeneralAssumption.Where(b => b.Forecastid == id && b.Entity_type_id == 3).ToList();

            DataTable DT = new DataTable();
            var patientass = ctx.PatientAssumption.Where(b => b.ForecastinfoID == id).ToList();

            DT.Columns.Add("ID");
            DT.Columns.Add("ForecastinfoID");
            DT.Columns.Add("SiteCategoryID");
            DT.Columns.Add("sitecategoryname");
            for (int i = 0; i < MMGeneralAssumption.Count; i++)
            {

                DT.Columns.Add(MMGeneralAssumption[i].VariableName.ToLower());

            }


            for (int j = 0; j < patientass.Count; j++)
            {
                DataRow Dr = DT.NewRow();
                Dr[0] = patientass[j].ID;
                Dr[1] = patientass[j].ForecastinfoID;

                //if (forecastinfo.ForecastType == "S")
                //{
                //    Dr[2] = patientass[j].SiteID;
                //    Dr[3] = ctx.SiteCategory.Where(b => b.CategoryID == patientass[j].SiteID).Select(g => g.CategoryName).FirstOrDefault();
                //    for (int i = 0; i < MMGeneralAssumption.Count; i++)
                //    {
                //        var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.SiteID == patientass[j].SiteID && b.Forecastid == id && b.Parameterid == MMGeneralAssumption[i].Id).FirstOrDefault();
                //        if (MMGeneralAssumptionvalue != null)
                //            Dr[MMGeneralAssumption[i].VariableName.ToLower()] = MMGeneralAssumptionvalue.Parametervalue;
                //        else
                //            Dr[MMGeneralAssumption[i].VariableName.ToLower()] = 0;
                //    }
                //    DT.Rows.Add(Dr);
                //}
                //else
                //{
                    Dr[2] = patientass[j].CategoryID;
                    Dr[3] = ctx.SiteCategory.Where(b => b.CategoryID == patientass[j].CategoryID).Select(g => g.CategoryName).FirstOrDefault();
                    for (int i = 0; i < MMGeneralAssumption.Count; i++)
                    {
                        var MMGeneralAssumptionvalue = ctx.MMGeneralAssumptionValue.Where(b => b.CategoryID == patientass[j].CategoryID && b.Forecastid == id && b.Parameterid == MMGeneralAssumption[i].Id).FirstOrDefault();
                        if (MMGeneralAssumptionvalue != null)
                            Dr[MMGeneralAssumption[i].VariableName.ToLower()] = MMGeneralAssumptionvalue.Parametervalue;
                        else
                            Dr[MMGeneralAssumption[i].VariableName.ToLower()] = 0;

                    }
                    DT.Rows.Add(Dr);
                }
            //}
            //  A = DT.Rows.Cast<DataRow>().ToArray();





            if (forecastinfo.ForecastType == "S")
            {
              //  var forecastsiteinfo = ctx.ForecastSiteInfo.Where(b => b.ForecastinfoID == id).ToList();
                var siteids= ctx.ForecastSiteInfo.Where(b => b.ForecastinfoID == id).Select(s=>s.SiteID).ToArray();
                var sitecategoryid = ctx.Site.Where(b => siteids.Contains(b.SiteID)).Select(s => s.CategoryID).ToList();
                var sitecategorynames = ctx.SiteCategory.Where(b => sitecategoryid.Contains(b.CategoryID)).Select(s => new { s.CategoryName, s.CategoryID }).ToList();
                for (int j = 0; j < sitecategoryid.Count; j++)
                {
                    if (DT.Rows.Count > 0)
                    {
                        DataRow[] filteredRows = DT.Select("SiteCategoryID = '" + sitecategoryid[j] + "'");
                        if (filteredRows.Length != 0)
                        {

                        }
                        else
                        {
                            DataRow Dr = DT.NewRow();
                            Dr[0] = 0;
                            Dr[1] = id;
                            Dr[2] = sitecategoryid[j];
                            Dr[3] = sitecategorynames.Where(b => b.CategoryID == sitecategoryid[j]).Select(g => g.CategoryName).FirstOrDefault();
                            for (int i = 0; i < MMGeneralAssumption.Count; i++)
                            {

                                Dr[MMGeneralAssumption[i].VariableName.ToLower()] = 0;

                            }
                            DT.Rows.Add(Dr);
                        }
                    }
                    else
                    {
                        DataRow Dr = DT.NewRow();
                        Dr[0] = 0;
                        Dr[1] = id;
                        Dr[2] = sitecategoryid[j];
                        Dr[3] = sitecategorynames.Where(b => b.CategoryID == sitecategoryid[j]).Select(g => g.CategoryName).FirstOrDefault();
                        for (int i = 0; i < MMGeneralAssumption.Count; i++)
                        {

                            Dr[MMGeneralAssumption[i].VariableName.ToLower()] = 0;

                        }
                        DT.Rows.Add(Dr);
                    }
                }
                // A = DT.Rows.Cast<DataRow>().ToArray();
            }
            else
            {
                var forecastcategoryinfo = ctx.ForecastCategoryInfo.Where(b => b.ForecastinfoID == id).ToList();
                for (int j = 0; j < forecastcategoryinfo.Count; j++)
                {
                    if (DT.Rows.Count > 0)
                    {
                        DataRow[] filteredRows = DT.Select("SiteCategoryID = '" + forecastcategoryinfo[j].SiteCategoryId + "'");
                        if (filteredRows.Length != 0)
                        {

                        }
                        else
                        {
                            DataRow Dr = DT.NewRow();
                            Dr[0] = 0;
                            Dr[1] = id;
                            Dr[2] = forecastcategoryinfo[j].SiteCategoryId;
                            Dr[3] = ctx.SiteCategory.Where(b => b.CategoryID == forecastcategoryinfo[j].SiteCategoryId).Select(g => g.CategoryName).FirstOrDefault();
                            for (int i = 0; i < MMGeneralAssumption.Count; i++)
                            {
                                Dr[MMGeneralAssumption[i].VariableName.ToLower()] = 0;


                            }
                            DT.Rows.Add(Dr);
                        }
                    }
                    else
                    {
                        DataRow Dr = DT.NewRow();
                        Dr[0] = 0;
                        Dr[1] = id;
                        Dr[2] = forecastcategoryinfo[j].SiteCategoryId;
                        Dr[3] = ctx.SiteCategory.Where(b => b.CategoryID == forecastcategoryinfo[j].SiteCategoryId).Select(g => g.CategoryName).FirstOrDefault();
                        for (int i = 0; i < MMGeneralAssumption.Count; i++)
                        {
                            Dr[MMGeneralAssumption[i].VariableName.ToLower()] = 0;


                        }
                        DT.Rows.Add(Dr);
                    }
                }
                //  A = DT.Rows.Cast<DataRow>().ToArray();
            }

            return DT;
        }
        public IList<Dynamiccontrol> GetlinearDynamiccontrol(int id)
        {
            DataTable DT = new DataTable();
            var forecastinfo = ctx.ForecastInfo.Where(b => b.ForecastID == id).FirstOrDefault();
            int i = 0;
            DateTime StartDate1 = DateTime.Parse(forecastinfo.StartDate.ToShortDateString());
            List<Dynamiccontrol> Db = new List<Dynamiccontrol>();
            DateTime EndDate1 = DateTime.Parse(forecastinfo.ForecastDate.Value.ToShortDateString());
            // StartDate1 = StartDate1.AddMonths(-1);
            int reportingPeriod = 0;
            if (forecastinfo.Period == "Bimonthly")
            {
                reportingPeriod = 2;
            }
            else if (forecastinfo.Period == "Monthly")
            {
                reportingPeriod = 1;
            }
            else if (forecastinfo.Period == "Quarterly")
            {
                reportingPeriod = 4;
            }
            else if (forecastinfo.Period == "Yearly")
            {
                reportingPeriod = 12;
            }

            DT.Columns.Add("sitecategoryname");
            Lstmonthname = MonthsBetween(StartDate1, EndDate1);
            while (i < Lstmonthname.Count)
            {
                DT.Columns.Add(Lstmonthname[i]);
                i = i + reportingPeriod;
            }




            for (int j = 0; j < DT.Columns.Count; j++)
            {
                Db.Add(new Dynamiccontrol
                {
                    name = Char.ToLowerInvariant(DT.Columns[j].Caption[0]) + DT.Columns[j].Caption.Substring(1),
                    type = "text"
                });

            }

            return Db;
        }
        public Array Getlineargrowth(int id)
        {
            Array lineargrowth;
            DataTable dtgrowth = new DataTable();

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
            lineargrowth = dtgrowth.Rows.Cast<DataRow>().ToArray();

            return lineargrowth;
        }
        private DataTable DisplayData(ForecastInfo b, IEnumerable<MMGeneralAssumption> f)
        {
            DataTable dt = new DataTable();
            DateTime StartDate1 = DateTime.Parse(b.StartDate.ToShortDateString());
            DateTime EndDate1 = DateTime.Parse(b.ForecastDate.Value.ToShortDateString());
            DateTime month = new DateTime(StartDate1.Year, StartDate1.Month, 1); // StartDate1.Month;
                                                                                 //   StartDate1.Month = month.AddMonths(-1);
                                                                                 //  StartDate1.Month =StartDate1.AddMonths(StartDate1.Month
                                                                                 //  StartDate1 = StartDate1.AddMonths(-1);


            int newpatient = 0;
            int totalnewpatient = 0;
            int targetpatient = 0;
            int totaltargetpatient = 0;
            int numberofslot = 0;
            int HIVpositivewithoutfollowing = 0;
            int totalcurrentpatient = 0;
            int totalmonth = GetMonthDifference(StartDate1, EndDate1);
            Lstmonthname = MonthsBetween(StartDate1, EndDate1);
            int aAmount = 0;
            int fPatient = 0;
            int tPatient = 0;

            DataTable dtpatient = new DataTable();
            dtpatient.Columns.Add("SiteCategoryID");
            dtpatient.Columns.Add("sitecategoryname");
            dtpatient.Columns.Add("CurrentPatient");
            dtpatient.Columns.Add("TargetPatient");
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
            if (b.ForecastType == "S")
            {
                var getpatient = ctx.ForecastSiteInfo.Where(c => c.ForecastinfoID == b.ForecastID).ToList();
                for (int i = 0; i < getpatient.Count; i++)
                {
                    DataRow Dr = dtpatient.NewRow();
                    Dr["SiteCategoryID"] = getpatient[i].SiteID;
                    Dr["sitecategoryname"] = ctx.Site.Where(e => e.SiteID == getpatient[i].SiteID).Select(g => g.SiteName).FirstOrDefault();
                    Dr["CurrentPatient"] = getpatient[i].CurrentPatient;
                    Dr["TargetPatient"] = getpatient[i].TargetPatient;
                    dtpatient.Rows.Add(Dr);
                }

            }
            else
            {
                var getpatient = ctx.ForecastCategoryInfo.Where(c => c.ForecastinfoID == b.ForecastID).ToList();
                for (int i = 0; i < getpatient.Count; i++)
                {
                    DataRow Dr = dtpatient.NewRow();
                    Dr["SiteCategoryID"] = getpatient[i].SiteCategoryId;
                    Dr["sitecategoryname"] = ctx.SiteCategory.Where(e => e.CategoryID == getpatient[i].SiteCategoryId).Select(g => g.CategoryName).FirstOrDefault();
                    Dr["CurrentPatient"] = getpatient[i].CurrentPatient;
                    Dr["TargetPatient"] = getpatient[i].TargetPatient;
                    dtpatient.Rows.Add(Dr);
                }
            }
            string[] arrids;
            int res = 0;
            arrids = varids.Trim(',').Split(',');
            for (int i = 0; i < dtpatient.Rows.Count; i++)
            {
                newpatient = Convert.ToInt32(dtpatient.Rows[i]["TargetPatient"].ToString()) - Convert.ToInt32(dtpatient.Rows[i]["CurrentPatient"].ToString());
                if (b.ForecastType == "S")
                {
                    DataRow Dr = dtValue.NewRow();
                    foreach (MMGeneralAssumption gAssumption in f)
                    {

                        var mmgeneralassumptionvalue = ctx.MMGeneralAssumptionValue.Where(c => c.SiteID == Convert.ToInt32(dtpatient.Rows[i]["SiteCategoryID"].ToString()) && c.Forecastid == b.ForecastID && c.Parameterid == gAssumption.Id).FirstOrDefault();
                        foreach (DataColumn dc in dtValue.Columns)
                        {
                            if (dc.Caption == gAssumption.VariableName)
                            {
                                if (mmgeneralassumptionvalue != null)
                                    Dr[gAssumption.VariableName] = mmgeneralassumptionvalue.Parametervalue;
                                else
                                    Dr[gAssumption.VariableName] = 0;
                            }
                        }
                    }
                    dtValue.Rows.Add(Dr);

                }
                else
                {
                    DataRow Dr = dtValue.NewRow();
                    foreach (MMGeneralAssumption gAssumption in f)
                    {

                        var mmgeneralassumptionvalue = ctx.MMGeneralAssumptionValue.Where(c => c.CategoryID == Convert.ToInt32(dtpatient.Rows[i]["SiteCategoryID"].ToString()) && c.Forecastid == b.ForecastID && c.Parameterid == gAssumption.Id).FirstOrDefault();
                        foreach (DataColumn dc in dtValue.Columns)
                        {
                            if (dc.Caption == gAssumption.VariableName)
                            {
                                if (mmgeneralassumptionvalue != null)
                                    Dr[gAssumption.VariableName] = mmgeneralassumptionvalue.Parametervalue;
                                else
                                    Dr[gAssumption.VariableName] = 0;
                            }
                        }
                    }
                    dtValue.Rows.Add(Dr);
                }



                //newpatient =newpatient + (fPatient);

                //newpatient = newpatient - ((newpatient * HIVpositivewithoutfollowing) / 100);
                targetpatient = newpatient + Convert.ToInt32(dtpatient.Rows[i]["CurrentPatient"].ToString());

                totaltargetpatient = targetpatient;
                totalcurrentpatient = Convert.ToInt32(dtpatient.Rows[i]["CurrentPatient"].ToString());
                // calculatelineargrowth(Convert.ToInt32(Dt1.Rows[i]["ID"].ToString()), Dt1.Rows[i]["Name"].ToString(), Convert.ToInt32(Dt1.Rows[i]["CurrentPatient"].ToString()), Convert.ToInt32(Dt1.Rows[i]["TargetPatient"].ToString()), numberofslot,i);
                //   SiteCategoryID

                int count = numberofslot;
                int slotvalue = 0;
                int colindex = 4;

                int diff = Convert.ToInt32(totaltargetpatient - totalcurrentpatient);
                List<string> list1 = new List<string>();
                string List = "";
                if (i == 0)
                {
                    //dt.Columns.Add("Id");
                    dt.Columns.Add("ForecastId");
                    dt.Columns.Add("SiteCategoryId");
                    dt.Columns.Add("sitecategoryname");
                    dt.Columns.Add("Currentpatient");
                    dt.Columns.Add("Targetpatient");
                    // dt.Columns.Add("Name");

                }
                DataRow dr = dt.NewRow();

                // dr["Id"] = Convert.ToString(ID);
                dr["ForecastId"] = Convert.ToString(b.ForecastID);
                dr["sitecategoryname"] = dtpatient.Rows[i]["sitecategoryname"].ToString();
                dr["SiteCategoryId"] = Convert.ToString(Convert.ToInt32(dtpatient.Rows[i]["SiteCategoryID"]));
                dr["Currentpatient"] = Convert.ToString(totalcurrentpatient);
                dr["Targetpatient"] = Convert.ToString(targetpatient);
                //dr["Name"] = sitename;


                List += "," + Convert.ToString(totalcurrentpatient);
                while (count > 1)
                {
                    slotvalue = totalcurrentpatient + (diff / numberofslot);
                    List += "," + Convert.ToString(slotvalue);
                    //  list1.Add(Convert.ToString(slotvalue));               
                    count--;
                    colindex++;
                    totalcurrentpatient = slotvalue;
                }
                List += "," + Convert.ToString(targetpatient);
                List = List.TrimStart(',');
                string[] gridvalue = List.Split(',');
                int j = 0; int i1 = 0;

                while (i1 < Lstmonthname.Count)
                {
                    if (i == 0)
                        dt.Columns.Add(Lstmonthname[i1].ToString());
                    dr[Lstmonthname[i1].ToString()] = gridvalue[j].ToString();

                    i1 = i1 + reportingPeriod;
                    j++;
                }
                dt.AcceptChanges();
                dt.Rows.Add(dr);



            }

            return dt;
        }
        private DataRow calculatelineargrowth(double currentpatient, double targetpatient, int numberslot, int k, int reportingPeriod, int forecastId, int sitecategoryid)//int ID,string sitename,
        {
            int count = numberslot;
            double slotvalue = 0;
            int colindex = 4;
            DataTable dt = new DataTable();
            double diff = Convert.ToDouble(targetpatient - currentpatient);
            List<string> list1 = new List<string>();
            string List = "";
            if (k == 0)
            {
                //dt.Columns.Add("Id");
                dt.Columns.Add("ForecastId");
                dt.Columns.Add("SiteCategoryId");
                dt.Columns.Add("Currentpatient");
                dt.Columns.Add("Targetpatient");
                // dt.Columns.Add("Name");

            }
            DataRow dr = dt.NewRow();

            // dr["Id"] = Convert.ToString(ID);
            dr["ForecastId"] = Convert.ToString(forecastId);
            dr["SiteCategoryId"] = Convert.ToString(sitecategoryid);
            dr["Currentpatient"] = Convert.ToString(currentpatient);
            dr["Targetpatient"] = Convert.ToString(targetpatient);
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
                if (k == 0)
                    dt.Columns.Add(Lstmonthname[i].ToString());
                dr[Lstmonthname[i].ToString()] = gridvalue[j].ToString();

                i = i + reportingPeriod;
                j++;
            }
            dt.AcceptChanges();
            dt.Rows.Add(dr);

            return dr;

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


        public int deletetestingprotocol(int id, string param)
        {
            int res = 0;
            string[] param1 = param.TrimEnd(',').Split(",");
            var testingprotocal = ctx.TestingProtocol.Where(b => b.TestID == id && b.ForecastinfoID == Convert.ToInt32(param1[0]) && b.PatientGroupID == Convert.ToInt32(param1[1])).FirstOrDefault();
            ctx.TestingProtocol.Remove(testingprotocal);
            ctx.SaveChanges();


            var generalassumptionvlaue = ctx.MMGeneralAssumptionValue.Where(b => b.Forecastid == Convert.ToInt32(param1[0]) && b.TestID == id).ToList();
            ctx.MMGeneralAssumptionValue.RemoveRange(generalassumptionvlaue);
            ctx.SaveChanges();



            return res;
        }

    }

}

