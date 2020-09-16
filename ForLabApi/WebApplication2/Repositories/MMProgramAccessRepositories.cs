using ForLabApi.DataInterface;
using ForLabApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Repositories
{
    public class MMProgramAccessRepositories : IMMProgram<MMProgramList,demographicMMGeneralAssumption, DemographicMMGroup, DemographicMMGroupList,MMProgram, ForecastInfoList, ForecastInfo, MMForecastParameterList, MMGroupList, MMGeneralAssumptionList, MMGroup, Suggustionlist>
    {
        ForLabContext ctx;
        public MMProgramAccessRepositories(ForLabContext c)
        {
            ctx = c;
            //return ctx;
        }
        public int SaveProgram(MMProgram b)
        {
            int res = 0;
            if (b.Id != 0)
            {
                var mmprogram = ctx.MMProgram.Where(c => c.ProgramName == b.ProgramName && c.UserId==b.UserId).ToList();
                if(mmprogram.Count==1 && b.Id.Equals(mmprogram[0].Id))
                {
                    mmprogram[0].ProgramName = b.ProgramName;
                    ctx.SaveChanges();
                    res = b.Id;
                }
                else
                {

                }
            }
            else
            {
                var mmprogram = ctx.MMProgram.FirstOrDefault(c => c.ProgramName == b.ProgramName  && c.UserId == b.UserId);
                if (mmprogram != null)
                {
                    return res;
                }
                ctx.MMProgram.Add(b);
                ctx.SaveChanges();
                res = b.Id;
            }
            
            return res;
        }
      
        public int updatedemographicprogram(int id, int months)
        {
            int i = 0;
            try
            {
                var program = ctx.ForecastInfo.Find(id);
                if (program != null)
                {
                    program.months = months;
                    ctx.SaveChanges();
                    switch (months)
                    {
                        case 13:
                            for (int j = 1; j < 13; j++)
                            {
                                var mmgeneralassumption = ctx.demographicMMGeneralAssumption.Where(c => c.VariableName == "Month" + Convert.ToString(j) && c.Forecastid==id && c.ProgramId== program.ProgramId).FirstOrDefault();
                                if (mmgeneralassumption == null)
                                {
                                    demographicMMGeneralAssumption std = new demographicMMGeneralAssumption();
                                    std.VariableName = "Month" + Convert.ToString(j);
                                    std.VariableDataType = 1;
                                    std.ProgramId = program.ProgramId;
                                    std.VariableFormula = "";
                                    std.UseOn = "OnAllSite";
                                    std.AssumptionType = 3;
                                    std.Entity_type_id = 5;
                                    std.VariableEffect = true;
                                    std.IsActive = true;
                                    std.Forecastid = id;
                                    ctx.demographicMMGeneralAssumption.Add(std);
                                    ctx.SaveChanges();
                                  //  res = std.Id;
                                    std.VarCode = getvarcoded(std);
                                    ctx.SaveChanges();
                                }
                            }
                            break;
                        case 12:
                            for (int j = 1; j < 13; j++)
                            {
                                var mmgeneralassumption = ctx.demographicMMGeneralAssumption.Where(c => c.VariableName == "Month" + Convert.ToString(j) && c.Forecastid == id && c.ProgramId == program.ProgramId).FirstOrDefault();
                                if (mmgeneralassumption == null)
                                {
                                    demographicMMGeneralAssumption std = new demographicMMGeneralAssumption();
                                    std.VariableName = "Month" + Convert.ToString(j);
                                    std.VariableDataType = 1;
                                    std.ProgramId = program.ProgramId;
                                    std.VariableFormula = "";
                                    std.UseOn = "OnAllSite";
                                    std.AssumptionType = 3;
                                    std.Entity_type_id = 5;
                                    std.VariableEffect = true;
                                    std.IsActive = true;
                                    std.Forecastid = id;
                                    ctx.demographicMMGeneralAssumption.Add(std);
                                    ctx.SaveChanges();
                                    //  res = std.Id;
                                    std.VarCode = getvarcoded(std);
                                    ctx.SaveChanges();
                                }
                            }
                            break;
                        case 18:
                            for (int j = 1; j < 19; j++)
                            {
                                var mmgeneralassumption = ctx.demographicMMGeneralAssumption.Where(c => c.VariableName == "Month" + Convert.ToString(j) && c.Forecastid == id && c.ProgramId == program.ProgramId).FirstOrDefault();
                                if (mmgeneralassumption == null)
                                {
                                    demographicMMGeneralAssumption std = new demographicMMGeneralAssumption();
                                    std.VariableName = "Month" + Convert.ToString(j);
                                    std.VariableDataType = 1;
                                    std.ProgramId = program.ProgramId;
                                    std.VariableFormula = "";
                                    std.UseOn = "OnAllSite";
                                    std.AssumptionType = 3;
                                    std.Entity_type_id = 5;
                                    std.VariableEffect = true;
                                    std.IsActive = true;
                                    std.Forecastid = id;
                                    ctx.demographicMMGeneralAssumption.Add(std);
                                    ctx.SaveChanges();
                                    //  res = std.Id;
                                    std.VarCode = getvarcoded(std);
                                    ctx.SaveChanges();
                                }
                            }
                            break;
                        case 24:
                            for (int j = 1; j < 25; j++)
                            {
                                var mmgeneralassumption = ctx.demographicMMGeneralAssumption.Where(c => c.VariableName == "Month" + Convert.ToString(j) && c.Forecastid == id && c.ProgramId ==program.ProgramId).FirstOrDefault();
                                if (mmgeneralassumption == null)
                                {
                                    demographicMMGeneralAssumption std = new demographicMMGeneralAssumption();
                                    std.VariableName = "Month" + Convert.ToString(j);
                                    std.VariableDataType = 1;
                                    std.ProgramId = program.ProgramId;
                                    std.VariableFormula = "";
                                    std.UseOn = "OnAllSite";
                                    std.AssumptionType = 3;
                                    std.Entity_type_id = 5;
                                    std.VariableEffect = true;
                                    std.IsActive = true;
                                    std.Forecastid =id;
                                    ctx.demographicMMGeneralAssumption.Add(std);
                                    ctx.SaveChanges();
                                    //  res = std.Id;
                                    std.VarCode = getvarcoded(std);
                                    ctx.SaveChanges();
                                }
                            }
                            break;

                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
            return i;
           
        }
        public int updateProgram(int id, MMProgram b)
        {
            int res = 0;

            var MMProgram = ctx.MMProgram.Find(b.Id);
            if (MMProgram != null)
            {
                MMProgram.NoofYear = b.NoofYear;
                ctx.SaveChanges();
                if (b.NoofYear == 2)
                {
                    for (int j = 1; j < 25; j++)
                    {
                        var mmgeneralassumption = ctx.MMGeneralAssumption.Where(c => c.VariableName == "Month" + Convert.ToString(j)).FirstOrDefault();
                        if (mmgeneralassumption == null)
                        {
                            MMGeneralAssumption std = new MMGeneralAssumption();
                            std.VariableName = "Month" + Convert.ToString(j);
                            std.VariableDataType = 1;
                            std.ProgramId = b.Id;
                            std.VariableFormula = "";
                            std.UseOn = "OnAllSite";
                            std.AssumptionType = 3;
                            std.Entity_type_id = 5;
                            std.IsActive = true;
                            ctx.MMGeneralAssumption.Add(std);
                            ctx.SaveChanges();
                            res = std.Id;
                            std.VarCode = getvarcode(std);
                            ctx.SaveChanges();
                        }
                    }
                }
                else
                {

                    for (int j = 1; j < 13; j++)
                    {
                        var mmgeneralassumption = ctx.MMGeneralAssumption.Where(c => c.VariableName == "Month" + Convert.ToString(j)).FirstOrDefault();
                        if (mmgeneralassumption == null)
                        {
                            MMGeneralAssumption std = new MMGeneralAssumption();
                            std.VariableName = "Month" + Convert.ToString(j);
                            std.VariableDataType = 1;
                            std.ProgramId = b.Id;
                            std.VariableFormula = "";
                            std.UseOn = "OnAllSite";
                            std.AssumptionType = 3;
                            std.Entity_type_id = 5;
                            std.IsActive = true;
                            ctx.MMGeneralAssumption.Add(std);
                            ctx.SaveChanges();
                            res = std.Id;
                            std.VarCode = getvarcode(std);
                            ctx.SaveChanges();
                        }
                    }
                    //for (int j = 13; j < 25; j++)
                    //{
                    //    var mmgeneralassumption = ctx.MMGeneralAssumption.Where(c => c.VariableName == "Month" + Convert.ToString(j)).FirstOrDefault();
                    //    if (mmgeneralassumption != null)
                    //    {
                    //        ctx.MMGeneralAssumption.Remove(mmgeneralassumption);
                    //        ctx.SaveChanges();
                    //    }
                    //}
                }
                res = b.Id;
            }
            return res;
        }
        public int savesuggustion(Suggustionlist c)
        {
            int res = 0;
            var suggustedlist = ctx.Suggustionlist.Where(b => b.Name == c.Name && b.Type_name == c.Type_name).FirstOrDefault();
            if (suggustedlist != null)
            {


                return res;
            }
            ctx.Suggustionlist.Add(c);
            ctx.SaveChanges();
            res = c.id;
            return res;


        }
        public int savegroup(MMGroup b)
        {
            int res = 0;
            var mmprogram = ctx.MMGroup.FirstOrDefault(c => c.GroupName == b.GroupName && c.ProgramId == b.ProgramId);
            if (mmprogram != null)
            {


                return res;
            }
            ctx.MMGroup.Add(b);
            ctx.SaveChanges();
            res = b.Id;
            return res;
        }
        public int updategroup(int id, MMGroupList b)
        {
            int res = 0;
            var group = ctx.MMGroup.Find(b.Id);
            if (group != null)
            {
                group.IsActive = b.IsActive == "Yes" ? true : false;
                ctx.SaveChanges();
                res = b.Id;
            }
            return res;
        }
        public int updategeneralassumptions(int id, MMGeneralAssumptionList b)
        {
            int res = 0;
            var MMGeneralAssumption = ctx.MMGeneralAssumption.Find(b.Id);
            if (MMGeneralAssumption != null)
            {
                MMGeneralAssumption.IsActive = b.IsActive == "Yes" ? true : false;
                MMGeneralAssumption.VariableFormula = b.VariableFormula;
                MMGeneralAssumption.VariableEffect = b.VariableEffect;
                ctx.SaveChanges();
                res = b.Id;
            }
            return res;
        }
        public IEnumerable<Suggustionlist> Getsuggustionlist(int userid)
        {
            var suggustionlist = ctx.Suggustionlist.ToList();
            return suggustionlist;
        }
        public int Saveforecastparameter(MMProgram b)
        {
            int res = 0;
            foreach (MMForecastParameterList SL in b._mMForecastParameter)
            {
                var MMForecastParameter = ctx.MMForecastParameter.Where(c => c.Forecastid == SL.forecastid && c.UseOn == "OnAllSite" && c.VariableName == SL.VariableName && c.VariableEffect==SL.VariableEffect).FirstOrDefault();
                if (MMForecastParameter != null)
                {
                    if (SL.IsActive == "Yes")
                        MMForecastParameter.IsActive = true;
                    else
                        MMForecastParameter.IsActive = false;
                    ctx.SaveChanges();
                }
                else
                {
                    var entity_type = ctx.entity_type.Where(e => e.parameter_type_id == 1).ToList();
                    foreach (entity_type ET in entity_type)
                    {
                        MMForecastParameter std = new MMForecastParameter();
                        std.VariableName = SL.VariableName;
                        std.VariableDataType = SL.VariableDataType;
                        std.ProgramId = SL.ProgramId;
                        std.IsPrimaryOutput = SL.IsPrimaryOutput;
                        std.UseOn = SL.UseOn;
                        std.VarCode = SL.VarCode;
                        std.ForecastMethod = SL.ForecastMethod;
                        std.Entity_type_id = ET.id;
                        std.Forecastid = SL.forecastid;
                     
                        if (SL.IsActive == "Yes")
                            std.IsActive = true;
                        else
                            std.IsActive = false;
                        ctx.MMForecastParameter.Add(std);
                        ctx.SaveChanges();
                        res = std.Id;
                    }

                }
            }
            return res;
        }

        public int savegeneralassumptions(MMGeneralAssumptionList b)
        {
            int res = 0;

            var MMGeneralAssumption = ctx.MMGeneralAssumption.FirstOrDefault(c => c.VariableName == b.VariableName && c.ProgramId == b.ProgramId);
            if (MMGeneralAssumption != null)
            {
                return res;
            }

            MMGeneralAssumption std = new MMGeneralAssumption();
            std.VariableName = b.VariableName;
            std.VariableDataType = b.VariableDataType;
            std.VariableEffect = b.VariableEffect;
            std.ProgramId = b.ProgramId;
            std.VariableFormula = b.VariableFormula;
            std.UseOn = b.UseOn;
            std.VarCode = b.VarCode;
            std.AssumptionType = b.AssumptionType;
            if (std.AssumptionType == 1)
                std.Entity_type_id = 3;
            else if (std.AssumptionType == 2)
                std.Entity_type_id = 4;
            else
                std.Entity_type_id = 5;
            std.IsActive = b.IsActive == "Yes" ? true : false;
            ctx.MMGeneralAssumption.Add(std);
            ctx.SaveChanges();
            res = std.Id;
            std.VarCode = getvarcode(std);
            ctx.SaveChanges();



            return res;
        }
        private string getvarcode(MMGeneralAssumption GA)
        {

            string varcode = "";
            System.Text.StringBuilder SB = new System.Text.StringBuilder();

            SB.Append(GA.VariableName.First());
            SB.Append(GA.ProgramId);
            SB.Append(GA.Id);

            varcode = SB.ToString();
            return varcode;
        }
        private string getvarcoded(demographicMMGeneralAssumption GA)
        {

            string varcode = "";
            System.Text.StringBuilder SB = new System.Text.StringBuilder();

            SB.Append(GA.VariableName.First());
            SB.Append(GA.ProgramId);
            SB.Append(GA.Id);

            varcode = SB.ToString();
            return varcode;
        }
        public IEnumerable<MMProgram> GetAllbyadmin()
        {
            var adminuserid = ctx.User.Where(b => b.Role == "admin").FirstOrDefault();
            var MMProgram = ctx.MMProgram.Where(b => b.UserId == adminuserid.Id).ToList();
            return MMProgram;


        }
        public List<MMProgramList> Getprogramlist(int userid)
        {
            MMProgramList MP1 = new MMProgramList();
            List<MMProgramList> MMP = new List<MMProgramList>();
            var mmprogram = ctx.MMProgram.OrderByDescending(x => x.Id).ToList();
            if (userid == 0)
            {
                mmprogram = mmprogram.Join(ctx.User, b => b.UserId, c => c.Id, (b, c) => new { b, c }).Where(x => x.c.Role == "admin").Select(x => new MMProgram
                {

                    Description = x.b.Description,
                    Gtp = x.b.Gtp,
                    Id = x.b.Id,
                    NoofYear = x.b.NoofYear,
                    ProgramName = x.b.ProgramName,
                    Rtp = x.b.Rtp,
                    UserId = x.b.UserId,
                    _mMForecastParameter = x.b._mMForecastParameter
                }).ToList();

                for (int i = 0; i < mmprogram.Count(); i++)
                {
                    MP1 = new MMProgramList();
                    MP1.Id = mmprogram[i].Id;
                    MP1.ProgramName = mmprogram[i].ProgramName;

                    if(ctx.MMForecastParameter.Where(b => b.ProgramId == mmprogram[i].Id).FirstOrDefault() !=null)
                    {
                        MP1.Forecastmethod = ctx.MMForecastParameter.Where(b => b.ProgramId == mmprogram[i].Id).FirstOrDefault().ForecastMethod == 1 ? "Target_Based" : "Population_Based";
                    }
                    else
                    {
                        MP1.Forecastmethod = "";
                    }


                    if (ctx.DemographicMMGroup.Where(b => b.ProgramId == mmprogram[i].Id).FirstOrDefault() != null)
                    {
                        MP1.TotalGrp = ctx.DemographicMMGroup.Where(b => b.ProgramId == mmprogram[i].Id).Count();
                    }
                    else
                    {
                        MP1.TotalGrp = 0;
                    }
                    if (ctx.ForecastInfo.Where(b => b.ProgramId == mmprogram[i].Id).FirstOrDefault() != null)
                    {
                        MP1.Totalforecast = ctx.ForecastInfo.Where(b => b.ProgramId == mmprogram[i].Id).Count();
                    }
                    else
                    {
                        MP1.Totalforecast = 0;
                    }
                    MMP.Add(MP1);
                }
                //MMP = mmprogram.Join(ctx.User, b => b.UserId, c => c.Id, (b, c) => new { b, c }).Where(x => x.c.Role == "admin").Select(x => new MMProgramList
                //{


                //    Id = x.b.Id,

                //    ProgramName = x.b.ProgramName,
                //    Forecastmethod = ctx.MMForecastParameter.Where(b => b.ProgramId == x.b.Id).FirstOrDefault().ForecastMethod == 1 ? "Target_Based" : "Population_Based",
                //    TotalGrp = ctx.DemographicMMGroup.Where(b => b.ProgramId == x.b.Id).Count(),
                //    Totalforecast=ctx.ForecastInfo.Where(b=>b.ProgramId==x.b.Id).Count()
                //}).ToList();
            }
            else
            {
                var Roles = ctx.User.Where(b => b.Id == userid).Select(x => x.Role).FirstOrDefault();
                if (Roles == "admin")
                {
                    for (int i = 0; i < mmprogram.Count(); i++)
                    {
                        MP1.Id = mmprogram[i].Id;
                        MP1.ProgramName = mmprogram[i].ProgramName;

                        if (ctx.MMForecastParameter.Where(b => b.ProgramId == mmprogram[i].Id).FirstOrDefault() != null)
                        {
                            MP1.Forecastmethod = ctx.MMForecastParameter.Where(b => b.ProgramId == mmprogram[i].Id).FirstOrDefault().ForecastMethod == 1 ? "Target_Based" : "Population_Based";
                    }
                        else
                        {
                            MP1.Forecastmethod = "";
                        }


                        if (ctx.DemographicMMGroup.Where(b => b.ProgramId == mmprogram[i].Id).FirstOrDefault() != null)
                        {
                            MP1.TotalGrp = ctx.DemographicMMGroup.Where(b => b.ProgramId == mmprogram[i].Id).Count();
                        }
                        else
                        {
                            MP1.TotalGrp = 0;
                        }
                        if (ctx.ForecastInfo.Where(b => b.ProgramId == mmprogram[i].Id).FirstOrDefault() != null)
                        {
                            MP1.Totalforecast = ctx.ForecastInfo.Where(b => b.ProgramId == mmprogram[i].Id).Count();
                        }
                        else
                        {
                            MP1.Totalforecast = 0;
                        }
                        MMP.Add(MP1);
                    }
                }
                else
                {
                    mmprogram = mmprogram.Where(b => b.UserId == userid).ToList();



                    for (int i = 0; i < mmprogram.Count(); i++)
                    {
                        MP1.Id = mmprogram[i].Id;
                        MP1.ProgramName = mmprogram[i].ProgramName;

                        if (ctx.MMForecastParameter.Where(b => b.ProgramId == mmprogram[i].Id).FirstOrDefault() != null)
                        {
                            MP1.Forecastmethod = ctx.MMForecastParameter.Where(b => b.ProgramId == mmprogram[i].Id).FirstOrDefault().ForecastMethod == 1 ? "Target_Based" : "Population_Based";
                    }
                        else
                        {
                            MP1.Forecastmethod = "";
                        }


                        if (ctx.DemographicMMGroup.Where(b => b.ProgramId == mmprogram[i].Id).FirstOrDefault() != null)
                        {
                            MP1.TotalGrp = ctx.DemographicMMGroup.Where(b => b.ProgramId == mmprogram[i].Id).Count();
                        }
                        else
                        {
                            MP1.TotalGrp = 0;
                        }
                        if (ctx.ForecastInfo.Where(b => b.ProgramId == mmprogram[i].Id).FirstOrDefault() != null)
                        {
                            MP1.Totalforecast = ctx.ForecastInfo.Where(b => b.ProgramId == mmprogram[i].Id).Count();
                        }
                        else
                        {
                            MP1.Totalforecast = 0;
                        }
                        MMP.Add(MP1);
                    }
                }
            }

            return MMP;

        }
        public IEnumerable<MMProgram> GetAll(int userid)
        {
            var mmprogram = ctx.MMProgram.OrderByDescending(x => x.Id).ToList();
            if (userid == 0)
            {
                mmprogram = mmprogram.Join(ctx.User, b => b.UserId, c => c.Id, (b, c) => new { b, c }).Where(x => x.c.Role == "admin").Select(x=> new MMProgram {

                    Description=x.b.Description,
                    Gtp=x.b.Gtp,
                    Id=x.b.Id,
                    NoofYear=x.b.NoofYear,
                    ProgramName=x.b.ProgramName,
                    Rtp=x.b.Rtp,
                    UserId=x.b.UserId,
                    _mMForecastParameter=x.b._mMForecastParameter
                }).ToList();
            }
            else
            {
                var Roles = ctx.User.Where(b => b.Id == userid).Select(x => x.Role).FirstOrDefault();
                if (Roles == "admin")
                {

                }
                else
                {
                    mmprogram = mmprogram.Where(b => b.UserId == userid).ToList();
                }
            }
         
            return mmprogram;
        }
        public IEnumerable<MMGeneralAssumptionList> GetGeneralAssumptionList()
        {
            var parameterlist = ctx.MMGeneralAssumption.Select(x =>
                  new MMGeneralAssumptionList
                  {
                      Id = x.Id,
                      VariableName = x.VariableName,
                      UseOn = x.UseOn,
                      ProgramId = x.ProgramId,
                      VariableDataType = x.VariableDataType,
                      VariableDataTypeName = x.VariableDataType == 1 ? "Numeric" : "Percentage",
                      AssumptionType = x.AssumptionType,
                      VariableFormula = x.VariableFormula,
                      VarCode = x.VarCode,
                      IsActive = x.IsActive == true ? "Yes" : "No"

                  }
             ).ToList();

            for (int i = 0; i < parameterlist.Count; i++)
            {
                if (parameterlist[i].AssumptionType == 1)
                {
                    parameterlist[i].AssumptionTypename = "Patient_Number_Assumption";
                }
                else if (parameterlist[i].AssumptionType == 2)
                {
                    parameterlist[i].AssumptionTypename = "Product_Assumption";
                }
                else
                {
                    parameterlist[i].AssumptionTypename = "Test_Assumption";
                }
            }

            return parameterlist;
        }
        public  void saveDemographicMMGeneralAssumptions(IEnumerable<demographicMMGeneralAssumption> b)
        {
           // int i = 0;
         
            foreach (var item in b)
            {

                if (item.IsActive == false)
                {
                    var existlist = ctx.demographicMMGeneralAssumption.Where(cb => cb.Forecastid == item.Forecastid && cb.AssumptionType == item.AssumptionType && cb.VariableName == item.VariableName).FirstOrDefault();
                    ctx.demographicMMGeneralAssumption.Remove(existlist);
                    ctx.SaveChanges();
                }
                else
                {
                    var exist = ctx.demographicMMGeneralAssumption.Where(cb => cb.Forecastid == item.Forecastid && cb.AssumptionType == item.AssumptionType && cb.VariableName == item.VariableName).FirstOrDefault();
                    if (exist == null)
                    {
                        //if (item.AssumptionType == 1)
                        //    item.Entity_type_id = 3;
                        //else if (item.AssumptionType == 2)
                        //    item.Entity_type_id = 4;
                        //else
                        //    item.Entity_type_id = 5;
                        ctx.demographicMMGeneralAssumption.Add(item);
                    ctx.SaveChanges();
                  }
                }
              //  i++;
              
            }
        }
        public IEnumerable<demographicMMGeneralAssumption> GetDemographicMMGeneralAssumptions(string param, int forecastid)
        {
            List<demographicMMGeneralAssumption> MGL;
            string[] param1 = param.TrimEnd(',').Split(",");
            MGL = ctx.demographicMMGeneralAssumption.Where(b => !b.VariableName.Contains("Mon") && b.Forecastid == forecastid && b.AssumptionType== Convert.ToInt32(param1[1])).ToList();
            if (MGL.Count == 0)
            {

                MGL = ctx.MMGeneralAssumption.Where(b => !b.VariableName.Contains("Mon") && b.ProgramId == Convert.ToInt32(param1[0]) && b.AssumptionType == Convert.ToInt32(param1[1])).Select(x =>
                 new demographicMMGeneralAssumption
                 {
                     Id = 0,
                     VariableName =x.VariableName,
       VariableDataType=x.VariableDataType,
    UseOn=x.UseOn,
      VariableFormula=x.VariableFormula, 
       ProgramId =x.ProgramId,
         VarCode =x.VarCode,
AssumptionType =x.AssumptionType,
      VariableEffect =x.VariableEffect,
    IsActive =x.IsActive,
     Entity_type_id =x.Entity_type_id,
     UserId =x.UserId,
       Forecastid =0


    }
            ).ToList();

            }

            return MGL;
        }
        public IEnumerable<MMForecastParameterList> Getforecastparameter()
        {
            var parameterlist = ctx.MMForecastParameter.Where(b => b.Entity_type_id == 1).Select(x =>
                       new MMForecastParameterList
                       {
                           Id = x.Id,
                           ForecastMethod = x.ForecastMethod,
                           ForecastMethodname = x.ForecastMethod == 1 ? "Target_Based" : "Population_Based",
                           VariableName = x.VariableName,
                           UseOn = x.UseOn,
                           ProgramId = x.ProgramId,
                           VariableDataType = x.VariableDataType,
                           VariableDataTypename = x.VariableDataType == 1 ? "Numeric" : "Percentage",
                           IsActive = x.IsActive == true ? "Yes" : "No"

                       }
              ).ToList();
            return parameterlist;
        }
        public IEnumerable<MMGroupList> Getpatientgroup()
        {
            var parameterlist = ctx.MMGroup.Select(x =>
                 new MMGroupList
                 {
                     Id = x.Id,
                     GroupName = x.GroupName,
                     ProgramId = x.ProgramId,
                     IsActive = x.IsActive == true ? "Yes" : "No"

                 }
            ).ToList();
            return parameterlist;
        }


        public IEnumerable<DemographicMMGroupList> Getpatientgroupforforecast(int id,int forecastid)
        {
          
            List<DemographicMMGroupList> MGL;
         

            MGL = ctx.DemographicMMGroup.Where(b => b.Forecastid == forecastid).Select(x =>
               new DemographicMMGroupList
               {
                   Id = x.Id,
                   GroupName = x.GroupName,
                   ProgramId = x.ProgramId,
                   IsActive = x.IsActive,
                   forecastid = forecastid,
                   

               }
          ).ToList();
            if (MGL.Count==0)
            {

                MGL = ctx.MMGroup.Where(b => b.ProgramId == id).Select(x =>
                 new DemographicMMGroupList
                 {
                     Id = 0,
                     GroupName = x.GroupName,
                     ProgramId = x.ProgramId,
                     IsActive = x.IsActive ,
                     forecastid = 0,
                    

                 }
            ).ToList();

            }

            return MGL;
        }
        public void saveforecastmmgroup(IEnumerable<DemographicMMGroup> b)
        {
            foreach (var item in b)
            {
                var exist = ctx.DemographicMMGroup.Where(c => c.Forecastid == item.Forecastid && c.GroupName == item.GroupName).FirstOrDefault();
                if (exist==null)
                {
                    ctx.DemographicMMGroup.Add(item);
                    ctx.SaveChanges();
                }
            }
        }
        public IEnumerable<MMForecastParameterList> Getforecastparameterbyprogramid(int id)
        {
            var parameterlist = ctx.MMForecastParameter.Where(b => b.ProgramId == id).Select(x =>
                     new MMForecastParameterList
                     {
                         Id = x.Id,
                         ForecastMethod = x.ForecastMethod,
                         ForecastMethodname = x.ForecastMethod == 1 ? "Target_Based" : "Population_Based",
                         VariableName = x.VariableName,
                         UseOn = x.UseOn,
                         ProgramId = x.ProgramId,
                         VariableDataType = x.VariableDataType,
                         VariableDataTypename = x.VariableDataType == 1 ? "Numeric" : "Percentage",
                         IsActive = x.IsActive == true ? "Yes" : "No"

                     }
                ).ToList();
            return parameterlist;
        }
        public IEnumerable<MMForecastParameterList> Getforecastparameterbyforecastid(int id)
        {
            var parameterlist = ctx.MMForecastParameter.Where(b => b.Forecastid == id).Select(x =>
                     new MMForecastParameterList
                     {
                         Id = x.Id,
                         ForecastMethod = x.ForecastMethod,
                         ForecastMethodname = x.ForecastMethod == 1 ? "Target_Based" : "Population_Based",
                         VariableName = x.VariableName,
                         UseOn = x.UseOn,
                         ProgramId = x.ProgramId,
                         VariableDataType = x.VariableDataType,
                         VariableDataTypename = x.VariableDataType == 1 ? "Numeric" : "Percentage",
                         IsActive = x.IsActive == true ? "Yes" : "No"

                     }
                ).ToList();
            return parameterlist;
        }
        public IEnumerable<ForecastInfoList> Getforecastinfobyprogramid(int id,int userid)
        {
            var demographicforcast = ctx.ForecastInfo.Where(b => b.ProgramId == id ).Select(item => new ForecastInfoList
            {
                ForecastID = item.ForecastID,
                ForecastNo = item.ForecastNo,
                Methodology = item.Methodology,
                DataUsage = item.DataUsage,
                Status = item.Status,
                StartDate = item.StartDate.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture),
                Period = item.Period,
                MonthInPeriod = item.MonthInPeriod,
                Extension = item.Extension,
                ScopeOfTheForecast = item.ScopeOfTheForecast,
                Note = item.Note,
                ActualCount = item.ActualCount,
                LastUpdated = item.LastUpdated.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture),
                ForecastDate = item.ForecastDate.Value.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture),
                Method = item.Method,
                Westage = item.Westage,
                Scaleup = item.Scaleup,
                ROrder = item.ROrder,
                ForecastType = item.ForecastType,
                ProgramId = item.ProgramId,
                Countryid=item.Countryid,
                Forecastlock=item.Forecastlock,
                  userid = item.UserId
            }).OrderByDescending(x=>x.ForecastID).ToList();

            var Roles = ctx.User.Where(b => b.Id == userid).Select(x => x.Role).FirstOrDefault();
            if (Roles == "admin")
            {

            }

            else
            {
                demographicforcast = demographicforcast.Where(b => b.userid == userid).ToList();
            }
            return demographicforcast;
        }
        public IEnumerable<ForecastInfoList> GetForecastInfoByMethodology(string metho,int userid)
        {
            var demographicforcast = ctx.ForecastInfo.Where(b => b.Methodology == metho ).Select(item => new ForecastInfoList
            {
                ForecastID = item.ForecastID,
                ForecastNo = item.ForecastNo,
                Methodology = item.Methodology,
                DataUsage = item.DataUsage,
                Status = item.Status,
                StartDate = item.StartDate.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture),
                Period = item.Period,
                MonthInPeriod = item.MonthInPeriod,
                Extension = item.Extension,
                ScopeOfTheForecast = item.ScopeOfTheForecast,
                Note = item.Note,
                ActualCount = item.ActualCount,
                LastUpdated = item.LastUpdated.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture),
                ForecastDate = item.ForecastDate.Value.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture),
                Method = item.Method,
                Westage = item.Westage,
                Scaleup = item.Scaleup,
                ROrder = item.ROrder,
                ForecastType = item.ForecastType,
                ProgramId = item.ProgramId,
                Countryid=item.Countryid,
                
                userid=item.UserId
            }).OrderByDescending(x=>x.ForecastID).ToList();
            var Roles = ctx.User.Where(b => b.Id == userid).Select(x => x.Role).FirstOrDefault();
            if (Roles == "admin")
            {

            }

         else
            {
                demographicforcast = demographicforcast.Where(b => b.userid == userid).ToList();
            }
            return demographicforcast;

        }
    }
}
