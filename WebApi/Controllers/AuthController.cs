using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineSchool.Core;
using OnlineSchool.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OnlineSchool.Application.DTOs;
using BCrypt.Net;
using OnlineSchool.Application.Services;

namespace OnlineSchool.WebApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly OnlineSchoolDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(IConfiguration configuration, OnlineSchoolDbContext context, JwtService jwtService)
        {
            _configuration = configuration;
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                return BadRequest("Пользователь с таким адресом электронной почты уже существует.");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.CreatedAt = DateTime.UtcNow;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Пользователь успешно зарегистрирован" });
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                return Unauthorized("Неверный адрес электронной почты или пароль");
            }

            var (accessToken, refreshToken) = _jwtService.GenerateTokens(user.Id.ToString(), user.Email);

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:RefreshTokenExpirationDays"])),
                UserId = user.Id
            };

            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken);
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Invalid access token");
            }

            if (!int.TryParse(userId, out int userIdInt))
            {
                return BadRequest("Invalid user ID");
            }

            var savedRefreshToken = await _context.RefreshTokens
                .SingleOrDefaultAsync(rt => rt.Token == request.RefreshToken && rt.UserId == int.Parse(userId));

            if (savedRefreshToken == null || savedRefreshToken.ExpiryDate < DateTime.UtcNow)
                return BadRequest("Invalid refresh token");

            var user = await _context.Users.FindAsync(int.Parse(userId));

            if (user == null)
            {
                return BadRequest("User not found");
            }

            var (accessToken, newRefreshToken) = _jwtService.GenerateTokens(userId, user.Email);

            savedRefreshToken.Token = newRefreshToken;
            savedRefreshToken.ExpiryDate = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:RefreshTokenExpirationDays"]));

            await _context.SaveChangesAsync();

            return Ok(new { AccessToken = accessToken, RefreshToken = newRefreshToken });
        }
    }
}

