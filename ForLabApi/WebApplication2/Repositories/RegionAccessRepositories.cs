using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForLabApi.DataInterface;
using ForLabApi.Models;
namespace ForLabApi.Repositories
{
    public class RegionAccessRepositories:IRegion<Region>
    {
        ForLabContext ctx;
        public RegionAccessRepositories(ForLabContext c)
        {
            ctx = c;
            //return ctx;
        }
        public int SaveData(Region b)
        {
            var region = ctx.Region.FirstOrDefault(c => c.RegionName == b.RegionName);
            if (region != null)
            {
                return 0;
            }
            ctx.Region.Add(b);
            int res = ctx.SaveChanges();
            return res;
        }

        public int DeleteData(int id)
        {
            int res = 0;
            var Region = ctx.Region.FirstOrDefault(b => b.RegionID == id);
            if (Region != null)
            {
                try
                {


                    ctx.Region.Remove(Region);
                    ctx.SaveChanges();
                    res = id;
                }
                catch(Exception ex)
                {

                }
            }
            return res;
        }

        public Region Getbyid(int id)
        {
            var region = ctx.Region.FirstOrDefault(b => b.RegionID == id);
            return region;
        }

        public IEnumerable<Region> GetAll()


        {
            var regions = ctx.Region.OrderByDescending(x=>x.RegionID).ToList();
            return regions;
        }

        public int UpdateData(int id, Region b)
        {
            int res = 0;
            var Regioninfo = ctx.Region.Where(c=>c.RegionID==b.RegionID).First();
            if (Regioninfo != null)
            {
                var region1 = ctx.Region.Where(c => c.RegionName == b.RegionName && c.CountryId==b.CountryId).FirstOrDefault();
                if (region1==null)
                {
                    Regioninfo.RegionName = b.RegionName;
                    Regioninfo.CountryId = b.CountryId;
                    ctx.SaveChanges();
                    res = id;
                }
              
            }
            return res;
        }

        public Region GetRegionByName(string name)
        {
            var region = ctx.Region.FirstOrDefault(b => b.RegionName == name);
            return region;
        }
    }
}

