using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Method { get; set; }
        [ForeignKey("Order")]
        public int? OrderID { get; set; }
        public virtual Order? Order { get; set; }
    }
}
