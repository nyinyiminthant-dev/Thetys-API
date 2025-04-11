using BAL.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MODEL.ApplicationConfig;

namespace ATM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadTransactionController : ControllerBase
    {
        private readonly IReadTransactionService _readTransactionService;

        public ReadTransactionController(IReadTransactionService readTransactionService)
        {
            _readTransactionService = readTransactionService;
        }


        [HttpGet("transaction/{userId}")]
        public async Task<IActionResult> GetTransaction(Guid userId)
        {
            try
            {
                var result = await _readTransactionService.GetTransactionByUserId(userId);

                return Ok(new ResponseModel { Message = result.Message, Status = APIStatus.Successful,  Data = result.Data });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
            }
        }
    }
}
