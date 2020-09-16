using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ForLabApi.DataInterface;
using ForLabApi.Models;
using System.Data;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForLabApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors("ApiCorsPolicy")]
    public class ReportController : Controller
    {
        IReport<columnname, Dynamicarray > _Report;


        
        public ReportController(IReport<columnname, Dynamicarray> b)
        {
            _Report = b;
        }
        public class aaData1
        {
            public Array aaData { get; set; }
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet("{id}")]
        public Array Getinstrumentlist(int id)
        {
            var res = _Report.Getinstrumentlist(id);
            return res;
        }
        [HttpGet("{id}")]

        public Array Getconsumptionsummarynew(int id)

        {
            var res = _Report.Getconsumptionsummarynew(id);
            return res;
        }

        [HttpGet("{id}")]
        public Dynamicarray Getconsumptionsummary(int id)
        {

            var res = _Report.Getconsumptionsummary(id);
            return res;
        }
        [HttpGet("{id}")]
        public Dynamicarray Getservicesummary(int id)
        {

            var res = _Report.Getservicesummary(id);
            return res;
        }
        [HttpGet("{id}")]
        public aaData1 Getproductpricelist(int id)
        {
            var res = _Report.Getproductpricelist(id);
            var gq = new aaData1
            {
                aaData = res
            };
            return gq;
        }
        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [HttpGet("{id}")]
        public Array GetForecastdescription(int id)
        {
            var arr = _Report.GetForecastdescription(id);
            return arr;
        }
        [HttpGet("{id}")]
        public Dynamicarray Getnoofpatientsummary(int id)
        {
            var arr = _Report.Getnoofpatientsummary(id);
            return arr;
        }
        [HttpGet("{id}")]
        public DataTable Getforecastcomparision(string id)
        {
            var comparision = _Report.Getcomparisionsummarydata(id);

            return comparision;
        }
        [HttpGet("{id}")]
        public Dynamicarray Getdemographicsummary(int id)
        {
            var arr = _Report.Getdemographicsummary(id);
            return arr;
        }
        [HttpGet("{id}")]
        public aaData1 Gettestlist(int id)
        {
            var res = _Report.Gettestlist(id);
            var gq = new aaData1
            {
                aaData = res

            };
            return gq;
        }
        [HttpGet]
        public Array GetProductUsagelist(string param)
        {
            var res = _Report.GetProductUsagelist(param);
            var gq = new aaData1
            {
                aaData = res

            };
            return res;
        }
        [HttpGet]
        public Array Getsiteinstruentlist(string param)
        {
            var res = _Report.Getsiteinstruentlist(param);
            var gq = new aaData1
            {
                aaData = res

            };
            return res;

           

        }
        [HttpGet("{noofsites}")]
        public aaData1 Getregionlist(int noofsites, string logic)
        {
            var res = _Report.Getregionlist(noofsites, logic);
            var gq = new aaData1
            {
                aaData = res

            };
            return gq;
          
        }
        [HttpGet]
       public IList<columnname> getcolumnname()
        {
            var columnname = _Report.getcolumnname();
            return columnname;
        }
        [HttpGet("{id}")]
        public aaData1 Getproductlist(int id)
        {
            var dynamicctrl = _Report.Getproductlist(id);
            var gq = new aaData1
            {
                aaData = dynamicctrl

            };
            return gq;
        }
        [HttpGet]
       public Array getdynamicctrl()
        {
            var dynamicctrl = _Report.getdynamicctrl();
            return dynamicctrl;
        }
        [HttpGet("{id}")]
        public Dynamicarray Getsitelist(int id, int categoryid)
        {
            var sitelist = _Report.Getsitelist(id, categoryid);
            //var gq = new aaData1
            //{
            //    aaData = sitelist

            //};
            return sitelist;
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
