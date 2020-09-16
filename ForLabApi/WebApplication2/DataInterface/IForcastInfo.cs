using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.DataInterface
{
    public interface IForcastInfo<siteinsvalidation,ForecastInfo, ForecastSiteInfoList, ForecastSiteInfonew, ForecastCategoryInfo, ForecastCategoryInfoList, ForecastCategorySiteInfo, PatientGroup, Test, ForecastCategorySiteInfolist, ForecastInstrumentlist, ForecastProductUsageDetail, ForecastConsumableUsageDetail, forecastusagesmodel>
    {
        int saveforecastinfo(ForecastInfo b);
        void updateforecast(int forecastid, string type);
        void updateforecast01(int forecastid, string type);
        void updateforecast02(int forecastid, string data);

        void saveforecastusges(forecastusagesmodel FUM);
        List<ForecastProductUsageDetail> getForecastProductUsage(int forecastid, int testid);

        List<ForecastProductUsageDetail> getControlProductUsage(int forecastid, int testid);
        List<ForecastConsumableUsageDetail> getForecastConsumbleUsagePertest(int forecastid, int testid);

        List<ForecastConsumableUsageDetail> getForecastConsumbleUsagePerPeriod(int forecastid, int testid);
        List<ForecastConsumableUsageDetail> getForecastConsumbleUsagePerinstrument(int forecastid, int testid);
        List<ForecastInstrumentlist> getallinstrumentbyforecasttest(int Forecstid, int userid,string Role);
        int saveforecastsiteinfo(ForecastSiteInfoList b);
        IEnumerable<PatientGroup> Getpatientgroupbydemoforecastid(int id, string programid);
        int updateprogram(int id, int programid);
        int saveforecastcategoryinfo(ForecastCategoryInfoList b);
        int savepatientgroup(IEnumerable<PatientGroup> b);
        string getforecasttype(int id);
        int getprogramid(int id);
        int delforecastsiteinfo(string ids);
        int delforecastcategoryinfo(string ids);
        string lockforecastinfo(int id);
        int delforecastcategorysiteinfo(string ids);
        int delpatientgroup(int id, int groupid);
        int getgroupexistintestingprotocol(int id, int groupid);
        decimal Gettotaltargetpatient(int id, string programid);
        ForecastInfo Getbyid(int id);
        IEnumerable<ForecastSiteInfonew> Getbyforecastid(int id);
        IEnumerable<PatientGroup> Getpatientgroupbyforecastid(int id, string programid);
        IEnumerable<siteinsvalidation> Validationforsiteinstrument(int id);
        IEnumerable<ForecastCategoryInfo> Getcategoryinfobyforecastid(int id);
        IEnumerable<ForecastCategorySiteInfolist> Getcategorysiteinfobyforecastid(int id, int userid,int categoryid);
        int Isdataimported(int id);
        Array getrecentforecast(int id, int userid, string Role);
        int savecategorysiteinfo(IEnumerable<ForecastCategorySiteInfolist> b,int userid);
    }
}
