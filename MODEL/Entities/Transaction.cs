using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.Entities;

[Table("Transaction_Tbl")]
public class Transaction
{
    [Key]
    public Guid TransactionID { get; set; }
    public Guid UserID { get; set; }
    public string TransactionType { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }

    public string? AccountNumber { get; set; }

    public string? FromAccount { get; set; }

    public string? ToAccount { get; set; }

    public string Status { get; set; }

}
