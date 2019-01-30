using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.DTO.Transaction
{
    public class TransactionResult
    {
        public bool IsSuccessful { set; get; }
        public IEnumerable<string> ErrorMessages { set; get; }
        public UserDto UserDetails { set; get; }
    }
}
