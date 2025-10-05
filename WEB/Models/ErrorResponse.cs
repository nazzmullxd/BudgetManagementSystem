using System.Text.Json.Serialization;

namespace WEB.Models
{
    /// <summary>
    /// Standardized error response model for API endpoints
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// HTTP status code
        /// </summary>
        [JsonPropertyName("status")]
        public int Status { get; set; }

        /// <summary>
        /// Error type/code for programmatic handling
        /// </summary>
        [JsonPropertyName("error")]
        public string Error { get; set; } = string.Empty;

        /// <summary>
        /// User-friendly error message
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Detailed error information (only in development)
        /// </summary>
        [JsonPropertyName("details")]
        public string? Details { get; set; }

        /// <summary>
        /// Request trace identifier for debugging
        /// </summary>
        [JsonPropertyName("traceId")]
        public string TraceId { get; set; } = string.Empty;

        /// <summary>
        /// Timestamp when the error occurred
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Request path where the error occurred
        /// </summary>
        [JsonPropertyName("path")]
        public string? Path { get; set; }

        /// <summary>
        /// Validation errors (for input validation failures)
        /// </summary>
        [JsonPropertyName("validationErrors")]
        public Dictionary<string, string[]>? ValidationErrors { get; set; }
    }

    /// <summary>
    /// Validation error response for model validation failures
    /// </summary>
    public class ValidationErrorResponse : ErrorResponse
    {
        public ValidationErrorResponse()
        {
            Error = "ValidationFailed";
            Message = "One or more validation errors occurred.";
        }
    }
}