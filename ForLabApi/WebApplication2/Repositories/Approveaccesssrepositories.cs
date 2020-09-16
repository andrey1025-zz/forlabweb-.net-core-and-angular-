using ForLabApi.DataInterface;
using ForLabApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Repositories
{
    public class Approveaccesssrepositories : IApprove<Approve, PendingApprovelist>
    {



        ForLabContext ctx;

        public Approveaccesssrepositories(ForLabContext c)
        {
            ctx = c;
            //return ctx;
        }
        public int approvalmasterdata(Approve approve)
        {
            int i = 1;
            try
            {

       
            switch (approve.mastertype)
            {
                case "Test":
                    var Test = ctx.Test.Find(approve.id);
                    if(Test!=null)
                    {
                        Test.Isapprove = approve.Isapprove;
                            Test.Isreject = approve.Isreject;
                        ctx.SaveChanges();
                    }
                    break;
                case "Region":
                    var Region = ctx.Region.Find(approve.id);
                    if (Region != null)
                    {
                        Region.Isapprove = approve.Isapprove;
                            Region.Isreject = approve.Isreject;
                            ctx.SaveChanges();
                    }
                    break;
                case "Site":
                    var Site = ctx.Site.Find(approve.id);
                    if (Site != null)
                    {
                        Site.Isapprove = approve.Isapprove;
                            Site.Isreject = approve.Isreject;
                            ctx.SaveChanges();
                    }
                    break;
                case "TestArea":
                    var TestArea = ctx.TestingArea.Find(approve.id);
                    if (TestArea != null)
                    {
                        TestArea.Isapprove = approve.Isapprove;
                            TestArea.Isreject = approve.Isreject;
                            ctx.SaveChanges();
                    }
                    break;
                case "Product Type":
                    var ProductType = ctx.ProductType.Find(approve.id);
                    if (ProductType != null)
                    {
                        ProductType.Isapprove = approve.Isapprove;
                            ProductType.Isreject = approve.Isreject;
                            ctx.SaveChanges();
                    }
                    break;
                case "Product":
                    var Product = ctx.MasterProduct.Find(approve.id);
                    if (Product != null)
                    {
                        Product.Isapprove = approve.Isapprove;
                            Product.Isreject = approve.Isreject;
                            ctx.SaveChanges();
                    }
                    break;
                case "Site Category":
                    var sitecategory = ctx.SiteCategory.Find(approve.id);
                    if (sitecategory != null)
                    {
                        sitecategory.Isapprove = approve.Isapprove;
                            sitecategory.Isreject = approve.Isreject;
                            ctx.SaveChanges();
                    }
                    break;
                    case "Instrument":
                        var Instrument = ctx.Instrument.Find(approve.id);
                        if (Instrument != null)
                        {
                            Instrument.Isapprove = approve.Isapprove;
                            Instrument.Isreject = approve.Isreject;
                            ctx.SaveChanges();
                        }
                        break;
                        
            }
            }
            catch (Exception)
            {

                i = 0;
            }
            return i;
        }
        public List<PendingApprovelist> getpendingapprovallist()
        {
            List<PendingApprovelist> pa = new List<PendingApprovelist>();
            var adminuserid = ctx.User.Where(b => b.Role == "admin").Select(c => c.Id).FirstOrDefault();


            var test = ctx.Test.Where(b => b.Isapprove == false && b.Isreject == false && b.UserId != adminuserid)
                .GroupJoin(ctx.User, b => b.UserId, c => c.Id, (b, c) => new { b, c = c.SingleOrDefault() })
                  .GroupJoin(ctx.Country, f => f.c.CountryId, g => g.Id, (f, g) => new { f, g = g.SingleOrDefault() })
                .Select(s => new PendingApprovelist
                {
                    Id = s.f.b.TestID,
                    Name = s.f.b.TestName,
                    mastertype = "Test",
                    userName = s.f.c.FirstName + " " + s.f.c.LastName,
                    country = s.g.Name

                }).ToList();
        

            var Region = ctx.Region.Where(b => b.Isapprove == false && b.Isreject == false && b.UserId != adminuserid)
           .GroupJoin(ctx.User, b => b.UserId, c => c.Id, (b, c) => new { b, c = c.SingleOrDefault() })
             .GroupJoin(ctx.Country, f => f.c.CountryId, g => g.Id, (f, g) => new { f, g = g.SingleOrDefault() })
           .Select(s => new PendingApprovelist
           {
               Id = s.f.b.RegionID,
               Name = s.f.b.RegionName,
               mastertype = "Region",
               userName = s.f.c.FirstName + " " + s.f.c.LastName,
               country = s.g.Name

           }).ToList();

            var site = ctx.Site.Where(b => b.Isapprove == false && b.Isreject == false && b.UserId != adminuserid)
        .GroupJoin(ctx.User, b => b.UserId, c => c.Id, (b, c) => new { b, c = c.SingleOrDefault() })
          .GroupJoin(ctx.Country, f => f.c.CountryId, g => g.Id, (f, g) => new { f, g = g.SingleOrDefault() })
        .Select(s => new PendingApprovelist
        {
            Id = s.f.b.SiteID,
            Name = s.f.b.SiteName,
            mastertype = "Site",
            userName = s.f.c.FirstName + " " + s.f.c.LastName,
            country = s.g.Name

        }).ToList();

            var testarea = ctx.TestingArea.Where(b => b.Isapprove == false && b.Isreject == false && b.UserId != adminuserid)
      .GroupJoin(ctx.User, b => b.UserId, c => c.Id, (b, c) => new { b, c = c.SingleOrDefault() })
        .GroupJoin(ctx.Country, f => f.c.CountryId, g => g.Id, (f, g) => new { f, g = g.SingleOrDefault() })
      .Select(s => new PendingApprovelist
      {
          Id = s.f.b.TestingAreaID,
          Name = s.f.b.AreaName,
          mastertype = "TestArea",
          userName = s.f.c.FirstName + " " + s.f.c.LastName,
          country = s.g.Name

      }).ToList();
            var producttype = ctx.ProductType.Where(b => b.Isapprove == false && b.Isreject == false && b.UserId != adminuserid)
   .GroupJoin(ctx.User, b => b.UserId, c => c.Id, (b, c) => new { b, c = c.SingleOrDefault() })
     .GroupJoin(ctx.Country, f => f.c.CountryId, g => g.Id, (f, g) => new { f, g = g.SingleOrDefault() })
   .Select(s => new PendingApprovelist
   {
       Id = s.f.b.TypeID,
       Name = s.f.b.TypeName,
       mastertype = "Product Type",
       userName = s.f.c.FirstName + " " + s.f.c.LastName,
       country = s.g.Name

   }).ToList();

            var mastproduct = ctx.MasterProduct.Where(b => b.Isapprove == false && b.Isreject == false && b.UserId != adminuserid)
.GroupJoin(ctx.User, b => b.UserId, c => c.Id, (b, c) => new { b, c = c.SingleOrDefault() })
.GroupJoin(ctx.Country, f => f.c.CountryId, g => g.Id, (f, g) => new { f, g = g.SingleOrDefault() })
.Select(s => new PendingApprovelist
{
    Id = s.f.b.ProductID,
    Name = s.f.b.ProductName,
    mastertype = "Product",
    userName = s.f.c.FirstName + " " + s.f.c.LastName,
    country = s.g.Name

}).ToList();
            var mastsitecategory= ctx.SiteCategory.Where(b => b.Isapprove == false && b.Isreject == false && b.UserId != adminuserid)
 .GroupJoin(ctx.User, b => b.UserId, c => c.Id, (b, c) => new { b, c = c.SingleOrDefault() })
 .GroupJoin(ctx.Country, f => f.c.CountryId, g => g.Id, (f, g) => new { f, g = g.SingleOrDefault() })
 .Select(s => new PendingApprovelist
 {
     Id = s.f.b.CategoryID,
     Name = s.f.b.CategoryName,
     mastertype = "Site Category",
     userName = s.f.c.FirstName + " " + s.f.c.LastName,
     country = s.g.Name

 }).ToList();

            var mastins = ctx.Instrument.Where(b => b.Isapprove == false && b.Isreject==false && b.UserId != adminuserid)
.GroupJoin(ctx.User, b => b.UserId, c => c.Id, (b, c) => new { b, c = c.SingleOrDefault() })
.GroupJoin(ctx.Country, f => f.c.CountryId, g => g.Id, (f, g) => new { f, g = g.SingleOrDefault() })
.Select(s => new PendingApprovelist
{
 Id = s.f.b.InstrumentID,
 Name = s.f.b.InstrumentName,
 mastertype = "Instrument",
 userName = s.f.c.FirstName + " " + s.f.c.LastName,
 country = s.g.Name

}).ToList();

            pa = test.Union(Region).Union(site).Union(testarea).Union(producttype).Union(mastproduct).Union(mastsitecategory).Union(mastins).ToList();
            return pa;
        }
    }
}
