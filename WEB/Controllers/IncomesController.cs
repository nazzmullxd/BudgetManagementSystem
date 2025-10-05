using AutoMapper;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB.Models.DTOs;
using WEB.Models.Requests;
using WEB.Models.Responses;
using Database.Model;

namespace WEB.Controllers
{
    /// <summary>
    /// Controller for managing income records with DTO pattern
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class IncomesController : BaseApiController
    {
        private readonly IIncomeService _incomeService;
        private readonly IMapper _mapper;

        public IncomesController(
            IIncomeService incomeService,
            IMapper mapper)
        {
            _incomeService = incomeService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get paginated list of incomes for the current user
        /// </summary>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Items per page (default: 10)</param>
        /// <param name="startDate">Optional start date filter</param>
        /// <param name="endDate">Optional end date filter</param>
        /// <returns>Paginated list of incomes</returns>
        [HttpGet]
        public ActionResult<PagedApiResponse<IncomeDto>> GetIncomes(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 10;

                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(PagedApiResponse<IncomeDto>.ErrorResult("User not authenticated"));
                }

                List<TrackIncome>? incomes;
                
                if (startDate.HasValue && endDate.HasValue)
                {
                    incomes = _incomeService.GetIncomesByUserIdAndDateRange(userId, startDate.Value, endDate.Value);
                }
                else
                {
                    incomes = _incomeService.GetIncomesByUserId(userId);
                }

                var incomeList = incomes ?? new List<TrackIncome>();
                var totalItems = incomeList.Count;
                
                var pagedIncomes = incomeList
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var incomeDtos = _mapper.Map<List<IncomeDto>>(pagedIncomes);

                return Ok(PagedApiResponse<IncomeDto>.SuccessResult(
                    incomeDtos, 
                    page, 
                    pageSize, 
                    totalItems,
                    $"Retrieved {incomeDtos.Count} income records successfully"));
            }
            catch (Exception)
            {
                return StatusCode(500, PagedApiResponse<IncomeDto>.ErrorResult(
                    "An error occurred while retrieving income records", 
                    HttpContext.TraceIdentifier));
            }
        }

        /// <summary>
        /// Get a specific income by ID
        /// </summary>
        /// <param name="id">Income ID</param>
        /// <returns>Income details</returns>
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<IncomeDto>> GetIncome(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest(ApiResponse<IncomeDto>.ErrorResult("Income ID is required"));
                }

                var income = _incomeService.GetIncomeById(id);
                if (income == null)
                {
                    return NotFound(ApiResponse<IncomeDto>.ErrorResult("Income record not found"));
                }

                var userId = GetCurrentUserId();
                if (income.UserId != userId)
                {
                    return Forbid();
                }

