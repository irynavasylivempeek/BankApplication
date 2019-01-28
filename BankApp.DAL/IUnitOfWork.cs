using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.DAL
{
    public interface IUnitOfWork
    {
        int SaveChanges();
    }
}
