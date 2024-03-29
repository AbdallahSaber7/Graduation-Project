using System.ComponentModel.DataAnnotations;

namespace Credit_Hours_System.Api.Dtos
{
    public class RolesDto
    {
        [Required]
        public string RoleName { get; set; }
    }
}
