using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Models
{
    public class MasterProduct
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string SerialNo { get; set; }
        public string Specification { get; set; }
        public string BasicUnit { get; set; }

        public string ProductNote { get; set; }
        public int MinimumPackPerSite { get; set; }
        public string RapidTestGroup { get; set; }

        public Boolean? SlowMoving { get; set; }
        public int ProductTypeId { get; set; }
        public int? UserId { get; set; }
        public bool Isapprove { get; set; }
        public bool Isreject { get; set; }
        public IList<ProductPrice> _productPrices { get; set; }

        public virtual pricedetail GetActiveProductPrice(DateTime date)
        {

            pricedetail activeProductPrice =null;
            DateTime? date1=DateTime.Now;
            foreach (ProductPrice p in _productPrices)
            {
                if (p.FromDate <= date)
                {
                    if (activeProductPrice == null)
                    {
                        activeProductPrice = new pricedetail();
                        activeProductPrice.packcost = p.Price;
                        activeProductPrice.packsize = p.PackSize;
                        activeProductPrice.FromDate = p.FromDate;
                        date1 = p.FromDate;
                    }
                    else if (p.FromDate > activeProductPrice.FromDate)
                    {
                        activeProductPrice = new pricedetail();
                        activeProductPrice.packcost = p.Price;
                        activeProductPrice.packsize = p.PackSize;
                        activeProductPrice.FromDate = p.FromDate;
                        date1 = p.FromDate;
                    }
                    else if (_productPrices.Count==1)
                    {
                        activeProductPrice = new pricedetail();
                        activeProductPrice.packcost = p.Price;
                        activeProductPrice.packsize = p.PackSize;
                        activeProductPrice.FromDate = p.FromDate;
                        date1 = p.FromDate;
                    }
                }
                else if (p.FromDate > date)
                {
                    activeProductPrice = new pricedetail();
                    activeProductPrice.packcost = p.Price;
                    activeProductPrice.packsize = p.PackSize;
                    activeProductPrice.FromDate = p.FromDate;
                    date1 = p.FromDate;
                }

            }
            return activeProductPrice;
        }



    }
    public class productdetail
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public string catalog { get; set; }
        public string BasicUnit { get; set; }


        public int minpacksize { get; set; }
    }
    public class pricedetail
    {
        public int packsize { get; set; }
        public decimal packcost { get; set; }
        public DateTime? FromDate { get; set; }
    }
        public class productlist
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public string catalog { get; set; }
        public string BasicUnit { get; set; }


        public int minpacksize { get; set; }

        public int packsize { get; set; }
        public decimal packcost { get; set; }

        public string PriceDate { get; set; }
        public int? UserId { get; set; }
        public bool Isapprove { get; set; }

    }
}
