using Database.Model;
using System.Security.Claims;

namespace Business.Services
{
    public interface IJwtService
    {
        /// <summary>
        /// Generates a JWT access token for the specified user
        /// </summary>
        /// <param name="user">The user for whom to generate the token</param>
        /// <returns>JWT access token string</returns>
        string GenerateAccessToken(User user);

        /// <summary>
        /// Generates a refresh token for secure token renewal
        /// </summary>
        /// <returns>Refresh token string</returns>
        string GenerateRefreshToken();

        /// <summary>
        /// Validates a JWT token and extracts claims
        /// </summary>
        /// <param name="token">JWT token to validate</param>
        /// <returns>ClaimsPrincipal if valid, null if invalid</returns>
        ClaimsPrincipal? ValidateAccessToken(string token);

        /// <summary>
        /// Extracts claims from an expired token for refresh purposes
        /// </summary>
        /// <param name="token">Expired JWT token</param>
        /// <returns>ClaimsPrincipal if token structure is valid, null otherwise</returns>
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);

        /// <summary>
        /// Gets the user ID from a JWT token
        /// </summary>
        /// <param name="token">JWT token</param>
        /// <returns>User ID if valid, null if invalid</returns>
        string? GetUserIdFromToken(string token);

        /// <summary>
        /// Validates a refresh token
        /// </summary>
        /// <param name="refreshToken">Refresh token to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        bool ValidateRefreshToken(string refreshToken);

        /// <summary>
        /// Gets the expiration time for access tokens
        /// </summary>
        /// <returns>Token expiration time in minutes</returns>
        int GetAccessTokenExpirationMinutes();
    }
}