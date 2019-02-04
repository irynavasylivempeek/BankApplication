using System.Collections.Generic;
using BankApp.DTO.Transactions;

namespace BankApp.DTO.Users
{
    public class User
    {
        public int UserId { set; get; }
        public string UserName { set; get; }
        public double Balance { set; get; }
        public List<TransactionDetails> Transactions { set; get; }
    }
}
