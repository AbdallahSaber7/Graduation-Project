using System.ComponentModel.DataAnnotations;

namespace Credit_Hours_System.Api.Dtos
{
    public class ResetPasswordDto
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm password is required")]
        [Compare(nameof(Password), ErrorMessage = "Password mismatch")]
        public string ConfirmPassword { get; set; }
    }
}
