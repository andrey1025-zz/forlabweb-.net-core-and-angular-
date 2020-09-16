using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.DataInterface
{
    public interface ITest<TestList_area,Gettotalcount, Test, TestingArea, testList, ProductUsagelist, ConsumableUsagelist, Masterconsumablelist, ProductUsageDetail, ConsumableUsageDetail, forecasttest, ForecastTest>
    {
        IEnumerable<testList> GetAll(int userid,string role);

        IEnumerable<ProductUsagelist> Getdefaulttestproduct(string areaids);
        IEnumerable<ConsumableUsagelist> Getdefaulttestconsumble(string areaids);
        List<testList> Getdefaulttest(string areaids);
        testList Getbyid(int id);
        int SaveData(Test b);
        int UpdateData(int id, Test b);
        int DeleteData(int id);
        int Deleteproductusage(string ids);
        int Deletconsumableusage(string ids);
        IEnumerable<TestingArea> GetAllbyadmin();
        List<TestList_area> Getallarea(int userid, string Role);
        List<forecasttest> getAlltestbytestingarea(int Forecstid, int userid,string Role);
        int UpdatConsumableeData(int id, Masterconsumablelist b);
        int saveconsumabledata(Masterconsumablelist b);
        IList<Test> GetAllTestsByAreaId(int areaid,int userid);
        IList<Test> GetAllTestsByforecastid(int id);
        // IList<Test> GetAllTestsByGroupId(int groupid);
        Test GetTestByName(string name);
       Gettotalcount GettotalcountNo(int userid, string role);
        Test GetTestByNameAndTestArea(string name, int areaid);
        // IList<Test> GetTestByPlatform(string platform);
        void saveforecasttest(int id,IEnumerable<ForecastTest> b);

        IEnumerable<ProductUsageDetail> GetProductUsagelist(int testid);

        IEnumerable<ProductUsageDetail> GetControltUsagelist(int testid);
        IEnumerable<ConsumableUsageDetail> GetConsumableUsagelist(int testid ,string type);
        //   IList<T> GetProductUsageByInsIdAndPlatform(int instrumentid, string platform);
    }
}
