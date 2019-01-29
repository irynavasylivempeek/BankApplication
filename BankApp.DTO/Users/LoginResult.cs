using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.DTO.Users
{
    public class LoginResult
    {
        public bool Succeed { set; get; }
        public int UserId { set; get; }
        public string ErrorMessage { set; get; }
    }
}
