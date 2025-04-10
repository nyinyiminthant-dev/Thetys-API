using BAL.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var result = await _readTransactionService.GetTransactionByUserId(userId);

            if (!result.IsSuccess)
                return NotFound("Transaction not found for this user.");

            return Ok(result);
        }
    }
}
