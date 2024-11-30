using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public class User
    {
        [Key]
        public string UserId { get; set; }=Guid.NewGuid().ToString();//To Genetate Default value when value not taken 
        [Required]
        public string? Name { get; set; }//Takes Null value given by User ?
        [Required,EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? PasswordHash { get; set; }
        public bool IsActive { get; set; }
    }
}
