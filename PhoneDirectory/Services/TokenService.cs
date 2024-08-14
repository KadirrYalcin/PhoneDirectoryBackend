using PhoneDirectory.Models;

namespace PhoneDirectory.Services
{
    public interface TokenService
    {
        string CreateToken(AppUser appUser);
    }
}
