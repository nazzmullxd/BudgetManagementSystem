using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Business.Services;
using WEB.Models;

namespace WEB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeService _incomeService;
        private readonly ILogger<IncomeController> _logger;

        public IncomeController(IIncomeService incomeService, ILogger<IncomeController> logger)
        {
            _incomeService = incomeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetIncomes([FromQuery] string? userId = null)
        {
            try
            {
                var currentUserId = userId ?? GetCurrentUserId();
                if (string.IsNullOrEmpty(currentUserId))
                    return Unauthorized("User ID not found");

                var incomes = _incomeService.GetIncomesByUserId(currentUserId);
                return Ok(incomes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving incomes");
                return StatusCode(500, "An error occurred while retrieving incomes");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetIncome(string id)
        {
            try
            {
                var income = _incomeService.GetIncomeById(id);
                if (income == null)
                    return NotFound($"Income with ID {id} not found");

                return Ok(income);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving income with ID {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the income");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateIncome([FromBody] CreateIncomeRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User ID not found");

                var income = new Database.Model.TrackIncome
                {
                    IncomeId = Guid.NewGuid().ToString(),
                    UserId = userId,
                    IncomeSource = request.IncomeSource,
                    IncomeType = request.IncomeType,
                    IncomeDescription = request.IncomeDescription,
                    IncomeAmount = request.IncomeAmount,
                    IncomeDate = request.IncomeDate,
                    IncomeTax = request.IncomeTax,
                    Frequency = request.Frequency,
                    CurrencyId = request.CurrencyId
                };

                _incomeService.CreateIncome(income);

                return Ok(new { Message = "Income created successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating income");
                return StatusCode(500, "An error occurred while creating the income");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIncome(string id, [FromBody] UpdateIncomeRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingIncome = _incomeService.GetIncomeById(id);
                if (existingIncome == null)
                    return NotFound($"Income with ID {id} not found");

                existingIncome.IncomeSource = request.IncomeSource;
                existingIncome.IncomeType = request.IncomeType;
                existingIncome.IncomeDescription = request.IncomeDescription;
                existingIncome.IncomeAmount = request.IncomeAmount;
                existingIncome.IncomeDate = request.IncomeDate;
                existingIncome.IncomeTax = request.IncomeTax;
                existingIncome.Frequency = request.Frequency;

                _incomeService.UpdateIncome(existingIncome);

                return Ok(new { Message = "Income updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating income with ID {Id}", id);
                return StatusCode(500, "An error occurred while updating the income");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncome(string id)
        {
            try
            {
                _incomeService.DeleteIncome(id);
                return Ok(new { Message = "Income deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting income with ID {Id}", id);
                return StatusCode(500, "An error occurred while deleting the income");
            }
        }

        private string? GetCurrentUserId()
        {
            return User.FindFirst("UserId")?.Value ?? User.Identity?.Name;
        }
    }
}