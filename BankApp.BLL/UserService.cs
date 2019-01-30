using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using BankApp.DAL;
using BankApp.Domain;
using BankApp.Domain.Transactions;
using BankApp.DTO;
using BankApp.DTO.Enums;
using BankApp.DTO.Users;
using BankApp.Utils;
using Microsoft.EntityFrameworkCore.Internal;

namespace BankApp.BLL
{
    public interface IUserService
    {
        UserDto Add(LoginUser user);
        UserDto GetUserFullInfoById(int userId);
        bool Exists(int id);
        LoginResult Login(LoginUser loginUser);
        

    }
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public UserDto Add(LoginUser user)
        {
            var salt = SaltedHashGenerator.GenerateSalt();
            var newUser = _unitOfWork.Users.Add(new User()
            {
                Login = user.Login,
                Password = SaltedHashGenerator.GenerateSaltedHash(salt, user.Password),
                Salt = Convert.ToBase64String(salt),
                Account = new Account()
            });
            _unitOfWork.SaveChanges();
            return new UserDto()
            {
                UserId = newUser.UserId,
                Login = newUser.Login,
                Balance = newUser.Account.Balance
            };
        }

        public UserDto GetUserFullInfoById(int userId)
        {
            var user = _unitOfWork.Users.GetIncludingAccount(c => c.UserId == userId);
            if (user == null)
                return null;
            return new UserDto()
            {
                UserId = userId,
                Balance = user.Account.Balance,
                Login = user.Login,
                Transactions = user.Account.Transactions.Union(user.Account.IncomingTransferTransactions).Select(c =>
                {
                    Enum.TryParse(c.GetType().ShortDisplayName(), out TransactionType type);
                    return new TransactionDto()
                    {
                        UserId = c.Account.UserId,
                        Amount = c.Amount,
                        TransactionId = c.TransactionId,
                        Type = type,
                        ReceiverId = (c as TransferTransaction)?.DestinationId ?? 0
                    };
                }).ToList()
            };
        }

        public bool Exists(int id)
        {
            return _unitOfWork.Users.Find(id) != null;
        }
        public LoginResult Login(LoginUser loginUser)
        {
            var user = _unitOfWork.Users.FindSingleOrDefault(c => c.Login == loginUser.Login);
            if (user == null)
                return new LoginResult()
                {
                    Succeed = false,
                    ErrorMessage = "Wrong login"
                };
            bool valid = SaltedHashGenerator.ValidateHash(user.Password, loginUser.Password, Convert.FromBase64String(user.Salt));
            if (valid)
                return new LoginResult()
                {
                    UserId = user.UserId,
                    Succeed = true
                };
            return new LoginResult()
            {
                Succeed = false,
                ErrorMessage = "Wrong password"
            };

        }
    }
}
