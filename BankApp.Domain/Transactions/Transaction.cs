using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using BankApp.Domain.Enums;

namespace BankApp.Domain.Transactions
{
    public class Transaction
    {
        [Key]
        public int TransactionId { set; get; }

        public double Amount { set; get; }

        public int SenderAccountId { set; get; }
        public Account SenderAccount { set; get; }

        public int? ReceiverAccountId { set; get; }
        public Account ReceiverAccount { set; get; }

        public TransactionType Type { set; get; }
    }
}
