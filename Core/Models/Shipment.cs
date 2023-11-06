using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Shipment
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public string? EstimatedDeliveryTime { get; set; }
        public float Cost { get; set; }
        public int TarckingNumber { get; set; }
        public DateTime Date { get; set; }
        public string? Status { get; set; }
        [ForeignKey("Order")]
        public int? OrderId { get; set; }
        public virtual Order? Order { get; set; }
    }
}
