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
        public string UserName { set; get; }
        public string Password { set; get; }
        public byte[] Salt { set; get; }
        public Account Account { set; get; }
    }
}
