using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime Date { get; set; }
        public PayMethods? Method { get; set; }
        public decimal TotalPrice { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }
        public ICollection<OrderDetails> OrderDetails { get; set; } = new HashSet<OrderDetails>();
        public ICollection<PaymentMethod> Methods { get; set; } = new HashSet<PaymentMethod>();
        public ICollection<Shipment> Shipments { get; set; } = new HashSet<Shipment>();
    }

    public enum OrderStatus
    {
        Processing,
        Pending,
        Shipped,
        Delivered,
        Cancelled
    }
    public enum PayMethods
    {
        CreditCard,
        PayPal,
        Cash
    }
}
