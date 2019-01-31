using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApp.BLL;
using BankApp.DTO;
using BankApp.DTO.Users;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public void Register([FromBody]Login user)
        {
            _userService.Add(user);
        }

        [HttpPost("login")]
        public LoginResult Login([FromBody] Login user)
        {
            return _userService.Login(user);
        }

        [HttpGet("userInfo/{id}")]
        public UserDto UserInfo(int id)
        {
            return _userService.GetUserFullInfoById(id);
        }
    }
}
