using System.ComponentModel.DataAnnotations;

namespace Auth_API.Entities.Models
{
    public class Login
    { 
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
