using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ForLabApi.DataInterface;
using ForLabApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForLabApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors("ApiCorsPolicy")]
    public class ProductController : Controller
    {
        IProduct<MasterProduct, productlist, ProductPrice,ProductType> _Product;
        public ProductController(IProduct<MasterProduct, productlist, ProductPrice, ProductType> b)
        {
            _Product = b;
        }
        public class aaData1
        {
            public IEnumerable<productlist> aaData { get; set; }
        }

        [HttpGet]
        public IEnumerable<ProductType> GetAllbyadmin()
        {
            var producttype = _Product.GetAllbyadmin();
            return producttype;
        }

        // GET: api/values
        [HttpGet, Authorize]
        [ActionName("GetAll")]
        public aaData1 Get()
        {
            var userid = 0;
            var role = "";
            if (Request.Headers["userid"] == "")
            {

            }
            else
            {
                userid = Convert.ToInt32(Request.Headers["userid"]);
                role = Convert.ToString(Request.Headers["role"]);

            }
            var Product = _Product.GetAll(userid,role);
            var gq = new aaData1
            {
                aaData = Product

            };
            return gq;

        }

        //Get Product PriceList
        [HttpGet("{id}"), Authorize]
        [ActionName("GetPriceList")]
        public IEnumerable<ProductPrice> GetPriceList(int id)
        {
            var productPricelist = _Product.GetProductPriceList(id);
            return productPricelist;
        }

        //Get Productby TypeIDt
        [HttpGet("{id}"), Authorize]
        [ActionName("GetAllProductByType")]
        public IEnumerable<MasterProduct> GetAllProductByType(int id)
        {
            var productPricelist = _Product.GetAllProductByType(id);
            return productPricelist;
        }

        [HttpGet("{keyword}"), Authorize]
        [ActionName("GetProductbykeyword")]
        public IEnumerable<productlist> GetProductbykeyword(string keyword)
        {
            var productlist = _Product.GetProductbykeyword(keyword);
            return productlist;
        }

        [HttpGet("{typeids}"), Authorize]
        [ActionName("getdefaultdatapro")]
        public IEnumerable<productlist> getdefaultdatapro(string typeids)
        {
            var productlist = _Product.getdefaultdatapro(typeids);
            return productlist;
        }
        // GET api/values/5
        [HttpGet("{id}"), Authorize]
        [ActionName("GetbyId")]
        public MasterProduct Get(int id)
        {
            var Product = _Product.Getbyid(id);
            return Product;
        }



        [HttpGet("{keyword}"), Authorize]
        [ActionName("Getproductbykeywordtypes")]
        public IEnumerable<productlist> Getproductbykeywordtypes(string keyword, string type)
        {
            var productlist = _Product.Getproductbykeywordtypes(keyword, type);
            return productlist;

        }

        // POST api/values
        [HttpPost, Authorize]
        [ActionName("Post01")]
        public int Post([FromBody]MasterProduct value)
        {
            var re = Request;
            var header = re.Headers;
            value.UserId = Convert.ToInt32(header["userid"]);
            int res = _Product.SaveData(value);
            if (res != 0)
            {
                return res;
            }
            return 0;
        }

        // PUT api/values/5
        [HttpPut("{id}"), Authorize]
        [ActionName("Put01")]
        public int Put(int id, [FromBody]MasterProduct value)
        {
            int res = 0;
            var re = Request;
            var header = re.Headers;
            value.UserId = Convert.ToInt32(header["userid"]);
            if (id == value.ProductID)
            {
                res = _Product.UpdateData(id, value);
                if (res != 0)
                {
                    return res;
                }
                return res;
            }
            return res;
        }

        // DELETE api/values/5
        [HttpDelete("{id}"), Authorize]
        [ActionName("Del01")]
        public int Delete(int id)
        {
            int res = _Product.DeleteData(id);
            if (res != 0)
            {
                return res;
            }
            return res;
        }



        [HttpDelete("{ids}"), Authorize]
        [ActionName("Deleteproudctprice")]
        public IActionResult Deleteproudctprice(string ids)
        {
            int res = _Product.Deleteproudctprice(ids);
            if (res != 0)
            {
                return Ok();
            }
            return NotFound();

        }
    }
}
