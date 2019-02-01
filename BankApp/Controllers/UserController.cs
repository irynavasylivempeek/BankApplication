using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BankApp.BLL;
using BankApp.DTO;
using BankApp.DTO.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BankApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;

        public UserController(IUserService userService, IConfiguration config)
        {
            _userService = userService;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public LoginResult Register([FromBody]Login user)
        {
            var registerResult = _userService.Add(user);
            if (registerResult.Success)
            {
                var token = GenerateJsonWebToken(registerResult.User);
                registerResult.Token = token;
            }
            return registerResult;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] Login user)
        {
            IActionResult response = Unauthorized();
            var loginResult = _userService.Login(user);
            if (loginResult.Success)
            {
                var token = GenerateJsonWebToken(loginResult.User);
                loginResult.Token = token;
                response = Ok(loginResult);
            }
            return response;
        }

        [HttpGet("userInfo/{id}")]
        public UserDetails UserInfo(int id)
        {
            return _userService.GetUserFullInfoById(id);
        }

        private string GenerateJsonWebToken(UserDetails user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expiryTime = DateTime.Now + new TimeSpan(0, int.Parse(_config["Jwt:ExpiryInMinutes"]), 0);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: expiryTime,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
