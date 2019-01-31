using BankApp.Domain;
using BankApp.Domain.Transactions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace BankApp.DAL
{
    public class BankContext : DbContext
    {
        public DbSet<User> Users { set; get; }
        public DbSet<Account> Accounts { set; get; }
        public DbSet<Domain.Transactions.Transaction> Transactions { set; get; }
        public BankContext(DbContextOptions<BankContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(b => b.UserName)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(b => b.Password)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(b => b.Salt)
                .IsRequired();
        }
    }
}
