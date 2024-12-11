using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public class User
    {
        [Key]
        public string UserId { get; set; }=Guid.NewGuid().ToString();//To Genetate Default value when value not taken 
        [Required,MaxLength(40)]
        public string? Name { get; set; }//Takes Null value given by User ?
        [Required,EmailAddress,MaxLength(254)]
        public string? Email { get; set; }
        [Required,MaxLength(255)]

        public string? PasswordHash { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        [Required]
        public DateTime? UpdatedAt { get; set; }
    }
}
