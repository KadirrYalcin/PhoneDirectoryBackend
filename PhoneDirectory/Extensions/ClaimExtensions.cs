using System.Security.Claims;

namespace PhoneDirectory.Extensions
{
    public static class ClaimExtensions
    {
        public static string GetEmail( this ClaimsPrincipal user ) {
            return user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        }
    }
}
