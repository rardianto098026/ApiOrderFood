using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiOrderFood.Models;
using Microsoft.AspNetCore.Authorization;
using ApiOrderFood.Services.User;
using ApiOrderFood.DTO;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

namespace ApiOrderFood.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IUsersServices _usersServices;
        private readonly IConfiguration _configuration;
        public AuthController(IUsersServices usersServices, IConfiguration configuration, DatabaseContext context)
        {
            _usersServices = usersServices;
            _configuration = configuration;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] UserDTO request)
        {
            var user = await _context.User.Where(x => x.Username == request.Username).SingleOrDefaultAsync();
            if (user == null) return NotFound();
            if (user.Username != request.Username)
            {
                return BadRequest("User Not Found");
            }
            //string salt = BCrypt.Net.BCrypt.GenerateSalt(); ;
            //string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);
            int salt = 11;
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);
            bool verified = BCrypt.Net.BCrypt.Verify(user.Password, passwordHash);
            if (verified)
            {
                string token = CreateToken(user);
                var data = new UserResponse();
                data.Username = user.Username;
                data.Password = user.Password;
                data.UserId = user.Id;
                data.Role = user.Role;
                data.Token = token;
                return Ok(data);
            }
            else
            {
                return Ok("Password is not match");
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("sub", user.Id.ToString())
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("JWT:SecretKey").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            DateTime d1 = DateTime.Now;
            DateTime d2 = d1.AddHours(3);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: d2,
                signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

    }
}
