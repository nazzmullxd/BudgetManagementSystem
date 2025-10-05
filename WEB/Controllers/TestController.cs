using Microsoft.AspNetCore.Mvc;

namespace WEB.Controllers
{
    /// <summary>
    /// Test controller for demonstrating global exception handling middleware
    /// </summary>
    public class TestController : BaseApiController
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Test endpoint that throws ArgumentException
        /// </summary>
        [HttpGet("argument-exception")]
        public IActionResult TestArgumentException()
        {
            throw new ArgumentException("This is a test ArgumentException to demonstrate global exception handling.");
        }

        /// <summary>
        /// Test endpoint that throws UnauthorizedAccessException
        /// </summary>
        [HttpGet("unauthorized-exception")]
        public IActionResult TestUnauthorizedException()
        {
            throw new UnauthorizedAccessException("This is a test UnauthorizedAccessException to demonstrate global exception handling.");
        }

        /// <summary>
        /// Test endpoint that throws KeyNotFoundException
        /// </summary>
        [HttpGet("notfound-exception")]
        public IActionResult TestNotFoundException()
        {
            throw new KeyNotFoundException("This is a test KeyNotFoundException to demonstrate global exception handling.");
        }

        /// <summary>
        /// Test endpoint that throws generic Exception
        /// </summary>
        [HttpGet("generic-exception")]
        public IActionResult TestGenericException()
        {
            throw new Exception("This is a test generic Exception to demonstrate global exception handling.");
        }

        /// <summary>
        /// Test endpoint that throws NotImplementedException
        /// </summary>
        [HttpGet("notimplemented-exception")]
        public IActionResult TestNotImplementedException()
        {
            throw new NotImplementedException("This is a test NotImplementedException to demonstrate global exception handling.");
        }

        /// <summary>
        /// Test endpoint with invalid model state to demonstrate validation error handling
        /// </summary>
        [HttpPost("validation-error")]
        public IActionResult TestValidationError([FromBody] TestValidationModel model)
        {
            var validationResult = ValidateModelState();
            if (validationResult != null)
                return validationResult;

            return Ok(new { message = "Model is valid!", data = model });
        }

        /// <summary>
        /// Test endpoint that succeeds to show normal operation
        /// </summary>
        [HttpGet("success")]
        public IActionResult TestSuccess()
        {
            _logger.LogInformation("Test success endpoint called");
            return Ok(new 
            { 
                message = "Global exception middleware is working! This endpoint succeeded without any exceptions.",
                timestamp = DateTime.UtcNow,
                traceId = HttpContext.TraceIdentifier
            });
        }
    }

    /// <summary>
    /// Test model for validation error demonstration
    /// </summary>
    public class TestValidationModel
    {
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.MinLength(3)]
        public string Name { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Range(1, 100)]
        public int Age { get; set; }

        [System.ComponentModel.DataAnnotations.EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}