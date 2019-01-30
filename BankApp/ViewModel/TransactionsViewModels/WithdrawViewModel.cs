using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApp.ValidationAttributes;

namespace BankApp.ViewModel.TransactionsViewModels
{
    public class WithdrawViewModel
    {
        [ExistsUser]
        public int UserId { set; get; }

        [PositiveNoZero]
        [LessThanBalance("UserId")]
        public double Amount { set; get; }
    }
}
