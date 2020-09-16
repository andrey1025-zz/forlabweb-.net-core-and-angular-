using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ForLabApi.Models;
using ForLabApi.DataInterface;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForLabApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors("ApiCorsPolicy")]
    [Authorize]
    public class DasboardController : Controller
    {
        // GET: api/values
        IDashboard<Dashboard, Dashboardchartdata> _Dashboard;
        public DasboardController(IDashboard<Dashboard, Dashboardchartdata> b)
        {
            _Dashboard = b;
        }

        [HttpGet("{id}")]
        public IList<Dashboard> Getnoofpatientpermonth(int id)
        {
            var noofpatientpermonth = _Dashboard.Getnoofpatientpermonth(id);
            return noofpatientpermonth;
        }
        // GET: api/values
        [HttpGet("{id}")]
        public IList<Dashboard> Getnoofsiteperregion(int id)
        {
            var dashboardsite = _Dashboard.Getnoofsiteperregion(id);
            return dashboardsite;
        }
        [HttpGet("{id}")]
        public Array gettstname(int id)
        {
            var montharray = _Dashboard.gettstname(id);
            return montharray;
        }
        [HttpGet("{id}")]
        public Array getproducttype(int id)
        {
            var productarray = _Dashboard.getproducttype(id);
            return productarray;
        }
        [HttpGet("{id}")]
        public Array Getmonthbyforecast(int id)
        {
            var montharray = _Dashboard.Getmonthbyforecast(id);
            return montharray;
        }
        [HttpGet]
        public IList<Dashboardchartdata> Getnoofproductpertype()
        {
            var noofproduct = _Dashboard.Getnoofproductpertype(Convert.ToInt32(Request.Headers["userid"]));
            return noofproduct;
        }
        [HttpGet]
        public IList<Dashboardchartdata> Getnoofinsperarea()
        {
            var nooftest = _Dashboard.Getnoofinsperarea(Convert.ToInt32(Request.Headers["userid"]));
            return nooftest;
        }
        
        [HttpGet]
        public IList<Dashboardchartdata> GetNooftestperarea()
        {
            var nooftest = _Dashboard.Getnooftestperarea(Convert.ToInt32(Request.Headers["userid"]));
            return nooftest;
        }
        [HttpGet("{id}")]
        public IList<Dashboardchartdata> Getnoofsitespercategory(int id)
        {
            var noofsite = _Dashboard.Getnoofsitespercategory(id);
            return noofsite;
        }
        // GET api/values/5
        [HttpGet("{id}")]
        public IList<Dashboard> GetChartNooftest(int id)
        {
            var nooftest = _Dashboard.GetChartNooftestpertest(id);
            return nooftest;
        }
        [HttpGet("{id}")]
        public IList<Dashboardchartdata> Getratiobytestarea(int id)
        {
            var ratio = _Dashboard.Getratiobytestarea(id);
            return ratio;
        }
        [HttpGet("{id}")]


        public IList<Dashboard> Getforecastcomparision(string id)
        {
            var comparision= _Dashboard.Getforecastcomparision(id);
            return comparision;
        }
        [HttpGet("{id}")]
        public IList<Dashboardchartdata> GetChartPatient(int id)
        {
            var noofpatient = _Dashboard.GetChartPatient(id);
            return noofpatient;
        }
        [HttpGet("{id}")]
        public IList<Dashboard> GetChartProductprice(int id)
        {
            var productprice= _Dashboard.GetChartProductprice(id);
            return productprice;
        }
        public string Get(int id)
        {
            return "value";
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
