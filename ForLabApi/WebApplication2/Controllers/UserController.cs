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
    public class UserController : Controller
    {
        // GET: api/values
        private IUserService<User,Importdata, updatepassword,GlobalRegion> _userService;

        public UserController(IUserService<User, Importdata, updatepassword,GlobalRegion> userService)
        {
            _userService = userService;
        }

        public class result
        {
            public string res { get; set; }
        }

        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<GlobalRegion> Getallglobalregions()
        {
            var res = _userService.Getallglobalregions();
            return res;
        }

        [AllowAnonymous]
        [HttpPost]
        public User Authenticate([FromBody]User userParam)


        {
            var user = _userService.Authenticate(userParam.UserName, userParam.Password);

            if (user == null)
                return user;

            return user;
        }

        [AllowAnonymous]
        [HttpPost]
        public string[] Saveuser([FromBody]User b)
        {
            var res = _userService.savedata(b);
            return res;
        }
        [AllowAnonymous]
        [HttpPost]
        public string Importdefaultdata([FromBody]Importdata b)
        {
            var res = _userService.Importdefaultdata(b);
            return res;
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public int updatelogincount(int id)

        {

             _userService.updatelogincount(id);
            return id;

        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public int Verifyaccount(int id)
        {
            var res = _userService.verifyaccount(id);
            return res;
        }
        [AllowAnonymous]
        [HttpPut("{id}")]
        public int Updatepassword(int id, [FromBody]updatepassword password)
        {
            var res = _userService.updateuserpassword(id,password);
            return res;

        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public result Getglobalregion(int id)
        {
            var res11 = _userService.Getglobalregion(id);
            var res1 = new result
            {
                res = res11
            };
            return res1;
        }
        [AllowAnonymous]
        [HttpGet("{email}")]
        public result Resetpassword(string email)
        {
            var res11 = _userService.Resetpassword(email);
            var res1 = new result
            {
                res = res11
            };
            return res1;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }
}
