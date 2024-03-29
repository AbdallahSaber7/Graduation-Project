using System.ComponentModel.DataAnnotations;

namespace Credit_Hours_System.Api.Dtos
{
    public class loginDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
