using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using BankApp.BLL;
using BankApp.Domain.Enums;
using BankApp.DTO;
using BankApp.DTO.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly IUserService _userService;

        public AccountController(ITransactionService transactionService, IUserService userService)
        {
            _transactionService = transactionService;
            _userService = userService;
        }

        [HttpPost("deposit")]
        public TransactionResult Deposit([FromBody]Transaction transaction)
        {
            transaction.Type = TransactionType.Deposit;
            return MakeTransaction(transaction);
        }

        [HttpPost("withdraw")]
        public TransactionResult Withdraw([FromBody] Transaction transaction)
        {
            transaction.Type = TransactionType.Withdraw;
            return MakeTransaction(transaction);
        }

        [HttpPost("transfer")]
        public TransactionResult Transfer([FromBody]Transaction transaction)
        {
            transaction.Type = TransactionType.Transfer;
            return MakeTransaction(transaction);
        }

        private TransactionResult MakeTransaction(Transaction transaction)
        {
            var id = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            transaction.SenderId = id;
            try
            {
                CheckTransactionIsCorrect(transaction);
                _transactionService.MakeTransaction(transaction);
            }
            catch (Exception e)
            {
                return new TransactionResult{ ErrorMessage = e.Message };
            }
            var freshUserDetails = _userService.GetFullInfoById(transaction.SenderId);
            return new TransactionResult { Success = true, User = freshUserDetails };
        }

        private void CheckTransactionIsCorrect(Transaction transaction)
        {
            if (transaction.Amount < 0)
            {
                throw new Exception("Amount has to be positive number");
            }

            var user = _userService.GetFullInfoById(transaction.SenderId);
            if (user == null)
            {
               throw new Exception("Sender was not found");
            }
            if (transaction.Type != TransactionType.Deposit && transaction.Amount > user.Balance)
            {
                throw new Exception("Lack of money to make transaction");
            }
            if (transaction.Type == TransactionType.Transfer && (transaction.ReceiverId == null || !_userService.Exists(transaction.ReceiverId.Value)))
            {
                throw new Exception("Receiver was not found");
            }
            if (transaction.SenderId == transaction.ReceiverId)
            {
                throw new Exception("Sender and receiver cannot be the same account");
            }
        }
    }
}
