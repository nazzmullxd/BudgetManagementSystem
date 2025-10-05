using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Business.Services;
using WEB.Models.Requests;
using WEB.Models;

namespace WEB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _budgetService;
        private readonly IBudgetGoalService _budgetGoalService;
        private readonly ILogger<BudgetController> _logger;

        public BudgetController(
            IBudgetService budgetService,
            IBudgetGoalService budgetGoalService,
            ILogger<BudgetController> logger)
        {
            _budgetService = budgetService;
            _budgetGoalService = budgetGoalService;
            _logger = logger;
        }

        [HttpGet("goals")]
        public IActionResult GetBudgetGoals()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User ID not found");

                var goals = _budgetGoalService.GetGoalsByUserId(userId);
                return Ok(goals);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving budget goals");
                return StatusCode(500, "An error occurred while retrieving budget goals");
            }
        }

        [HttpPost("goals")]
        public IActionResult CreateBudgetGoal([FromBody] CreateBudgetGoalRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User ID not found");

                var budgetGoal = new Database.Model.BudgetGoal
                {
                    BudgetGoalId = Guid.NewGuid().ToString(),
                    UserId = userId,
                    GoalName = request.GoalName,
                    TargetAmount = request.TargetAmount,
                    TargetDate = request.EndDate,
                    ExpenseCategoryId = request.ExpenseCategoryId
                };

                _budgetGoalService.CreateGoal(budgetGoal);

                return Ok(new { Message = "Budget goal created successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating budget goal");
                return StatusCode(500, "An error occurred while creating the budget goal");
            }
        }

        [HttpGet("goals/{id}")]
        public IActionResult GetBudgetGoal(string id)
        {
            try
            {
                var goal = _budgetGoalService.GetGoalById(id);
                if (goal == null)
                    return NotFound($"Budget goal with ID {id} not found");

                return Ok(goal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving budget goal with ID {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the budget goal");
            }
        }

        [HttpDelete("goals/{id}")]
        public IActionResult DeleteBudgetGoal(string id)
        {
            try
            {
                _budgetGoalService.DeleteGoal(id);
                return Ok(new { Message = "Budget goal deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting budget goal with ID {Id}", id);
                return StatusCode(500, "An error occurred while deleting the budget goal");
            }
        }

        private string? GetCurrentUserId()
        {
            return User.FindFirst("UserId")?.Value ?? User.Identity?.Name;
        }
    }
}