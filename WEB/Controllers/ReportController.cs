using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Business.Services;

namespace WEB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly ILogger<ReportController> _logger;

        public ReportController(IReportService reportService, ILogger<ReportController> logger)
        {
            _reportService = reportService;
            _logger = logger;
        }

        [HttpGet("financial-summary")]
        public async Task<IActionResult> GetFinancialSummary(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User ID not found");

                var start = startDate ?? DateTime.Now.AddMonths(-1);
                var end = endDate ?? DateTime.Now;

                var summary = _reportService.GetFinancialSummary(userId, start, end);
                return Ok(new
                {
                    TotalExpenses = summary.totalExpenses,
                    TotalIncome = summary.totalIncome,
                    NetAmount = summary.netAmount,
                    StartDate = start,
                    EndDate = end
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving financial summary");
                return StatusCode(500, "An error occurred while retrieving the financial summary");
            }
        }

        [HttpGet("expenses-by-category")]
        public async Task<IActionResult> GetExpensesByCategory(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User ID not found");

                var start = startDate ?? DateTime.Now.AddMonths(-1);
                var end = endDate ?? DateTime.Now;

                var expensesByCategory = _reportService.GetExpensesByCategory(userId, start, end);
                return Ok(expensesByCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving expenses by category");
                return StatusCode(500, "An error occurred while retrieving expenses by category");
            }
        }

        [HttpGet("top-expenses")]
        public async Task<IActionResult> GetTopExpenses(
            [FromQuery] int count = 10,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User ID not found");

                var topExpenses = _reportService.GetTopExpenses(userId, count, startDate, endDate);
                return Ok(topExpenses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving top expenses");
                return StatusCode(500, "An error occurred while retrieving top expenses");
            }
        }

        [HttpGet("income-history")]
        public async Task<IActionResult> GetIncomeHistory(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User ID not found");

                var start = startDate ?? DateTime.Now.AddMonths(-3);
                var end = endDate ?? DateTime.Now;

                var incomeHistory = _reportService.GetIncomeHistory(userId, start, end);
                return Ok(incomeHistory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving income history");
                return StatusCode(500, "An error occurred while retrieving income history");
            }
        }

        private string? GetCurrentUserId()
        {
            return User.FindFirst("UserId")?.Value ?? User.Identity?.Name;
        }
    }
}