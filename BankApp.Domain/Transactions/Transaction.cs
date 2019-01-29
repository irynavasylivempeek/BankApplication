using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankApp.Domain.Transactions
{
    public class Transaction
    {
        [Key]
        public int TransactionId { set; get; }
        public double Amount { set; get; }
        public int AccountId { set; get; }
        public Account Account { set; get; }
    }
}
