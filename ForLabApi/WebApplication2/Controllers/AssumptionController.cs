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
    [Authorize]
    [Route("api/[controller]/[action]")]
    [EnableCors("ApiCorsPolicy")]
    public class AssumptionController : Controller
    {


        IAssumption<Dynamiccontrol, PatientAssumption, MMGeneralAssumptionValue, TestingAssumption, patientnumberlist, TestingProtocol> _Assumption;
        public AssumptionController(IAssumption<Dynamiccontrol, PatientAssumption, MMGeneralAssumptionValue, TestingAssumption, patientnumberlist, TestingProtocol> b)
        {
            _Assumption = b;
        }
        [HttpPost, Authorize]

        public int Savelineargrowth([FromBody]patientnumberlist b)
        {
            var re = Request;
            var header = re.Headers;
            b.UserId = Convert.ToInt32(header["userid"]);
            int res = _Assumption.Savelineargrowth(b);
            if (res != 0)
            {
                return res;
            }
            return res;

        }

        [HttpPost, Authorize]
        public int saveproductgeneralassumptionvalue([FromBody]IEnumerable<MMGeneralAssumptionValue> b)
        {

            var re = Request;
            var header = re.Headers;
            b = b.Select(g => new MMGeneralAssumptionValue
            {
                PatientGroupID = g.PatientGroupID,
                CategoryID = g.CategoryID,
                SiteID = g.SiteID,
                Forecastid = g.Forecastid,
                Parameterid = g.Parameterid,
                Parametervalue = g.Parametervalue,
                ProductTypeID = g.ProductTypeID,
                TestID = g.TestID,
                Userid = Convert.ToInt32(header["userid"]),
                ID = g.ID


            }).ToList();

            int res = _Assumption.saveproductgeneralassumptionvalue(b);
            if (res != 0)
            {
                return res;
            }
            return res;
        }
        [HttpPost, Authorize]
        public int Savetestingprotocol([FromBody]IEnumerable<TestingProtocol> b)
        {
            var re = Request;
            var header = re.Headers;
            b = b.Select(g => new TestingProtocol
            {
                PatientGroupID = g.PatientGroupID,
                Baseline = g.Baseline,
                ForecastinfoID = g.ForecastinfoID,
                PercentagePanel = g.PercentagePanel,
                TestID = g.TestID,
                TotalTestPerYear = g.TotalTestPerYear,
                ID = g.ID,
                UserId = Convert.ToInt32(header["userid"])

            }).ToList();
            int res = _Assumption.Savetestingprotocol(b);
            if (res != 0)
            {
                return res;
            }
            return res;
        }
        [HttpPost, Authorize]
        public int SaveproductAssumption([FromBody]IEnumerable<TestingAssumption> b)
        {
            var re = Request;
            var header = re.Headers;
            b = b.Select(g => new TestingAssumption
            {
                ProductTypeID = g.ProductTypeID,
                ForecastinfoID = g.ForecastinfoID,
                ID = g.ID,
                UserId = Convert.ToInt32(header["userid"])
            }).ToList();
            int res = _Assumption.SaveproductAssumption(b);
            if (res != 0)
            {
                return res;
            }
            return res;
        }


        [HttpPost, Authorize]
        public int savemmgeneralassumptionvalue([FromBody]IEnumerable<MMGeneralAssumptionValue> b)
        {
            var re = Request;
            var header = re.Headers;
            b = b.Select(g => new MMGeneralAssumptionValue
            {
                Forecastid = g.Forecastid,
                CategoryID = g.CategoryID,
                ID = g.ID,
                SiteID = g.SiteID,
                TestID = g.TestID,
                Parameterid = g.Parameterid,
                Parametervalue = g.Parametervalue,
                PatientGroupID = g.PatientGroupID,
                ProductTypeID = g.ProductTypeID,
                Userid = Convert.ToInt32(header["userid"])

            }).ToList();
            int res = _Assumption.savemmgeneralassumptionvalue(b);
            if (res != 0)
            {
                return res;
            }
            return res;
        }
        [HttpPost, Authorize]
        public int savetestinggeneralassumptionvalue([FromBody]IEnumerable<MMGeneralAssumptionValue> b)
        {
            var re = Request;
            var header = re.Headers;
            b = b.Select(c => new MMGeneralAssumptionValue
            {
                CategoryID = c.CategoryID,
                TestID = c.TestID,
                Forecastid = c.Forecastid,
                Parameterid = c.Parameterid,
                Parametervalue = c.Parametervalue,
                PatientGroupID = c.PatientGroupID,
                ProductTypeID = c.ProductTypeID,
                SiteID = c.SiteID,
                ID = c.ID,
                Userid = Convert.ToInt32(header["userid"])

            }).ToList();
            int res = _Assumption.savetestinggeneralassumptionvalue(b);
            if (res != 0)
            {
                return res;
            }
            return res;
        }
        [HttpPost, Authorize]
        public int SavepatientAssumption([FromBody]IEnumerable<PatientAssumption> b)
        {
            var re = Request;
            var header = re.Headers;
            b = b.Select(c => new PatientAssumption
            {
                CategoryID = c.CategoryID,
                ForecastinfoID = c.ForecastinfoID,
                SiteID = c.SiteID,
                ID = c.ID,
                Userid = Convert.ToInt32(header["userid"])

            }).ToList();
            int res = _Assumption.SavepatientAssumption(b);
            if (res != 0)
            {
                return res;
            }
            return res;
        }
        // GET: api/values
        [HttpGet, Authorize]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet("{id}"), Authorize]
        public IList<Dynamiccontrol> GetlinearDynamiccontrol(int id)
        {
            var dynamiccontrol = _Assumption.GetlinearDynamiccontrol(id);
            return dynamiccontrol;
        }
        [HttpGet("{id}"), Authorize]
        public Array Gettestfromtestingprotocol(int id)
        {
            var tests = _Assumption.Gettestfromtestingprotocol(id);
            return tests;
        }

        [HttpGet("{id}"), Authorize]
        public DataTable GettestforecastAssumption(int id, int testid)
        {
            var parameter = _Assumption.GettestforecastAssumption(id, testid);
            return parameter;
        }
        [HttpGet("{id}"), Authorize]
        public DataTable GettestforecastAssumptionnew(int id)
        {
            var parameter = _Assumption.GettestforecastAssumptionnew(id);
            return parameter;
        }

        [HttpGet("{id}"), Authorize]
        public DataTable GettestAssumption(int id, int testid)
        {
            var parameter = _Assumption.GettestAssumption(id, testid);
            return parameter;
        }
        [HttpGet("{id}"), Authorize]
        public IList<Dynamiccontrol> GetforecastDynamiccontrol(int id, int entitytype)
        {
            var dynamiccontrol = _Assumption.GetforecastDynamiccontrol(id, entitytype);
            return dynamiccontrol;
        }

        [HttpGet("{id}"), Authorize]
        public IList<Dynamiccontrol> GetDynamiccontrol(int id, int entitytype)
        {
            var dynamiccontrol = _Assumption.GetDynamiccontrol(id, entitytype);
            return dynamiccontrol;
        }

        [HttpGet("{id}")]
        public List<string> getforecastdynamicheader(int id, int entitytype)
        {
            var dynamicheader = _Assumption.getforecastdynamicheader(id, entitytype);
            return dynamicheader;
        }
        [HttpGet("{id}")]
        public List<string> getvariablrdynamicheader(int id, int entitytype)
        {
            var dynamicheader = _Assumption.getvariablrdynamicheader(id, entitytype);
            return dynamicheader;
        }

        [HttpGet("{id}")]
        public List<string> getdynamicheader(int id, int entitytype)
        {
            var dynamicheader = _Assumption.getdynamicheader(id, entitytype);
            return dynamicheader;
        }
        [HttpGet("{id}"), Authorize]
        public Array Getlineargrowth(int id)
        {
            var parameter = _Assumption.Getlineargrowth(id);
            return parameter;

        }
        [HttpGet("{id}"), Authorize]
        public Array GetproductAssumption(int id)
        {
            var parameter = _Assumption.GetproductAssumption(id);
            return parameter;

        }

        [HttpGet("{id}"), Authorize]
        public Array GetforecastproductAssumption(int id)
        {
            var parameter = _Assumption.GetforecastproductAssumption(id);
            return parameter;
        }
        // GET api/values/5
        [HttpGet("{id}"), Authorize]
        public DataTable GetpatientAssumption(int id)
        {
            var parameter = _Assumption.GetpatientAssumption(id);
            return parameter;

        }
        [HttpGet("{id}"), Authorize]
        public DataTable GetforecastpatientAssumption(int id)
        {
            var parameter = _Assumption.GetforecastpatientAssumption(id);
            return parameter;
        }

        [HttpGet("{id}"), Authorize]
        public DataTable GettestforecastAssumptionnewbytestId(int id, string param)
        {
            var parameter = _Assumption.GettestforecastAssumptionnewbytestId(id,param);
            return parameter;
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

        public int deletetestingprotocol(int id, string param)
        {
            var res = _Assumption.deletetestingprotocol(id, param);
            return res;
        }
        public void Delete(int id)
        {
        }
    }
}
