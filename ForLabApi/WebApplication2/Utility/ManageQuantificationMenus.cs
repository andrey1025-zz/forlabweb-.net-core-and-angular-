using ForLabApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Utility
{
    public class ManageQuantificationMenus
    {

        ForLabContext ctx;
        public ManageQuantificationMenus(ForLabContext c)
        {
            ctx = c;
            //return ctx;
        }
        public  string BuildTestName(string insname, ClassOfMorbidityTestEnum classOfTest)
        {
            string tname = "";
            switch (classOfTest)
            {
                case ClassOfMorbidityTestEnum.CD4:
                    tname = String.Format("CD4 Tests-{0}", insname);
                    break;
                case ClassOfMorbidityTestEnum.Chemistry:
                    tname = String.Format("Total patient samples run on {0}", insname);
                    break;
                case ClassOfMorbidityTestEnum.Hematology:
                    tname = String.Format("Hematology Tests-{0}", insname);
                    break;
                case ClassOfMorbidityTestEnum.OtherTest:
                    tname = String.Format("Total {0}", insname);
                    break;
                case ClassOfMorbidityTestEnum.ViralLoad:
                    tname = String.Format("Viral Load Tests-{0}", insname);
                    break;
            }
            return tname;
        }

        public  void CreateQuantifyMenus(MorbidityTest morbidityTest)
        {


            var instrumentname = ctx.Instrument.Where(b => b.InstrumentID == morbidityTest.InstrumentId).Select(c => new
            {
                c.InstrumentName
            }).FirstOrDefault().ToString();

            ClassOfMorbidityTestEnum ce=(ClassOfMorbidityTestEnum)Enum.Parse(typeof(ClassOfMorbidityTestEnum), morbidityTest.ClassOfTest, true);
            QuantifyMenu qmenuTest = CreateNewQMenu(morbidityTest);
            qmenuTest.Title = BuildTestName(instrumentname, ce);
            if (ce != ClassOfMorbidityTestEnum.Chemistry)
            {
                qmenuTest.TestType = TestTypeEnum.Test.ToString();
                qmenuTest.MorbidityTetsId = morbidityTest.Id;
                ctx.QuantifyMenu.Add(qmenuTest);
                ctx.SaveChanges();
            }
            else
            {
                qmenuTest.TestType = TestTypeEnum.SamplesRunOn.ToString();
                qmenuTest.MorbidityTetsId = morbidityTest.Id;
                CreateChemistryTestQM(morbidityTest);
            }

            QuantifyMenu qmenuCon = CreateNewQMenu(morbidityTest);
            qmenuCon.Title = String.Format("{0} Controls - {1}", morbidityTest.ClassOfTest, instrumentname);
            qmenuCon.TestType = TestTypeEnum.ControlTest.ToString();
            qmenuCon.Duration = TestingDurationEnum.TotalControl.ToString();
            qmenuCon.MorbidityTetsId = morbidityTest.Id;
            ctx.QuantifyMenu.Add(qmenuCon);
            ctx.SaveChanges();

            QuantifyMenu qmenuPertest = CreateNewQMenu(morbidityTest);
            qmenuPertest.Title = String.Format("{0} Controls Per Test-{1}", morbidityTest.ClassOfTest, instrumentname);
            qmenuPertest.TestType = TestTypeEnum.ControlTest.ToString();
            qmenuPertest.Duration = TestingDurationEnum.PerTest.ToString();
            qmenuPertest.MorbidityTetsId = morbidityTest.Id;
            ctx.QuantifyMenu.Add(qmenuPertest);
            ctx.SaveChanges();

            QuantifyMenu qmenuDaily = CreateNewQMenu(morbidityTest);
            qmenuDaily.Title = String.Format("{0} Daily Controls-{1}", GetTestShortName(ce), instrumentname);
            qmenuDaily.TestType = TestTypeEnum.ControlTest.ToString();
            qmenuDaily.Duration = TestingDurationEnum.Daily.ToString();
            qmenuDaily.MorbidityTetsId = morbidityTest.Id;
            ctx.QuantifyMenu.Add(qmenuDaily);
            ctx.SaveChanges();

            QuantifyMenu qmenuWeekly = CreateNewQMenu(morbidityTest);
            qmenuWeekly.Title = String.Format("{0} Weekly Controls-{1}", GetTestShortName(ce), instrumentname);
            qmenuWeekly.TestType = TestTypeEnum.ControlTest.ToString();
            qmenuWeekly.Duration = TestingDurationEnum.Weekly.ToString();
            qmenuWeekly.MorbidityTetsId = morbidityTest.Id;
            ctx.QuantifyMenu.Add(qmenuWeekly);
            ctx.SaveChanges();

            QuantifyMenu qmenuMonth = CreateNewQMenu(morbidityTest);
            qmenuMonth.Title = String.Format("{0} Monthly Controls-{1}", GetTestShortName(ce), instrumentname);
            qmenuMonth.TestType = TestTypeEnum.ControlTest.ToString();
            qmenuMonth.Duration = TestingDurationEnum.Monthly.ToString();
            qmenuMonth.MorbidityTetsId = morbidityTest.Id;
            ctx.QuantifyMenu.Add(qmenuMonth);
            ctx.SaveChanges();

            QuantifyMenu qmenuQua = CreateNewQMenu(morbidityTest);
            qmenuQua.Title = String.Format("{0} Quarterly Controls-{1}", GetTestShortName(ce), instrumentname);
            qmenuQua.TestType = TestTypeEnum.ControlTest.ToString();
            qmenuQua.Duration = TestingDurationEnum.Quarterly.ToString();
            qmenuQua.MorbidityTetsId = morbidityTest.Id;
            ctx.QuantifyMenu.Add(qmenuQua);
            ctx.SaveChanges();

            QuantifyMenu qmenuPerins = CreateNewQMenu(morbidityTest);
            qmenuPerins.Title = String.Format("Per Instrument-{0}", instrumentname);
            qmenuPerins.TestType = TestTypeEnum.PerInstrument.ToString();
            qmenuPerins.MorbidityTetsId = morbidityTest.Id;
            ctx.QuantifyMenu.Add(qmenuPerins);
            ctx.SaveChanges();


            QuantifyMenu qmenuPerday = CreateNewQMenu(morbidityTest);
            qmenuPerday.Title = String.Format("Per Day-{0}", instrumentname);
            qmenuPerday.TestType = TestTypeEnum.PerDay.ToString();
            qmenuPerday.MorbidityTetsId = morbidityTest.Id;
            ctx.QuantifyMenu.Add(qmenuPerday);
            ctx.SaveChanges();

        }

        private  void CreateChemistryTestQM(MorbidityTest _morbidityTest)
        {
            //ChemistryTestNameEnum[] chem = LqtUtil.EnumToArray<ChemistryTestNameEnum>();
            //for (int i = 0; i < chem.Length; i++)
            //{
            //    QuantifyMenu qmenuTest = CreateNewQMenu(_morbidityTest);
            //    qmenuTest.ChemTestName = chem[i].ToString();
            //    qmenuTest.Title = String.Format("{0} Tests - {1}", chem[i], _morbidityTest.Instrument.InstrumentName);
            //    qmenuTest.TestType = TestTypeEnum.Test.ToString();
            //    _morbidityTest.QuantifyMenus.Add(qmenuTest);
            //}
        }

        private  QuantifyMenu CreateNewQMenu(MorbidityTest morbidityTest)
        {
            QuantifyMenu qmenu = new QuantifyMenu();
            qmenu.ClassOfTest = morbidityTest.ClassOfTest;
            qmenu.InstrumentId = morbidityTest.InstrumentId;
            qmenu.MorbidityTetsId = morbidityTest.Id;
            ctx.QuantifyMenu.Add(qmenu);
            ctx.SaveChanges();

            return qmenu;
        }

        private  string GetTestShortName(ClassOfMorbidityTestEnum ttype)
        {
            string result = "";
            switch (ttype)
            {
                case ClassOfMorbidityTestEnum.CD4:
                    result = "CD4";
                    break;
                case ClassOfMorbidityTestEnum.Chemistry:
                    result = "Chem";
                    break;
                case ClassOfMorbidityTestEnum.Hematology:
                    result = "Hem";
                    break;
                case ClassOfMorbidityTestEnum.ViralLoad:
                    result = "VL";
                    break;
            }
            return result;
        }
    }
}
