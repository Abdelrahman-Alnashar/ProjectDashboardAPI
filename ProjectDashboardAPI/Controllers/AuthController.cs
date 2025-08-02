using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectDashboardAPI.Data;
using ProjectDashboardAPI.Models;
using ProjectDashboardAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectDashboardAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly AppDbContext _appDbContext;
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext context, JwtService jwtService)
        {
            _appDbContext = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest req)
        {
            var user = _appDbContext.Users.FirstOrDefault(user => user.Email == req.Email);
            if (user == null) return Unauthorized("User not found");

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.Password, req.Password);

            if (result != PasswordVerificationResult.Success)
                return Unauthorized("Invalid password");

            var accessToken = _jwtService.GenerateToken(user);

            var refreshToken = Guid.NewGuid().ToString();

            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            });


            return Ok(new { Token = accessToken, User = user });

        }

        [HttpPost("refresh")]
        public IActionResult Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (refreshToken == null) return Unauthorized("Refresh token missing");

            var user = _appDbContext.Users.FirstOrDefault(u => u.RefreshToken == refreshToken);

            if (user == null) return Unauthorized("Invalid refresh token");

            var newAccessToken = _jwtService.GenerateToken(user);

            var newRefreshToken = Guid.NewGuid().ToString();
            user.RefreshToken = newRefreshToken;
            _appDbContext.SaveChanges();

            Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            });

            return Ok(new { Token = newAccessToken, User = user });
         
            
        }

        [HttpPost("logout")]
        public IActionResult logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var user = _appDbContext.Users.FirstOrDefault(u => u.RefreshToken == refreshToken);
            if (user != null)
            {
                user.RefreshToken = null;
                _appDbContext.SaveChanges();
            }

            Response.Cookies.Delete("refreshToken");
            return Ok("Logged out successfully");
        }

    }
}
