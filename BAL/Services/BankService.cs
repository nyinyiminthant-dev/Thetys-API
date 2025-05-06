using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BAL.IServices;
using MODEL.DTOs;
using MODEL.Entities;
using REPOSITORY.UnitOfWork;
using System.Security.Cryptography;
using static System.Net.WebRequestMethods;

namespace BAL.Services
{
    public class BankService : IBankService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BankService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BankResponseDTO> CreateUser(RegisterRequestDTO requestDTO)
        {
            BankResponseDTO model = new BankResponseDTO();
            var existingUser = await _unitOfWork.User.GetByEmail(requestDTO.Email);

            if (existingUser != null)
            {
                model.IsSuccess = false;
                model.Message = "User already exists. Register Failed";
                model.Data = existingUser;
                return model;
            }



            var hashedPassword = HashPassword(requestDTO.Password);
            var otpCode = GenerateOTP();
            var accountNuber = GenerateAccountNumber();

            var user = new User
            {
                UserName = requestDTO.UserName,
                Email = requestDTO.Email,
                Password = hashedPassword,
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now,
                Islock = "N",
                AccountNumber = accountNuber,
                PIN = 0,
                Wallet = requestDTO.Wallet,                                                                                         
                OTP = otpCode,
                OTP_Exp = DateTime.Now.AddMinutes(5),
                Status = "N"


            };

            if (user.Wallet is 0)
            {
                model.IsSuccess = false;
                model.Message = "Please add the Wallet";
                model.Data = null;
                
                return model;
            }

            if (user.Wallet < 10000)
            {
                model.IsSuccess = false;
                model.Message = "Please add at least over 10000";
                model.Data = null;

                return model;
            }

            await _unitOfWork.User.Add(user);
            var result = await _unitOfWork.SaveChangesAsync();

            if (result > 0)
            {
                bool emailSent = SendOTPEmail(user.Email, user.UserName, otpCode,accountNuber);
                if (!emailSent)
                {
                    model.IsSuccess = false;
                    model.Message = "User registered but failed to send OTP email.";
                    model.Data = user;
                    return model;
                }

                model.IsSuccess = true;
                model.Message = "User Register Successful. OTP sent to email.";
                model.Data = user;
            }
            else
            {
                model.IsSuccess = false;
                model.Message = "User Register Failed";
                model.Data = user;
            }

            return model;

        }


        private static string HashPassword(string password)
        {
            using SHA256 sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private static string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private static string GenerateAccountNumber()
        {
            Random random = new Random();
            return random.Next(10000000, 99999999).ToString();
        }   

        private static bool SendOTPEmail(string toEmail, string userName, string otpCode, string accountnumber)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("nnyi37389@gmail.com");
                mail.To.Add(toEmail);
                mail.Subject = "Your OTP Code from RetailManagement System";


                string htmlBody = $@"
            <div style='font-family: Arial, sans-serif; padding: 20px; border: 1px solid #ddd; border-radius: 10px; max-width: 500px; margin: auto; background-color: #f9f9f9;'>
                <h2 style='color: #007bff; text-align: center;'>Your OTP Code</h2>
                <p style='font-size: 16px; color: #333;'>Dear <strong>{userName}</strong>,</p>
                <p style='font-size: 16px; color: #333;'>Your One-Time Password (OTP) for verification is:</p>
                <p style='font-size: 24px; font-weight: bold; color: #28a745; text-align: center; padding: 10px; border: 2px dashed #28a745; display: inline-block;'>{otpCode}</p>
                <p style='font-size: 14px; color: #ff0000; text-align: center;'>This OTP will expire in 5 minutes.</p>
                <p style='font-size: 24px; font-weight: bold; color: #28a745; text-align: center; padding: 10px; border: 2px dashed #28a745; display: inline-block;'>Your account number is {accountnumber}</p>
               
                <br>
                <p style='font-size: 14px; color: #666; text-align: center;'>Best regards,</p>
                <p style='font-size: 14px; color: #666; text-align: center;'><strong>TravelAgency Team</strong></p>
            </div>";

                mail.Body = htmlBody;
                mail.IsBodyHtml = true;

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("nnyi37389@gmail.com", "jbrq aqmv ukix sfdv"),
                    EnableSsl = true
                };

