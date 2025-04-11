using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MODEL.ApplicationConfig;
using MODEL.Entities;

namespace MODEL.DTOs;

public class TransactionDTO
{
    public Guid UserID { get; set; }
    public string TransactionType { get; set; }
    public decimal Amount { get; set; }
}

public class TransactionResponseDTO : Common
{
    public Guid TransactionID { get; set; }
    public Guid UserID { get; set; }
    public string TransactionType { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }

    public Transaction Data { get; set; }
   

    
}
