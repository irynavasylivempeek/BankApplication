using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BankApp.BLL;
using BankApp.DTO;
using BankApp.DTO.Transaction;
using BankApp.ViewModel.TransactionsViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

        [HttpPost("deposite")]
        public TransactionResult Deposite([FromBody]DepositeViewModel depositeViewModel)
        {
            if (ModelState.IsValid)
            {
                _transactionService.Deposit(depositeViewModel.UserId, depositeViewModel.Amount);
                var freshUserDetails = _userService.GetUserFullInfoById(depositeViewModel.UserId);
                return new TransactionResult {IsSuccessful = true, UserDetails = freshUserDetails};
            }
            var result = new TransactionResult
            {
                IsSuccessful = false,
                ErrorMessages = typeof(DepositeViewModel)
                    .GetProperties()
                    .SelectMany(c =>
                    {
                        if (ModelState.TryGetValue(c.Name, out ModelStateEntry value))
                        {
                            var errors = value.Errors.Select(error => error.ErrorMessage);
                            return errors;
                        }
                        return new List<string>();
                    }).ToList()
            };
            return result;
        }

        [HttpPost("withdraw")]

        public TransactionResult Withdraw([FromBody]WithdrawViewModel withdrawViewModel)
        {
            if (ModelState.IsValid)
            {
                _transactionService.Withdraw(withdrawViewModel.UserId, withdrawViewModel.Amount);
                var freshUserDetails = _userService.GetUserFullInfoById(withdrawViewModel.UserId);
                return new TransactionResult { IsSuccessful = true, UserDetails = freshUserDetails };
            }
            var result = new TransactionResult
            {
                IsSuccessful = false,
                ErrorMessages = typeof(WithdrawViewModel)
                    .GetProperties()
                    .SelectMany(c =>
                    {
                        if (ModelState.TryGetValue(c.Name, out ModelStateEntry value))
                        {
                            var errors = value.Errors.Select(error => error.ErrorMessage);
                            return errors;
                        }
                        return new List<string>();
                    }).ToList()
            };
            return result;
        }

        [HttpPost("transfer")]
        public TransactionResult Transfer([FromBody]TransferViewModel transferViewModel)
        {
            if (ModelState.IsValid)
            {
                _transactionService.Transfer(transferViewModel.UserId, transferViewModel.ReceiverId, transferViewModel.Amount);
                var freshUserDetails = _userService.GetUserFullInfoById(transferViewModel.UserId);
                return new TransactionResult { IsSuccessful = true, UserDetails = freshUserDetails };
            }

            var result =  new TransactionResult{
                IsSuccessful = false,
                ErrorMessages = typeof(TransferViewModel)
                    .GetProperties()
                    .SelectMany(c=>
                    {
                        if(ModelState.TryGetValue(c.Name, out ModelStateEntry value))
                        {
                            var errors =  value.Errors.Select(error => error.ErrorMessage);
                            return errors;
                        }
                        return new List<string>();
                    }).ToList()
            };
            return result;



        }
    }
}
