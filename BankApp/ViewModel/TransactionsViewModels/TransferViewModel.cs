using System.ComponentModel.DataAnnotations;
using BankApp.ValidationAttributes;

namespace BankApp.ViewModel.TransactionsViewModels
{
    public class TransferViewModel
    {
        [ExistsUser]
        public int UserId { set; get; }

        [Unlike("UserId")]
        [ExistsUser]
        public int ReceiverId { set; get; }

        [PositiveNoZero]
        [LessThanBalance("UserId")]
        public double Amount { set; get; }
    }
}
