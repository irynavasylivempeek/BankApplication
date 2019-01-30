using System;
using System.Collections.Generic;
using System.Text;
using BankApp.DTO.Enums;

namespace BankApp.DTO
{
    public class TransactionDto
    {
        public int UserId { set; get; }
        public int TransactionId { set; get; }
        public double Amount { set; get; }
        public int ReceiverId { set; get; }
        public TransactionType Type { set; get; }

    }
}
