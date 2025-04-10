using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MODEL.ApplicationConfig;

namespace MODEL.DTOs;

public class UserLoginDTO 
{
    [Required(AllowEmptyStrings = false)]
    public string UserName { get; set; } = null!;

    [Required(AllowEmptyStrings = false)] 
    public string Password { get; set; } = null!;
}

public class ResponseUserLoginDTO 
{

    public Guid UserID { get; set; }
    public string UserName { get; set; } = null!;
    public decimal Wallet { get; set; }
    public string Islock { get; set; } = null!;

    public bool IsSuccess { get; set; } 

}

