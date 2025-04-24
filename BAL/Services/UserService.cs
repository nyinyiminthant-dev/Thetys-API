using System;
using System.Linq;
using System.Threading.Tasks;
using BAL.IServices;
using MODEL.DTOs;
using REPOSITORY.UnitOfWork;

namespace BAL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserResponseDTO> WithDraw(WithDrawRequestDTO withDrawRequest)
        {
            var response = new UserResponseDTO();
            var user = (await _unitOfWork.User.GetByCondition(x => x.UserID == withDrawRequest.UserID)).FirstOrDefault();

            if (user == null)
            {
                response.IsSuccess = false;
                response.Message = "User not found.";
                return response;
            }

            if (user.Wallet < withDrawRequest.Amount)
            {
                response.IsSuccess = false;
                response.Message = "Insufficient balance.";
                return response;
            }

            user.Wallet -= withDrawRequest.Amount;
            _unitOfWork.User.Update(user);

            await _unitOfWork.SaveChangesAsync();

            response.IsSuccess = true;
            response.Message = "Withdrawal successful.";
            response.Balance = user.Wallet;
            return response;
        }

        public async Task<UserResponseDTO> Deposit(DepositRequestDTO depositRequest)
        {
            var response = new UserResponseDTO();
            var user = (await _unitOfWork.User.GetByCondition(x => x.UserID == depositRequest.UserID)).FirstOrDefault(); // Fixed here

            if (user == null)
            {
                response.IsSuccess = false;
                response.Message = "User not found.";
                return response;
            }

           
            user.Wallet += depositRequest.Amount;
            _unitOfWork.User.Update(user);

            await _unitOfWork.SaveChangesAsync();

            response.IsSuccess = true;
            response.Message = "Deposit successful.";
            response.Balance = user.Wallet;
            return response;
        }

        public async Task<UserResponseDTO> CheckBalance(BalanceRequestDTO balanceRequest)
        {
            var response = new UserResponseDTO();
            var user = (await _unitOfWork.User.GetByCondition(x => x.UserID == balanceRequest.UserID)).FirstOrDefault(); 

            if (user == null)
            {
                response.IsSuccess = false;
                response.Message = "User not found.";
                return response;
            }

            response.IsSuccess = true;
            response.Message = "Success";
            response.Balance = user.Wallet;
            return response;
        }

        public Task<UserResponseDTO> Register(RegisterRequestDTO registerRequest)
        {
            throw new NotImplementedException();
        }
    }
}
