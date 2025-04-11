using BAL.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MODEL.ApplicationConfig;
using MODEL.DTOs;

namespace ATM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDTO)
        {
            try
            {
                var result = await _authenticationService.LoginWeb(loginDTO);
                return Ok(new ResponseModel { Message = result.Message, Status = APIStatus.Successful, Data = result.Data} );
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError});
            }
        }
    }
}
