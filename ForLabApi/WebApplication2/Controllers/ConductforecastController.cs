using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForLabApi.DataInterface;
using ForLabApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForLabApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors("ApiCorsPolicy")]
    public class ConductforecastController : Controller
    {
        IConductforecast<Costclass,ConductforecastDasboard, ConductDashboardchartdata> _Conductforecast;

        public ConductforecastController(IConductforecast<Costclass,ConductforecastDasboard, ConductDashboardchartdata> b)
        {
            _Conductforecast = b;
        }
        public class result
        {
            public string res { get; set; }
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet("{id}"), Authorize]
        public IList<ConductDashboardchartdata> GetProducttypecostratio(int id, int ForecastID)
        {
            var productcostratio = _Conductforecast.GetProducttypecostratio(id, ForecastID);
            return productcostratio;
        }

        [HttpGet("{id}"), Authorize]
        public Costclass getcostparameter(int id)
        {
            var costcenter = _Conductforecast.getcostparameter(id);
            return costcenter;
        }


        [HttpGet("{id}"), Authorize]
        public Costclass getdemocostparameter(int id)
        {
            var costcenter = _Conductforecast.getdemocostparameter(id);
            return costcenter;
        }

        [HttpGet("{id}"), Authorize]
        public Array Getdistinctdurationservice(int id)
        {
            var durationlist = _Conductforecast.Getdistinctdurationservice(id);
            return durationlist;
        }
        [HttpGet("{id}"), Authorize]
        public Array Getdistinctdurationnew(int id)
        {
            var durationlist = _Conductforecast.Getdistinctdurationnew(id);
            return durationlist;
        }
        [HttpGet("{id}"), Authorize]
        public Array Getdistinctduration(int id, int ForecastID)
        {
            var durationlist = _Conductforecast.Getdistinctduration(id, ForecastID);
            return durationlist;
        }
        [HttpGet("{id}"), Authorize]
        public IList<ConductforecastDasboard> Getforecastsummarydurationforsite(int id, int ForecastID)
        {
            var forecastlist = _Conductforecast.Getforecastsummarydurationforsite(id, ForecastID);
            return forecastlist;
        }

        [HttpGet("{id}"), Authorize]
        public IList<ConductforecastDasboard> Getforecastsummarydurationforsiteservice(int id)
        {
            var forecastlist = _Conductforecast.Getforecastsummarydurationforsiteservice(id);
            return forecastlist;

        }
        [HttpGet("{id}"), Authorize]
        public IList<ConductforecastDasboard> Getforecastsummarydurationforsitenew(int id)
        {
            var forecastlist = _Conductforecast.Getforecastsummarydurationforsitenew(id);
            return forecastlist;
        }
        [HttpGet("{id}"), Authorize]
        public IList<ConductDashboardchartdata> GettestingareacostratioNEW(int id)
        {
            var forecastlist = _Conductforecast.GettestingareacostratioNEW(id);
            return forecastlist;
        }

        [HttpGet("{id}"), Authorize]
        public IList<ConductDashboardchartdata> GetProducttypecostratioNEW(int id)
        {
            var forecastlist = _Conductforecast.GetProducttypecostratioNEW(id);
            return forecastlist;
        }

        [HttpGet("{id}"), Authorize]
        public IList<ConductDashboardchartdata> GetdemoProducttypecostratioNEW(int id)
        {
            var forecastlist = _Conductforecast.GetdemoProducttypecostratioNEW(id);
            return forecastlist;
        }

        [HttpGet("{id}"), Authorize]
        public IList<ConductforecastDasboard> Getforecastsummarydurationforcategory(int id, int ForecastID)
        {
            var forecastlist = _Conductforecast.Getforecastsummarydurationforcategory(id, ForecastID);
            return forecastlist;
        }

        [HttpGet("{id}"), Authorize]
        public IList<ConductDashboardchartdata> GetProducttypecostratiocategory(int id, int ForecastID)
        {
            var productcostratio = _Conductforecast.GetProducttypecostratiocategory(id, ForecastID);
            return productcostratio;
        }
        // GET api/<controller>/5
        [HttpGet("{metho}"), Authorize]

        public Array Getforecastlist(string metho, string datausage)
        {
            var re = Request;
            var header = re.Headers;
      
            var forecastlist = _Conductforecast.Getforecastlist(metho, datausage, Convert.ToInt32(header["userid"]));
            return forecastlist;
        }
        [HttpGet("{id}"), Authorize]
        public result Calculateforecast(int id, string MethodType)
            {
            var result = _Conductforecast.Calculateforecast(id, MethodType);

            var res1 = new result
            {
                res = result
            };
            return res1;

          
        }

        [HttpGet("{id}")]
        public Array Getforecastsite(int id)
        {
            var forecastlist = _Conductforecast.Getforecastsite(id);
            return forecastlist;
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