                smtpClient.Send(mail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        

        public async Task<BankListResponseDTO> GetAllUsers()
        {
            BankListResponseDTO model = new BankListResponseDTO();

            var users = await _unitOfWork.User.GetAll();

            if(users is null)
            {
                model.IsSuccess = false;
                model.Message = "Not found";
                model.Data = null;

                return model;
            }

            model.IsSuccess = true;
            model.Message = "Success";
            model.Data = users.ToList();

            return model;
        }

        public async Task<BankResponseDTO> GetUserById(int id)
        {
            BankResponseDTO model = new BankResponseDTO();

            var user = await _unitOfWork.User.GetById(id);

            if(user is null)
            {
                model.IsSuccess = false;
                model.Message = "Not found";
                model.Data = null;
                return model;
            }

            model.IsSuccess = true;
            model.Message = "Success";
            model.Data = user;

            return model;
        }

        public async Task<BankResponseDTO> UnLockRequest(string accountNumber)
        {
            BankResponseDTO model = new BankResponseDTO();
            var user = (_unitOfWork.User.GetByExp(x => x.AccountNumber == accountNumber)).FirstOrDefault();


            if (user is null)
            {
                model.IsSuccess = false;
                model.Message = "User not found.";
                model.Data = null;
                return model;
            }

            if (user.Islock == "N")
            {
                model.IsSuccess = false;
                model.Message = "Account is already unlocked.";
                model.Data = null;
                return model;
            }

            user.Islock = "N";
            _unitOfWork.User.Update(user);
            var result = await _unitOfWork.SaveChangesAsync();
            string message = result > 0 ? "Account unlocked successfully." : "Account unlock failed.";

            model.IsSuccess = result > 0;
            model.Message = message;
            model.Data = user;

            return model;
        }

        public async Task<BankResponseDTO> UpdateUser(int id, BankRequestDTO requestDTO)
        {
            BankResponseDTO model = new BankResponseDTO();
            var user = await _unitOfWork.User.GetById(id);

            if(user is null)
            {
                model.IsSuccess = false;
                model.Message = "User not found.";
                model.Data = null;
                return model;
            }

            if (user.Islock == "Y")
            {
                model.IsSuccess = false;
                model.Message = "Account is locked.";
                model.Data = null;
                return model;
            }

            if (user.Status == "N")
            {
                model.IsSuccess = false;
                model.Message = "Account not verified.";
                model.Data = null;
                return model;
            }

            if (user.PIN != 0)
            {
                model.IsSuccess = false;
                model.Message = "Please Create a PIN.";
                model.Data = null;
                return model;
            }

            if(user.Wallet == 0)
            {
                model.IsSuccess = false;
                model.Message = "Wallet is empty.";
                model.Data = null;
                return model;
            }   

            if(requestDTO.UserName is not "string")
            {
                user.UserName = requestDTO.UserName;
            }

            if (requestDTO.Email is not "string")
            {
                user.Email = requestDTO.Email;
            }

            if (requestDTO.Password is not "string")
            {
                user.Password = HashPassword(requestDTO.Password);
            }

            if (requestDTO.PIN is not 0)
            {
                user.PIN = requestDTO.PIN;
            }

             _unitOfWork.User.Update(user);
            var result = await _unitOfWork.SaveChangesAsync();

            string message = result > 0 ? "User updated successfully." : "User update failed.";

            model.IsSuccess = result > 0;
            model.Message = message;
            model.Data = user;

            return model;


        }

        public async Task<BankResponseDTO> VerifyAccount(string accountNumber, string otp)
        {
            BankResponseDTO model = new BankResponseDTO();
            var user = (_unitOfWork.User.GetByExp(x => x.AccountNumber == accountNumber)).FirstOrDefault();

            if (user == null)
            {
                model.IsSuccess = false;
                model.Message = "User not found.";
                model.Data = null;

                return model;
            }

            

            if (user.OTP != otp)
            {
                model.IsSuccess = false;
                model.Message = "Invalid OTP.";
                model.Data = null;

                return model;
            }

            if (user.OTP_Exp < DateTime.Now)
            {
                model.IsSuccess = false;
                model.Message = "OTP expired.";
                model.Data = null;


                return model;
            }

            user.Status = "Y";
            user.OTP = null;
            user.OTP_Exp = DateTime.Now;
            _unitOfWork.User.Update(user);
            var result = await _unitOfWork.SaveChangesAsync();

            string message = result > 0 ? "Account verified successfully." : "Account verification failed.";

            model.IsSuccess = result > 0;
            model.Message = message;
            model.Data = user;

            return model;
        }

        public async Task<BankResponseDTO> ResendOTP(string accountNumber)
        {
            BankResponseDTO model = new BankResponseDTO();  
            var user = (_unitOfWork.User.GetByExp(x => x.AccountNumber == accountNumber)).FirstOrDefault();
            
            if (user == null)
            {
                model.IsSuccess = false;
                model.Message = "User not found.";
                model.Data = null;  
                
                return model; 
            }
            
            if (user.OTP_Exp > DateTime.Now)
            {
                model.IsSuccess = false;
                model.Message = "OTP already sent.";
                model.Data = null;  
                
                return model; 
            }
            
            if (user.Status == "Y")
            {
                model.IsSuccess = false;
                model.Message = "Account already verified.";
            }
            
            if (user.Islock == "Y")
            {
                model.IsSuccess = false;
                model.Message = "Account is locked.";
                model.Data = null;  
                
                return model; 
            }

            var newOtp = GenerateOTP();
            user.OTP = newOtp;
            user.OTP_Exp = DateTime.Now.AddMinutes(5);
            user.UpdateAt = DateTime.Now;
            
            _unitOfWork.User.Update(user);
            var result = await _unitOfWork.SaveChangesAsync();
            string message = result > 0 ? "OTP resent successfully." : "OTP resent failed.";
            
            if (result > 0)
            {
                bool emailSent = SendOTPEmail( user.Email,user.UserName, newOtp,   user.AccountNumber);

                if (!emailSent)
                {
                    model.IsSuccess = false;
                    model.Message = "Failed to send OTP email.";
                    model.Data = user;
                    return model;
                }

                model.IsSuccess = true;
                model.Message = "OTP has been resent to your email.";
                model.Data = user;
            }
            else
            {
                model.IsSuccess = false;
                model.Message = "Failed to update OTP.";
                model.Data = user;
            }
            
            return model;
        }

        public async Task<BankResponseDTO> CreatePIN(string accountNumber, PINRequestDTO requestDTO)
        {
            BankResponseDTO model = new BankResponseDTO();

            var user = (_unitOfWork.User.GetByExp(x => x.AccountNumber == accountNumber)).FirstOrDefault();


            if (user is null)
            {
                model.IsSuccess = false;
                model.Message = "User not found.";
                model.Data = null;

                return model;
            }

        

            if (user.PIN != 0)
            {
                model.IsSuccess = false;
                model.Message = "PIN already exists.";
                model.Data = null;
                return model;
            }

            if(user.Status == "N")
            {
                model.IsSuccess = false;
                model.Message = "Account not verified.";
                model.Data = null;
                return model;
            }

           if(user.Islock == "Y")
            {
                model.IsSuccess = false;
                model.Message = "Account is locked.";
                model.Data = null;
                return model;
            }

           if (user.Wallet == 0)
            {
                model.IsSuccess = false;
                model.Message = "Wallet is empty.";
                model.Data = null;
                return model;
            }


            if (user.PIN is  0)
            {
                user.PIN = requestDTO.PIN;
            }
            
            _unitOfWork.User.Update(user);
            var result = await _unitOfWork.SaveChangesAsync();
            string message = result > 0 ? "PIN created successfully." : "PIN creation failed.";
            model.IsSuccess = result > 0;
            model.Message = message;
            model.Data = user;
            return model;

        }
    }
}
