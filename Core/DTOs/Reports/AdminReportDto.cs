using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Reports
{
    public class AdminReportDto
    {
        public int? OrderId { get; set; }
        public DateTime? Date {  get; set; }
        public decimal? TotalPrice { get; set; }
        public int? ProductsCount { get; set; }
        public OrderStatus? Status{ get; set; }
    }
}
