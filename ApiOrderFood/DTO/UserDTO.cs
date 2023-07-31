using System.ComponentModel.DataAnnotations;

namespace ApiOrderFood.DTO
{
    public class UserDTO
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
