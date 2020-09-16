using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Models
{


    public class User
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Organization { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public int? CountryId { get; set; }
        public int? logincnt { get; set; }
        public int? GlobalRegionId { get; set; }
        public Boolean? Emailverify { get; set; }
    }
    public class EmailSender
    {
        public string host { get; set; }
        public int port { get; set; }
        public bool enableSSL { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
    }
    public class updatepassword
    {
        public string newpassword { get; set; }
    }
}
