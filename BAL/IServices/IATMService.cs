using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MODEL.DTOs;

namespace BAL.IServices
{
    public interface IATMService
    {

        Task<ATMListResponseDTO> GetAllTransactions(string AccountNumber);
        Task<ATMResponseDTO> ValidatePin(int pin);
        Task<ATMResponseDTO> Withdraw(string AccountNumber,int pin, decimal amount);
        Task<ATMResponseDTO> Deposit(string AccountNumber,int pin, decimal amount);
        Task<ATMResponseDTO> CheckBalance(string AccountNumber);
        Task<ATMResponseDTO> Transfer(string fromAccountNumber, string toAccountNumber, decimal amount, int pin);
        
        Task<BankResponseDTO> ChangePIN(string AccountNumber, int pin, int newPin);
    }
}
