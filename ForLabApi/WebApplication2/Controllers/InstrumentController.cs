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
    public class InstrumentController : Controller
    {
        // GET: api/values
        IInstrument<Instrument,InstrumentList, getinstrument, ForecastIns, ForecastInsmodel, forecastinslist> _Instrument;
        public InstrumentController(IInstrument<Instrument, InstrumentList, getinstrument, ForecastIns, ForecastInsmodel, forecastinslist> b)
        {
            _Instrument = b;
        }
        public class aaData1
        {
            public IEnumerable<InstrumentList> aaData { get; set; }
        }

      
        [HttpGet, Authorize]
        public List<InstrumentList> GetdefaultdataIns()
        {
            var Instrumentlist = _Instrument.GetdefaultdataIns();
            return Instrumentlist;

        }
        // GET: api/values
        [HttpGet, Authorize]
        [ActionName("GetAll")]
        public aaData1 Get()
        {
            var userid = 0;
            var role = "";
            if (Request.Headers["userid"] == "")
            {

            }
            else
            {
                userid = Convert.ToInt32(Request.Headers["userid"]);
                role = Convert.ToString(Request.Headers["role"]);
            }
            var Instrument = _Instrument.GetAll(userid,role);
            var gq = new aaData1
            {
                aaData = Instrument

            };
            return gq;

        }

        // GET api/values/5
        [HttpGet("{id}"), Authorize]
        [ActionName("GetbyId")]
        public Instrument Get(int id)
        {
            var Instrument = _Instrument.Getbyid(id);
            return Instrument;
        }

        [HttpGet("{id}"), Authorize]
        public List<forecastinslist> getAllforecastinstrumentlist(int id)
        {
            var Instrument = _Instrument.getAllforecastinstrumentlist(id);
            return Instrument;
        }
        // POST api/values
        [HttpPost, Authorize]
        [ActionName("Post01")]
        public string Post([FromBody]getinstrument value)
        {
            var re = Request;
            var header = re.Headers;
            value.UserId = Convert.ToInt32(header["userid"]);
            int res = _Instrument.SaveData(value);
            if (res != 0)
            {
                return "Success";
            }
            return "xxx";
        }
        [HttpPut("{id}"), Authorize]
        public int saveforecastIns(int id,[FromBody]IEnumerable<ForecastIns> b)
        {
            int i = 0;
            _Instrument.saveforecastIns(id,b);
            return i;
        }
        [HttpPost, Authorize]
        public int Updateforecastinstrument([FromBody]ForecastInsmodel b)
        {
            int i = 0;
            _Instrument.Updateforecastinstrument(b);
            return i;
        }
        // PUT api/values/5
        [HttpPut("{id}"), Authorize]
        [ActionName("Put01")]
        public string Put(int id, [FromBody]getinstrument value)
        {
            if (id == value.InstrumentID)
            {
                var re = Request;
                var header = re.Headers;
                value.UserId = Convert.ToInt32(header["userid"]);
                int res = _Instrument.UpdateData(id, value);
                if (res != 0)
                {
                    return "Success";
                }
                return "Failure";
            }
            return "Failure";
        }

        // DELETE api/values/5
        [HttpDelete("{id}"), Authorize]
        [ActionName("Del01")]
        public int Delete(int id)
        {
            int res = _Instrument.DeleteData(id);
            if (res != 0)
            {
                return res;
            }
            return res;
        }
        [HttpGet("{id}"), Authorize]
        [ActionName("getInsbyareaid")]
        public IEnumerable<InstrumentList> GetInsbyareaid(int id)
        {
            var Instrument = _Instrument.GetListOfInstrumentByTestingArea(id);
           
            return Instrument;

        }
    }
}
