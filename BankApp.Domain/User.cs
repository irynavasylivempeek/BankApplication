using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankApp.Domain
{
    public class User
    {
        [Key]
        public int UserId { set; get; }
        public string Login { set; get; }
        public string Password { set; get; }

        public Account Account { set; get; }
    }
}
