using LoginRegister_Project.IService;
using LoginRegister_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoginRegister_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _configuration;
        private readonly IGenericService<User> _userService;
        public UserController(IConfiguration configuration, IGenericService<User> userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<User> Register(UserDTO request)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.Email = request.Email;
            user.PasswordHash = passwordHash;
            user.Phone = request.Phone;
            user.Name = request.Name;
            user.LastName = request.LastName;

            await _userService.AddUser(user);
            return user;
        }

        [HttpPost("login")]
        public ActionResult<User> Login(LoginDTO request)
        {
            if(user.Email==request.Email && BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                string token = CreateToken(user);
                var response = new ApiResponse<object>
                {
                    IsSuccess = true,
                    Data = new { Token = token },
                    Error = null
                };

                return Ok(response);
            }
            else
            {
                var errorResponse = new ApiResponse<object>
                {
                    IsSuccess = false,
                    Data = null,
                    Error = "hata"
                };

                return BadRequest(errorResponse);
            }
        }
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name)
                };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
