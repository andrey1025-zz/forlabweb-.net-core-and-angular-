using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Models
{
    public class ProductPrice
    {
        [Key]
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int PackSize { get; set; }
        public DateTime? FromDate { get; set; }
        public int ProductId { get; set; }
        public bool Isapprove { get; set; }
        public bool Isreject { get; set; }
        public int? UserId { get; set; }
    }
}
