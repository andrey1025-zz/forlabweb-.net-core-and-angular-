using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.DataInterface
{
    public interface IAssumption<Dynamiccontrol, PatientAssumption, MMGeneralAssumptionValue, TestingAssumption, patientnumberlist, TestingProtocol>
    {
        DataTable GettestforecastAssumptionnew(int id);
        DataTable GettestforecastAssumption(int id, int testid);
        DataTable GettestAssumption(int id, int testid);
        DataTable GetpatientAssumption(int id);
        DataTable GetforecastpatientAssumption(int id);
        DataTable GettestforecastAssumptionnewbytestId(int id, string param);
        Array GetproductAssumption(int id);
        Array GetforecastproductAssumption(int id);
        Array Getlineargrowth(int id);
       Array Gettestfromtestingprotocol(int id);
        List<string> getforecastdynamicheader(int id, int entitytype);

        List<string> getdynamicheader(int id, int entitytype);
        List<string> getvariablrdynamicheader(int id, int entitytype);
        IList<Dynamiccontrol> GetforecastDynamiccontrol(int id, int entitytype);
        IList<Dynamiccontrol> GetDynamiccontrol(int id, int entitytype);
        IList<Dynamiccontrol> GetlinearDynamiccontrol(int id);
        int Savelineargrowth(patientnumberlist b);
        int SavepatientAssumption(IEnumerable<PatientAssumption> b);
        int savemmgeneralassumptionvalue(IEnumerable<MMGeneralAssumptionValue> b);
        int savetestinggeneralassumptionvalue(IEnumerable<MMGeneralAssumptionValue> b);
        int Savetestingprotocol(IEnumerable<TestingProtocol> b);
        int SaveproductAssumption(IEnumerable<TestingAssumption> b);
        int saveproductgeneralassumptionvalue(IEnumerable<MMGeneralAssumptionValue> b);
        int deletetestingprotocol(int id, string param);
    }
}
