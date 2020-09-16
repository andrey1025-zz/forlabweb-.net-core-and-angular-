using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ForLabApi.Models;
using ForLabApi.DataInterface;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForLabApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("ApiCorsPolicy")]
    public class SiteCategoryController : Controller
    {
  
      ISiteCategory<SiteCategory> _SiteCategoryRepo;
      
        public SiteCategoryController(ISiteCategory<SiteCategory> b)
        {
            _SiteCategoryRepo = b;
        }
        public class aaData1
        {
            public IEnumerable<SiteCategory> aaData { get; set; }
        }
        public class result1
        {
            public IEnumerable<Group> result { get; set; }
        }

        // GET: api/values
        [HttpGet]
        public aaData1 Get()
        {
            var userid = 0;
            int countryid = 0;
            var role = "";
            if (Request.Headers["userid"] == "")
            {

            }
            else
            {
                userid = Convert.ToInt32(Request.Headers["userid"]);
                role = Convert.ToString(Request.Headers["role"]);
            }
            countryid= Convert.ToInt32(Request.Headers["countryid"]);
            var SiteCategories = _SiteCategoryRepo.GetAll(userid,countryid,role);
            var gq = new aaData1
            {
                aaData = SiteCategories

            };
         
      return gq;
        }
        //[HttpGet]
        //public result1 Getgroup()
        //{
        //    var SiteCategories = _SiteCategoryRepo.Getgroup();
        //    var gq = new result1
        //    {
        //        result = SiteCategories

        //    };

        //    return gq;

        //}

        // GET api/values/5
        [HttpGet("{id}"), Authorize]
        public SiteCategory Get(int id)
        {
            var SiteCategory = _SiteCategoryRepo.Getbyid(id);
            return SiteCategory;
        }

        // POST api/values
        [HttpPost, Authorize]
        public int Post([FromBody]SiteCategory value)
        {
            var re = Request;
            var header = re.Headers;
            value.UserId = Convert.ToInt32(header["userid"]);
            int res = _SiteCategoryRepo.SaveData(value);
            if (res != 0)
            {
                return res;
            }
            else if (res == 0)
            {
                return res;
            }
            return res;
        }

        // PUT api/values/5
        [HttpPut("{id}"), Authorize]
        public string Put(int id, [FromBody]SiteCategory value)
        {
            int res = 0;
            if (id == value.CategoryID)
            {
                var re = Request;
                var header = re.Headers;
                value.UserId = Convert.ToInt32(header["userid"]);
                res = _SiteCategoryRepo.UpdateData(id, value);
                if (res != 0)
                {
                    return "Success";
                }
                return "faliure";
            }
            return "faliure";
        }

        // DELETE api/values/5
        [HttpDelete("{id}"), Authorize]
        public int Delete(int id)
        {
            int res = _SiteCategoryRepo.DeleteData(id);
            if (res != 0)
            {
                return res;
            }
            return res;
        }
    }
}
