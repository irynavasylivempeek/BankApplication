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
using BankApp.DTO.Transactions;
using BankApp.DTO.Users;
using BankApp.Utils;
using Microsoft.EntityFrameworkCore.Internal;
using Transaction = BankApp.DTO.Transactions.Transaction;

namespace BankApp.BLL
{
    public interface IUserService
    {
        LoginResult Add(Login loginUser);
        UserDetails GetFullInfoById(int userId);
        LoginResult Login(Login loginUser);
        IEnumerable<UserDetails> GetAll();
        bool Exists(int userId);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public LoginResult Add(Login loginUser)
        {
            if (_userRepository.SingleOrDefault(c => c.UserName == loginUser.UserName) != null)
                return new LoginResult()
                {
                    Success = false,
                    ErrorMessage = "There is a userDetails with the same login"
                };
            var salt = SaltedHashGenerator.GenerateSalt();
            var newUser = _userRepository.Add(new Domain.User
            {
                UserName = loginUser.UserName,
                Password = SaltedHashGenerator.GenerateSaltedHash(loginUser.Password, salt),
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

        public UserDetails GetFullInfoById(int userId)
        {
            var user = _userRepository.GetWithTransactions(c => c.UserId == userId);
            if (user == null)
                return null;
            return new UserDetails()
            {
                UserId = user.UserId,
                Balance = user.Account.Balance,
                UserName = user.UserName,
                Transactions = user.Account.Transactions
                    .Union(user.Account.IncomingTransferTransactions)
                    .OrderByDescending(c => c.TransactionId)
                    .Select(c => new TransactionDetails()
                    {
                        Sender = new DTO.Users.User()
                        {
                            UserId = c.SenderAccount.UserId,
                            UserName = c.SenderAccount.User.UserName
                        },
                        Receiver = c.ReceiverAccountId == null ? null : new DTO.Users.User()
                        {
                            UserId = c.ReceiverAccount.UserId,
                            UserName = c.ReceiverAccount.User.UserName
                        },
                        Amount = c.Amount,
                        TransactionId = c.TransactionId,
                        Type = c.Type,
                        Income = c.Type == TransactionType.Deposit || c.Type == TransactionType.Transfer && c.ReceiverAccountId == user.Account.AccountId
                    }).ToList()
            };
        }

        public LoginResult Login(Login loginUser)
        {
            var user = _userRepository.SingleOrDefault(c => c.UserName == loginUser.UserName);
            if (user == null)
                return new LoginResult()
                {
                    Success = false,
                    ErrorMessage = "Wrong login"
                };
            bool valid = SaltedHashGenerator.VerifyHash(user.Password, loginUser.Password, user.Salt);
            return new LoginResult()
            {
                User = valid ? new UserDetails()
                {
                    UserId = user.UserId,
                    UserName = user.UserName
                } : null,
                ErrorMessage = valid ? null : "Wrong password",
                Success = valid
            };
        }

        public IEnumerable<UserDetails> GetAll()
        {
            return _userRepository.GetAll().Select(c => new UserDetails()
            {
                UserId = c.UserId,
                UserName = c.UserName
            });
        }

        public bool Exists(int userId)
        {
            return _userRepository.Find(userId) != null;
        }
    }
}
