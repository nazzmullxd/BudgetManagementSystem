using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Services;
using WEB.Controllers;
using WEB.Models.DTOs;
using WEB.Models.Requests;
using WEB.Models.Responses;
using Database.Model;

namespace WEB.Controllers
{
    /// <summary>
    /// Controller for managing expenses with DTO pattern implementation
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExpensesController : BaseApiController
    {
        private readonly IExpenseService _expenseService;
        private readonly IMapper _mapper;

        public ExpensesController(
            IExpenseService expenseService,
            IMapper mapper)
        {
            _expenseService = expenseService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all expenses for the current user with pagination
        /// </summary>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Items per page (default: 10, max: 100)</param>
        /// <returns>Paginated list of expenses</returns>
        [HttpGet]
        public async Task<ActionResult<PagedApiResponse<ExpenseDto>>> GetExpenses(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10)
        {
            try
            {
                // Validate pagination parameters
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 10;
                if (pageSize > 100) pageSize = 100;

                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(PagedApiResponse<ExpenseDto>.ErrorResult("User not authenticated"));
                }

                var expenses = await _expenseService.GetExpensesByUserIdAsync(userId);
                var totalItems = expenses?.Count() ?? 0;
                
                var pagedExpenses = (expenses ?? new List<TrackExpense>())
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var expenseDtos = _mapper.Map<List<ExpenseDto>>(pagedExpenses);

                return Ok(PagedApiResponse<ExpenseDto>.SuccessResult(
                    expenseDtos, 
                    page, 
                    pageSize, 
                    totalItems,
                    $"Retrieved {expenseDtos.Count} expenses successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, PagedApiResponse<ExpenseDto>.ErrorResult(
                    "An error occurred while retrieving expenses", 
                    HttpContext.TraceIdentifier));
            }
        }

        /// <summary>
        /// Get a specific expense by ID
        /// </summary>
        /// <param name="id">Expense ID</param>
        /// <returns>Expense details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ExpenseDto>>> GetExpense(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest(ApiResponse<ExpenseDto>.ErrorResult("Expense ID is required"));
                }

                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ApiResponse<ExpenseDto>.ErrorResult("User not authenticated"));
                }

                var expense = await _expenseService.GetExpenseByIdAsync(id);
                if (expense == null)
                {
                    return NotFound(ApiResponse<ExpenseDto>.ErrorResult("Expense not found"));
                }

                // Verify the expense belongs to the current user
                if (expense.UserId != userId)
                {
                    return Forbid();
                }

                var expenseDto = _mapper.Map<ExpenseDto>(expense);
                return Ok(ApiResponse<ExpenseDto>.SuccessResult(expenseDto, "Expense retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ExpenseDto>.ErrorResult(
                    "An error occurred while retrieving the expense",
                    HttpContext.TraceIdentifier));
            }
        }

        /// <summary>
        /// Create a new expense
        /// </summary>
        /// <param name="request">Expense creation request</param>
        /// <returns>Created expense details</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<ExpenseDto>>> CreateExpense([FromBody] CreateExpenseRequest request)
        {
            try
            {
                // Validate model state
                var validationResult = ValidateModelState();
                if (validationResult != null)
                {
                    return BadRequest(ApiResponse<ExpenseDto>.ErrorResult("Validation failed"));
                }

                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ApiResponse<ExpenseDto>.ErrorResult("User not authenticated"));
                }

                // Map request to entity
                var expense = _mapper.Map<TrackExpense>(request);
                expense.UserId = userId;

                // Create the expense
                await _expenseService.CreateExpenseAsync(expense);
                var expenseDto = _mapper.Map<ExpenseDto>(expense);

                return CreatedAtAction(
                    nameof(GetExpense), 
                    new { id = expense.TrackExpenseId }, 
                    ApiResponse<ExpenseDto>.SuccessResult(expenseDto, "Expense created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ExpenseDto>.ErrorResult(
                    "An error occurred while creating the expense",
                    HttpContext.TraceIdentifier));
            }
        }

        /// <summary>
        /// Update an existing expense
        /// </summary>
        /// <param name="id">Expense ID</param>
        /// <param name="request">Expense update request</param>
        /// <returns>Updated expense details</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<ExpenseDto>>> UpdateExpense(string id, [FromBody] UpdateExpenseRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest(ApiResponse<ExpenseDto>.ErrorResult("Expense ID is required"));
                }

                // Validate model state
                var validationResult = ValidateModelState();
                if (validationResult != null)
                {
                    return BadRequest(ApiResponse<ExpenseDto>.ErrorResult("Validation failed"));
                }

                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ApiResponse<ExpenseDto>.ErrorResult("User not authenticated"));
                }

                // Check if expense exists and belongs to user
                var existingExpense = await _expenseService.GetExpenseByIdAsync(id);
                if (existingExpense == null)
                {
                    return NotFound(ApiResponse<ExpenseDto>.ErrorResult("Expense not found"));
                }

                if (existingExpense.UserId != userId)
                {
                    return Forbid();
                }

                // Map update request to entity
                _mapper.Map(request, existingExpense);
                existingExpense.TrackExpenseId = id; // Ensure ID doesn't change

                // Update the expense
                await _expenseService.UpdateExpenseAsync(existingExpense);
                var expenseDto = _mapper.Map<ExpenseDto>(existingExpense);

                return Ok(ApiResponse<ExpenseDto>.SuccessResult(expenseDto, "Expense updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ExpenseDto>.ErrorResult(
                    "An error occurred while updating the expense",
                    HttpContext.TraceIdentifier));
            }
        }

        /// <summary>
        /// Delete an expense
        /// </summary>
        /// <param name="id">Expense ID</param>
        /// <returns>Operation result</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<OperationResponse>> DeleteExpense(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest(OperationResponse.ErrorResult("Expense ID is required"));
                }

                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(OperationResponse.ErrorResult("User not authenticated"));
                }

                // Check if expense exists and belongs to user
                var existingExpense = await _expenseService.GetExpenseByIdAsync(id);
                if (existingExpense == null)
                {
                    return NotFound(OperationResponse.ErrorResult("Expense not found"));
                }

                if (existingExpense.UserId != userId)
                {
                    return Forbid();
                }

                // Delete the expense
                await _expenseService.DeleteExpenseAsync(id);

                return Ok(OperationResponse.SuccessResult("Expense deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, OperationResponse.ErrorResult(
                    "An error occurred while deleting the expense",
                    HttpContext.TraceIdentifier));
            }
        }

        /// <summary>
        /// Get expense summary statistics for the current user
        /// </summary>
        /// <returns>Expense summary data</returns>
        [HttpGet("summary")]
        public async Task<ActionResult<ApiResponse<object>>> GetExpenseSummary()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ApiResponse<object>.ErrorResult("User not authenticated"));
                }

                var expenses = await _expenseService.GetExpensesByUserIdAsync(userId);
                
                var expenseList = expenses ?? new List<TrackExpense>();
                var summary = new
                {
                    TotalExpenses = expenseList.Count,
                    TotalAmount = expenseList.Sum(e => e.ItemPrice * e.Quantity),
                    AverageAmount = expenseList.Any() ? expenseList.Average(e => e.ItemPrice * e.Quantity) : 0,
                    ThisMonthExpenses = expenseList.Count(e => e.TransactionDate.Month == DateTime.Now.Month && e.TransactionDate.Year == DateTime.Now.Year),
                    ThisMonthAmount = expenseList
                        .Where(e => e.TransactionDate.Month == DateTime.Now.Month && e.TransactionDate.Year == DateTime.Now.Year)
                        .Sum(e => e.ItemPrice * e.Quantity),
                    TopCategories = expenseList
                        .GroupBy(e => e.Category?.CategoryName ?? "Uncategorized")
                        .Select(g => new { Category = g.Key, Count = g.Count(), Amount = g.Sum(e => e.ItemPrice * e.Quantity) })
                        .OrderByDescending(x => x.Amount)
                        .Take(5)
                        .ToList()
                };

                return Ok(ApiResponse<object>.SuccessResult(summary, "Expense summary retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult(
                    "An error occurred while retrieving expense summary",
                    HttpContext.TraceIdentifier));
            }
        }
    }
}