using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApp.BLL;
using BankApp.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly IUserService _userService;

        public AccountController(ITransactionService transactionService, IUserService userService)
        {
            _transactionService = transactionService;
            _userService = userService;
        }

        [HttpPost("deposite/{id}")]
        public string Deposite(int id, [FromBody] double amount)
        {
            _transactionService.Deposit(id, amount);
            var newBalance = _userService.GetById(id).Balance;
            return $"Rest: {newBalance}";
        }

        [HttpPost("withdraw/{id}")]

        public string Withdraw(int id, [FromBody]double amount)
        {
            var account = _userService.GetById(id);
            if (account == null)
                return null;
            if (account.Balance < amount)
                return "Lack of money on the account to withdraw";
            _transactionService.Withdraw(id, amount);
            var newBalance = _userService.GetById(id).Balance;
            return $"Rest: {newBalance}";
        }

        [HttpPost("transfer/{id}")]
        public string Transfer(int id, [FromBody]int receiverId, [FromBody]double amount)
        {
            var account = _userService.GetById(id);
            if (account == null)
                return null;
            if (account.Balance < amount)
                return "Lack of money on the account to make transfer";
            _transactionService.Transfer(id, receiverId, amount);
            var newBalance = _userService.GetById(id).Balance;
            return $"Rest: {newBalance}";
        }


    }
}
