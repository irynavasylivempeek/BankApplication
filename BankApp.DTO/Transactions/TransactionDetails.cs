using System;
using System.Collections.Generic;
using System.Text;
using BankApp.Domain.Enums;
using BankApp.DTO.Users;

namespace BankApp.DTO.Transactions
{
    public class TransactionDetails
    {
        public int TransactionId { set; get; }
        public User Sender { set; get; }
        public User Receiver { set; get; }
        public double Amount { set; get; }
        public TransactionType Type { set; get; }
        public string TypeDescription { set; get; }
        public bool Income { set; get; }
    }
}
