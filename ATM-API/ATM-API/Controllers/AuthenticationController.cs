using BAL.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MODEL.ApplicationConfig;
using MODEL.DTOs;

namespace ATM_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }






    [HttpPost("LoginWeb")]
    public async Task<IActionResult> LoginWeb(UserLoginDTO request)
    {
        try
        {
            var returndata = await _authenticationService.LoginWeb(request);
            if (!returndata.AccountStatus)
            {
                return Ok(new ResponseModel { Message = "Your email is invalid!", Status = APIStatus.Error });
            }
            else if (!returndata.PasswordStatus)
            {
                return Ok(new ResponseModel { Message = "Your password is invalid!", Status = APIStatus.Error });
            }
            return Ok(new ResponseModel { Message = Messages.Successfully, Status = APIStatus.Successful, Data = returndata });
        }
        catch (Exception ex)
        {
            return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }



}
