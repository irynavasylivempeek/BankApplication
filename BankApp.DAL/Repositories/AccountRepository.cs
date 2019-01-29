using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using BankApp.DAL.Repositories.Interfaces;
using BankApp.Domain;
using Microsoft.EntityFrameworkCore;

namespace BankApp.DAL.Repositories
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(BankContext context) : base(context)
        {
        }
    }
}
