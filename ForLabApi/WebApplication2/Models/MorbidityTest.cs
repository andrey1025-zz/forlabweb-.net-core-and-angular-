using ForLabApi.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Models
{
    public class MorbidityTest
    {
        [Key]
      
        public int Id { get; set; }

          public int InstrumentId { get; set; }
        public string ClassOfTest { get; set; }
        public string TestName { get; set; }
        public string TestSpecificationGroup { get; set; }
    }


    public class QuantifyMenu
    {
        [Key]
        public int Id { get; set;}
        public int InstrumentId  {get; set;}
        public int ProductId { get; set;}
        public string ClassOfTest { get; set;}
        public string Title { get; set;}
        public string TestType { get; set;}
        public string Duration { get; set;}
        public string ChemTestName { get; set;}
        public int MorbidityTetsId { get; set;}
        public IList<QuantificationMetric> _quantificationMetrics  {get; set;}
    }


    public class QuantificationMetric
    {
        [Key]
        public int Id { get ; set ;}
        public string ClassOfTest { get ; set ;}
        public double UsageRate { get ; set ;}
        public string CollectionSupplieAppliedTo { get ; set ;}
        public int ProductId { get ; set ;}
        public int QuantifyMenuId { get ; set ;}
    }
   
}
