using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using BankApp.Domain;
using Microsoft.EntityFrameworkCore;

namespace BankApp.DAL.Repositories
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        
    }
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(BankContext context) : base(context)
        {
        }
    }
}
