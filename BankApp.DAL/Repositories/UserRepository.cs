using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using BankApp.Domain;
using Microsoft.EntityFrameworkCore;

namespace BankApp.DAL.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
    }
    public class UserRepository : GenericRepository<User>,  IUserRepository
    {
        public UserRepository(BankContext context) : base(context)
        {
        }
    }
}
