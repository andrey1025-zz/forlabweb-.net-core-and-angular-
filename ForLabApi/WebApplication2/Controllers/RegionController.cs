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
    [Route("api/[controller]")]
    [EnableCors("ApiCorsPolicy")]
    public class RegionController : Controller
    {
        IRegion<Region> _Region;
        public RegionController(IRegion<Region> b)
        {
            _Region = b;
        }
        public class aaData1
        {
            public IEnumerable<Region> aaData { get; set; }
        }

        // GET: api/values
        [HttpGet, Authorize]
        public aaData1 Get()
        {
            var Region = _Region.GetAll();
            var gq = new aaData1
            {
                aaData = Region

            };
            return gq;

        }

        // GET api/values/5
        [HttpGet("{id}"), Authorize]
        public Region Get(int id)
        {
            var Region = _Region.Getbyid(id);
            return Region;
        }

        // POST api/values
        [HttpPost, Authorize]
        public string Post([FromBody]Region value)
        {
            var re = Request;
            var header = re.Headers;
            value.UserId = Convert.ToInt32(header["userid"]);
            int res = _Region.SaveData(value);
            if (res != 0)
            {
                return "Success";
            }
            return "xxx";
        }

        // PUT api/values/5
        [HttpPut("{id}"), Authorize]
        public string Put(int id, [FromBody]Region value)
        {
            if (id == value.RegionID)
            {
                int res = _Region.UpdateData(id, value);
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
            int res = _Region.DeleteData(id);
            if (res != 0)
            {
                return res;
            }
            return res;
        }
    }
}
