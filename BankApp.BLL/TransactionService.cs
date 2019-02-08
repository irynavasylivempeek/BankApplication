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
using Microsoft.EntityFrameworkCore.Update;
using Transaction = BankApp.DTO.Transactions.Transaction;
using User = BankApp.DTO.Users.User;

namespace BankApp.BLL
{
    public interface ITransactionService
    {
        void MakeTransaction(Transaction transactionDto);
    }

    public class TransactionService : ITransactionService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private int attempts = 5;

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
                catch (Exception e)
                {
                    transaction.Rollback();
                    _accountRepository.DetachAllEntities();
                    if (e is DbUpdateConcurrencyException)
                        throw;
                    throw new Exception("Sorry, your transaction was canceled! Try again later");
                }
            }
        }
    }
}
