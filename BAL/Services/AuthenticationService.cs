using System;
using System.Linq;
using System.Threading.Tasks;
using BAL.IServices;
using BAL.Shared;
using MODEL.DTOs;
using REPOSITORY.UnitOfWork;

namespace BAL.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CommonAuthentication _commonAuthentication;

        public AuthenticationService(IUnitOfWork unitOfWork, CommonAuthentication commonAuthentication)
        {
            _unitOfWork = unitOfWork;
            _commonAuthentication = commonAuthentication;
        }

        public async Task<ResponseUserLoginDTO> LoginWeb(UserLoginDTO loginDTO)
        {
            try
            {
                var returndata = new ResponseUserLoginDTO();

                var userdata = (await _unitOfWork.User
                    .GetByCondition(x => x.AccountNumber == loginDTO.AccountNumber && x.Status == "Y"))
                    .FirstOrDefault();

                if (userdata == null)
                {
                    returndata.AccountStatus = false;
                    return returndata;
                }

                returndata.AccountStatus = true;

                bool isValid = _commonAuthentication.VerifyPasswordHash(loginDTO.Password, userdata.Password);

                if (isValid)
                {
                    returndata.UserId = userdata.UserID;

                    returndata.Token = CommonTokenGenerator.GenerateToken(userdata, "User");

                    returndata.AccountNumber = userdata.AccountNumber;
                    returndata.PasswordStatus = true;

                    return returndata;
                }
                else
                {
                    returndata.PasswordStatus = false;
                    return returndata;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
