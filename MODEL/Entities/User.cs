using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MODEL.ApplicationConfig;

namespace MODEL.Entities;

[Table("User_Tbl")]
public class User
{
    [Key]
    public Guid UserID { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }
    public string Password { get; set; }
    public decimal Wallet { get; set; }

    public string Islock { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public string? OTP { get; set; }

    public DateTime? OTP_Exp { get; set; }


}