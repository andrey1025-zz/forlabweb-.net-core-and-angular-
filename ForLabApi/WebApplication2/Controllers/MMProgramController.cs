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
    [Route("api/[controller]/[action]")]
    //[EnableCors("ApiCorsPolicy")]
    public class MMProgramController : Controller
    {

        IMMProgram<MMProgramList,demographicMMGeneralAssumption, DemographicMMGroup, DemographicMMGroupList,MMProgram,  ForecastInfoList, ForecastInfo, MMForecastParameterList, MMGroupList, MMGeneralAssumptionList, MMGroup, Suggustionlist> _MMProgram;
        public MMProgramController(IMMProgram<MMProgramList,demographicMMGeneralAssumption, DemographicMMGroup, DemographicMMGroupList,MMProgram, ForecastInfoList, ForecastInfo, MMForecastParameterList, MMGroupList, MMGeneralAssumptionList, MMGroup, Suggustionlist> b)
        {
            _MMProgram = b;
        }
        [HttpGet]
        public IEnumerable<MMProgram> GetAllbyadmin()
        {

            var MMProgram = _MMProgram.GetAllbyadmin();
            return MMProgram;
        }
        [HttpGet]
        public List<MMProgramList> Getprogramlist()
        {
            var MMProgram = _MMProgram.Getprogramlist(Convert.ToInt32(Request.Headers["userid"]));
            return MMProgram;
        }
        // GET: api/values
        [HttpGet] 
        public IEnumerable<MMProgram> Get()
        {
            var MMProgram = _MMProgram.GetAll(Convert.ToInt32(Request.Headers["userid"]));
            return MMProgram;
        }
        [HttpGet, Authorize]
       
        public IEnumerable<MMGeneralAssumptionList> GetGeneralAssumptionList()
        {
            var generalaasumption = _MMProgram.GetGeneralAssumptionList();
            return generalaasumption;
        }



        [HttpGet, Authorize]
     
        public IEnumerable<MMGroupList> Getpatientgroup()
        {
            var patientgroup = _MMProgram.Getpatientgroup();
            return patientgroup;
        }

        [HttpGet, Authorize]
       
        public IEnumerable<MMForecastParameterList> Getforecastparameter()
        {
            var parameterlist = _MMProgram.Getforecastparameter();
            return parameterlist;
        }

        [HttpGet("{id}"), Authorize]
     
        public IEnumerable<MMForecastParameterList> Getforecastparameterbyprogramid(int id)
        {
            var parameterlist = _MMProgram.Getforecastparameterbyprogramid(id);
            return parameterlist;
        }


        [HttpGet("{id}"), Authorize]

        public IEnumerable<MMForecastParameterList> Getforecastparameterbyforecastid(int id)
        {
            var parameterlist = _MMProgram.Getforecastparameterbyforecastid(id);
            return parameterlist;
        }

        [HttpGet("{id}"), Authorize]
      
        public IEnumerable<ForecastInfoList> Getforecastinfobyprogramid(int id)

        {
            var re = Request;
            var header = re.Headers;
            var demograhicmorbidity1 = _MMProgram.Getforecastinfobyprogramid(id, Convert.ToInt32(header["userid"]));
            return demograhicmorbidity1;
        }

        [HttpGet("{metho}"), Authorize]     

        public IEnumerable<ForecastInfoList> GetForecastInfoByMethodology(string metho,int CID)
        {
            var re = Request;
            var header = re.Headers;
            var demograhicmorbidity = _MMProgram.GetForecastInfoByMethodology(metho, Convert.ToInt32(header["userid"]));
            return demograhicmorbidity;
        }

        [HttpGet("{forecastid}"), Authorize]

        public IEnumerable<DemographicMMGroupList> Getpatientgroupforforecast(int id, int forecastid)
        {
            var patientgroup = _MMProgram.Getpatientgroupforforecast(id,forecastid);
            return patientgroup;
        }


        [HttpGet("{forecastid}"), Authorize]

        public IEnumerable<demographicMMGeneralAssumption> GetDemographicMMGeneralAssumptions(string param, int forecastid)
        {
            var patientgroup = _MMProgram.GetDemographicMMGeneralAssumptions(param, forecastid);
            return patientgroup;
        }
        // GET api/values/5
        [HttpGet("{id}"), Authorize]
        public string Get(int id)
        {
            return "value";
        }
        [HttpGet(), Authorize]
        public IEnumerable<Suggustionlist> Getsuggustionlist()
        {
            var re = Request;
            var header = re.Headers;
            IEnumerable<Suggustionlist> list = _MMProgram.Getsuggustionlist(Convert.ToInt32(header["userid"]));
            return list;
        }
        [HttpPost, Authorize]
        public void saveforecastmmgroup([FromBody]IEnumerable<DemographicMMGroup> b)
        {
            _MMProgram.saveforecastmmgroup(b);

        }

        [HttpPost, Authorize]
        public void saveDemographicMMGeneralAssumptions([FromBody]IEnumerable<demographicMMGeneralAssumption> b)
        {
            _MMProgram.saveDemographicMMGeneralAssumptions(b);
        }
        [HttpPost, Authorize]
        public int savesuggustion([FromBody]Suggustionlist value)
        {
            var re = Request;
            var header = re.Headers;
       value.UserId = 0;
            int res = _MMProgram.savesuggustion(value);
            if (res != 0)
            {
                return res;
            }
            return 0;
        }
        // POST api/values
        [HttpPost, Authorize]
       
        public int SaveProgram([FromBody]MMProgram value)
        {
            var re = Request;
            var header = re.Headers;
            value.UserId = Convert.ToInt32(header["userid"]);
            int res = _MMProgram.SaveProgram(value);
            if (res != 0)
            {
                return res;
            }
            return 0;
        }
        [HttpPost, Authorize]
      
        public int Saveforecastparameter([FromBody]MMProgram value)
        {
            var re = Request;
            var header = re.Headers;
            value.UserId = Convert.ToInt32(header["userid"]);

            int res = _MMProgram.Saveforecastparameter(value);
            if (res != 0)
            {
                return res;
            }
            return 0;
        }
        [HttpPost, Authorize]
        
        public int savegeneralassumptions([FromBody]MMGeneralAssumptionList value)
        {
            int res = _MMProgram.savegeneralassumptions(value);
            if (res != 0)
            {
                return res;
            }
            return 0;

        }


        [HttpPost, Authorize]
        public int savegroup([FromBody]MMGroup value)
        {
            int res = _MMProgram.savegroup(value);
            if (res != 0)
            {
                return res;
            }
            return 0;
        }

        // PUT api/values/5
        [HttpPut("{id1}"), Authorize]       
        public IActionResult updategroup(int id1, [FromBody]MMGroupList value1)
        {
            if (id1 == value1.Id)
            {
                int res = _MMProgram.updategroup(id1, value1);
                if (res != 0)
                {
                    return Ok(res);
                }
                return NotFound();
            }
            return NotFound();
        }

        [HttpPut("{id}"), Authorize]
        public int updatedemographicprogram(int id ,[FromBody]int months)
        {
            var updateprogram = _MMProgram.updatedemographicprogram(id,months);
            return updateprogram;
        }
        [HttpPut("{id}"), Authorize]
        public IActionResult updateProgram(int id, [FromBody]MMProgram value)
        {
            if (id == value.Id)
            {
                int res = _MMProgram.updateProgram(id, value);
                if (res != 0)
                {
                    return Ok(res);
                }
                return NotFound();
            }
            return NotFound();
        }
        [HttpPut("{id}"), Authorize]
     
        public IActionResult updategeneralassumptions(int id,[FromBody]MMGeneralAssumptionList value)
        {
            if (id == value.Id)
            {
                int res = _MMProgram.updategeneralassumptions(id, value);
                if (res != 0)
                {
                    return Ok(res);
                }
                return NotFound();
            }
            return NotFound();
        }
        // DELETE api/values/5
        [HttpDelete("{id}"), Authorize]
        public void Delete(int id)
        {
            MMProgram m = new MMProgram();
         
        }
       
    }
}
