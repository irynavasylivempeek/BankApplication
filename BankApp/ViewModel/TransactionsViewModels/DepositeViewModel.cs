using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApp.ValidationAttributes;

namespace BankApp.ViewModel.TransactionsViewModels
{
    public class DepositeViewModel
    {
        [ExistsUser]
        public int UserId { set; get; }

        [PositiveNoZero]
        public double Amount { set; get; }
    }
}
