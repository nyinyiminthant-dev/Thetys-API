using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MODEL.DTOs;

namespace BAL.IServices;

public interface IBankService
{
   Task<BankListResponseDTO> GetAllUsers();
   Task<BankResponseDTO> GetUserById(int id);
   Task<BankResponseDTO> CreateUser(RegisterRequestDTO requestDTO);
   Task<BankResponseDTO> UpdateUser(int id, BankRequestDTO requestDTO);
   Task<BankResponseDTO> UnLockRequest(string accountNumber);

   Task<BankResponseDTO> CreatePIN(string accountNumber, PINRequestDTO requestDTO);
   Task<BankResponseDTO> VerifyAccount(string accountNumber, string OTP);

}
