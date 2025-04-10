using Microsoft.AspNetCore.Mvc;
using BAL.IServices;
using MODEL.DTOs;
using System.Threading.Tasks;

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
            if (withDrawRequest == null)
            {
                return BadRequest("Invalid request.");
            }

            var response = await _userService.WithDraw(withDrawRequest);

            if (!response.IsSuccess)
            {
                return BadRequest(response.Message);
            }

            return Ok(response);
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] DepositRequestDTO depositRequest)
        {
            if (depositRequest == null)
            {
                return BadRequest("Invalid request.");
            }

            var response = await _userService.Deposit(depositRequest);

            if (!response.IsSuccess)
            {
                return BadRequest(response.Message);
            }

            return Ok(response);
        }

        [HttpPost("checkbalance")]
        public async Task<IActionResult> CheckBalance([FromBody] BalanceRequestDTO balanceRequest)
        {
            if (balanceRequest == null)
            {
                return BadRequest("Invalid request.");
            }

            var response = await _userService.CheckBalance(balanceRequest);

            if (!response.IsSuccess)
            {
                return BadRequest(response.Message);
            }

            return Ok(response);
        }
    }
}
