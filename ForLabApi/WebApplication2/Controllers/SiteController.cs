using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ForLabApi.DataInterface;
using ForLabApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForLabApi.Controllers
{
    [Route("api/[controller]/[action]")]
   
    public class SiteController : Controller
    {
              // GET: api/values
      
        IForLabSite<Site,sitebyregion,Region, defaultsite,SiteInstrumentList> _Site;
     
      
        public SiteController(IForLabSite<Site,sitebyregion,Region, defaultsite, SiteInstrumentList> b)
        {
            _Site = b;
          
        }
        public class aaData1
        {
            public IEnumerable<sitebyregion> aaData { get; set; }
        }
        [HttpGet("{id}"), Authorize]
        [ActionName("GetregionbycategoryID")]
        public IEnumerable<Region> GetregionbycategoryID(int id,int id2)
        {
            var region = _Site.GetregionbycategoryID(id,id2, Convert.ToInt32(Request.Headers["userid"]));

            return region;
        }
        [HttpGet,Authorize]
        public IEnumerable<SiteInstrumentList> Getdefaultsiteinstrument(int countryid)
        {
            var siteins = _Site.Getdefaultsiteinstrument(Convert.ToInt32(Request.Headers["countryid"]));
            return siteins;
        }
        [HttpGet, Authorize]
        public defaultsite Getdefaultdata()
        {
            var defaultsite = _Site.Getdefaultdata(Convert.ToInt32(Request.Headers["countryid"]));
            return defaultsite;
        }
        [HttpGet("{id}"), Authorize]
        [ActionName("GetregionbyCountryID")]
        public Array GetregionbyCountryID(int id)
        {
         
            var regions= _Site.GetregionbyCountryID(id, Convert.ToInt32(Request.Headers["userid"]), Convert.ToString(Request.Headers["role"]));
            return regions;
        }
        [HttpGet("{id}"), Authorize]
        [ActionName("GetSitebyReg")]
        public IEnumerable<sitebyregion> GetSitebyReg(int id)
        {
            var sitebyregion = _Site.GetAllSiteByRegionId(id);
           
            return sitebyregion;
        }

        [HttpGet("{regionids}"), Authorize]
        [ActionName("GetSitebyregions")]
        public IEnumerable<Site> GetSitebyregions(string regionids)
        {
            var sites = _Site.GetSitebyregions(regionids);
            return sites;
        }


        // GET: api/values
        [HttpGet("{id}"), Authorize]
        [ActionName("GetAll")]
        public aaData1 GetAll(int id)
        {
            var Site = _Site.GetAll(id, Convert.ToInt32(Request.Headers["userid"]), Convert.ToString(Request.Headers["role"]));
            var gq = new aaData1
            {
                aaData = Site

            };
            return gq;

        }

        // GET api/values/5
        [HttpGet("{id}"), Authorize]
        [ActionName("GetbyId")]
        public Site Get(int id)
        {
            var Region = _Site.Getbyid(id);
            return Region;
        }

        // POST api/values
        [HttpPost, Authorize]
        [ActionName("Post01")]
        public string Post([FromBody]Site value)
        {
            value.UserId = Convert.ToInt32(Request.Headers["userid"]);
            int res = _Site.SaveData(value);
            if (res != 0)
            {
                return "Success";
            }
            return "Duplicate Site name";
        }

        // PUT api/values/5
        [HttpPut("{id}"), Authorize]
        [ActionName("Put01")]
        public IActionResult Put(int id, [FromBody]Site value)
        {
            if (id == value.SiteID)
            {
                int res = _Site.UpdateData(id, value);
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
        [ActionName("Del01")]
        public IActionResult Delete(int id)
        {
            int res = _Site.DeleteData(id);
            if (res != 0)
            {
                return Ok();
            }
            return NotFound();
        }


        // DELETE api/values/5
        [HttpDelete("{ids}"), Authorize]
        [ActionName("Deletesiteinstrument")]
        public IActionResult Deletesiteinstrument(string ids)
        {
            if (ids != "")
            {
                int res = _Site.Deletesiteinstrument(ids);
                if (res != 0)
                {
                    return Ok();
                }
            }
            return NotFound();
        }

        [HttpDelete("{ids}"), Authorize]
        [ActionName("Deletesitetestingdays")]
        public IActionResult Deletesitetestingdays(string ids)
        {
            if (ids != "")
            {
                int res = _Site.Deletesitetestingdays(ids);
                if (res != 0)
                {
                    return Ok();
                }
            }
            return NotFound();
        }


        [HttpDelete("{ids}"), Authorize]
        [ActionName("Deletereferrallink")]
        public IActionResult Deletereferrallink(string ids)
        {
            if (ids != "")
            {
                int res = _Site.Deletereferrallink(ids);
                if (res != 0)
                {
                    return Ok();
                }
            }
            return NotFound();
        }

        [HttpGet("{keyword}"), Authorize]
        [ActionName("GetSitebykeyword")]
        public IEnumerable<sitebyregion> GetSitebykeyword(string keyword)
        {
            var sitelist = _Site.GetSitebykeyword(keyword);
            return sitelist;
        }
        [HttpGet("{keyword}"), Authorize]
        [ActionName("GetSitebykeywordtypes")]
        public IEnumerable<sitebyregion> GetSitebykeywordtypes(string keyword, string type)
        {
            var productlist = _Site.GetSitebykeywordtypes(keyword, type);
            return productlist;

        }
     
        [HttpGet]
       public Array Getcountrylist()
        {
            var countrylist = _Site.Getcountrylist();
            return countrylist;
        }

    }
}
