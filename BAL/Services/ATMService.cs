using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using BAL.IServices;
using Microsoft.Extensions.Logging.Abstractions;
using MODEL.DTOs;
using MODEL.Entities;
using REPOSITORY.UnitOfWork;

namespace BAL.Services;

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

        var atm = new Transaction
        {
            TransactionType = "Deposit",
            TransactionDate = DateTime.Now,
            Amount = amount,
            AccountNumber = user.AccountNumber,
            Status = "Y",
            UserID = user.UserID
                
        };
           

        await _unitOfWork.transaction.Add(atm);
        await _unitOfWork.SaveChangesAsync();
            
        SendMail(user.Email, user.UserName, $"You deposited {amount} MMK to your account.", user.AccountNumber);

            
        response.Message = "Deposit successful";
        response.IsSuccess = true;
        response.Data = atm;
        return response;
    }

        
        
       
        
        
    public async Task<ATMResponseDTO> Withdraw(string acountNumber, int pin, decimal amount)
    {
        var response = new ATMResponseDTO();
        var user = _unitOfWork.User.GetByExp(x => x.AccountNumber == acountNumber && x.PIN == pin).FirstOrDefault();

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

        var atm = new Transaction
        {
            TransactionType = "Withdraw",
            TransactionDate = DateTime.Now,
            Amount = amount,
            AccountNumber = user.AccountNumber,
            Status = "Y",
            UserID = user.UserID
                
        };
            
            
        await _unitOfWork.transaction.Add(atm);
        await _unitOfWork.SaveChangesAsync();
        
        SendMail(user.Email, user.UserName, $"You withdrew {amount} MMK from your account.", user.AccountNumber);

            
        response.IsSuccess = true;
        response.Message = "Withdrawal successful";
        response.Data = atm;

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

        var atm = new Transaction
        {
            TransactionType = "Transfer",
            TransactionDate = DateTime.Now,
            Amount = amount,
            FromAccount = fromUser.AccountNumber,
            ToAccount = toUser.AccountNumber,
            Status = "Y",
            UserID = fromUser.UserID,
            AccountNumber = fromUser.AccountNumber,
                
                
        };
           
            
        await _unitOfWork.transaction.Add(atm);
        await _unitOfWork.SaveChangesAsync();

        SendMail(fromUser.Email, fromUser.UserName, $"You transferred {amount} MMK to {toUser.UserName} ({toUser.AccountNumber})", fromUser.AccountNumber);
        SendMail(toUser.Email, toUser.UserName, $"You received {amount} MMK from {fromUser.UserName} ({fromUser.AccountNumber})", toUser.AccountNumber);

            
        response.Message = "Transfer successful";
        response.IsSuccess = true;
        response.Data = atm;
        return response;
    }

    public async Task<BankResponseDTO> ChangePIN(string AccountNumber, int pin, int newPin)
    {
        BankResponseDTO model = new BankResponseDTO();
        var user =  _unitOfWork.User.GetByExp(x => x.AccountNumber == AccountNumber && x.PIN == pin).FirstOrDefault();
        if (user == null)
        {
            model.Message = "Invalid account or PIN";
            model.IsSuccess = false;
            return model;
        }

        if (user.PIN is 0)
        {
            model.Message = "PIN is not set";   
            model.IsSuccess = false;
            return model;
        }

        if (newPin is not 0)
        {
            user.PIN = newPin;   
               
        }
            
        user.UpdateAt = DateTime.Now;
        _unitOfWork.User.Update(user);
        await _unitOfWork.SaveChangesAsync();
        
        SendMailForPinChange(user.Email, user.UserName, user.AccountNumber);
            
        model.IsSuccess = true;
        model.Message = "PIN changed successfully";
        model.Data = user;
            
        return model;
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
        response.Data = transactions.ToList();

        return response;
    }
    
    
    private void SendMail(string toEmail, string fullName, string message, string accountNumber)
    {
        try
        {
            string fromEmail = "nnyi37389@gmail.com";
            string fromPassword = "jbrq aqmv ukix sfdv"; 

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromEmail, "ATM System");
            mail.To.Add(toEmail);
            mail.Subject = "ATM Transaction Notification";

            mail.Body = $@"
Hello {fullName},

This is to inform you that a transaction has been made on your account.

Details:
----------------------
Account Number: {accountNumber}
Message: {message}
Date: {DateTime.Now}
----------------------

If you did not authorize this transaction, please contact our support team immediately.

Thank you,
ATM System Support Team
";
            mail.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(fromEmail, fromPassword);
            smtp.EnableSsl = true;

            smtp.Send(mail);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error sending email: " + ex.Message);
        }
    }

        
    public void SendMailForPinChange(string toEmail, string fullName, string accountNumber)
    {
        try
        {
            string fromEmail = "nnyi37389@gmail.com";
            string fromPassword = "jbrq aqmv ukix sfdv"; 

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromEmail, "ATM System");
            mail.To.Add(toEmail);
            mail.Subject = "ATM PIN Change Notification";

            mail.Body = $@"
Hello {fullName},

We would like to inform you that your ATM PIN has been successfully changed.

Details:
----------------------
Account Number: {accountNumber}
Date and Time: {DateTime.Now}
----------------------

If you did not request this change, please contact our customer support immediately to secure your account.

Thank you,
ATM System Security Team
";
            mail.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(fromEmail, fromPassword);
            smtp.EnableSsl = true;

            smtp.Send(mail);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error sending PIN change email: " + ex.Message);
        }
    }

}