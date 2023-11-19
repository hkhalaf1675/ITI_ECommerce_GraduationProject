using Core.DTOs.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface IAdminReportRepository
    {
        Task<ICollection<AdminReportDto>> GetReportByDate(DateTime startDate, DateTime endDate);
    }
}
