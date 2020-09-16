using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.DataInterface
{
    public interface IDashboard<Dashboard, Dashboardchartdata>
    {
        IList<Dashboard> Getnoofsiteperregion(int id);
        IList<Dashboard> Getforecastcomparision(string param);
        IList<Dashboardchartdata> Getnoofinsperarea(int userid);
        IList<Dashboardchartdata> Getnooftestperarea(int userid);
        IList<Dashboardchartdata> Getnoofproductpertype(int userid);
        IList<Dashboardchartdata> Getnoofsitespercategory(int id);

        IList<Dashboard> Getnoofpatientpermonth(int id);
        IList<Dashboardchartdata> GetChartPatient(int id);
        IList<Dashboard> GetChartNooftestpertest(int id);
        IList<Dashboardchartdata> Getratiobytestarea(int id);
        IList<Dashboard> GetChartProductprice(int id);
        Array Getmonthbyforecast(int id);
        Array gettstname(int id);
        Array getproducttype(int id);
    }
}
