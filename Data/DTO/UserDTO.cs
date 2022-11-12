using System.ComponentModel.DataAnnotations;

namespace JD.API.Data.DTO
{
    public class UserDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; } 
    }
}