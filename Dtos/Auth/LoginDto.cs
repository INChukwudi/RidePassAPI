using System.ComponentModel.DataAnnotations;

namespace RidePassAPI.Dtos.Auth
{
    public class LoginDto
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
