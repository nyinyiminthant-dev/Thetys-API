using System.Net.NetworkInformation;
using BAL.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MODEL.ApplicationConfig;

namespace ATM_API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ATMController : ControllerBase
{
  private readonly IATMService _atmService;

    public ATMController(IATMService atmService)
    {
        _atmService = atmService;
    }


    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll(string AccountNumber)
    {
        try
        {
            var response = await _atmService.GetAllTransactions( AccountNumber);
            return Ok(new ResponseModel { Message = response.Message, Status = APIStatus.Successful, Data = response.Data });
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }

    [HttpPost("Deposit")]
    public async Task<IActionResult> Deposit([FromQuery] string AccountNumber,[FromQuery] int pin,[FromQuery] decimal amount)
    {
        try
        {
            var response = await _atmService.Deposit( AccountNumber,  pin,  amount);
            return Ok(new ResponseModel { Message = response.Message, Status = APIStatus.Successful, Data = response.Data });
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }
    [HttpPost("Withdraw")]
    public async Task<IActionResult> Withdraw(string accountNumber,int pin, decimal amount)
    {
        try
        {
            var response = await _atmService.Withdraw(accountNumber,pin, amount);
            return Ok(new ResponseModel { Message = response.Message, Status = APIStatus.Successful, Data = response.Data });
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }
    [HttpGet("CheckBalance")]
    public async Task<IActionResult> CheckBalance(string accountNumber)
    {
        try
        {
            var response = await _atmService.CheckBalance(accountNumber);
            return Ok(new ResponseModel { Message = response.Message, Status = APIStatus.Successful, Data = response.Data });
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }

    [HttpPost("Transfer")]
    public async Task<IActionResult> Transfer(string fromAccountNumber, string toAccountNumber, decimal amount,int pin)
    {
        try
        {
            var response = await _atmService.Transfer(fromAccountNumber, toAccountNumber, amount, pin);
            return Ok(new ResponseModel { Message = response.Message, Status = APIStatus.Successful, Data = response.Data });
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }

    [HttpPost("ValidatePin")]
    public async Task<IActionResult> ValidatePin(int pin)
    {
        try
        {
            var response = await _atmService.ValidatePin(pin);
            return Ok(new ResponseModel { Message = response.Message, Status = APIStatus.Successful, Data = response.Data });
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }

    [HttpPost("ChangePIn")]
    public async Task<IActionResult> ChangePin(string AccountNumber, int pin, int newPin)
    {
        try
        {
            var response = await _atmService.ChangePIN( AccountNumber, pin, newPin);
            return Ok(new ResponseModel { Message = response.Message, Status = APIStatus.Successful, Data = response.Data });
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }
}
