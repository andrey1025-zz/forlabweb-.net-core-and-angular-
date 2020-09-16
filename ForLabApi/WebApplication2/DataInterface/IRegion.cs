using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.DataInterface
{
    public interface IRegion<Region>
    {
        IEnumerable<Region> GetAll();
        Region Getbyid(int id);
        int SaveData(Region b);
        int UpdateData(int id, Region b);
        int DeleteData(int id);
        Region GetRegionByName(string name);
    }
}
