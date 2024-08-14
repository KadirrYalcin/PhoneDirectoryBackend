using Microsoft.IdentityModel.Tokens;
using PhoneDirectory.Models;
using PhoneDirectory.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PhoneDirectory.Repostory
{
    public class TokenRepostory : TokenService
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;
        public TokenRepostory(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]));
        }
        public string CreateToken(AppUser appUser)
        {
            var claims = new List<Claim>
          {
              new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
              new Claim(JwtRegisteredClaimNames.GivenName, appUser.UserName),
              new Claim(ClaimTypes.Role, "User")
          };
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = creds,
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"]
            };
            var tokenHandler=new JwtSecurityTokenHandler();
            var token=tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
