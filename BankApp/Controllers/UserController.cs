using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApp.BLL;
using BankApp.DTO;
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
        public void Register([FromBody]LoginUser user)
        {
            _userService.Add(user);
        }

        [HttpPost("login")]
        public bool Login([FromBody] LoginUser user)
        {
            return _userService.Login(user).Succeed;
        }
    }
}
