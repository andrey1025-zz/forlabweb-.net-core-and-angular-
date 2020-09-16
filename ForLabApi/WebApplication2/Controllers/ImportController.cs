using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using ForLabApi.Models;
using System.IO;
using System.Net.Http.Headers;
using System.Text;

using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Drawing;
using ForLabApi.Utility;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using System.Globalization;
using ForLabApi.DataInterface;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForLabApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors("ApiCorsPolicy")]
    public class ImportController : Controller
    {
  
        private IImport<receivereportdata,Reportobject, ForecastSiteInfonew> _import;
        public ImportController(IImport<receivereportdata,Reportobject, ForecastSiteInfonew> importservice)
        {
            _import = importservice;
        }


        public class responsemsg
        {
            public string msg { get; set; }
        }
        [HttpPut("{id}"),Authorize]
        public DataTable importservice([FromBody]List<string[]> jArray, int id)
        {
            var re = Request;
            var header = re.Headers;
            var service = _import.importservice(jArray, id, Convert.ToInt32(header["userid"]));
            return service;
        }

        [HttpPost]
        public string saveimportservice([FromBody]Reportobject _rdata)
        {
            var service = _import.saveimportservice(_rdata);
            return service;
        }
        [HttpGet("{id}")]

        public Array getimporteddata(int id)
        {
            var service = _import.getimporteddata(id);
            return service;
        }
        [HttpPost,Authorize]
        public string saveimportconsumption([FromBody]List<receivereportdata> _rdata)
        {
            var service = _import.saveimportconsumption(_rdata);
            return service;
        }

        [HttpGet]
        public void importdatafromwho()
        {
           _import.importdatafromwho();
        }

        [HttpPut("{id}"), Authorize]
        public DataTable importconsumption([FromBody]List<string []> jArray ,int id)
        {
            var re = Request;
            var header = re.Headers;
            var consumption = _import.importconsumption(jArray, id, Convert.ToInt32(header["userid"]));
            return consumption;
        }
        [HttpGet("{id}"), Authorize]
        public Array getimportedservicedata(int id)
        {
            var consumption = _import.getimportedservicedata(id);
            return consumption;
        }
        [HttpPut("{id}"), Authorize]
        public IEnumerable<ForecastSiteInfonew> Importpatient( int id)
        {
            IFormFile file = Request.Form.Files[0];
            string str = "";
            var re = Request;
            var header = re.Headers;

            IEnumerable<ForecastSiteInfonew> ss = _import.Importpatient(file, id, Convert.ToInt32(header["userid"]), Convert.ToInt32(header["countryid"]));
            return ss;
        }


        [HttpPost("{sheets}"), Authorize]
        public responsemsg Uploadfile(string sheets)
        {

            IFormFile file = Request.Form.Files[0];

     
            string folderName = "Upload";
            string str = "";
            var re = Request;
            var header = re.Headers;
           
            str = _import.importexcel(file, Convert.ToInt32(header["userid"]), Convert.ToInt32(header["countryid"]),sheets);
            
            var msg = new responsemsg
            {
                msg = str
            };
            return msg;
        
    }


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
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
