using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ForLabApi.DataInterface;
using ForLabApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForLabApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class CloudController : Controller
    {
        ICloudStorage _cloudStorage;

        public CloudController(ICloudStorage b)
        {
            _cloudStorage = b;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<string> Post([FromBody]usefullresource value)
        {
            string res = "";
            try
            {
                if(value.files !=null)
                {
                    await UploadFile(value);
                }
                res = "1";
            }
            catch (Exception ex)
            {

                res = "0";
            }
            return res;
        }

        // PUT api/<controller>/5
        [HttpPut("{title}")]
        public async Task<string> Put(string title)
        {
            IFormFile file = Request.Form.Files[0];
            usefullresource value = new usefullresource();
            string docurl = "";
            value.title = title;

            value.files = file;
            string res = "";
            try
            {
                if (value.files != null)
                {
                    docurl= await UploadFile(value);
                }
                res = docurl;
            }
            catch (Exception ex)
            {

                res = "0";
            }
            return res;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private async Task<string> UploadFile(usefullresource usefullresource)
        {
            string fileNameForStorage = FormFileName(usefullresource.title, usefullresource.files.FileName);
            usefullresource.docurl = await _cloudStorage.UploadFileAsync(usefullresource.files, fileNameForStorage);
            usefullresource.docstoragename = fileNameForStorage;

            return usefullresource.docurl;
        }

        private static string FormFileName(string title, string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            var fileNameForStorage = $"{title}-{DateTime.Now.ToString("yyyyMMddHHmmss")}{fileExtension}";
            return fileNameForStorage;
        }
    }
}
