using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Models
{
    public class Test
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int TestID { get; set; }
        public string TestName { get; set; }
        public int TestingAreaID { get; set; }

        public IList<ProductUsage> ProductUsageArray { get; set; }
      


        public string TestType { get; set; }
        public string TestingDuration { get; set; }
        public int? UserId { get; set; }
        public bool Isapprove { get; set; }
        public bool Isreject { get; set; }
        public virtual IList<ProductUsage> GetProductUsageByType(bool isForControl)
        {
            IList<ProductUsage> result = new List<ProductUsage>();
            foreach (ProductUsage p in ProductUsageArray)
            {
                if (p.IsForControl == isForControl)
                {
                    result.Add(p);
                }
            }
            return result;
        }
    }
    public class Gettotalcount
    {
        public int Totaltestarea { get; set; }
        public int Totaltest { get; set; }
        public int Totalins { get; set; }
        public int Totalsite { get; set; }
        public int Totalsitecategory { get; set; }
        public int Totalregion { get; set; }
        public int Totalproduct { get; set; }

        public int Totalproducttype { get; set; }
    }
    public class testList
    {
        public int TestID { get; set; }
        public string TestName { get; set; }
      
        public string testingArea { get; set; }
        public string username { get; set; }
        public int testingAreaID { get; set; }
        public int? UserId { get; set; }
        public IEnumerable<ProductUsageDetail> Productusage { get; set; }
        public IEnumerable<ProductUsageDetail> controlusage { get; set; }
        public IEnumerable<ConsumableUsageDetail> consumablepertest { get; set; }
        public IEnumerable<ConsumableUsageDetail> consumableperperiod { get; set; }
        public IEnumerable<ConsumableUsageDetail> consumableperins { get; set; }
    }
    public class ProductUsagelist
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        public string ProductName { get; set; }
        public int InstrumentId { get; set; }
        public string InstrumentName { get; set; }


        public string test { get; set; }
        public decimal Rate { get; set; }
        public string ProductUsedIn { get; set; }
        public bool IsForControl { get; set; }
        public int TestId { get; set; }
    }
    public class ProductUsageDetail
    {
        public string name { get; set; }
        public List<ProductUsagelist> value { get; set; }
    }
    public class ConsumableUsagelist
    {
        public int Id { get; set; }
        public int ConsumableId { get; set; }
        public int testId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int ProductTypeId { get; set; }

        public string ProductTypeName { get; set; }
        public int InstrumentId { get; set; }
        public string InstrumentName { get; set; }

        public decimal UsageRate { get; set; }
        public bool PerTest { get; set; }
        public bool PerPeriod { get; set; }
        public bool PerInstrument { get; set; }

        public string test { get; set; }
        public int NoOfTest { get; set; }
        public string Period { get; set; }
    }
    public class ConsumableUsageDetail
    {
    
        public string name { get; set; }

        public List<ConsumableUsagelist> value { get; set; }
    }
    public class forecasttest
    {
        public int testareaid { get; set; }
        public string testareaname { get; set; }

        public Array tests { get; set; }
    }

    public class Testlist1
    {
      
        public int TestID { get; set; }
        public string TestName { get; set; }
        public int TestingAreaID { get; set; }
        public string type { get; set; }
    }
    public class TestList_area
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }

}
