using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.DTO.Users
{
    public class LoginResult
    {
        public bool Success { set; get; }
        public UserDetails User { set; get; }
        public string ErrorMessage { set; get; }
    }
}
