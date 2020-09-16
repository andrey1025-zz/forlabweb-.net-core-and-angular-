using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.DataInterface
{
     public interface IForLabSite<Site, sitebyregion, Region, defaultsite, SiteInstrumentList>
    {
        IEnumerable<sitebyregion> GetAll(int id,int userid,string role);
        Site Getbyid(int id);
        int SaveData(Site b);
        int UpdateData(int id, Site b);
        int DeleteData(int id);
        Array Getcountrylist();
        int Deletesiteinstrument(string ids);
        int Deletesitetestingdays(string ids);

        int Deletereferrallink(string ids);

        IEnumerable<SiteInstrumentList> Getdefaultsiteinstrument(int countryid);
       IEnumerable<sitebyregion> GetAllSiteByRegionId(int regionid);

        IEnumerable<Site> GetSitebyregions(string regionsid);
        IEnumerable<sitebyregion> GetSitebykeyword(string keyword);

        IEnumerable<sitebyregion> GetSitebykeywordtypes(string keyword, string type);

        IEnumerable<Region> GetregionbycategoryID(int id,int id2,int userid);
        Array GetregionbyCountryID(int id,int userid,string role);
        defaultsite Getdefaultdata(int countryid);
        //Site GetSiteByName(string name, int regionid);

        //IList<Site> GetReferingSiteByPlatform(string platform);
        //IList<Site> GetReferingSiteByPlatform(int siteId, int platform);
        //bool GetRefSiteBySiteId(int siteId, string platform);
        //IList<Site> GetAllSiteByRegionandPlatform(int regionid, string platform);
        //void deleteReferingSite(int siteId, string platform);

        //Site GetSiteByName(string sname);
        //IList<int> GetListOfReferedSites(int siteId, string platform);
        //IList<int> GetListOfSiteInstruments(int instId);
    }
}
