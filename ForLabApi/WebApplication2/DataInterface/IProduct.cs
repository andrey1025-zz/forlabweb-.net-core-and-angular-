using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.DataInterface
{
    public interface IProduct<MasterProduct, productlist, ProductPrice, ProductType>
    {

        IEnumerable<productlist> getdefaultdatapro(string typeids);
        IEnumerable<productlist> GetAll(int userid,string role);
        IEnumerable<ProductType> GetAllbyadmin();
        MasterProduct Getbyid(int id);
        int SaveData(MasterProduct b);
        int UpdateData(int id, MasterProduct b);
        int DeleteData(int id);
       int Deleteproudctprice(string ids);
        IEnumerable<MasterProduct> GetAllProductByType(int typeid);
        IEnumerable<MasterProduct> GetAllProductByClassOfTest(string classofTest);
        IEnumerable<MasterProduct> GetAllProductByClassOfTest(string classofTest, string rapidtestGroup);
        MasterProduct GetProductByName(string pname);

        IEnumerable<ProductPrice> GetProductPriceList(int ProductId);

        IEnumerable<productlist> GetProductbykeyword(string keyword);

        IEnumerable<productlist> Getproductbykeywordtypes(string keyword, string type);
        decimal GetProductPrice(int proid, DateTime fromdate);
      //  IEnumerable<Product> GetPagingProducts(int typeId, int firstResult, int maxResult);
       // int GetTotalCountOfProducts(int typeId);
    }
}
