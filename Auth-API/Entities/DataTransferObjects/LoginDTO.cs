using System.ComponentModel.DataAnnotations;

namespace Auth_API.Entities.DataTransferObjects
{
    public class LoginDTO
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
