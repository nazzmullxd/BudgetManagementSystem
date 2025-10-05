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
    /// Controller for managing expense categories with DTO pattern
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(
            ICategoryService categoryService,
            IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get paginated list of categories for the current user
        /// </summary>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Items per page (default: 10)</param>
        /// <returns>Paginated list of categories</returns>
        [HttpGet]
        public async Task<ActionResult<PagedApiResponse<CategoryDto>>> GetCategories(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 10;

                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(PagedApiResponse<CategoryDto>.ErrorResult("User not authenticated"));
                }

                var categories = await _categoryService.GetCategoriesForUserAsync(userId);
                var categoryList = categories?.ToList() ?? new List<ExpenseCategory>();
                var totalItems = categoryList.Count;
                
                var pagedCategories = categoryList
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var categoryDtos = _mapper.Map<List<CategoryDto>>(pagedCategories);

                return Ok(PagedApiResponse<CategoryDto>.SuccessResult(
                    categoryDtos, 
                    page, 
                    pageSize, 
                    totalItems,
                    $"Retrieved {categoryDtos.Count} categories successfully"));
            }
            catch (Exception)
            {
                return StatusCode(500, PagedApiResponse<CategoryDto>.ErrorResult(
                    "An error occurred while retrieving categories", 
                    HttpContext.TraceIdentifier));
            }
        }

        /// <summary>
        /// Get a specific category by ID
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>Category details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> GetCategory(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest(ApiResponse<CategoryDto>.ErrorResult("Category ID is required"));
                }

                var category = await _categoryService.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    return NotFound(ApiResponse<CategoryDto>.ErrorResult("Category not found"));
                }

                var userId = GetCurrentUserId();
                if (category.UserId != userId)
                {
                    return Forbid();
                }

                var categoryDto = _mapper.Map<CategoryDto>(category);
                return Ok(ApiResponse<CategoryDto>.SuccessResult(categoryDto, "Category retrieved successfully"));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<CategoryDto>.ErrorResult(
                    "An error occurred while retrieving the category",
                    HttpContext.TraceIdentifier));
            }
        }

        /// <summary>
        /// Create a new category
        /// </summary>
        /// <param name="request">Category creation request</param>
        /// <returns>Created category details</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<CategoryDto>.ErrorResult("Invalid request data"));
                }

                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ApiResponse<CategoryDto>.ErrorResult("User not authenticated"));
                }

                // Check if category name already exists for this user
                var existingCategories = await _categoryService.GetCategoriesForUserAsync(userId);
                if (existingCategories?.Any(c => c.CategoryName.Equals(request.Name, StringComparison.OrdinalIgnoreCase)) == true)
                {
                    return BadRequest(ApiResponse<CategoryDto>.ErrorResult("A category with this name already exists"));
                }

                // Create the category
                await _categoryService.CreateCategoryAsync(userId, request.Name, request.Description);
                
                // Get the created category to return with ID
                var updatedCategories = await _categoryService.GetCategoriesForUserAsync(userId);
                var newCategory = updatedCategories?.FirstOrDefault(c => c.CategoryName == request.Name);
                
                if (newCategory == null)
                {
                    return StatusCode(500, ApiResponse<CategoryDto>.ErrorResult("Category was created but could not be retrieved"));
                }

                var categoryDto = _mapper.Map<CategoryDto>(newCategory);

                return CreatedAtAction(
                    nameof(GetCategory), 
                    new { id = newCategory.ExpenseCategoryId }, 
                    ApiResponse<CategoryDto>.SuccessResult(categoryDto, "Category created successfully"));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<CategoryDto>.ErrorResult(
                    "An error occurred while creating the category",
                    HttpContext.TraceIdentifier));
            }
        }

        /// <summary>
        /// Update an existing category
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <param name="request">Category update request</param>
        /// <returns>Updated category details</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> UpdateCategory(string id, [FromBody] UpdateCategoryRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest(ApiResponse<CategoryDto>.ErrorResult("Category ID is required"));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<CategoryDto>.ErrorResult("Invalid request data"));
                }

                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ApiResponse<CategoryDto>.ErrorResult("User not authenticated"));
                }

                var existingCategory = await _categoryService.GetCategoryByIdAsync(id);
                if (existingCategory == null)
                {
                    return NotFound(ApiResponse<CategoryDto>.ErrorResult("Category not found"));
                }

                if (existingCategory.UserId != userId)
                {
                    return Forbid();
                }

                // Check if the new name conflicts with another category
                var userCategories = await _categoryService.GetCategoriesForUserAsync(userId);
                if (userCategories?.Any(c => 
                    c.ExpenseCategoryId != id && 
                    c.CategoryName.Equals(request.Name, StringComparison.OrdinalIgnoreCase)) == true)
                {
                    return BadRequest(ApiResponse<CategoryDto>.ErrorResult("A category with this name already exists"));
                }

                // Update the category
                await _categoryService.UpdateCategoryAsync(id, request.Name, request.Description);
                
                // Get updated category
                var updatedCategory = await _categoryService.GetCategoryByIdAsync(id);
                var categoryDto = _mapper.Map<CategoryDto>(updatedCategory);

                return Ok(ApiResponse<CategoryDto>.SuccessResult(categoryDto, "Category updated successfully"));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<CategoryDto>.ErrorResult(
                    "An error occurred while updating the category",
                    HttpContext.TraceIdentifier));
            }
        }

        /// <summary>
        /// Delete a category
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>Operation result</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<OperationResponse>> DeleteCategory(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest(OperationResponse.ErrorResult("Category ID is required"));
                }

                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(OperationResponse.ErrorResult("User not authenticated"));
                }

                var category = await _categoryService.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    return NotFound(OperationResponse.ErrorResult("Category not found"));
                }

                if (category.UserId != userId)
                {
                    return Forbid();
                }

                await _categoryService.DeleteCategoryAsync(id);
                return Ok(OperationResponse.SuccessResult("Category deleted successfully"));
            }
            catch (Exception)
            {
                return StatusCode(500, OperationResponse.ErrorResult(
                    "An error occurred while deleting the category",
                    HttpContext.TraceIdentifier));
            }
        }

        /// <summary>
        /// Get category statistics for the current user
        /// </summary>
        /// <returns>Category statistics</returns>
        [HttpGet("stats")]
        public async Task<ActionResult<ApiResponse<object>>> GetCategoryStats()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ApiResponse<object>.ErrorResult("User not authenticated"));
                }

                var userCategories = await _categoryService.GetCategoriesForUserAsync(userId);
                var categories = userCategories?.ToList() ?? new List<ExpenseCategory>();

                var stats = new
                {
                    TotalCategories = categories.Count,
                    Categories = categories.Select(c => new
                    {
                        Id = c.ExpenseCategoryId,
                        Name = c.CategoryName,
                        Description = c.CategoryDescription
                    }).ToList()
                };

                return Ok(ApiResponse<object>.SuccessResult(stats, "Category statistics retrieved successfully"));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult(
                    "An error occurred while retrieving category statistics",
                    HttpContext.TraceIdentifier));
            }
        }
    }
}