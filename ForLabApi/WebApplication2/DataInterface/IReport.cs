using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.DataInterface
{
    public interface IReport<columnname, Dynamicarray>
    {
        Array Getregionlist(int noofsites,string logic);
        Dynamicarray Getsitelist(int regionid, int categoryid);
        DataTable Getcomparisionsummarydata(string param);
        Array Getsiteinstruentlist(string param);
        Array Getinstrumentlist(int araeid);

        Array GetProductUsagelist(string param);
        Array Gettestlist(int areaid);
        Array Getproductlist(int proid);
        Array getdynamicctrl();
        Array Getproductpricelist(int typeid);
        IList<columnname> getcolumnname();
        Array Getconsumptionsummarynew(int id);

        Dynamicarray Getconsumptionsummary(int id);
        Dynamicarray Getservicesummary(int id);

        Dynamicarray Getdemographicsummary(int id);


        Dynamicarray Getnoofpatientsummary(int id);
        Array GetForecastdescription(int id);


    }
}
