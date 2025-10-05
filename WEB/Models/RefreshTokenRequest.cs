using System.ComponentModel.DataAnnotations;

namespace WEB.Models
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}