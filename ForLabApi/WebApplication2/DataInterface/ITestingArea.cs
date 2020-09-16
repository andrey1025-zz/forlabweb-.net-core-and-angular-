using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.DataInterface
{
    public interface ITestingArea<TestingArea>
    {
        IEnumerable<TestingArea> GetAll(int userid,string Role);
     
        TestingArea Getbyid(int id);
        int SaveData(TestingArea b);
        int UpdateData(int id, TestingArea b);
        int DeleteData(int id);
        TestingArea GetTestingAreaByName(string name);
        IEnumerable<TestingArea> GetTestingAreaByDemography(Boolean inDemo);
        TestingArea GetTestingAreaByClassOfMorbidity(string category);

     
    }

}
