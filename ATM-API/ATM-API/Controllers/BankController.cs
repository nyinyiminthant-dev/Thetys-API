using BAL.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MODEL.ApplicationConfig;
using MODEL.DTOs;

namespace ATM_API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class BankController : ControllerBase
{
    private readonly IBankService _bankService;

    public BankController(IBankService bankService)
    {
        _bankService = bankService;
    }


    [Authorize]
    [HttpGet]

    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var response = await _bankService.GetAllUsers();
            return Ok(new ResponseModel { Message = response.Message, Status = APIStatus.Successful, Data = response.Data });
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var response = await _bankService.GetUserById(id);
            return Ok(new ResponseModel { Message = response.Message, Status = APIStatus.Successful, Data = response.Data });
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }


    [HttpPost("Register")]
    public async Task<IActionResult> CreateUser([FromBody] RegisterRequestDTO user)
    {
        try
        {
            var response = await _bankService.CreateUser(user);
            return Ok(new ResponseModel { Message = response.Message, Status = APIStatus.Successful, Data = response.Data });
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }

    [Authorize]
    [HttpPatch("Update")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] BankRequestDTO user)
    {
        try
        {
            var response = await _bankService.UpdateUser(id, user);
            return Ok(new ResponseModel { Message = response.Message, Status = APIStatus.Successful, Data = response.Data });
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }



    [HttpPost("CreatePIN")]
    public async Task<IActionResult> CreatePIN(string accountNumber, [FromBody] PINRequestDTO user)
    {
        try
        {
            var response = await _bankService.CreatePIN(accountNumber, user);
            return Ok(new ResponseModel { Message = response.Message, Status = APIStatus.Successful, Data = response.Data });
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }


    [HttpPost("VerifyAccount")]
    public async Task<IActionResult> VerifyAccount(string accountNumber, string OTP)
    {
        try
        {
            var response = await _bankService.VerifyAccount(accountNumber, OTP);
            return Ok(new ResponseModel { Message = response.Message, Status = APIStatus.Successful, Data = response.Data });
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }

    [Authorize]
    [HttpPost("UnLockRequest")]
    public async Task<IActionResult> UnLockRequest(string accountNumber)
    {
        try
        {
            var response = await _bankService.UnLockRequest(accountNumber);
            return Ok(new ResponseModel { Message = response.Message, Status = APIStatus.Successful, Data = response.Data });
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }


    [HttpPost ("ResendOTP")]
    public async Task<IActionResult> ResendOTP(string accountNumber)
    {
        try
        {
             var response = await _bankService.ResendOTP(accountNumber);
             return Ok(new ResponseModel { Message = response.Message, Status = APIStatus.Successful, Data = response.Data });
        }
        catch (Exception e)
        {
            return BadRequest(new ResponseModel { Message = e.Message, Status = APIStatus.SystemError });
        }
    }

}
