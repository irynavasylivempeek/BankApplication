using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Transactions;
using BankApp.Domain.Transactions;
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

        [InverseProperty("SenderAccount")]
        public List<Transaction> Transactions { set; get; }

        [InverseProperty("ReceiverAccount")]
        public List<Transaction> IncomingTransferTransactions { get; set; }
    }
}
