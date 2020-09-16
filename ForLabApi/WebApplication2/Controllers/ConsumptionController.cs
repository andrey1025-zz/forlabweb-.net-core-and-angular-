using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ForLabApi.DataInterface;
using System.Collections;
using ForLabApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForLabApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors("ApiCorsPolicy")]
    public class ConsumptionController : Controller
    {
        IConsumption<Datausagewithcontrol, siteproductno, adjustedvolume> _Consumption;
        public ConsumptionController(IConsumption<Datausagewithcontrol, siteproductno, adjustedvolume> b)
        {
            _Consumption = b;
        }

        public class aaData1
        {
            public string removedatausage { get; set; }
        }
        [HttpGet("{id}"), Authorize]
        public Array Getcategorysite(int id)
        {

            var category = _Consumption.Getcategorysite(id);
            return category;
        }
        [HttpGet("{id}"), Authorize]
        public Array GetcategoryList(int id)
        {
            var category = _Consumption.GetcategoryList(id);
            return category;
        }
        [HttpGet("{id}"), Authorize]
        public Datausagewithcontrol Bindforecastsiteproduct(int id, int siteorcatid)
        {
            var forecastsite = _Consumption.Bindforecastsiteproduct(id, siteorcatid);
            return forecastsite;
        }
        [HttpDelete("{id}"), Authorize]
        public string removestefromdatausage(int id, int siteid)
        {

            var forecastsite = _Consumption.removestefromdatausage(id, siteid);

            return forecastsite;

        }
        [HttpGet("{id}"), Authorize]
        public siteproductno Gettotalsiteandproduct(int id)
        {
            var totalnoofsite = _Consumption.Gettotalsiteandproduct(id);
            return totalnoofsite;
        }

        [HttpGet("{id}"), Authorize]
        public Array Getforecastsite(int id)
        {
            var forecastsite = _Consumption.Getforecastsite(id);
            return forecastsite;
        }
        [HttpGet("{id}"), Authorize]
        public Array Getforecastnonreportedsite(int id, int siteid)
        {
            var nonreportedsites = _Consumption.Getforecastnonreportedsite(id, siteid);
            return nonreportedsites;
        }

        [HttpGet("{id}"), Authorize]
        public aaData1 Addsiteincategory(int id, string param)
        {
            var addsite = _Consumption.Addsiteincategory(id, param);
            var gq = new aaData1
            {
                removedatausage = addsite

            };
            return gq;
        }
        [HttpGet("{id}"), Authorize]
        public aaData1 Addnonrportedsites(int id, string param)
        {
            var addnonreportedsite = _Consumption.Addnonrportedsites(id, param);
            var gq = new aaData1
            {
                removedatausage = addnonreportedsite

            };
            return gq;
        }
        [HttpGet("{id}"), Authorize]
        public aaData1 Removedatausagefromsite(int id, string param)
        {
            var datausagelist = _Consumption.Removedatausagefromsite(id, param);
            var gq = new aaData1
            {
                removedatausage = datausagelist

            };
            return gq;

        }
        [HttpGet("{id}"), Authorize]
        public Datausagewithcontrol Addactualconsumption(int id, string param)
        {
            var datausagelist = _Consumption.Addactualconsumption(id, param);
            return datausagelist;
        }
        [HttpGet("{id}"), Authorize]
        public Datausagewithcontrol GetDataUasge(int id, string param)
        {
            var datausagelist = _Consumption.GetDataUasge(id, param);
            return datausagelist;
        }
        [HttpGet("{id}"), Authorize]
        public aaData1 Addcategory(int id, string param)
        {
            var addcategory = _Consumption.Addcategory(id, param);
            var gq = new aaData1
            {
                removedatausage = addcategory

            };
            return gq;
        }
        // GET: api/values
        [HttpGet, Authorize]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}"), Authorize]
        public string Get(int id)
        {
            return "value";
        }
        [HttpPut("{id}"), Authorize]
        public Datausagewithcontrol GetAdjustedVolume(int id,[FromBody]adjustedvolume A)
        {
            string STR1 = "";
            var deletestatus = _Consumption.GetAdjustedVolume(id,A);
            return deletestatus;
        
        }
        // POST api/values
        [HttpPost, Authorize]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}"), Authorize]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}"), Authorize]
        public string deletesiteformcategory(int id, string param)
        {
            var deletestatus = _Consumption.deletesiteformcategory(id, param);
            return deletestatus;
        }

        [HttpDelete("{id}"), Authorize]
        public string Deleteservicestatistic(int id)
        {
            var deleteconsumption = _Consumption.Deleteservicestatistic(id);
            return deleteconsumption;
        }
        [HttpDelete("{id}"), Authorize]
        public string Deleteconsumption(int id)
        {
            var deleteconsumption = _Consumption.Deleteconsumption(id);
            return deleteconsumption;
        }

        [HttpDelete("{id}"),Authorize]
        public string deletecategorydatausage(int id)
        {
            var deletestatus = _Consumption.deletecategorydatausage(id);
            return deletestatus;
        }
    }
    }
