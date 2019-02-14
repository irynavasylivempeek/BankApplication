using System;
using System.Collections.Generic;
using System.Text;
using BankApp.DTO.Users;

namespace BankApp.DTO.Transactions
{
    public class TransactionResult
    {
        public bool Success { set; get; }
        public string ErrorMessage { set; get; }
        public UserDetails User { set; get; }
    }
}
