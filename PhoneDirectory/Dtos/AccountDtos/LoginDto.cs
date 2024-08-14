using System.ComponentModel.DataAnnotations;

namespace PhoneDirectory.Dtos.AccountDtos
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
