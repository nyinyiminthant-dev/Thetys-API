using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BAL.IServices;
using MODEL.DTOs;
using MODEL.Entities;
using REPOSITORY.UnitOfWork;

namespace BAL.Services
{
    public class ATMService : IATMService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ATMService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ATMResponseDTO> CheckBalance(string AccountNumber)
        {
            var response = new ATMResponseDTO();
            var user = _unitOfWork.User.GetByExp(x => x.AccountNumber == AccountNumber).FirstOrDefault();

            if (user == null)
            {
                response.Message = "Account not found";
                response.IsSuccess = false;
                return response;
            }

            response.Message = "Balance retrieved successfully";
            response.IsSuccess = true;
            response.Data = new Transaction
            {
                AccountNumber = user.AccountNumber,
                Amount = user.Wallet,
                TransactionType = "Balance Inquiry",
                TransactionDate = DateTime.Now
            };

            return response;
        }

        public async Task<ATMResponseDTO> Deposit(string AccountNumber, int pin, decimal amount)
        {
            var response = new ATMResponseDTO();
            var user = _unitOfWork.User.GetByExp(x => x.AccountNumber == AccountNumber && x.PIN == pin).FirstOrDefault();

            if (user == null)
            {
                response.Message = "Invalid account or PIN";
                response.IsSuccess = false;
                return response;
            }

            user.Wallet += amount;
            user.UpdateAt = DateTime.Now;
            _unitOfWork.User.Update(user);
            await _unitOfWork.SaveChangesAsync();

            response.Message = "Deposit successful";
            response.IsSuccess = true;
            response.Data = new Transaction
            {
                AccountNumber = user.AccountNumber,
                Amount = amount,
                TransactionType = "Deposit",
                TransactionDate = DateTime.Now
            };

            return response;
        }

        public async Task<ATMResponseDTO> Withdraw(string AccountNumber, int pin, decimal amount)
        {
            var response = new ATMResponseDTO();
            var user = _unitOfWork.User.GetByExp(x => x.AccountNumber == AccountNumber && x.PIN == pin).FirstOrDefault();

            if (user == null)
            {
                response.Message = "Invalid account or PIN";
                response.IsSuccess = false;
                return response;
            }

            if (user.Wallet < amount)
            {
                response.Message = "Insufficient balance";
                response.IsSuccess = false;
                return response;
            }

            user.Wallet -= amount;
            user.UpdateAt = DateTime.Now;
            _unitOfWork.User.Update(user);
            await _unitOfWork.SaveChangesAsync();

            response.Message = "Withdrawal successful";
            response.IsSuccess = true;
            response.Data = new Transaction
            {
                AccountNumber = user.AccountNumber,
                Amount = amount,
                TransactionType = "Withdrawal",
                TransactionDate = DateTime.Now
            };

            return response;
        }

        public async Task<ATMResponseDTO> Transfer(string fromAccountNumber, string toAccountNumber, decimal amount, int pin)
        {
            var response = new ATMResponseDTO();
            var fromUser = _unitOfWork.User.GetByExp(x => x.AccountNumber == fromAccountNumber && x.PIN == pin).FirstOrDefault();
            var toUser = _unitOfWork.User.GetByExp(x => x.AccountNumber == toAccountNumber).FirstOrDefault();

            if (fromUser == null || toUser == null)
            {
                response.Message = "Invalid account or PIN";
                response.IsSuccess = false;
                return response;
            }

            if (fromUser.Wallet < amount)
            {
                response.Message = "Insufficient balance";
                response.IsSuccess = false;
                return response;
            }

            fromUser.Wallet -= amount;
            toUser.Wallet += amount;

            fromUser.UpdateAt = DateTime.Now;
            toUser.UpdateAt = DateTime.Now;

            _unitOfWork.User.Update(fromUser);
            _unitOfWork.User.Update(toUser);
            await _unitOfWork.SaveChangesAsync();

            response.Message = "Transfer successful";
            response.IsSuccess = true;
            response.Data = new Transaction
            {
                FromAccount = fromUser.AccountNumber,
                ToAccount = toUser.AccountNumber,
                Amount = amount,
                TransactionType = "Transfer",
                TransactionDate = DateTime.Now
            };

            return response;
        }

        public async Task<ATMResponseDTO> ValidatePin(int pin)
        {
            var response = new ATMResponseDTO();
            var user = _unitOfWork.User.GetByExp(x => x.PIN == pin).FirstOrDefault();

            if (user == null)
            {
                response.Message = "Invalid PIN";
                response.IsSuccess = false;
                return response;
            }

            response.Message = "PIN is valid";
            response.IsSuccess = true;
            response.Data = new Transaction
            {
                AccountNumber = user.AccountNumber,
                Amount = user.Wallet,
                
            };

            return response;
        }

        public async Task<ATMListResponseDTO> GetAllTransactions(string AccountNumber)
        {
            var response = new ATMListResponseDTO();
            var user = _unitOfWork.User.GetByExp(x => x.AccountNumber == AccountNumber).FirstOrDefault();

            if (user == null)
            {
                response.Message = "Account not found";
                response.IsSuccess = false;
                return response;
            }

            var transactions = _unitOfWork.transaction.GetByExp(t => t.AccountNumber == AccountNumber || t.FromAccount == AccountNumber || t.ToAccount == AccountNumber).ToList();

            response.Message = "Transactions retrieved";
            response.IsSuccess = true;
            response.Data = transactions;

            return response;
        }
    }
}
