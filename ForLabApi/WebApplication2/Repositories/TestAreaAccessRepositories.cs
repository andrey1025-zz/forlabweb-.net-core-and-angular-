using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForLabApi.DataInterface;

using ForLabApi.Models;

namespace ForLabApi.Repositories
{

    public class TestAreaAccessRepositories : ITestingArea<TestingArea>

    {
        ForLabContext ctx;
        public TestAreaAccessRepositories(ForLabContext c)
        {
            ctx = c;
            //return ctx;
        }
        public int SaveData(TestingArea b)
        {
            int res = 0;
            var TestingArea = ctx.TestingArea.FirstOrDefault(c => c.AreaName == b.AreaName && c.UserId==b.UserId);
            if (TestingArea != null)
            {
                return res;
            }
            ctx.TestingArea.Add(b);
            ctx.SaveChanges();
            res = b.TestingAreaID;
            return res;
        }

        public int DeleteData(int id)
        {
            int res = 0;
            var TestingArea = ctx.TestingArea.FirstOrDefault(b => b.TestingAreaID == id);
            if (TestingArea != null)
            {
                try
                {
                    ctx.TestingArea.Remove(TestingArea);
                    ctx.SaveChanges();
                    res = id;
                }
                catch (Exception ex)
                {


                }

            }
            return res;
        }

        public TestingArea Getbyid(int id)
        {
            var TestingArea = ctx.TestingArea.FirstOrDefault(b => b.TestingAreaID == id);
            return TestingArea;
        }
   
        public IEnumerable<TestingArea> GetAll(int userid,string Role)


        {
            //var Roles = ctx.User.Where(b => b.Id == userid).Select(x => x.Role).FirstOrDefault();
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
                    TestingArea = TestingArea.Where(b => b.UserId == userid || b.UserId==adminuserid || b.Isapprove==true ).ToList();
                }
            }


            return TestingArea;
        }

        public int UpdateData(int id, TestingArea b)
        {
            int res = 0;
            //   var TestArea = ctx.TestingArea.Find(id);
            var TestingArea = ctx.TestingArea.Where(c => c.AreaName == b.AreaName && c.UserId==b.UserId).ToList();
            if (TestingArea != null)
            {
                if (TestingArea.Count == 1 && b.TestingAreaID.Equals(TestingArea[0].TestingAreaID))
                {
                    TestingArea[0].AreaName = b.AreaName;
                    ctx.SaveChanges();
                    res = b.TestingAreaID;
                }

                // res = ctx.SaveChanges();
            }
            return res;
        }
        public TestingArea GetTestingAreaByName(string name)
        {
            var TestingArea = ctx.TestingArea.FirstOrDefault(b => b.AreaName == name);
            return TestingArea;
        }
        public IEnumerable<TestingArea> GetTestingAreaByDemography(Boolean inDemo)
        {
            var TestingArea = ctx.TestingArea.Where(b => b.UseInDemography == inDemo).ToList();
            return TestingArea;
        }

        public TestingArea GetTestingAreaByClassOfMorbidity(string category)
        {
            var TestingArea = ctx.TestingArea.FirstOrDefault(b => b.Category == category);
            return TestingArea;
        }
     
    }
}
