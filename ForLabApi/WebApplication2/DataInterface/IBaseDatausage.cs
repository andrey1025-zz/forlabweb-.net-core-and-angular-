using ForLabApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.DataInterface
{
    public interface IBaseDataUsage
    {
        int Id
        {
            get;
            set;
        }
        decimal AmountUsed
        {
            get;
            set;
        }

        string CDuration
        {
            get;
            set;
        }

        int StockOut
        {
            get;
            set;
        }

        decimal Adjusted
        {
            get;
            set;
        }

      
        DateTime? DurationDateTime
        {
            get;
            set;
        }

        int InstrumentDowntime
        {
            get;
            set;
        }
         int? UserId { get; set; }
    }
}
