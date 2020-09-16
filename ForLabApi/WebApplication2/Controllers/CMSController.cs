using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ForLabApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForLabApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors("ApiCorsPolicy")]
    public class CMSController : Controller
    {
        ForLabContext ctx;
        private IHostingEnvironment _hostingEnvironment;
        public CMSController(ForLabContext c, IHostingEnvironment hostingEnvironment)
        {
            ctx = c;
            _hostingEnvironment = hostingEnvironment;
            //return ctx;
        }

        // GET: api/<controller>
        [HttpGet]
        public cmsnew Get()
        {
            return ctx.cmsnew.FirstOrDefault();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [HttpPost]
        public void Post()
        {

            IFormFile file = Request.Form.Files[0];


            string folderName = "Upload";
            string str = "";
            var re = Request;
            var header = re.Headers;


            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            StringBuilder sb = new StringBuilder();
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }

            if (file.Length > 0)
            {
                string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                string fullPath = Path.Combine(newPath, fileName);
                // string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                //string fullPath = Path.Combine(newPath, file.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }



        }
        // POST api/<controller>
        //[HttpPost]
        //public void Post([FromBody]CMSData value)
        //{
        //    var cms = ctx.CMSData.FirstOrDefault();
        //    if (cms !=null)
        //    {
        //        cms.Homesection = value.Homesection;
        //        ctx.SaveChanges();
        //    }
        //}

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]cmsnewlist value)
        {
            var cms = ctx.cmsnew.Find(id);
            if (cms != null)
            {
                cms.hometitle = value.hometitle;
                cms.Homedet = value.Homedet;

                cms.videourl1 = value.videourl1;
                cms.videotitle1 = value.videotitle1;
                cms.videourl2 = value.videourl2;
                cms.videotitle2 = value.videotitle2;
                cms.videourl3 = value.videourl3;
                cms.videotitle3 = value.videotitle3;
                cms.videourl4 = value.videourl4;
                cms.videotitle4 = value.videotitle4;
                cms.faqq1 = value.faqq1;
                cms.faqa1 = value.faqa1;
                cms.faqq2 = value.faqq2;
                cms.faqa2 = value.faqa2;
                cms.faqq3 = value.faqq3;
                cms.faqa3 = value.faqa3;
                cms.faqq4 = value.faqq4;
                cms.faqa4 = value.faqa4;
                cms.faqq5 = value.faqq5;
                cms.faqa5 = value.faqa5;
                cms.faqq6 = value.faqq6;
                cms.faqa6 = value.faqa6;
                cms.AT1 = value.AT1;
                cms.ATS1 = value.ATS1;
                cms.AT2 = value.AT2;
                cms.ATS2 = value.ATS2;
                cms.AT3 = value.AT3;
                cms.ATS3 = value.ATS3;
                cms.Contemail = value.Contemail;
                cms.Contmobile = value.Contmobile;
                cms.aturl1 = value.aturl1;
                cms.aturl2 = value.aturl2;
                cms.aturl3 = value.aturl3;
                cms.contactaddress = value.contactaddress;




                ctx.SaveChanges();
            }
            //else
            //{
            //    ctx.cmsnew.Add(value);
            //    ctx.SaveChanges();
            //}
        }
        [HttpPost]
        public string sendmail([FromBody]Emailcls value)
        {
            string str = "";
            try
            {

            
                string htmlMessage = "";
                string subject = "";
               


                    htmlMessage = "Dear Sir/ Mam, <br><br>" + value.Message + "<br>Thanks,<br>The ForLab+ Team ";
                    //   htmlMessage = "Dear Sir/ Mam, <br><br> You are successfully registered on ForLab.To Activate your account Please <a href='https://www.google.com' style= 'width:140px;background:linear-gradient(to bottom,#007fb8 1%,#6ebad5 3%,#007fb8 7%,#007fb8 100%);background-color:#007fb8;text-align:center;border:#004b91 solid -1px;padding:4px 0;text-decoration:none;border-radius:2px;display:block;color:#fff;font-size:13px'> Click here</a><br>Thanks.<br>ForLab Team ";

                    // htmlMessage = "Dear Sir/ Mam, <br><br> You are successfully registered on ForLab.To Activate your account Please <a href='http://localhost:4200/#/verifylink/" + id + "' style= 'width:140px;background:linear-gradient(to bottom,#007fb8 1%,#6ebad5 3%,#007fb8 7%,#007fb8 100%);background-color:#007fb8;text-align:center;border:#004b91 solid -1px;padding:4px 0;text-decoration:none;border-radius:2px;display:block;color:#fff;font-size:13px'> Click here</a><br>Thanks.<br>ForLab Team ";
                    subject = "New Enquiry";
              

                SmtpClient client = new SmtpClient("smtp.gmail.com");
                client.Port = 587;
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential("admin@forlabplus.com", "ZtcTetkS");



                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("admin@forlabplus.com");
                mailMessage.To.Add("contact@opiantech.com");
                mailMessage.Body = htmlMessage;
                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                mailMessage.SubjectEncoding = System.Text.Encoding.Default;
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;

                client.Send(mailMessage);

                //var client = new SmtpClient("smtp.gmail.com", 587)
                //{
                //    Credentials = new NetworkCredential("jyotiawasthi198902@gmail.com", "jyoti@123"),
                //    EnableSsl = false
                //};
                //client.SendMailAsync(
                //    new MailMessage("jyotiawasthi198902@gmail.com", b.Email, "Verification Eamil for ForLab", htmlMessage) { IsBodyHtml = true }
                //);

                str = "success";

                return str;
            }
            catch (Exception ex)
            {
                return "Something went wrong";
            }
        }


        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
