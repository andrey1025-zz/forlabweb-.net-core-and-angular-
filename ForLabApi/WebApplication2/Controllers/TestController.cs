using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ForLabApi.DataInterface;
using ForLabApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForLabApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors("ApiCorsPolicy")]
    public class TestController : Controller
    {
        ITest<TestList_area,Gettotalcount, Test, TestingArea, testList, ProductUsagelist, ConsumableUsagelist, Masterconsumablelist, ProductUsageDetail, ConsumableUsageDetail, forecasttest, ForecastTest> _Test;
        public TestController(ITest<TestList_area,Gettotalcount, Test, TestingArea, testList, ProductUsagelist, ConsumableUsagelist, Masterconsumablelist, ProductUsageDetail, ConsumableUsageDetail, forecasttest, ForecastTest> b)
        {
            _Test = b;
        }
        public class aaData1
        {
            public IEnumerable<testList> aaData { get; set; }
        }
        [HttpGet, Authorize]
        public List<TestList_area> Getallarea()
        {
            var tests = _Test.Getallarea( Convert.ToInt32(Request.Headers["userid"]), Convert.ToString(Request.Headers["role"]));
            return tests;
        }

      [HttpGet("{id}"), Authorize]
        public List<forecasttest> getAlltestbytestingarea(int id)
        {
            var tests = _Test.getAlltestbytestingarea(id, Convert.ToInt32(Request.Headers["userid"]), Convert.ToString(Request.Headers["role"]));
            return tests;
        }
        [HttpGet, Authorize]
        public Gettotalcount Gettotalcount()
        {
            var totalcount = _Test.GettotalcountNo(Convert.ToInt32(Request.Headers["userid"]), Convert.ToString(Request.Headers["role"]));
            return totalcount;
        }
        // GET: api/values
        [HttpGet, Authorize]

        public aaData1 GetAll()
        {


            var Test = _Test.GetAll(Convert.ToInt32(Request.Headers["userid"]),Convert.ToString(Request.Headers["role"]));
            var gq = new aaData1
            {
                aaData = Test

            };

            return gq;

        }

        //Get Product PriceList
        //[HttpGet("{id}")]
        //[ActionName("GetPriceList")]
        //public IEnumerable<ProductPrice> GetPriceList(int id)
        //{
        //    var productPricelist = _Product.GetProductPriceList(id);
        //    return productPricelist;
        //}

        //Get Productby TypeIDt
        //[HttpGet("{id}")]
        //[ActionName("GetAllProductByType")]
        //public IEnumerable<MasterProduct> GetAllProductByType(int id)
        //{
        //    var productPricelist = _Product.GetAllProductByType(id);
        //    return productPricelist;
        //}

        // GET api/values/5

     
        [HttpGet]
        public IEnumerable<TestingArea> GetAllbyadmin()
        {
            var testingarea = _Test.GetAllbyadmin();
            return testingarea;
        }
        [HttpGet("{id}"), Authorize]
        public IList<Test> GetAllTestsByforecastid(int id)
        {
            var Test = _Test.GetAllTestsByforecastid(id);
            return Test;

        }
        [HttpGet("{id}"), Authorize]
        public IList<Test> GetAllTestsByAreaId(int id)
        {
            var Test = _Test.GetAllTestsByAreaId(id, Convert.ToInt32(Request.Headers["userid"]));
            return Test;
        }
        [HttpGet("{id}"), Authorize]

        public testList GetbyId(int id)
        {
            var Test = _Test.Getbyid(id);
            return Test;
        }

        // POST api/values
        [HttpPost, Authorize]

        public int Post01([FromBody]Test value)
        {
            var re = Request;
            var header = re.Headers;
            value.UserId = Convert.ToInt32(header["userid"]);
            int res = _Test.SaveData(value);
            if (res != 0)
            {
                return res;
            }
            return 0;
        }
        [HttpPut("{id}"), Authorize]
        public int Saveforecasttest(int id,[FromBody]IEnumerable<ForecastTest> b)
        {
              int i = 0;
            _Test.saveforecasttest(id,b);
            return i;
        }
        [HttpPost, Authorize]
        public int postconsumable([FromBody]Masterconsumablelist value)
        {

            var re = Request;
            var header = re.Headers;
            value.UserId = Convert.ToInt32(header["userid"]);

            int res = _Test.saveconsumabledata(value);
            if (res != 0)
            {
                return res;
            }
            return 0;
        }
        // PUT api/values/5
        [HttpPut("{id}"), Authorize]

        public int Put01(int id, [FromBody]Test value)
        {
            int res = 0;
            var re = Request;
            var header = re.Headers;
            value.UserId = Convert.ToInt32(header["userid"]);
            if (id == value.TestID)
            {
                res = _Test.UpdateData(id, value);
                if (res != 0)
                {
                    return res;
                }
                return res;
            }
            return res;
        }
        [HttpPut("{id}"), Authorize]

        public int Put02(int id, [FromBody]Masterconsumablelist value)
        {
            int res = 0;
            var re = Request;
            var header = re.Headers;
            value.UserId = Convert.ToInt32(header["userid"]);
            if (id == value._consumablesUsages[0].ConsumableId)
            {
                res = _Test.UpdatConsumableeData(id, value);
                if (res != 0)
                {
                    return res;
                }
                return res;
            }
            return res;
        }
        // DELETE api/values/5
        [HttpDelete("{id}"), Authorize]

        public int Del01(int id)
        {
            int res = _Test.DeleteData(id);
            if (res != 0)
            {
                return res;
            }
            return res;
        }

        // DELETE api/values/5
        [HttpDelete("{ids}"), Authorize]

        public IActionResult Delproductusage(string ids)
        {
            if (ids != "")
            {
                int res = _Test.Deleteproductusage(ids);
                if (res != 0)
                {
                    return Ok();
                }
            }
            return NotFound();
        }

        [HttpDelete("{ids}"), Authorize]

        public IActionResult DelConsumableUsage(string ids)
        {
            if (ids != "")
            {
                int res = _Test.Deletconsumableusage(ids);
                if (res != 0)
                {
                    return Ok();
                }
            }
            return NotFound();
        }
        [HttpGet("{id}"), Authorize]
        public IEnumerable<ProductUsageDetail> GetProductUsagelist(int id)
        {
            var productusagelist = _Test.GetProductUsagelist(id);
            return productusagelist;

        }


        [HttpGet("{id}"), Authorize]
        public IEnumerable<ProductUsageDetail> GetControltUsagelist(int id)
        {
            var productusagelist = _Test.GetControltUsagelist(id);
            return productusagelist;

        }
        [HttpGet("{testid}"), Authorize]
        public IEnumerable<ConsumableUsageDetail> GetConsumableUsagelist(int testid, string type)
        {
            var consumableusagelist = _Test.GetConsumableUsagelist(testid, type);
            return consumableusagelist;

        }
        [HttpGet("{areaids}"), Authorize]
        public List<testList> Getdefaulttest(string areaids)
        {

            var testlist = _Test.Getdefaulttest(areaids);
            return testlist;
        }

        [HttpGet("{areaids}"), Authorize]
        public IEnumerable<ProductUsagelist> Getdefaulttestproduct(string areaids)
        {
            var testlist = _Test.Getdefaulttestproduct(areaids);
            return testlist;
        }

        [HttpGet("{areaids}"),Authorize]
        public IEnumerable<ConsumableUsagelist> Getdefaulttestconsumble(string areaids)
        {
            var consumbleslist = _Test.Getdefaulttestconsumble(areaids);
            return consumbleslist;
        }

    }

}
