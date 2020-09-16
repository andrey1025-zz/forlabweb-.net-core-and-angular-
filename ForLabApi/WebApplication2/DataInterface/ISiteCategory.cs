using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForLabApi.Models;

namespace ForLabApi.DataInterface
{
    public interface ISiteCategory<SiteCategory>
    {
        IEnumerable<SiteCategory> GetAll(int userid,int countryid,string Role);
        SiteCategory Getbyid(int id);
        int SaveData(SiteCategory b);
        int UpdateData(int id, SiteCategory b);
        int DeleteData(int id);
        SiteCategory GetSiteCategoryByName(string name);
      //  IEnumerable<Group> Getgroup();
    }

}
