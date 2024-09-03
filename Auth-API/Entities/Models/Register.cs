using System.ComponentModel.DataAnnotations;

namespace Auth_API.Entities.Models
{
    public class Register
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
