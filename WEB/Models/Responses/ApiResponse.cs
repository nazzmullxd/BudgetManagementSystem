namespace WEB.Models.Responses
{
    /// <summary>
    /// Standard API response wrapper for single items
    /// </summary>
    /// <typeparam name="T">Type of data being returned</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Whether the operation was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// The data returned by the operation
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Message describing the result
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Timestamp of the response
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Unique identifier for tracking the request
        /// </summary>
        public string TraceId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Creates a successful response with data
        /// </summary>
        public static ApiResponse<T> SuccessResult(T data, string message = "Operation completed successfully")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message
            };
        }

        /// <summary>
        /// Creates a successful response without data
        /// </summary>
        public static ApiResponse<T> SuccessResult(string message = "Operation completed successfully")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message
            };
        }

        /// <summary>
        /// Creates an error response
        /// </summary>
        public static ApiResponse<T> ErrorResult(string message, string? traceId = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                TraceId = traceId ?? Guid.NewGuid().ToString()
            };
        }
    }

    /// <summary>
    /// Standard API response wrapper for paginated lists
    /// </summary>
    /// <typeparam name="T">Type of items in the list</typeparam>
    public class PagedApiResponse<T> : ApiResponse<List<T>>
    {
        /// <summary>
        /// Current page number (1-based)
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Number of items per page
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total number of items across all pages
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Total number of pages
        /// </summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        /// <summary>
        /// Whether there are more pages after this one
        /// </summary>
        public bool HasNextPage => Page < TotalPages;

        /// <summary>
        /// Whether there are pages before this one
        /// </summary>
        public bool HasPreviousPage => Page > 1;

        /// <summary>
        /// Creates a successful paginated response
        /// </summary>
        public static PagedApiResponse<T> SuccessResult(
            List<T> data, 
            int page, 
            int pageSize, 
            int totalItems, 
            string message = "Data retrieved successfully")
        {
            return new PagedApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };
        }

        /// <summary>
        /// Creates an error paginated response
        /// </summary>
        public static new PagedApiResponse<T> ErrorResult(string message, string? traceId = null)
        {
            return new PagedApiResponse<T>
            {
                Success = false,
                Message = message,
                TraceId = traceId ?? Guid.NewGuid().ToString(),
                Data = new List<T>()
            };
        }
    }

    /// <summary>
    /// Simple response for operations that don't return data
    /// </summary>
    public class OperationResponse : ApiResponse<object>
    {
        /// <summary>
        /// Creates a successful operation response
        /// </summary>
        public static OperationResponse SuccessResult(string message = "Operation completed successfully")
        {
            return new OperationResponse
            {
                Success = true,
                Message = message
            };
        }

        /// <summary>
        /// Creates an error operation response
        /// </summary>
        public static new OperationResponse ErrorResult(string message, string? traceId = null)
        {
            return new OperationResponse
            {
                Success = false,
                Message = message,
                TraceId = traceId ?? Guid.NewGuid().ToString()
            };
        }
    }
}