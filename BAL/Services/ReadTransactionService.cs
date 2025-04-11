using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL.IServices;
using Microsoft.EntityFrameworkCore;
using MODEL.DTOs;
using REPOSITORY.UnitOfWork;

namespace BAL.Services;

internal class ReadTransactionService : IReadTransactionService
{
    private readonly IUnitOfWork _unitOfWork;
    public ReadTransactionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<TransactionResponseDTO> GetTransactionByUserId(Guid userId)
    {
        var response = new TransactionResponseDTO();

       
        var transaction = (await _unitOfWork.transaction
            .GetByCondition(x => x.UserID == userId))
            .OrderByDescending(x => x.TransactionDate)
            .FirstOrDefault();

        if (transaction == null)
        {
            response.IsSuccess = false;
            return response;
        }

        response.IsSuccess = true;
        response.Message = "Success";
        response.Data = transaction;

        return response;
    }
}


