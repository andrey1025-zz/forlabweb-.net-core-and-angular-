using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.DataInterface
{
    public interface IProducttype<ProductType>
    {
        IEnumerable<ProductType> GetAll(int userid,string role);
      
        ProductType Getbyid(int id);
        int SaveData(ProductType b);
        int UpdateData(int id, ProductType b);
        int DeleteData(int id);
        ProductType GetProductTypeByName(string name);
    }
}
