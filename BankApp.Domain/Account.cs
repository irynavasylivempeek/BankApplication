using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Transactions;
using Transaction = BankApp.Domain.Transactions.Transaction;

namespace BankApp.Domain
{
    public class Account
    {
        [Key]
        public int AccountId { set; get; }
        public double Balance { set; get; }
        public int UserId { set; get; }
        public User User { set; get; }

        public List<Transaction> Transactions { set; get; }   
              
        
    }
}
