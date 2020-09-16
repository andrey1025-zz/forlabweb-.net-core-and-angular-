using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForLabApi.DataInterface;
using ForLabApi.Models;


namespace ForLabApi.Repositories
{
  
    public class DataAccessRepositories :ISiteCategory<SiteCategory>
    { 
        ForLabContext ctx;
        public DataAccessRepositories(ForLabContext c)
        {
            ctx = c;
            //return ctx;
        }
        public int SaveData(SiteCategory b)
        {
            var SiteCategory = ctx.SiteCategory.FirstOrDefault(c => c.CategoryName == b.CategoryName  && c.UserId==b.UserId);
           if (SiteCategory != null)
            {
                return 0;
            }
            ctx.SiteCategory.Add(b);
            int res = ctx.SaveChanges();
            res = b.CategoryID;
            return res;
        }

        public int DeleteData(int id)
        {
            int res = 0;
            var SiteCategory = ctx.SiteCategory.FirstOrDefault(b => b.CategoryID == id);
            if (SiteCategory != null)
            {
                try
                {
                    ctx.SiteCategory.Remove(SiteCategory);
                   ctx.SaveChanges();
                    res = id;
                }
                catch(Exception ex)
                {

                }
            }
            return res;
        }

        public SiteCategory Getbyid(int id)
        {
            var SiteCategory = ctx.SiteCategory.FirstOrDefault(b => b.CategoryID == id);
            return SiteCategory;
        }
    //    public IEnumerable<Group> Getgroup()
        //{
        //    Group[] grp = new Group[]
        //  {

        //        new Group()
        //        {
        //            Name="Testing14",
        //            Parent="Cash-in-Hand"
        //        },
        //          new Group()
        //        {
        //            Name="Testing15",
        //            Parent="Cash-in-Hand"
        //        }
        //  };
        //    return grp;
        //}
        public IEnumerable<SiteCategory> GetAll(int userid,int countryid,string role)

        {


            var SiteCategories = ctx.SiteCategory.OrderByDescending(x=>x.CategoryID).ToList();
            if (userid != 0)
            {
                //var Roles = ctx.User.Where(b => b.Id == userid).Select(x => x.Role).FirstOrDefault();

                if (role == "admin")
                {

                }
                else
                {
                    var adminuserid = ctx.User.Where(b => b.Role == "admin").Select(x =>

                 x.Id

            ).FirstOrDefault();

                    var SiteCategories1 = SiteCategories.Join(ctx.Site, b => b.CategoryID, c => c.CategoryID, (b, c) => new { b, c }).Where(x => x.c.CountryId ==countryid).GroupBy(x => x.b.CategoryID).Select(g => new SiteCategory
                    {

                        CategoryID = g.Key,
                        CategoryName = g.Max(f => f.b.CategoryName),
                        UserId = g.Max(f => f.b.UserId)
                    }).ToList();
                    var SiteCategories2 = SiteCategories.Where(X => X.UserId == userid || X.UserId==adminuserid || X.Isapprove==true).Select(g => new SiteCategory
                    {

                        CategoryID = g.CategoryID,
                        CategoryName = g.CategoryName,
                        UserId = g.UserId
                    }).ToList();
                    
                 //   SiteCategories = SiteCategories1;
                    SiteCategories = SiteCategories1.Union(SiteCategories2).GroupBy(x => x.CategoryID).Select(g => new SiteCategory
                    {

                        CategoryID = g.Key,
                        CategoryName = g.Max(x => x.CategoryName),
                        UserId = g.Max(x => x.UserId)
                    }).OrderByDescending(s => s.CategoryID).ToList();


                }
            }

            return SiteCategories;
        }

        public int UpdateData(int id, SiteCategory b)
        {
            int res = 0;
        //    var siteCategory = ctx.SiteCategory.Find(id);

            var siteCategory = ctx.SiteCategory.Where(c => c.CategoryName == b.CategoryName).ToList();
            if (siteCategory != null)
            {
                if (siteCategory.Count == 1 && b.CategoryID.Equals(siteCategory[0].CategoryID))
                {
                    siteCategory[0].CategoryName = b.CategoryName;
                    siteCategory[0].UserId = b.UserId;
                    ctx.SaveChanges();
                    res = b.CategoryID;
                }

                // res = ctx.SaveChanges();
            }
         
            return res;
        }

        public SiteCategory GetSiteCategoryByName(string name)
        {
            var SiteCategory = ctx.SiteCategory.FirstOrDefault(b => b.CategoryName == name);
            return SiteCategory;
        }
    }
}

