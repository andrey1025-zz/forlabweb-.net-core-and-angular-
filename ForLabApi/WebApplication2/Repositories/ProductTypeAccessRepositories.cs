using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForLabApi.DataInterface;
using ForLabApi.Models;

namespace ForLabApi.Repositories
{
    public class ProductTypeAccessRepositories : IProducttype<ProductType>

    {
        ForLabContext ctx;
        public ProductTypeAccessRepositories(ForLabContext c)
        {
            ctx = c;
            //return ctx;
        }
        public int SaveData(ProductType b)
        {
            var ProductType = ctx.ProductType.FirstOrDefault(c => c.TypeName == b.TypeName && c.UserId==b.UserId);
            if (ProductType != null)
            {
                return 0;
            }
            ctx.ProductType.Add(b);
            int res = ctx.SaveChanges();
            return res;
        }

        public int DeleteData(int id)
        {
            int res = 0;
            var ProductType = ctx.ProductType.FirstOrDefault(b => b.TypeID == id);
            if (ProductType != null)
            {
                try
                {
                    ctx.ProductType.Remove(ProductType);
                    ctx.SaveChanges();
                    res = id;
                }
                catch(Exception ex)
                {

                }
            }
            return res;
        }

        public ProductType Getbyid(int id)
        {
            var ProductType = ctx.ProductType.FirstOrDefault(b => b.TypeID == id);
            return ProductType;
        }
      
        public IEnumerable<ProductType> GetAll(int userid,string role)


        {


            var ProductTypes = ctx.ProductType.OrderByDescending(x => x.TypeID).ToList();

            if (userid == 0)
            {
                ProductTypes = ProductTypes.Join(ctx.User, b => b.UserId, c => c.Id, (b, c) => new { b, c }).Where(x => x.c.Role == "admin").Select(x => new ProductType
                {
                    TypeID = x.b.TypeID,
                    TypeName = x.b.TypeName,
                    ClassOfTest = x.b.ClassOfTest,
                    Description=x.b.Description,
                    UseInDemography = x.b.UseInDemography,
                    UserId = x.b.UserId
                }).ToList();
            }
            else
            {
               // var Roles = ctx.User.Where(b => b.Id == userid).Select(x => x.Role).FirstOrDefault();
                if (role == "admin")
                {

                }
                else
                {
                    var adminuserid = ctx.User.Where(b => b.Role == "admin").Select(x =>

                  x.Id

             ).FirstOrDefault();
                    ProductTypes = ProductTypes.Where(b => b.UserId == userid || b.UserId ==adminuserid || b.Isapprove==true).ToList();
                }
            }
         
            return ProductTypes;
        }

        public int UpdateData(int id, ProductType b)
        {
            int res = 0;
            //var ProductType = ctx.ProductType.Find(id);
            var ProductType = ctx.ProductType.Where(c => c.TypeName == b.TypeName).ToList();
            if (ProductType != null)
            {
                if (ProductType.Count == 1 && b.TypeID.Equals(ProductType[0].TypeID))
                {

                    ProductType[0].TypeName = b.TypeName;

                     ctx.SaveChanges();
                    res = id;
                }
            }
            return res;
        }

        public ProductType GetProductTypeByName(string name)
        {
            var ProductType = ctx.ProductType.FirstOrDefault(b => b.TypeName == name);
            return ProductType;
        }
    }
}

