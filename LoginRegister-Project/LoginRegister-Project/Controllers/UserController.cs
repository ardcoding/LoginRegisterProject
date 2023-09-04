using LoginRegister_Project.Domain.Interface;
using LoginRegister_Project.Domain.Models;
using LoginRegister_Project.Domain.DTOs;
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
        public async Task<ActionResult<User>> Login(LoginDTO request)
        {
            var findUser = await _userService.GetUser(request.Email);
            if (findUser == null) { return NotFound("User Not Found"); }
            if(findUser.Email==request.Email && BCrypt.Net.BCrypt.Verify(request.Password, findUser.PasswordHash))
            {
                string token = CreateToken(findUser);
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

        [HttpGet("GetAllUser")]
        public async Task<List<User>> GetAllUser()
        {
           return await _userService.GetAllUser();
        }


        [HttpGet("GetAllUser/{id}")]
        public async Task DeleteUser(int id)
        {
           await _userService.DeleteUser(id);
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
