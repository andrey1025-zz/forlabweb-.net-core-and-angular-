using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ForLabApi.DataInterface;
using ForLabApi.Models;
using System.Xml;
using System.Net.Http;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForLabApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("ApiCorsPolicy")]
    public class GroupController : Controller
    {
        IGroup<Group> _group;

        public GroupController(IGroup<Group> b)
        {
            _group = b;
        }
        public class result12
        {
            public string result { get; set; }
        }

        public class result1
        {
            public IEnumerable<Group> result { get; set; }
        }
        // GET: api/values
        [HttpGet]
        //[Produces("application/xml")]
        public result1 Get()
        {
            var gq = new result1
            {
                result = _group.GetGroup()

            };

            return gq;

        }

     

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
       
        public string Post([FromBody]Group group)
        {
            //var doc = new XmlDocument();
            //doc.Load(request.Content.ReadAsStreamAsync().Result);
            //   xml.LoadXml(value); // suppose that myXmlString contains "<Names>...</Names>"

            var savestatus = "rrrrrrrrrrrr";
            //var gq = new result12
            //{
            //    result =_group.SaveGroup(value)

            //};

            return savestatus;
          
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
