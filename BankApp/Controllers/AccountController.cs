using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BankApp.BLL;
using BankApp.Domain.Enums;
using BankApp.DTO;
using BankApp.DTO.Transaction;
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
        public TransactionResult Deposit([FromBody]TransactionDetails transaction)
        {
            try
            {
                transaction.Type = TransactionType.DepositTransaction;
                _transactionService.MakeTransaction(transaction);
            }
            catch (Exception)
            {
                return new TransactionResult { Success = false };
            }
            var freshUserDetails = _userService.GetUserFullInfoById(transaction.SenderId);
            return new TransactionResult { Success = true, UserDetails = freshUserDetails };
        }

        [HttpPost("withdraw")]
        public TransactionResult Withdraw([FromBody] TransactionDetails transaction)
        {
            try
            {
                transaction.Type = TransactionType.WithdrawTransaction;
                _transactionService.MakeTransaction(transaction);
            }
            catch (Exception)
            {
                return new TransactionResult { Success = false };
            }
            var freshUserDetails = _userService.GetUserFullInfoById(transaction.SenderId);
            return new TransactionResult { Success = true, UserDetails = freshUserDetails };
        }

        [HttpPost("transfer")]
        public TransactionResult Transfer([FromBody]TransactionDetails transaction)
        {
            try
            {
                transaction.Type = TransactionType.TransferTransaction;
                _transactionService.MakeTransaction(transaction);
            }
            catch (Exception)
            {
                return new TransactionResult { Success = false };
            }
            var freshUserDetails = _userService.GetUserFullInfoById(transaction.SenderId);
            return new TransactionResult { Success = true, UserDetails = freshUserDetails };
        }
    }
}
