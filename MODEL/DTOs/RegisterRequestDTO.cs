using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.DTOs;

public class RegisterRequestDTO
{
    public string UserName { get; set; }

    public string Email { get; set; }
    public string Password { get; set; }
    public Decimal Wallet { get; set; }
}
