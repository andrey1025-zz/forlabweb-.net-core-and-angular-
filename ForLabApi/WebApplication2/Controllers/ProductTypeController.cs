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
    public class ProductTypeController : Controller
    {
        
        IProducttype<ProductType> _ProductType;
        public ProductTypeController(IProducttype<ProductType> b)
        {
            _ProductType = b;
        }
        public class aaData1
        {
            public IEnumerable<ProductType> aaData { get; set; }
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
            var ProductType = _ProductType.GetAll(userid,role);
            var gq = new aaData1
            {
                aaData = ProductType

            };
            return gq;

        }
     
        // GET api/values/5
        [HttpGet("{id}"), Authorize]
        public ProductType Get(int id)
        {
            var ProductType = _ProductType.Getbyid(id);
            return ProductType;
        }

        // POST api/values
        [HttpPost, Authorize]
        public string Post([FromBody]ProductType value)
        {
            var re = Request;
            var header = re.Headers;
            value.UserId = Convert.ToInt32(header["userid"]);
            int res = _ProductType.SaveData(value);
            if (res != 0)
            {
                return "Success";
            }
            return "xxx";
        }

        // PUT api/values/5
        [HttpPut("{id}"), Authorize]
        public string Put(int id, [FromBody]ProductType value)
        {
            if (id == value.TypeID)
            {
                int res = _ProductType.UpdateData(id, value);
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
            int res = _ProductType.DeleteData(id);
            if (res != 0)
            {
                return res;
            }
            return res;
        }
    }
}
