using System;
using System.Collections.Generic;
using System.Text;
using BankApp.DAL;

namespace BankApp.BLL
{
    public interface IUserService
    {
    }
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
