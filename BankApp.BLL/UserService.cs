using System;
using System.Collections.Generic;
using System.Text;
using BankApp.DAL;
using BankApp.Domain;
using BankApp.DTO;
using BankApp.Utils;

namespace BankApp.BLL
{
    public interface IUserService
    {
        UserDto Add(LoginUser user);

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

       
    }
}
