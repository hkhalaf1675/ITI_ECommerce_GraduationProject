using Core.DTOs.Reports;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AdminReportRepository : IAdminReportRepository
    {
        private readonly ECommerceDBContext context;

        public AdminReportRepository(ECommerceDBContext _context)
        {
            context = _context;
        }
        public async Task<ICollection<AdminReportDto>> GetReportByDate(DateTime startDate, DateTime endDate)
        {
            List<AdminReportDto> reportDtos = new List<AdminReportDto>();

            var orders = context.Orders.Include(O => O.OrderDetails)
                .Where(O => O.Date >= startDate && O.Date <= endDate);

            foreach (var order in orders)
            {
                reportDtos.Add(new AdminReportDto
                {
                    OrderId = order.Id,
                    Date = order.Date,
                    Status = order.Status,
                    TotalPrice = order.TotalPrice,
                    ProductsCount = order.OrderDetails.Sum(OD => OD.Quantity)
                });
            }

            return reportDtos;
        }
    }
}
