using BankApp.Domain.Enums;

namespace BankApp.DTO.Transaction
{
    public class TransactionDto
    {
        public int SenderId { set; get; }
        public int TransactionId { set; get; }
        public double Amount { set; get; }
        public int? ReceiverId { set; get; }
        public TransactionType Type { set; get; }
    }
}
