using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Business.Services;
using Database.Model;
using WEB.Models.Requests;

namespace WEB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly ILogger<ExpenseController> _logger;

        public ExpenseController(IExpenseService expenseService, ILogger<ExpenseController> logger)
        {
            _expenseService = expenseService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetExpenses([FromQuery] string? userId = null)
        {
            try
            {
                var currentUserId = userId ?? GetCurrentUserId();
                if (string.IsNullOrEmpty(currentUserId))
                    return Unauthorized("User ID not found");

                var expenses = await _expenseService.GetExpensesByUserIdAsync(currentUserId);
                return Ok(expenses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving expenses for user {UserId}", userId);
                return StatusCode(500, new { message = "An error occurred while retrieving expenses" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpense(string id)
        {
            try
            {
                var expense = await _expenseService.GetExpenseByIdAsync(id);
                if (expense == null)
                    return NotFound(new { message = $"Expense with ID {id} not found" });

                return Ok(expense);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving expense with ID {Id}", id);
                return StatusCode(500, new { message = "An error occurred while retrieving the expense" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User ID not found");

                var expense = new Database.Model.TrackExpense
                {
                    TrackExpenseId = Guid.NewGuid().ToString(),
                    UserId = userId,
                    ItemName = request.ItemName,
                    ItemPrice = request.ItemPrice,
                    Quantity = request.Quantity,
                    ExpenseCategoryId = request.CategoryId,
                    TransactionDate = request.TransactionDate,
                    CurrencyId = request.CurrencyId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _expenseService.CreateExpenseAsync(expense);

                return Ok(new { Message = "Expense created successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating expense");
                return StatusCode(500, "An error occurred while creating the expense");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(string id, [FromBody] UpdateExpenseRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingExpense = await _expenseService.GetExpenseByIdAsync(id);
                if (existingExpense == null)
                    return NotFound(new { message = $"Expense with ID {id} not found" });

                existingExpense.ItemName = request.ItemName;
                existingExpense.ItemPrice = request.ItemPrice;
                existingExpense.Quantity = request.Quantity;
                existingExpense.ExpenseCategoryId = request.CategoryId;
                existingExpense.TransactionDate = request.TransactionDate;
                existingExpense.UpdatedAt = DateTime.UtcNow;

                await _expenseService.UpdateExpenseAsync(existingExpense);

                return Ok(new { Message = "Expense updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating expense with ID {Id}", id);
                return StatusCode(500, "An error occurred while updating the expense");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(string id)
        {
            try
            {
                await _expenseService.DeleteExpenseAsync(id);
                return Ok(new { Message = "Expense deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting expense with ID {Id}", id);
                return StatusCode(500, new { message = "An error occurred while deleting the expense" });
            }
        }

        [HttpGet("by-date-range")]
        public async Task<IActionResult> GetExpensesByDateRange([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "User ID not found" });

                var start = startDate ?? DateTime.Now.AddMonths(-1);
                var end = endDate ?? DateTime.Now;

                var expenses = await _expenseService.GetExpensesByUserIdAndDateRangeAsync(userId, start, end);
                return Ok(expenses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving expenses by date range for user {UserId}", GetCurrentUserId());
                return StatusCode(500, new { message = "An error occurred while retrieving expenses by date range" });
            }
        }

        private string? GetCurrentUserId()
        {
            return User.FindFirst("UserId")?.Value ?? User.Identity?.Name;
        }
    }
}