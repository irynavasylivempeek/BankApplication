using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankApp.DAL;
using BankApp.Domain;
using BankApp.DTO;
using BankApp.DTO.Users;
using BankApp.Utils;

namespace BankApp.BLL
{
    public interface IUserService
    {
        UserDto Add(LoginUser user);
        UserDto GetById(int userId);
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

        public UserDto GetById(int userId)
        {
            var user = _unitOfWork.Users.GetIncludingAccount(c => c.UserId == userId);
            if (user == null)
                return null;
            return new UserDto()
            {
                UserId = userId,
                Balance = user.Account.Balance,
                Login = user.Login,
                Transactions = user.Account.Transactions.Select(t=>new TransactionDto()
                {
                    TransactionId = t.TransactionId,
                    Amount = t.Amount
                }).ToList()
            };
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
