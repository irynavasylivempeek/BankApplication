using System;
using System.Collections.Generic;
using System.Text;
using BankApp.DTO.Transaction;

namespace BankApp.DTO
{
    public class UserDto
    {
        public int UserId { set; get; }
        public string UserName { set; get; }
        public double Balance { set; get; }
        public List<TransactionDto> Transactions { set; get; }
    }
}
