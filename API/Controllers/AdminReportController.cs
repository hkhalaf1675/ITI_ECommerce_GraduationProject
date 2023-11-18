using Core.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class AdminReportController : ControllerBase
    {
        private readonly IAdminReportRepository reportRepository;

        public AdminReportController(IAdminReportRepository _reportRepository)
        {
            reportRepository = _reportRepository;
        }

        [HttpGet("GetByDate")]
        public async Task<IActionResult> GetByDate(DateTime startDate,DateTime endDate)
        {
            var report = await reportRepository.GetReportByDate(startDate, endDate);

            if(report == null)
            {
                return NotFound();
            }

            return Ok(report);
        }
    }
}
