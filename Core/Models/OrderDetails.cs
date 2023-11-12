using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int? Quantity { get; set; }
        public decimal? TotalPrice { get; set; }
        [ForeignKey("Order")]
        public int? OrderID { get; set; }
        public virtual Order? Order { get; set; }
        [ForeignKey("Product")]
        public int? ProductID { get; set; }
        public virtual Product? Product { get; set; }
    }
}
