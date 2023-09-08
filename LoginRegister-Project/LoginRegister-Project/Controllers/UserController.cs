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
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetAllUser")]
        public async Task<List<User>> GetAllUser()
        {
            return await _userService.GetAllUser();
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("DeleteUser/{id}")]
        public async Task DeleteUser(int id)
        {
            await _userService.DeleteUser(id);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("ProfileUpdate")]
        public async Task<ActionResult<User>> UpdateUser([FromBody] User request)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);
            user.Id = request.Id;
            user.Email = request.Email;
            user.PasswordHash = passwordHash;
            user.Phone = request.Phone;
            user.Name = request.Name;
            user.LastName = request.LastName;

            await _userService.UpdateUser(user);
            return Ok(user);
        }
    }
}