                var incomeDto = _mapper.Map<IncomeDto>(income);
                return Ok(ApiResponse<IncomeDto>.SuccessResult(incomeDto, "Income record retrieved successfully"));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<IncomeDto>.ErrorResult(
                    "An error occurred while retrieving the income record",
                    HttpContext.TraceIdentifier));
            }
        }

        /// <summary>
        /// Create a new income record
        /// </summary>
        /// <param name="request">Income creation request</param>
        /// <returns>Created income details</returns>
        [HttpPost]
        public ActionResult<ApiResponse<IncomeDto>> CreateIncome([FromBody] CreateIncomeRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<IncomeDto>.ErrorResult("Invalid request data"));
                }

                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ApiResponse<IncomeDto>.ErrorResult("User not authenticated"));
                }

                // Map request to entity
                var income = _mapper.Map<TrackIncome>(request);
                income.UserId = userId;
                income.IncomeId = Guid.NewGuid().ToString();
                // Note: TrackIncome doesn't have CreatedDate property

                // Create the income record
                _incomeService.CreateIncome(income);
                
                // Retrieve the created income to return with full data
                var createdIncome = _incomeService.GetIncomeById(income.IncomeId);
                
                if (createdIncome == null)
                {
                    return StatusCode(500, ApiResponse<IncomeDto>.ErrorResult("Income record was created but could not be retrieved"));
                }

                var incomeDto = _mapper.Map<IncomeDto>(createdIncome);

                return CreatedAtAction(
                    nameof(GetIncome), 
                    new { id = createdIncome.IncomeId }, 
                    ApiResponse<IncomeDto>.SuccessResult(incomeDto, "Income record created successfully"));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<IncomeDto>.ErrorResult(
                    "An error occurred while creating the income record",
                    HttpContext.TraceIdentifier));
            }
        }

        /// <summary>
        /// Update an existing income record
        /// </summary>
        /// <param name="id">Income ID</param>
        /// <param name="request">Income update request</param>
        /// <returns>Updated income details</returns>
        [HttpPut("{id}")]
        public ActionResult<ApiResponse<IncomeDto>> UpdateIncome(string id, [FromBody] UpdateIncomeRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest(ApiResponse<IncomeDto>.ErrorResult("Income ID is required"));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<IncomeDto>.ErrorResult("Invalid request data"));
                }

                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ApiResponse<IncomeDto>.ErrorResult("User not authenticated"));
                }

                var existingIncome = _incomeService.GetIncomeById(id);
                if (existingIncome == null)
                {
                    return NotFound(ApiResponse<IncomeDto>.ErrorResult("Income record not found"));
                }

                if (existingIncome.UserId != userId)
                {
                    return Forbid();
                }

                // Map request to existing entity
                _mapper.Map(request, existingIncome);
                // Note: TrackIncome doesn't have ModifiedDate property

                // Update the income record
                _incomeService.UpdateIncome(existingIncome);
                
                // Get updated income
                var updatedIncome = _incomeService.GetIncomeById(id);
                var incomeDto = _mapper.Map<IncomeDto>(updatedIncome);

                return Ok(ApiResponse<IncomeDto>.SuccessResult(incomeDto, "Income record updated successfully"));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<IncomeDto>.ErrorResult(
                    "An error occurred while updating the income record",
                    HttpContext.TraceIdentifier));
            }
        }

        /// <summary>
        /// Delete an income record
        /// </summary>
        /// <param name="id">Income ID</param>
        /// <returns>Operation result</returns>
        [HttpDelete("{id}")]
        public ActionResult<OperationResponse> DeleteIncome(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest(OperationResponse.ErrorResult("Income ID is required"));
                }

                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(OperationResponse.ErrorResult("User not authenticated"));
                }

                var income = _incomeService.GetIncomeById(id);
                if (income == null)
                {
                    return NotFound(OperationResponse.ErrorResult("Income record not found"));
                }

                if (income.UserId != userId)
                {
                    return Forbid();
                }

                _incomeService.DeleteIncome(id);
                return Ok(OperationResponse.SuccessResult("Income record deleted successfully"));
            }
            catch (Exception)
            {
                return StatusCode(500, OperationResponse.ErrorResult(
                    "An error occurred while deleting the income record",
                    HttpContext.TraceIdentifier));
            }
        }

        /// <summary>
        /// Get income statistics and summary for the current user
        /// </summary>
        /// <param name="startDate">Optional start date for filtering</param>
        /// <param name="endDate">Optional end date for filtering</param>
        /// <returns>Income statistics</returns>
        [HttpGet("summary")]
        public ActionResult<ApiResponse<object>> GetIncomeSummary(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ApiResponse<object>.ErrorResult("User not authenticated"));
                }

                List<TrackIncome>? userIncomes;
                
                if (startDate.HasValue && endDate.HasValue)
                {
                    userIncomes = _incomeService.GetIncomesByUserIdAndDateRange(userId, startDate.Value, endDate.Value);
                }
                else
                {
                    userIncomes = _incomeService.GetIncomesByUserId(userId);
                }

                var incomes = userIncomes ?? new List<TrackIncome>();

                var summary = new
                {
                    TotalIncomes = incomes.Count,
                    TotalAmount = incomes.Sum(i => i.IncomeAmount),
                    AverageAmount = incomes.Any() ? incomes.Average(i => i.IncomeAmount) : 0,
                    DateRange = new
                    {
                        StartDate = startDate,
                        EndDate = endDate,
                        ActualStartDate = incomes.Any() ? incomes.Min(i => i.IncomeDate) : (DateTime?)null,
                        ActualEndDate = incomes.Any() ? incomes.Max(i => i.IncomeDate) : (DateTime?)null
                    },
                    BySource = incomes.GroupBy(i => i.IncomeSource)
                        .Select(g => new
                        {
                            Source = g.Key,
                            Count = g.Count(),
                            TotalAmount = g.Sum(i => i.IncomeAmount)
                        }).ToList()
                };

                return Ok(ApiResponse<object>.SuccessResult(summary, "Income summary retrieved successfully"));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult(
                    "An error occurred while retrieving income summary",
                    HttpContext.TraceIdentifier));
            }
        }
    }
}