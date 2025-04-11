using Microsoft.AspNetCore.Mvc;
using BAL.IServices;
using MODEL.DTOs;
using System.Threading.Tasks;
using MODEL.ApplicationConfig;

namespace ATM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> WithDraw([FromBody] WithDrawRequestDTO withDrawRequest)
        {
            try
            {
                var response = await _userService.WithDraw(withDrawRequest);

                return Ok(new ResponseModel { Message = response.Message, Status = APIStatus.Successful, Data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
            }
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] DepositRequestDTO depositRequest)
        {
            try
            {
                var response = await _userService.Deposit(depositRequest);

                return Ok(new ResponseModel { Message = response.Message, Status = APIStatus.Successful, Data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
            }
        }

        [HttpPost("checkbalance")]
        public async Task<IActionResult> CheckBalance([FromBody] BalanceRequestDTO balanceRequest)
        {
            try
            {
                var response = await _userService.CheckBalance(balanceRequest);
                return Ok(new ResponseModel { Message = response.Message, Status = APIStatus.Successful, Data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
            }
        }
    }
}
