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
    [EnableCors("ApiCorsPolicy")]
    public class ForecsatinfoController : Controller
    {
        IForcastInfo<Siteinsvalidation, ForecastInfo, ForecastSiteInfoList, ForecastSiteInfonew, ForecastCategoryInfo, ForecastCategoryInfoList, ForecastCategorySiteInfo,PatientGroup, Test, ForecastCategorySiteInfolist, ForecastInstrumentlist, ForecastProductUsageDetail, ForecastConsumableUsageDetail, forecastusagesmodel> _Forecastinfo;
        public ForecsatinfoController(IForcastInfo<Siteinsvalidation, ForecastInfo, ForecastSiteInfoList, ForecastSiteInfonew, ForecastCategoryInfo, ForecastCategoryInfoList, ForecastCategorySiteInfo, PatientGroup, Test, ForecastCategorySiteInfolist, ForecastInstrumentlist, ForecastProductUsageDetail, ForecastConsumableUsageDetail, forecastusagesmodel> b)
        {
            _Forecastinfo = b;
        }

        public class aaData1
        {
            public string forecasttype { get; set; }
        }
        public class result
        {
            public string forecastlockstatus { get; set; }
        }
        // GET: api/values
        [HttpGet, Authorize]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet("{id}"), Authorize]
        public IEnumerable<PatientGroup> Getpatientgroupbyforecastid(int id,string programid)
        {
            var patientgroup = _Forecastinfo.Getpatientgroupbyforecastid(id,programid);
            return patientgroup;
        }
        [HttpGet("{id}"), Authorize]
        public IEnumerable<PatientGroup> Getpatientgroupbydemoforecastid(int id, string programid)
        {
            var patientgroup = _Forecastinfo.Getpatientgroupbydemoforecastid(id, programid);
            return patientgroup;
        }
        [HttpGet("{id}"), Authorize]
        public List<ForecastProductUsageDetail> getControlProductUsage(int id, int testid)
        {

            var forecastuages = _Forecastinfo.getControlProductUsage(id, testid);
            return forecastuages;
        }
        [HttpPost,Authorize]
        public void saveforecastusges([FromBody]forecastusagesmodel FUM)
        {
            _Forecastinfo.saveforecastusges(FUM);
        }
        [HttpGet("{id}"), Authorize]
        public List<ForecastProductUsageDetail> getForecastProductUsage(int id, int testid)
        {
            var forecastuages = _Forecastinfo.getForecastProductUsage(id, testid);
            return forecastuages;
        }
        [HttpGet("{id}"), Authorize]
        public List<ForecastConsumableUsageDetail> getForecastConsumbleUsagePertest(int id, int testid)
        {
            var forecastuages = _Forecastinfo.getForecastConsumbleUsagePertest(id, testid);
            return forecastuages;
        }
        [HttpGet("{id}"), Authorize]
        public List<ForecastConsumableUsageDetail> getForecastConsumbleUsagePerPeriod(int id, int testid)
        {
            var forecastuages = _Forecastinfo.getForecastConsumbleUsagePerPeriod(id, testid);
            return forecastuages;
        }
        [HttpGet("{id}"), Authorize]
        public List<ForecastConsumableUsageDetail> getForecastConsumbleUsagePerinstrument(int id, int testid)
        {

            var forecastuages = _Forecastinfo.getForecastConsumbleUsagePerinstrument(id, testid);
            return forecastuages;
        }
        [HttpGet("{id}"), Authorize]
        public int getgroupexistintestingprotocol(int id, int groupid)
        {
            int groupcnt = _Forecastinfo.getgroupexistintestingprotocol(id, groupid);
            return groupid;
        }
        [HttpGet("{id}"), Authorize]
        public decimal Gettotaltargetpatient(int id, string programid)
        {
            decimal TotalTargetpatient = _Forecastinfo.Gettotaltargetpatient(id, programid);
            return TotalTargetpatient;
        }
        [HttpGet("{id}"), Authorize]
        public int Getprogramid(int id)
        {
            var programid = _Forecastinfo.getprogramid(id);
            return programid;
        }
        [HttpGet("{id}"), Authorize]
        public IEnumerable<ForecastSiteInfonew> Getbyforecastid(int id)
        {
            var forecastinfo = _Forecastinfo.Getbyforecastid(id);
            return forecastinfo;
        }
        [HttpGet("{id}"), Authorize]
        public IEnumerable<ForecastCategoryInfo> Getcategoryinfobyforecastid(int id)
        {
            var forecastcategoryinfo = _Forecastinfo.Getcategoryinfobyforecastid(id);
            return forecastcategoryinfo;
        }
        [HttpGet("{id}"), Authorize]
        public IEnumerable<ForecastCategorySiteInfolist> Getcategorysiteinfobyforecastid(int id,int categoryid)
        {
            var re = Request;
            var header = re.Headers;
            var ForecastCategorySiteInfo = _Forecastinfo.Getcategorysiteinfobyforecastid(id, Convert.ToInt32(header["userid"]),categoryid);
            return ForecastCategorySiteInfo;
        }
        // GET api/values/5
        [HttpGet("{id}"), Authorize]
        public ForecastInfo Getbyid(int id)
        {
            var forecastinfo = _Forecastinfo.Getbyid(id);
            return forecastinfo;
        }
        [HttpGet("{id}"), Authorize]

        public int Isdataimported(int id)
        {
            var forecastinfo = _Forecastinfo.Isdataimported(id);
            return forecastinfo;
        }
        [HttpGet("{id}"), Authorize]
        public Array getrecentforecast(int id)
        {
            var re = Request;
            var header = re.Headers;
            var recentforecast = _Forecastinfo.getrecentforecast( id, Convert.ToInt32(header["userid"]), Convert.ToString(header["role"]));
            return recentforecast;
        }

        [HttpPost,Authorize]
        public int savecategorysiteinfo([FromBody]IEnumerable<ForecastCategorySiteInfolist> b, int userid)
        {
            var re = Request;
            var header = re.Headers;
           
            int res = _Forecastinfo.savecategorysiteinfo(b, Convert.ToInt32(header["userid"]));
            if (res != 0)
            {
                return res;
            }
            return res;

        }
        [HttpGet("{id}"), Authorize]
        [ActionName("getallinstrumentbyforecasttest")]
        public List<ForecastInstrumentlist> getallinstrumentbyforecasttest(int id)
        {
            var re = Request;
            var header = re.Headers;
            var Instrumentlist = _Forecastinfo.getallinstrumentbyforecasttest(id, Convert.ToInt32(header["userid"]), Convert.ToString(header["role"]));
            return Instrumentlist;
        }
        // POST api/values
        [HttpPost, Authorize]
        public int saveforecastinfo([FromBody]ForecastInfo value)
        {
            var re = Request;
            var header = re.Headers;
            value.UserId = Convert.ToInt32(header["userid"]);
            int res = _Forecastinfo.saveforecastinfo(value);
            if (res != 0)
            {
                return res;
            }
            return res;
        }
        [HttpGet("{id}"), Authorize]
        public result lockforecastinfo(int id)
        {
            var lockforecast = _Forecastinfo.lockforecastinfo(id);
            var res = new result
            {
                forecastlockstatus = lockforecast

        };
            return res;
        }


        [HttpGet("{id}"), Authorize]
        public IEnumerable<Siteinsvalidation> vaidationforsiteinstrument(int id)
        {

            //string res = "";
            //return res;
            var validate = _Forecastinfo.Validationforsiteinstrument(id);
            return validate;
        }
        [HttpPost, Authorize]
        public int savepatientgroup([FromBody]IEnumerable<PatientGroup> b)
        {
            var re = Request;
            var header = re.Headers;
            b = b.Select(c => new PatientGroup
            {
                ForecastinfoID=c.ForecastinfoID,
                GroupID=c.GroupID,
                ID  =c.ID,
                PatientGroupName=c.PatientGroupName,
                PatientPercentage=c.PatientPercentage,
                PatientRatio=c.PatientRatio,
                UserId= Convert.ToInt32(header["userid"])

        }).ToList();
            int res = _Forecastinfo.savepatientgroup(b);
            if (res != 0)
            {
                return res;
            }
            return res;
        }
        [HttpPost, Authorize]
        public int saveforecastsiteinfo([FromBody]ForecastSiteInfoList value)
        {
            var re = Request;
            var header = re.Headers;
            
            int res = _Forecastinfo.saveforecastsiteinfo(value);
            if (res != 0)
            {
                return res;
            }
            return res;
        }
        [HttpPost, Authorize]
        public int saveforecastcategoryinfo([FromBody]ForecastCategoryInfoList value)
        {
            int res = _Forecastinfo.saveforecastcategoryinfo(value);
            if (res != 0)
            {
                return res;
            }
            return res;
        }
        [HttpDelete("{ids}"), Authorize]
        public IActionResult Delforecastsiteinfo(string ids)
        {
           
                if (ids != "")
                {
                    int res = _Forecastinfo.delforecastsiteinfo(ids);
                    if (res != 0)
                    {
                        return Ok();
                    }
                }
                return NotFound();
          
        }
        [HttpDelete("{ids}"), Authorize]
        public IActionResult delforecastcategoryinfo(string ids)
        {

            if (ids != "")
            {
                int res = _Forecastinfo.delforecastcategoryinfo(ids);
                if (res != 0)
                {
                    return Ok();
                }
            }
            return NotFound();

        }
        [HttpDelete("{id}"), Authorize]
        public IActionResult delpatientgroup(int id, int groupid)
        {
          
                int res = _Forecastinfo.delpatientgroup(id,groupid);
                if (res != 0)
                {
                    return Ok();
                }
            
            return NotFound();
        }
        [HttpDelete("{ids}"), Authorize]
        public IActionResult delforecastcategorysiteinfo(string ids)
        {

            if (ids != "")
            {
                int res = _Forecastinfo.delforecastcategorysiteinfo(ids);
                if (res != 0)
                {
                    return Ok();
                }
            }
            return NotFound();

        }
        [HttpGet("{id}"), Authorize]
        public aaData1 getforecasttype(int id)
        {
            var type = _Forecastinfo.getforecasttype(id);
            var gq = new aaData1
            {
                forecasttype = type

            };
            return gq;
          
        }
        // PUT api/values/5
        [HttpPut("{id}"), Authorize]
        public void Put(int id, [FromBody]string value)
        {
            _Forecastinfo.updateforecast(id, value);
        }
        [HttpPut("{id}"), Authorize]
        public void Put01(int id, [FromBody]string value)
        {
            _Forecastinfo.updateforecast01(id, value);
        }
        [HttpPut("{id}"), Authorize]
        public void Put02(int id, [FromBody]string value)
        {
            _Forecastinfo.updateforecast02(id, value);
        }
        [HttpPut("{id}"), Authorize]
        public  int updateprogram(int id, [FromBody]int programid)
        {
            var fid = _Forecastinfo.updateprogram(id, programid);
            return fid;
        }
        // DELETE api/values/5
        [HttpDelete("{id}"), Authorize]
        public void Delete(int id)
        {
        }

    }
}
