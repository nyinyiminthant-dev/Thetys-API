using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL.IServices;
using BAL.Shared;
using MODEL.DTOs;
using REPOSITORY.UnitOfWork;

namespace BAL.Services;

internal class AuthenticationService : IAuthenticationService
{

    private readonly IUnitOfWork _unitOfWork;

    public AuthenticationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }



    public async Task<ResponseUserLoginDTO> LoginWeb(UserLoginDTO loginDTO)
    {
        try
        {
            var returndata = new ResponseUserLoginDTO();
            var userdata = (await _unitOfWork.User.GetByCondition(x => x.UserName == loginDTO.UserName )).FirstOrDefault();
            if (userdata == null)
            {
                returndata.Message = "User not found";
                return returndata;
            }
            else
            {
                returndata.Islock = "N";

               
                string inputPassword = loginDTO.Password;
                string actualPassword = userdata.Password;

                var checkpassword = CommonAuthentication.VerifyPassword(inputPassword, actualPassword);

                if (checkpassword)
                {
                    returndata.Data = userdata;

                    returndata.IsSuccess = true;
                    returndata.Message = "Success";

                }
                else
                {
                    returndata.IsSuccess = false;
                    returndata.Message = "Invalid password";

                }

                return returndata;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error in LoginWeb: " + ex.Message);
        }
    }

}
