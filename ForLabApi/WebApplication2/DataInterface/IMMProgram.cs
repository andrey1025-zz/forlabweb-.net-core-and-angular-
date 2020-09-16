using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.DataInterface
{
    public interface IMMProgram <MMProgramList, demographicMMGeneralAssumption, DemographicMMGroup, DemographicMMGroupList, MMProgram, ForecastInfoList, ForecastInfo, MMForecastParameterList, MMGroupList, MMGeneralAssumptionList, MMGroup, Suggustionlist>
    {
        IEnumerable<MMProgram> GetAll(int userid);
        List<MMProgramList> Getprogramlist(int userid);
        IEnumerable<DemographicMMGroupList> Getpatientgroupforforecast(int id, int forecastid);
        IEnumerable<MMProgram> GetAllbyadmin();
       // ForecastProgramyear getforecastyear(int id);
        int updatedemographicprogram(int id,int months);
        int SaveProgram(MMProgram b);
        int updateProgram(int id, MMProgram b);
        int Saveforecastparameter(MMProgram b);
        void saveforecastmmgroup(IEnumerable<DemographicMMGroup> b);
        int savegroup(MMGroup b);
        int updategroup(int id, MMGroupList b);
        int savegeneralassumptions(MMGeneralAssumptionList b);
        int updategeneralassumptions(int id, MMGeneralAssumptionList b);
        void saveDemographicMMGeneralAssumptions(IEnumerable<demographicMMGeneralAssumption> b);
        IEnumerable<demographicMMGeneralAssumption> GetDemographicMMGeneralAssumptions(string param,  int forecastid);
        IEnumerable<MMGeneralAssumptionList> GetGeneralAssumptionList();
        IEnumerable<MMGroupList> Getpatientgroup();
        IEnumerable<MMForecastParameterList> Getforecastparameter();
        IEnumerable<MMForecastParameterList> Getforecastparameterbyprogramid(int id);
        IEnumerable<MMForecastParameterList> Getforecastparameterbyforecastid(int id);
        IEnumerable<ForecastInfoList> Getforecastinfobyprogramid(int id,int userid);
        IEnumerable<ForecastInfoList> GetForecastInfoByMethodology(string metho,int userid);

        IEnumerable<Suggustionlist> Getsuggustionlist(int userid);

        int savesuggustion(Suggustionlist b);
    }
}
