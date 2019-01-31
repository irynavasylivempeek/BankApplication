using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankApp.DAL;
using BankApp.DAL.Repositories;
using BankApp.Domain;
using BankApp.Domain.Enums;
using BankApp.Domain.Transactions;
using BankApp.DTO;
using BankApp.DTO.Transaction;
using Microsoft.EntityFrameworkCore.Internal;

namespace BankApp.BLL
{
    public interface ITransactionService
    {
        void MakeTransaction(TransactionDto transactionDto);
        IEnumerable<TransactionDto> GetAllByUserId(int userId, int page);
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

        public void MakeTransaction(TransactionDto transactionDto)
        {
            var senderAccount = _accountRepository.SingleOrDefault(c => c.UserId == transactionDto.SenderId);
            Account receiverAccount = null;
            using (var transaction = _accountRepository.BeginTransaction())
            {
                try
                {
                    switch (transactionDto.Type)
                    {
                        case TransactionType.DepositTransaction:
                            senderAccount.Balance += transactionDto.Amount;
                            break;

                        case TransactionType.WithdrawTransaction:
                            senderAccount.Balance -= transactionDto.Amount;
                            break;

                        case TransactionType.TransferTransaction:
                            receiverAccount =
                                _accountRepository.SingleOrDefault(c => c.UserId == transactionDto.ReceiverId);
                            receiverAccount.Balance += transactionDto.Amount;
                            senderAccount.Balance -= transactionDto.Amount;
                            _accountRepository.Update(receiverAccount);
                            break;
                    }
                    _transactionRepository.Add(new Transaction()
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
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
        }
       
        public IEnumerable<TransactionDto> GetAllByUserId(int userId, int page)
        {
            var userTransactions = _transactionRepository.GetWithReceiver(c => c.SenderAccount.UserId == userId);
            return userTransactions.Select(c => new TransactionDto()
            {
                SenderId = c.SenderAccount.UserId,
                Amount = c.Amount,
                TransactionId = c.TransactionId,
                Type = c.Type
            });
        }

    }
}
