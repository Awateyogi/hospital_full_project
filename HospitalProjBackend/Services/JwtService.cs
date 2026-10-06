//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using Microsoft.IdentityModel.Tokens;
//using HospitalProjBackend.Models;


//namespace HospitalProjBackend.Services
//{
//    public interface IJwtService { string GenerateToken(User user); }
//    public class JwtService : IJwtService
//    {
//        private readonly IConfiguration _config;
//        public JwtService(IConfiguration config) { _config = config; }
//        public string GenerateToken(User user)
//        {
//            var key = _config["Jwt__Key"] ?? _config["Jwt:Key"] ?? throw new Exception("Missing JWT Key");
//            var issuer = _config["Jwt__Issuer"] ?? _config["Jwt:Issuer"] ?? "hospital.local";
//            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
//            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

//            var claims = new List<Claim> {
//            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
//            new Claim("id", user.Id.ToString()),
//            new Claim(ClaimTypes.Role, user.Role ?? "User"),
//            new Claim("role", user.Role ?? "User")
//        };

//            var token = new JwtSecurityToken(issuer: issuer,
//              claims: claims,
//              expires: DateTime.UtcNow.AddDays(7),
//              signingCredentials: credentials);

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }

//    }
//}
// Services/JwtService.cs
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HospitalProjBackend.Models;
using Microsoft.IdentityModel.Tokens;

namespace HospitalProjBackend.Services
{
    public class JwtService
    {
        private readonly IConfiguration _config;
        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
