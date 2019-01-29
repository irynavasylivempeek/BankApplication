using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankApp.DAL;
using BankApp.Domain;
using BankApp.Domain.Transactions;
using BankApp.DTO;
using BankApp.DTO.Enums;

namespace BankApp.BLL
{
    public interface ITransactionService
    {
        void Withdraw(int userId, double amount);
        void Deposit(int userId, double amount);
        void Transfer(int userId, int receiverId, double amount);
        IEnumerable<TransactionDto> GetAllByUserId(int userId);
    }
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private void MakeTransaction(TransactionDto transactionInfo)
        {
            var account = _unitOfWork.Accounts.FindSingleOrDefault(c => c.UserId == transactionInfo.UserId);
            switch (transactionInfo.Type)
            {
                case TransactionType.Deposit:
                    account.Balance += transactionInfo.Amount;
                    _unitOfWork.Accounts.Update(account);
                    _unitOfWork.Transactions.Add(new DepositTransaction() { AccountId = account.AccountId, Amount = transactionInfo.Amount });
                    break;

                case TransactionType.Withdraw:
                    account.Balance -= transactionInfo.Amount;
                    _unitOfWork.Accounts.Update(account);
                    _unitOfWork.Transactions.Add(new WithdrawTransaction() { AccountId = account.AccountId, Amount = transactionInfo.Amount });
                    break;

                case TransactionType.Transfer:
                    var receiver =
                        _unitOfWork.Accounts.FindSingleOrDefault(c => c.UserId == transactionInfo.ReceiverId);
                    receiver.Balance += transactionInfo.Amount;
                    account.Balance -= transactionInfo.Amount;
                    _unitOfWork.Accounts.UpdateRange(new List<Account>() { account, receiver });
                    _unitOfWork.Transactions.Add(new TransferTransaction() { AccountId = account.AccountId, Amount = transactionInfo.Amount, DestinationId = transactionInfo.ReceiverId });
                    break;
            }
            _unitOfWork.SaveChanges();
        }
        public void Withdraw(int userId, double amount)
        {
            MakeTransaction(new TransactionDto()
            {
                Amount = amount,
                UserId = userId,
                Type = TransactionType.Withdraw
            });
        }

        public void Deposit(int userId, double amount)
        {
            MakeTransaction(new TransactionDto()
            {
                Amount = amount,
                UserId = userId,
                Type = TransactionType.Deposit
            });
        }

        public void Transfer(int userId, int receiverId, double amount)
        {
            MakeTransaction(new TransactionDto()
            {
                Amount = amount,
                UserId = userId,
                ReceiverId = receiverId,
                Type = TransactionType.Transfer
            });
        }

        public IEnumerable<TransactionDto> GetAllByUserId(int userId)
        {
            var userTransactions = _unitOfWork.Transactions.GetIncludingAccount(c => c.Account.UserId == userId);
            return userTransactions.Select(c => new TransactionDto()
            {
                UserId = c.Account.UserId,
                Amount = c.Amount,
                TransactionId = c.TransactionId
            });
        }


    }
}
