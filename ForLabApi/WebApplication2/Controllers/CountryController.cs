using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForLabApi.DataInterface;
using ForLabApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForLabApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors("ApiCorsPolicy")]
    public class CountryController : Controller
    {
        ICountry<CountryList, MastDiseases, Countrylistusedortrained>  _country;
        public CountryController(ICountry<CountryList, MastDiseases, Countrylistusedortrained> b)
        {
            _country = b;
        }
        // GET: api/<controller>
        [HttpGet]
        public List<Countrylistusedortrained> Countrylistusedortraine()
        {
            var countrylist = _country.Countrylistusedortraine();
            return countrylist;
        }
        [HttpGet]
        public List<MastDiseases> GetMastDiseaseslist()
        {
            var Diseaseslist = _country.GetMastDiseaseslist();
            return Diseaseslist;
        }
        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [HttpGet("{id}")]
        public CountryList getcountrydatabyid(int Id)
        {
            var Diseaseslist = _country.getcountrydatabyid(Id);
            return Diseaseslist;

        }
        [HttpPost]
        public string  Importhistoricaldata()
        {
            IFormFile file = Request.Form.Files[0];
            string str = "";
            var re = Request;
            var header = re.Headers;

            str = _country.Importhistoricaldata(file);
            return str;
        }
        [HttpPost]
        public string Savecountry([FromBody]CountryList CL)
        {
            string res = "";
            try
            {
                _country.Savecountry(CL);
                res = "1";
            }
            catch (Exception ex)
            {

                res = "0";
            }
            return res;
                
           
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
