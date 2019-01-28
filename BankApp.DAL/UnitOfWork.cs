using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly BankContext _context;
        public UnitOfWork(BankContext context)
        {
            _context = context;
        }
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
