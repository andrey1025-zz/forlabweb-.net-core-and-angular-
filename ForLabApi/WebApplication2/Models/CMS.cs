using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Models
{
    public class CMSCountry
    {
        public int Id { get; set; }
        public int countryID { get; set; }
        public int type { get; set; }
    }
    public class CMSData
    {
        public int Id { get; set; }
        public string Homesection { get; set; }
        public string homeheader { get; set; }
        public string homefunctionality { get; set; }
        public string homebenefits { get; set; }
        public string aboutusheader { get; set; }

        public string faq { get; set; }
        public string resource { get; set; }
    }

    public class cmsnew
    {
        public int Id { get; set; }
        public string hometitle { get; set; }
        public string Homedet { get; set; }

        public string videourl1 { get; set; }
        public string videotitle1 { get; set; }
        public string videourl2 { get; set; }
        public string videotitle2 { get; set; }
        public string videourl3 { get; set; }
        public string videotitle3 { get; set; }
        public string videourl4 { get; set; }
        public string videotitle4 { get; set; }
        public string faqq1 { get; set; }
        public string faqa1 { get; set; }
        public string faqq2 { get; set; }
        public string faqa2 { get; set; }
        public string faqq3 { get; set; }
        public string faqa3 { get; set; }
        public string faqq4 { get; set; }
        public string faqa4 { get; set; }
        public string faqq5 { get; set; }
        public string faqa5 { get; set; }
        public string faqq6 { get; set; }
        public string faqa6 { get; set; }
        public string AT1 { get; set; }
        public string ATS1 { get; set; }
        public string AT2 { get; set; }
        public string ATS2 { get; set; }
        public string AT3 { get; set; }
        public string ATS3 { get; set; }
        public string Contemail { get; set; }
        public string Contmobile { get; set; }
        public string contactaddress { get; set; }

        public string aturl1 { get; set; }
        public string aturl2 { get; set; }
        public string aturl3 { get; set; }
    }


    public class cmsnewlist
    {
        public int Id { get; set; }
        public string hometitle { get; set; }
        public string Homedet { get; set; }

        public string videourl1 { get; set; }
        public string videotitle1 { get; set; }
        public string videourl2 { get; set; }
        public string videotitle2 { get; set; }
        public string videourl3 { get; set; }
        public string videotitle3 { get; set; }
        public string videourl4 { get; set; }
        public string videotitle4 { get; set; }
        public string faqq1 { get; set; }
        public string faqa1 { get; set; }
        public string faqq2 { get; set; }
        public string faqa2 { get; set; }
        public string faqq3 { get; set; }
        public string faqa3 { get; set; }
        public string faqq4 { get; set; }
        public string faqa4 { get; set; }
        public string faqq5 { get; set; }
        public string faqa5 { get; set; }
        public string faqq6 { get; set; }
        public string faqa6 { get; set; }
        public string AT1 { get; set; }
        public string ATS1 { get; set; }
        public string AT2 { get; set; }
        public string ATS2 { get; set; }
        public string AT3 { get; set; }
        public string ATS3 { get; set; }
        public string Contemail { get; set; }
        public string Contmobile { get; set; }
        public string contactaddress { get; set; }

        public string aturl1 { get; set; }
        public string aturl2 { get; set; }
        public string aturl3 { get; set; }
        public List<cmsdoc> doclist { get; set; }
    }
    public class cmsdoc
    {
        public string title { get; set; }
        public string docurl { get; set; }

    }
    public class Emailcls
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
    public  class usefullresource
    {
        public string title { get; set; }
        public string docurl { get; set; }
        public string docstoragename { get; set; }
        public virtual IFormFile  files { get; set; }
}
}
