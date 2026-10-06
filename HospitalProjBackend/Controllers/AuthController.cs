////using Microsoft.AspNetCore.Mvc;
////using Microsoft.EntityFrameworkCore;
////using Microsoft.AspNetCore.Identity;
////using HospitalProjBackend.Data;
////using HospitalProjBackend.Models;
////using HospitalProjBackend.Services;
////using Microsoft.AspNetCore.Authorization;


////namespace HospitalProjBackend.Controllers
////{
////    [ApiController]
////    [Route("api/[controller]")]
////    public class AuthController : ControllerBase
////    {
////        private readonly AppDbContext _db;
////        private readonly IJwtService _jwt;
////        private readonly IPasswordHasher<User> _hasher;

////        public AuthController(AppDbContext db, IJwtService jwt, IPasswordHasher<User> hasher)
////        {
////            _db = db; _jwt = jwt; _hasher = hasher;
////        }

////        [HttpPost("register")]
////        public async Task<IActionResult> Register([FromBody] User model)
////        {
////            if (await _db.Users.AnyAsync(u => u.Username == model.Username)) return BadRequest(new { error = "Username taken" });
////            var user = new User { Username = model.Username, Role = "Staff" };
////            user.PasswordHash = _hasher.HashPassword(user, model.PasswordHash); // expect raw password in PasswordHash field on register
////            _db.Users.Add(user);
////            await _db.SaveChangesAsync();
////            return Ok(new { message = "Registered" });
////        }

////        [HttpPost("register-admin"), Authorize(Roles = "Admin")]
////        public async Task<IActionResult> RegisterAdmin([FromBody] User model)
////        {
////            if (await _db.Users.AnyAsync(u => u.Username == model.Username)) return BadRequest(new { error = "Username taken" });
////            var user = new User { Username = model.Username, Role = model.Role ?? "Staff" };
////            user.PasswordHash = _hasher.HashPassword(user, model.PasswordHash);
////            _db.Users.Add(user);
////            await _db.SaveChangesAsync();
////            return Ok(new { message = "Registered" });
////        }

////        [HttpPost("login")]
////        public async Task<IActionResult> Login([FromBody] User model)
////        {
////            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
////            if (user == null) return Unauthorized();
////            var res = _hasher.VerifyHashedPassword(user, user.PasswordHash, model.PasswordHash);
////            if (res == PasswordVerificationResult.Failed) return Unauthorized();
////            var token = _jwt.GenerateToken(user);
////            return Ok(new { token });
////        }
////    }


////}
//using HospitalProjBackend.Data;
//using HospitalProjBackend.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using Org.BouncyCastle.Crypto.Generators;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace HospitalProjBackend.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class AuthController : ControllerBase
//    {
//        private readonly AppDbContext _ctx;
//        private readonly IConfiguration _config;

//        public AuthController(AppDbContext ctx, IConfiguration config)
//        {
//            _ctx = ctx;
//            _config = config;
//        }

//        [HttpPost("register")]
//        public IActionResult Register([FromBody] User user)
//        {
//            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
//            _ctx.Users.Add(user);
//            _ctx.SaveChanges();
//            return Ok();
//        }

//        [HttpPost("login")]
//        public IActionResult Login([FromBody] LoginRequest model)
//        {
//            var user = _ctx.Users.FirstOrDefault(u => u.Username == model.Username);
//            if (user == null || !BCrypt.Net.BCrypt.Verify(model.PasswordHash, user.PasswordHash))
//                return Unauthorized(new { message = "Invalid username or password" });

//            var token = _jwtService.GenerateToken(user); // JWT token
//            return Ok(new { token }); // <-- must return `token` property
//        }

//    }
//}
using Microsoft.AspNetCore.Mvc;
using HospitalProjBackend.Models;
using HospitalProjBackend.Models.DTO;
using HospitalProjBackend.Services;
using BCrypt.Net;
using HospitalProjBackend.Data;

namespace HospitalProjBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == request.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.PasswordHash, user.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            var token = _jwtService.GenerateToken(user);
            return Ok(new { token });
        }
    }
}
