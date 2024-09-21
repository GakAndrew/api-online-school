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

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                return Unauthorized("Неверный адрес электронной почты или пароль");
            }

            var token = _jwtService.GenerateToken(user.Id.ToString(), user.Email);

            return Ok(new { token });
        }
    }
}
