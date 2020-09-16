using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.DataInterface
{
    public interface IConductforecast<Costclass,ConductforecastDasboard, ConductDashboardchartdata>
    {
        Array Getforecastlist(string metho, string datausage,int userid);
        string Calculateforecast(int id, string MethodType);
        Costclass getcostparameter(int id);
        Costclass getdemocostparameter(int id);
        Array Getforecastsite(int id);
        IList<ConductforecastDasboard> Getforecastsummarydurationforsiteservice(int fid);
        IList<ConductforecastDasboard> Getforecastsummarydurationforsitenew(int fid);
    IList<ConductforecastDasboard> Getforecastsummarydurationforsite(int id, int fid);
        IList<ConductDashboardchartdata> GetProducttypecostratio(int id, int fid);
        IList<ConductDashboardchartdata> GettestingareacostratioNEW(int fid);
        IList<ConductDashboardchartdata> GetProducttypecostratioNEW(int fid);

        IList<ConductDashboardchartdata> GetdemoProducttypecostratioNEW(int fid);

        IList<ConductforecastDasboard> Getforecastsummarydurationforcategory(int id, int fid);
        IList<ConductDashboardchartdata> GetProducttypecostratiocategory(int id, int fid);
        Array Getdistinctduration(int id, int fid);
        Array Getdistinctdurationnew(int fid);

        Array Getdistinctdurationservice(int fid);

    }
    }
