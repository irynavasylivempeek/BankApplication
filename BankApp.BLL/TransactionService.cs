using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankApp.DAL;
using BankApp.DAL.Repositories;
using BankApp.Domain;
using BankApp.Domain.Enums;
using BankApp.DTO.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Transaction = BankApp.DTO.Transactions.Transaction;
using User = BankApp.DTO.Users.User;

namespace BankApp.BLL
{
    public interface ITransactionService
    {
        void MakeTransaction(Transaction transactionDto);
        IEnumerable<TransactionDetails> GetAllByUserId(int userId);
    }

    public class TransactionService : ITransactionService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(IAccountRepository accountRepository, ITransactionRepository transactionRepository)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }

        public void MakeTransaction(Transaction transactionDto)
        {
            var senderAccount = _accountRepository.SingleOrDefault(c => c.UserId == transactionDto.SenderId);
            Account receiverAccount = null;
            int transactionDirection = 1;
            using (var transaction = _accountRepository.BeginTransaction())
            {
                try
                {
                    switch (transactionDto.Type)
                    {
                        case TransactionType.Withdraw:
                            transactionDirection = -1;
                            break;

                        case TransactionType.Transfer:
                            receiverAccount =
                                _accountRepository.SingleOrDefault(c => c.UserId == transactionDto.ReceiverId);
                            receiverAccount.Balance += transactionDto.Amount;
                            transactionDirection = -1;
                            _accountRepository.Update(receiverAccount);
                            break;
                    }

                    senderAccount.Balance += transactionDto.Amount * transactionDirection;
                    _transactionRepository.Add(new Domain.Transaction()
                    {
                        SenderAccountId = senderAccount.AccountId,
                        Amount = transactionDto.Amount,
                        ReceiverAccountId = receiverAccount?.AccountId,
                        Type = transactionDto.Type
                    });

                    _accountRepository.Update(senderAccount);
                    _accountRepository.SaveChanges();

                    transaction.Commit();
                }
                catch (DbUpdateConcurrencyException)
                {
                    transaction.Rollback();
                    throw new Exception("Sorry, your transaction was canceled! Try again");
                }
            }
        }

        public IEnumerable<TransactionDetails> GetAllByUserId(int userId)
        {
            var userTransactions = _transactionRepository.GetWithReceiver(c => c.SenderAccount.UserId == userId);
            return userTransactions.Select(c => new TransactionDetails()
            {
                Sender = new User
                {
                    UserId = c.SenderAccount.UserId,
                    UserName = c.SenderAccount.User.UserName
                },
                Receiver = c.ReceiverAccountId == null ? null : new User
                {
                    UserId = c.ReceiverAccount.UserId,
                    UserName = c.ReceiverAccount.User.UserName
                },
                Amount = c.Amount,
                TransactionId = c.TransactionId,
                Type = c.Type
            });
        }
    }
}
