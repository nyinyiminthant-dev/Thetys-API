using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MODEL.DTOs;

namespace BAL.IServices;

public interface IUserService
{
    Task<UserResponseDTO> WithDraw(WithDrawRequestDTO withDrawRequest);
    Task<UserResponseDTO> Deposit(DepositRequestDTO depositRequest);

    Task<UserResponseDTO> CheckBalance(BalanceRequestDTO balanceRequest);

    Task<UserResponseDTO> Register(RegisterRequestDTO registerRequest);

}
