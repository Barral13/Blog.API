using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Blog.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace Blog.API.Services;
public class TokenService
{
   public string GenerateToken(User user)
   {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new (ClaimTypes.Name, "dhanglersonbarral"), // User.Identity.Name
                new (ClaimTypes.Role, "user"), 
                new (ClaimTypes.Role, "admin") // User.IsInRole
            }),
         Expires = DateTime.UtcNow.AddHours(8),
         SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      
      return tokenHandler.WriteToken(token);
   }
}