using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Domain.Transactions
{
    public class TransferTransaction : Transaction
    {
        public int? DestinationId { set; get; }
        public Account Destination { set; get; }
    }
}
