using System.ComponentModel.DataAnnotations;

namespace Credit_Hours_System.Api.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string DisplayName { get; set; }
    }
}
