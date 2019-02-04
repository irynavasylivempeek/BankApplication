using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.DTO.Users
{
    public class LoginResult
    {
        public bool Success { set; get; }
        public User User { set; get; }
        public string ErrorMessage { set; get; }
        public string Token { set; get; }
    }
}
