using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using BankApp.DAL;
using BankApp.DAL.Repositories;
using BankApp.Domain;
using BankApp.Domain.Enums;
using BankApp.DTO;
using BankApp.DTO.Transaction;
using BankApp.DTO.Users;
using BankApp.Utils;
using Microsoft.EntityFrameworkCore.Internal;

namespace BankApp.BLL
{
    public interface IUserService
    {
        LoginResult Add(Login user);
        UserDetails GetUserFullInfoById(int userId);
        bool Exists(int id);
        LoginResult Login(Login loginUser);
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public LoginResult Add(Login user)
        {
            if (_userRepository.SingleOrDefault(c => c.UserName == user.UserName) != null)
                return new LoginResult()
                {
                    Success = false,
                    ErrorMessage = "There is a user with the same UserName"
                };
            var salt = SaltedHashGenerator.GenerateSalt();
            var newUser = _userRepository.Add(new User()
            {
                UserName = user.UserName,
                Password = SaltedHashGenerator.GenerateSaltedHash(user.Password, salt),
                Salt = salt,
                Account = new Account()
            });
            _userRepository.SaveChanges();
            return new LoginResult()
            {
                Success = true,
                User = new UserDetails()
                {
                    UserId = newUser.UserId,
                    UserName = newUser.UserName,
                    Balance = newUser.Account.Balance
                }
            };
        }

        public UserDetails GetUserFullInfoById(int userId)
        {
            var user = _userRepository.GetWithTransactions(c => c.UserId == userId);
            if (user == null)
                return null;
            return new UserDetails()
            {
                UserId = userId,
                Balance = user.Account.Balance,
                UserName = user.UserName,
                Transactions = user.Account.Transactions.Union(user.Account.IncomingTransferTransactions).Select(c => new TransactionDetails()
                {
                    SenderId = c.SenderAccount.UserId,
                    Amount = c.Amount,
                    TransactionId = c.TransactionId,
                    Type = c.Type,
                    ReceiverId = c.ReceiverAccount?.UserId ?? 0,
                }).ToList()
            };
        }

        public bool Exists(int id)
        {
            return _userRepository.Find(id) != null;
        }
        public LoginResult Login(Login loginUser)
        {
            var user = _userRepository.GetWithTransactions(c => c.UserName == loginUser.UserName);
            if (user == null)
                return new LoginResult()
                {
                    Success = false,
                    ErrorMessage = "Wrong login"
                };
            bool valid = SaltedHashGenerator.VerifyHash(user.Password, loginUser.Password, user.Salt);
            if (valid)
                return new LoginResult()
                {
                    User = new UserDetails()
                    {
                        UserId = user.UserId,
                        Balance = user.Account.Balance,
                        UserName = user.UserName
                    },
                    Success = true
                };
            return new LoginResult()
            {
                Success = false,
                ErrorMessage = "Wrong password"
            };
        }
    }
}
