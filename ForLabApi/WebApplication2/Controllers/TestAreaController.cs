using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
using ForLabApi.Repositories;
using ForLabApi.Models;
using ForLabApi.DataInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForLabApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("ApiCorsPolicy")]
    public class TestAreaController : Controller
    {

        ITestingArea<TestingArea> _TestArea;
        public TestAreaController(ITestingArea<TestingArea> b)
        {
            _TestArea = b;
        }
        public class aaData1
        {
            public IEnumerable<TestingArea> aaData { get; set; }
        }

        // GET: api/values
        [HttpGet]
        public aaData1 Get()
        {
            var userid = 0;
            var role = "";
            if (Request.Headers["userid"] == "")
            {

            }
            else
            {
                userid = Convert.ToInt32(Request.Headers["userid"]);
                role = Convert.ToString(Request.Headers["role"]);

            }
            var TestAreas = _TestArea.GetAll(userid,role);
            var gq = new aaData1
            {
                aaData = TestAreas

            };
            return gq;
            //Using Newtonsoft.json. Dump is an extension method of [Linqpad][4]
            //   JsonConvert.SerializeObject(gq);

            // JsonResult jsonResult = new JsonResult();
            // jsonResult.Value 
            //    return gq;
        }
      
        // GET api/values/5
        [HttpGet("{id}"), Authorize]
        public TestingArea Get(int id)
        {
            var TestArea = _TestArea.Getbyid(id);
            return TestArea;
        }

        // POST api/values
        [HttpPost, Authorize]
        public string Post([FromBody]TestingArea value)
        {
            var re = Request;
            var header = re.Headers;
            value.UserId = Convert.ToInt32(header["userid"]);
            int res = _TestArea.SaveData(value);
            if (res != 0)
            {
                return "Success";
            }
            return "xxx";
        }

        // PUT api/values/5
        [HttpPut("{id}"), Authorize]
        public string Put(int id, [FromBody]TestingArea value)
        {
            int res=0;
            if (id == value.TestingAreaID)
            {
                var re = Request;
                var header = re.Headers;
                value.UserId = Convert.ToInt32(header["userid"]);
                res = _TestArea.UpdateData(id, value);
                if (res != 0)
                {
                    return "Success";
                }
                return "Failure";
            }
            return "Failure";
        }

        // DELETE api/values/5
        [HttpDelete("{id}"), Authorize]
        public int Delete(int id)
        {
            int res = _TestArea.DeleteData(id);
            if (res != 0)
            {
                return res;
            }
            return res;
        }
    }
}
