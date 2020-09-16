using ForLabApi.DataInterface;
using ForLabApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Repositories
{
    public class ProductAccessRepositories : IProduct<MasterProduct, productlist,ProductPrice,ProductType>
    {

        ForLabContext ctx;
        public ProductAccessRepositories(ForLabContext c)
        {
            ctx = c;
            //return ctx;
        }
        public IEnumerable<ProductType> GetAllbyadmin()
        {
            var adminuserid = ctx.User.Where(b => b.Role == "admin").FirstOrDefault();
            var Producttype = ctx.ProductType.Where(b => b.UserId == adminuserid.Id).ToList();
            return Producttype;
        }
        public int SaveData(MasterProduct b)
        {
            var Product = ctx.MasterProduct.FirstOrDefault(c => c.ProductName == b.ProductName  && c.UserId==b.UserId);
            if (Product != null)
            {
                return 0;
            }
            ctx.MasterProduct.Add(b);
            int res = ctx.SaveChanges();
            int id = b.ProductID;
            return id;
        }

        public int DeleteData(int id)
        {
            int res = 0;
            var Product = ctx.MasterProduct.FirstOrDefault(b => b.ProductID == id);
            if (Product != null)
            {
                try
                {
                 
                    var ProductPrice = ctx.ProductPrice.Where(b => b.ProductId == id).ToList();
                    ctx.ProductPrice.RemoveRange(ProductPrice);
                    ctx.MasterProduct.Remove(Product);
                    res = ctx.SaveChanges();
                    ctx.SaveChanges();
                    res = id;


                }
                catch(Exception ex)
                {

                }
            }
            return res;
        }

        public MasterProduct Getbyid(int id)
        {
            var Product = ctx.MasterProduct.FirstOrDefault(b => b.ProductID == id);

            Product._productPrices = ctx.ProductPrice.Where(b => b.ProductId == id).ToList();
            return Product;
        }
       public IEnumerable<productlist> getdefaultdatapro(string typeids)
        {
            string[] typeids1;
            int res = 0;
            typeids1 = typeids.Trim(',').Split(',');

            var products = (from prd in ctx.MasterProduct.Where(b => typeids1.Contains(Convert.ToString(b.ProductTypeId)))
                            select new MasterProduct
                            {
                                ProductID = prd.ProductID,
                                ProductName = prd.ProductName,
                                ProductTypeId = prd.ProductTypeId,
                                SerialNo = prd.SerialNo,
                                BasicUnit = prd.BasicUnit,
                                MinimumPackPerSite = prd.MinimumPackPerSite,
                                _productPrices = ctx.ProductPrice.Where(b => b.ProductId == prd.ProductID).ToList(),
                                UserId = prd.UserId
                            }).OrderByDescending(x => x.ProductID).ToList();



            var results = (from prd in products
                               //join pp in ctx.ProductPrice on prd.ProductID equals pp.Product.ProductID                       
                           select new productlist
                           {
                               ProductID = prd.ProductID,
                               ProductName = prd.ProductName,
                               ProductType = ctx.ProductType.Where(b => b.TypeID == prd.ProductTypeId).Select(x => x.TypeName).FirstOrDefault(),
                               catalog = prd.SerialNo,
                               BasicUnit = prd.BasicUnit,
                               minpacksize = prd.MinimumPackPerSite,
                               packcost = prd.GetActiveProductPrice(DateTime.Now) != null ? prd.GetActiveProductPrice(DateTime.Now).packcost : 0,
                               packsize = prd.GetActiveProductPrice(DateTime.Now) != null ? prd.GetActiveProductPrice(DateTime.Now).packsize : 0,
                               PriceDate = prd.GetActiveProductPrice(DateTime.Now) != null ? prd.GetActiveProductPrice(DateTime.Now).FromDate.Value.ToString("dd/MMM/yyyy") : DateTime.Now.ToString("dd/MMM/yyyy"),
                               UserId = prd.UserId
                           }).ToList();

            return results;

        }
        public IEnumerable<productlist> GetAll(int userid,string role)
        {
            MasterProduct p = new MasterProduct();
                var products=( from prd in ctx.MasterProduct
                               select new MasterProduct
                               {
                                   ProductID = prd.ProductID,
                                   ProductName = prd.ProductName,
                                   ProductTypeId =  prd.ProductTypeId,
                                   SerialNo = prd.SerialNo,
                                   BasicUnit = prd.BasicUnit,
                                   MinimumPackPerSite = prd.MinimumPackPerSite,
                                   Isapprove=prd.Isapprove,
                              _productPrices=ctx.ProductPrice.Where(b=>b.ProductId==prd.ProductID).ToList(),
                              UserId=prd.UserId
                               }).OrderByDescending(x=>x.ProductID).ToList();
                               
                              

            var results = (from prd in products
                               //join pp in ctx.ProductPrice on prd.ProductID equals pp.Product.ProductID                       
                           select new productlist
                           {
                               ProductID = prd.ProductID,
                               ProductName = prd.ProductName,
                               ProductType = ctx.ProductType.Where(b => b.TypeID ==prd.ProductTypeId).Select(x=>x.TypeName).FirstOrDefault(),
                               catalog = prd.SerialNo,
                               BasicUnit = prd.BasicUnit,
                               minpacksize = prd.MinimumPackPerSite,
                               packcost = prd.GetActiveProductPrice(DateTime.Now) !=null ? prd.GetActiveProductPrice(DateTime.Now).packcost :0,
                               packsize = prd.GetActiveProductPrice(DateTime.Now) != null? prd.GetActiveProductPrice(DateTime.Now).packsize:0,
                               PriceDate = prd.GetActiveProductPrice(DateTime.Now) != null ? prd.GetActiveProductPrice(DateTime.Now).FromDate.Value.ToString("dd/MMM/yyyy") :DateTime.Now.ToString("dd/MMM/yyyy"),
                               UserId =prd.UserId,
                               Isapprove=prd.Isapprove
                           }).ToList();
         //   var Roles = ctx.User.Where(b => b.Id == userid).Select(x => x.Role).FirstOrDefault();
            if (role == "admin")
            {

            }
            else
            {
                var adminuserid = ctx.User.Where(b => b.Role == "admin").Select(x =>

               x.Id

          ).FirstOrDefault();

                results = results.Where(b => b.UserId == userid || b.UserId==adminuserid || b.Isapprove==true).ToList();
            }


            return results;

         
        }

        public int UpdateData(int id, MasterProduct b)
        {
            int res = 0;
            var Product = ctx.MasterProduct.Where(c=>c.ProductName==b.ProductName && c.UserId==b.UserId ).ToList();
            if (Product != null && Product.Count==1 && b.ProductID.Equals(Product[0].ProductID))
            {
                Product[0].ProductName = b.ProductName;
                Product[0].SerialNo = b.SerialNo;
                Product[0].MinimumPackPerSite = b.MinimumPackPerSite;
                foreach(ProductPrice P in b._productPrices)
                {
                    var price = ctx.ProductPrice.Find(P.Id);
                    if(price !=null)
                    {
                        price.Price = P.Price;
                        price.PackSize = P.PackSize;
                        price.FromDate = P.FromDate;
                        ctx.SaveChanges();
                    }
                    else
                    {
                       
                        P.ProductId=b.ProductID;
                        ctx.ProductPrice.Add(P);
                         res = ctx.SaveChanges();
                    }
                }
           //     Product._productPrices = b._productPrices;
                res = ctx.SaveChanges();
                res = b.ProductID;
            }
            return res;
        }


        public IEnumerable<MasterProduct> GetAllProductByType(int typeid)
        {
            var Product = ctx.MasterProduct.Where(b => b.ProductTypeId == typeid).ToList();
            return Product;
        }
        public IEnumerable<MasterProduct> GetAllProductByClassOfTest(string classofTest)
        {
            List<MasterProduct> Product = new List<MasterProduct>();
          //  var Product = ctx.MasterProduct.Where(b => b.ProductType.ClassOfTest == classofTest).ToList();
            return Product;

        }
        public IEnumerable<MasterProduct> GetAllProductByClassOfTest(string classofTest, string rapidtestGroup)
        {
            List<MasterProduct> Product = new List<MasterProduct>();
           // var Product = ctx.MasterProduct.Where(b => b.ProductType.ClassOfTest == classofTest && b.RapidTestGroup == rapidtestGroup).ToList();
            return Product;

        }
        public MasterProduct GetProductByName(string pname)
        {
            var Product = ctx.MasterProduct.FirstOrDefault(b => b.ProductName == pname);
            return Product;
        }
        public decimal GetProductPrice(int proid, DateTime fromdate)
        {



            var price = ctx.ProductPrice.Where(b => b.FromDate == ctx.ProductPrice.Where(x =>
            x.ProductId == b.ProductId && x.FromDate == fromdate && x.ProductId == proid).Max(x => x.FromDate)).Select(d => new { d.Price });

            decimal productprice = Convert.ToDecimal(price);


            return productprice;
        }

        public IEnumerable<ProductPrice> GetProductPriceList(int ProductId)
        {
            var ProductPriceList = ctx.ProductPrice.Where(b => b.ProductId == ProductId).ToList();
            return ProductPriceList;


        }


        public int Deleteproudctprice(string ids)
        {
            string[] arrids;
            int res = 0;
            arrids = ids.Trim(',').Split(',');
          
            var ProductPrice = ctx.ProductPrice.Where(b => arrids.Contains(Convert.ToString(b.Id))).ToList();
            ctx.ProductPrice.RemoveRange(ProductPrice);
            res = ctx.SaveChanges();
            return res;

        }
        public IEnumerable<productlist> GetProductbykeyword(string keyword)
        {
            var productlist = (from prd in ctx.MasterProduct
                               join type in ctx.ProductType on prd.ProductTypeId equals type.TypeID
                               where prd.ProductName.Contains(keyword)
                               //join pp in ctx.ProductPrice on prd.ProductID equals pp.Product.ProductID                       
                               select new productlist
                               {
                                   ProductID = prd.ProductID,
                                   ProductName = prd.ProductName,
                                   ProductType = type.TypeName,
                                  // ProductType = ctx.ProductType.Where(b => b.TypeID == prd.ProductTypeId).Select(x => x.TypeName).FirstOrDefault(),
                                   catalog = prd.SerialNo,
                                   BasicUnit = prd.BasicUnit,
                                   minpacksize = prd.MinimumPackPerSite
                              
                               }).ToList();
            return productlist;
        }


        public IEnumerable<productlist> Getproductbykeywordtypes(string keyword, string type)
        {
            var comparestring = type.TrimStart(',');
            var productlist = (from prd in ctx.MasterProduct
                               join type1 in ctx.ProductType on prd.ProductTypeId equals type1.TypeID
                               where (prd.ProductName.Contains(keyword) && Convert.ToString(type1.TypeID).Contains(comparestring))
                               //join pp in ctx.ProductPrice on prd.ProductID equals pp.Product.ProductID                       
                               select new productlist
                               {
                                   ProductID = prd.ProductID,
                                   ProductName = prd.ProductName,
                                   ProductType = type1.TypeName,
                                   // ProductType = ctx.ProductType.Where(b => b.TypeID == prd.ProductTypeId).Select(x => x.TypeName).FirstOrDefault(),
                                   catalog = prd.SerialNo,
                                   BasicUnit = prd.BasicUnit,
                                   minpacksize = prd.MinimumPackPerSite

                               }).ToList();
            return productlist;
        }
    }
}
