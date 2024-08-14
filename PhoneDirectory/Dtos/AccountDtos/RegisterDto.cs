using System.ComponentModel.DataAnnotations;

namespace PhoneDirectory.Dtos.AccountDtos
{
    public class RegisterDto
    {
        [Required]
        public String? FullName { get; set; }
        [Required]
        [EmailAddress]
        public String? Email { get; set; }
        [Required]
        public String? Password { get; set; }
    }
}
