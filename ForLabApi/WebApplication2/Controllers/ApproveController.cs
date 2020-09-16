using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForLabApi.DataInterface;
using ForLabApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForLabApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ApproveController : Controller
    {
        IApprove<Approve,PendingApprovelist> _Approve;
        public ApproveController(IApprove<Approve, PendingApprovelist> b)
        {
            _Approve = b;
        }
        // GET: api/<controller>
        [HttpGet, Authorize]
        public List<PendingApprovelist> getpendingapprovallist()
        {
            var pendingapprove = _Approve.getpendingapprovallist();
            return pendingapprove;
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost, Authorize]
        public int Post([FromBody]Approve value)
        {
            var res = _Approve.approvalmasterdata(value);
            return res;
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
