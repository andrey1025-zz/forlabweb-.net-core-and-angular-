using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.DataInterface
{
    public interface IConsumption<Datausagewithcontrol, siteproductno, adjustedvolume>
    {
        Datausagewithcontrol GetDataUasge(int id, string param);
        Datausagewithcontrol GetAdjustedVolume(int id,adjustedvolume A);
        string Addcategory(int id, string param);
        string deletecategorydatausage(int id);
        string deletesiteformcategory(int id, string param);
        string Addnonrportedsites(int id, string param);
        string Addsiteincategory(int id, string param);
        Array Getforecastsite(int forecastid);
        Array GetcategoryList(int forecastid);
        string Deleteconsumption(int id);

        string Deleteservicestatistic(int id);

        Array Getcategorysite(int catid);
        Array Getforecastnonreportedsite(int forecastid,int siteid);
        Datausagewithcontrol Addactualconsumption(int id, string param);
        Datausagewithcontrol Bindforecastsiteproduct(int id, int siteid);
        string removestefromdatausage(int siteid, int id);
        siteproductno Gettotalsiteandproduct(int id);
        string Removedatausagefromsite(int id, string param);

    }
}
